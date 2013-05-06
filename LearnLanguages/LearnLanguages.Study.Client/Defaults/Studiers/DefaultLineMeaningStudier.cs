using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Common;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;
using Caliburn.Micro;
using LearnLanguages.Common.Delegates;
using LearnLanguages.History;
using System.Threading;
using LearnLanguages.Navigation.EventMessages;
using LearnLanguages.History.Events;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  /// <summary>
  /// Contains a single Line's information about its aggregate phrases.  It maintains its own 
  /// knowledge of PhraseTexts "KnownPhraseTexts", and exposes methods for marking known/unknown,
  /// asking if IsKnown(phraseText), and performing the actual aggregation.  This includes 
  /// populating aggregate phrase texts with the OriginalAggregateSize, as well as aggregating
  /// adjacent known phrase texts, according to its own knowledge.
  /// </summary>
  public class DefaultLineMeaningStudier : StudierBase<LineEdit>, 
                                           IHandle<ReviewedPhraseEvent>, 
                                           IHandle<NavigationRequestedEventMessage>
  {
    #region Ctors and Init

    public DefaultLineMeaningStudier()
    {
      _Studiers = new Dictionary<int, DefaultPhraseMeaningStudier>();
      Exchange.Ton.SubscribeToStatusUpdates(this);
      History.HistoryPublisher.Ton.SubscribeToEvents(this);
      Services.EventAggregator.Subscribe(this);//navigation
    }

    #endregion

    #region Properties

    /// <summary>
    /// Dictionary of phrase meaning studiers, indexed by their position in this line.
    /// </summary>
    private Dictionary<int, DefaultPhraseMeaningStudier> _Studiers { get; set; }

    /// <summary>
    /// This AggregateSize this object was created with.
    /// </summary>
    public int AggregateSize { get; private set; }

    /// <summary>
    /// The Phrase's order in the list is their order in the line.
    /// </summary>
    public List<string> AggregatePhraseTexts { get; private set; }

    /// <summary>
    /// Contains list of known phrase texts according to this object.  This does not reference
    /// any outside source for knowledge.  It only is an internal reference.
    /// </summary>
    public List<string> KnownPhraseTexts { get; private set; }

    public double KnowledgeThreshold { get; private set; }

    /// <summary>
    /// The index of the last studied phrase in AggregatePhraseTexts
    /// </summary>
    private int _LastStudiedIndex { get; set; }

    private object _IsReadyLock = new object();
    private bool _isReady = false;
    protected bool _IsReady
    {
      get
      {
        lock (_IsReadyLock)
        {
          return _isReady;
        }
      }
      set
      {
        lock (_IsReadyLock)
        {
          _isReady = value;
        }
      }
    }

    /// <summary>
    /// This is the count of unknown phrases to be studied in this cycle of "GetNextStudyItemViewModel".
    /// </summary>
    private int _StudyCycleCountOfUnknownPhrases { get; set; }

    #endregion

    #region Methods

    public override async Task InitializeForNewStudySessionAsync(LineEdit target)
    {
      #region Thinking (methodThinkId)
      var methodThinkId = target.Id;
      History.Events.ThinkingAboutTargetEvent.Publish(methodThinkId);
      #endregion

      #region Initialize variables
      _AbortIsFlagged = false;
      _IsReady = false;
      _Target = target;
      AggregateSize = int.Parse(StudyResources.DefaultMeaningStudierAggregateSize);
      KnowledgeThreshold = double.Parse(StudyResources.DefaultKnowledgeThreshold);
      AggregatePhraseTexts = new List<string>();
      KnownPhraseTexts = new List<string>();
      #endregion

      if (_AbortIsFlagged)
      {
        History.Events.ThinkedAboutTargetEvent.Publish(methodThinkId);
        return;
      }
      #region Thinking (try...)
      var targetId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      try
      {
      #endregion
        await UpdateKnowledge();
      #region (..finally) Thinked
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(targetId);
      }
      #endregion

      if (_AbortIsFlagged)
      {
        #region Thinked (methodThinkId)
        History.Events.ThinkedAboutTargetEvent.Publish(methodThinkId);
        #endregion
        return;
      }

      PopulateAggregatePhraseTexts(AggregateSize);

      #region Thinking (try..)
      var targetId2 = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId2);
      try
      {
      #endregion
        AggregateAdjacentKnownPhraseTexts(
        int.Parse(StudyResources.DefaultMaxIterationsAggregateAdjacentKnownPhraseTexts));

      #region (...finally) Thinked
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(targetId2);
      }
        #endregion
     
      await PopulateStudiersWithAggregatePhraseTexts();
      #region Thinked (methodThinkId)
      History.Events.ThinkedAboutTargetEvent.Publish(methodThinkId);
      #endregion
    }

    public override async Task<ResultArgs<StudyItemViewModelArgs>> GetNextStudyItemViewModelAsync()
    {
      #region Abort Check
      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();
      #endregion

      //IF OUR _LASTSTUDIEDINDEX IS MAXED OUT STUDIERS COUNT, THEN WE
      //HAVE COMPLETED ONE ITERATION THROUGH OUR UNKNOWN AGGREGATE PHRASE TEXTS.  
      if (_Studiers == null || _LastStudiedIndex >= (_Studiers.Count - 1))
      {
        //NOW WE WILL AGGREGATE OUR ADJACENT KNOWN TEXTS AND REPOPULATE OUR PHRASE STUDIERS
        //WITH ONLY THOSE PHRASES THAT WE DON'T KNOW
        var maxIterations = int.Parse(StudyResources.DefaultMaxIterationsAggregateAdjacentKnownPhraseTexts);
        #region Abort Check
        if (_AbortIsFlagged)
          return await StudyHelper.GetAbortedAsync();
        #endregion
        AggregateAdjacentKnownPhraseTexts(maxIterations);

        #region Abort Check
        if (_AbortIsFlagged)
          return await StudyHelper.GetAbortedAsync();
        #endregion
        await PopulateStudiersWithAggregatePhraseTexts();
        
        #region Abort Check
        if (_AbortIsFlagged)
          return await StudyHelper.GetAbortedAsync();
        #endregion
        if (_Studiers.Count == 0)
        {
          //WE KNOW OUR ENTIRE LINE, SO POPULATE STUDIERS WITH A SINGLE PHRASE STUDIER WITH OUR LINE.PHRASE
          var studier = new DefaultPhraseMeaningStudier();
          await studier.InitializeForNewStudySessionAsync(_Target.Phrase);
          _Studiers.Add(0, studier);
          _LastStudiedIndex = 0;
          return await _Studiers[0].GetNextStudyItemViewModelAsync();
        }
        else
        {
          //WE RETURN OUR FIRST STUDIER FOR THIS NEW STUDY CYCLE
          return await _Studiers[0].GetNextStudyItemViewModelAsync();
        }
      }
      else
      {
        #region Abort Check
        if (_AbortIsFlagged)
          return await StudyHelper.GetAbortedAsync();
        #endregion

        //WE ARE CONTINUING A STUDY CYCLE, SO RUN THE NEXT ONE AND UPDATE OUR LAST STUDIED INDEX.
        var indexToStudy = _LastStudiedIndex + 1;
        _LastStudiedIndex++;
        return await _Studiers[indexToStudy].GetNextStudyItemViewModelAsync();
      }
    }

    /// <summary>
    /// When the viewmodel is shown, we know we are the entity that started it.  We should
    /// publish an event saying we are studying an entire line.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //private void ViewModelShownWhenStudyingAnEntireLine(object sender, EventArgs e)
    //{
    //  var viewModel = (IStudyItemViewModelBase)sender;
    //  var reviewMethodId = viewModel.ReviewMethodId;
    //  var reviewingLineEvent = new History.Events.ReviewingLineEvent(_Target, reviewMethodId);
    //  HistoryPublisher.Ton.PublishEvent(reviewingLineEvent);
    //}

    private async Task PopulateStudiersWithAggregatePhraseTexts()
    {
      if (_AbortIsFlagged)
        return;
      _Studiers.Clear();
      _LastStudiedIndex = -1;

      #region IF WE ONLY HAVE ONE AGGREGATE PHRASE TEXT, THEN POPULATE OUR STUDIER WITH OUR TARGET LINE

      if (AggregatePhraseTexts.Count == 1)
      {
        var studier = new DefaultPhraseMeaningStudier();
        #region Thinking (try..)
        var targetId = Guid.NewGuid();
        History.Events.ThinkingAboutTargetEvent.Publish(targetId);
        try
        {
        #endregion
          await studier.InitializeForNewStudySessionAsync(_Target.Phrase);
        #region (...finally) Thinked
        }
        finally
        {
          History.Events.ThinkedAboutTargetEvent.Publish(targetId);
        }
          #endregion
        _Studiers.Add(0, studier);
        return;
      }

      #endregion

      var phraseTextsCriteria =
        new Business.Criteria.PhraseTextsCriteria(_Target.Phrase.Language.Text, AggregatePhraseTexts);
      PhraseList phraseList = null;
      #region Thinking (try..)
      var targetId2 = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId2);
      try
      {
      #endregion
        phraseList = await PhraseList.NewPhraseListAsync(phraseTextsCriteria);
      #region (...finally) Thinked
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(targetId2);
      }
        #endregion
      if (_AbortIsFlagged)
        return;

      List<PhraseEdit> unknownPhraseEdits = new List<PhraseEdit>();
      for (int i = 0; i < AggregatePhraseTexts.Count; i++)
      {
        if (_AbortIsFlagged)
          return;
        var results = from phrase in phraseList
                      where phrase.Text == AggregatePhraseTexts[i]
                      select phrase;
        unknownPhraseEdits.Insert(i, results.First());
      }

      //WE NOW HAVE AN ORDERED LIST OF KNOWN AND UNKNOWN AGGREGATE PHRASEEDITS.
      //WE NEED TO FIND, AND KEEP IN RELATIVE ORDER, ONLY THE UNKNOWN PHRASEEDITS.
      //EACH PHRASE EDIT IS THE SAME INDEX AS ITS PHRASETEXT COUNTERPART.

      //WE NEED TO FIND THE COUNT OF UNKNOWN TEXTS FIRST, TO INCREMENT OUR COUNTER FOR ASYNC FUNCTIONALITY.
      int unknownCount = 0;
      for (int i = 0; i < AggregatePhraseTexts.Count; i++)
      {
        if (_AbortIsFlagged)
          return;

        var phraseText = AggregatePhraseTexts[i];
        if (!IsPhraseKnown(phraseText))
          unknownCount++;
      }

      //UNKNOWNRELATIVEORDER: UNKNOWN IN THAT OUR PHRASE IS UNKNOWN.  RELATIVE ORDER MEANS
      //THE ORDER/POSITION OF THIS PHRASE RELATIVE TO THE OTHER _UNKNOWN_ PHRASES.
      var unknownRelativeOrder = -1;
      int initializedCount = 0;
      for (int i = 0; i < AggregatePhraseTexts.Count; i++)
      {
        if (_AbortIsFlagged)
          return;

        var phraseText = AggregatePhraseTexts[i];
        if (IsPhraseKnown(phraseText))
          continue;

        //PHRASE IS UNKNOWN, SO INC OUR RELATIVE ORDER AND ADD THE STUDIER TO STUDIERS, INITIALIZING EACH STUDIER.
        unknownRelativeOrder++;
        var studier = new DefaultPhraseMeaningStudier();
        #region Thinking (try..)
        var targetId3 = Guid.NewGuid();
        History.Events.ThinkingAboutTargetEvent.Publish(targetId3);
        try
        {
        #endregion
          await studier.InitializeForNewStudySessionAsync(unknownPhraseEdits[i]);
        #region (...finally) Thinked
        }
        finally
        {
          History.Events.ThinkedAboutTargetEvent.Publish(targetId3);
        }
          #endregion
        if (_AbortIsFlagged)
          return;

        _Studiers.Add(unknownRelativeOrder, studier);
        initializedCount++;
        //IF WE HAVE INITIALIZED ALL OF OUR UNKNOWN PHRASES, THEN WE ARE DONE AND CAN CALL CALLBACK.
        if (initializedCount == unknownCount)
          return;
      }

      //THIS LINE IS ENTIRELY KNOWN, SO INITIALIZE A NEW STUDIER WITH THE ENTIRE LINE.
      if (unknownCount == 0)
      {
        var studier = new DefaultPhraseMeaningStudier();

        #region Thinking (try..)
        var targetId4 = Guid.NewGuid();
        History.Events.ThinkingAboutTargetEvent.Publish(targetId4);
        try
        {
        #endregion
          await studier.InitializeForNewStudySessionAsync(_Target.Phrase);
        #region (...finally) Thinked
        }
        finally
        {
          History.Events.ThinkedAboutTargetEvent.Publish(targetId4);
        }
          #endregion
        _Studiers.Add(0, studier);
        return;
      }
    }

    /// <summary>
    /// Parses the Line.Text into phrase texts, each phrase with a word count of aggregateSize, excepting
    /// the final phrase which may have fewer words.
    /// </summary>
    /// <param name="aggregateSize">the number of words contained in each aggregate phrase text.</param>
    private void PopulateAggregatePhraseTexts(int aggregateSize, bool skipAggregationIfLineIsKnown = true)
    {
      #region Thinking (try..)
      var methodThinkId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(methodThinkId);
      try
      {
      #endregion
        if (aggregateSize == 0)
          throw new ArgumentOutOfRangeException("aggregateSize");

        AggregatePhraseTexts.Clear();

        var lineText = _Target.Phrase.Text;
        var lineIsKnown = IsPhraseKnown(lineText);
        if (skipAggregationIfLineIsKnown && lineIsKnown)
        {
          //IF WE ARE SKIPPING THE AGGREGATION (WE'RE NOT FORCING LINE TO BREAK INTO AGGREGATES)
          //AND IF THE ENTIRE LINE TEXT IS KNOWN, 
          //THEN ADD THE ENTIRE LINE TEXT TO THE AGGREGATE PHRASE TEXTS AND WE'RE DONE.
          AggregatePhraseTexts.Add(lineText);
          return;
        }

        //lets say lineText is 20 words long (long line).  our aggregateSize is 2.  
        //We will end up with 20/2 = 10 aggregate phrases.  
        //Now, say aggregate size is 7.  We will have 20 / 7 = 2 6/7, rounded up = 3 
        //phrases of lengths 7, 7, and 6.  So, in our for loop, we will iterate 
        //Count/AggregateSize rounded up number of times.

        var words = lineText.ParseIntoWords();
        var wordCount = words.Count;
        var aggregateCount = (wordCount - 1) / aggregateSize + 1; //equivalent to Count/AggregateSize rounded up

        //IF AGGREGATE COUNT == 1, THEN OUR ENTIRE LINE IS OUR AGGREGATE PHRASE TEXT
        if (aggregateCount == 1)
        {
          AggregatePhraseTexts.Add(lineText);
          return;
        }

        //our first aggregate phrases up to the very last one will be of size aggregate size
        //but our last one is the remainder and may contain anywhere from 1 to aggregateSize
        //words in it.  so we must be mindful of this.
        for (int i = 0; i < aggregateCount; i++)
        {
          var aggregatePhraseText = "";
          for (int j = 0; j < aggregateSize; j++)
          {
            var wordIndex = (i * aggregateSize) + j;
            if (wordIndex >= words.Count)
              break;//we have reached the end of the last phrase
            var currentWord = words[wordIndex];

            if (string.IsNullOrEmpty(aggregatePhraseText))
              aggregatePhraseText = currentWord;
            else
              aggregatePhraseText += " " + currentWord;
          }

          if (!string.IsNullOrEmpty(aggregatePhraseText))
            AggregatePhraseTexts.Insert(i, aggregatePhraseText);

          //remove any phrases that are empty.  this shouldn't happen, but we'll log it if we find one.
          var count = AggregatePhraseTexts.Count;
          //counting down from bottom of index because we will be removing items if needed.
          for (int k = count - 1; k >= 0; k--)
          {
            if (string.IsNullOrEmpty(AggregatePhraseTexts[k]))
            {
              Services.Log(StudyResources.WarningMsgEmptyAggregatePhraseTextFound,
                           LogPriority.Medium,
                           LogCategory.Warning);
              AggregatePhraseTexts.RemoveAt(k);
            }
          }
        }
        #region (...finally) Thinked
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(methodThinkId);
      }
        #endregion
    }

    /// <summary>
    /// Using this object's internal knowledge base, this will combine adjacent known aggregates 
    /// into their largest sizes possible.  It will iterate through the phrase texts a maximum of
    /// maxIterations.
    /// </summary>
    private void AggregateAdjacentKnownPhraseTexts(int maxIterations)
    {
      if (_AbortIsFlagged)
        return;
      
      //FIRST, GIVE US A FRESH START...I'M NOT ENTIRELY SURE THIS IS NECESSARY.
      PopulateAggregatePhraseTexts(AggregateSize);

      //IF AGGREGATE PHRASE TEXTS ONLY HAS ONE PHRASE, THEN WE DON'T HAVE ANY AGGREGATING TO DO
      if (AggregatePhraseTexts.Count == 1)
        return;

      //SET UP A LOOP THAT ITERATES THROUGH THE LIST OF AGGREGATE PHRASES
      //
      //var maxIterations = 100; //escape for our while loop
      var iterations = 0;
      var stillLooking = true;
      while (stillLooking && (iterations < maxIterations))
      {
        if (_AbortIsFlagged)
          return;
        
        //This is "starting" as in before our next for loop which may modify AggregatePhraseTexts.Count.
        var startingPhraseCount = AggregatePhraseTexts.Count;
        var foundNewAggregate = false;

        //We do this until count - 1, because we are indexing i and i+1.
        for (int i = 0; i < startingPhraseCount - 1; i++)
        {
          if (_AbortIsFlagged)
            return;
          
          var phraseA = AggregatePhraseTexts[i];
          var phraseB = AggregatePhraseTexts[i + 1];
          if (IsPhraseKnown(phraseA) && IsPhraseKnown(phraseB))
          {
            var newAggregate = phraseA + " " + phraseB;

            AggregatePhraseTexts[i] = newAggregate;
            if ((i + 1) < AggregatePhraseTexts.Count)
              AggregatePhraseTexts.RemoveAt(i + 1);
            foundNewAggregate = true;
            break;
          }
        }

        //IF WE FOUND A NEW AGGREGATE, THEN WE MAY YET FIND MORE IN ANOTHER ITERATION, 
        //SO WE ARE STILL LOOKING.
        if (foundNewAggregate)
          stillLooking = true;
        else
          //IF WE DIDN'T FIND A NEW AGGREGATE, THEN WE SEARCHED THE ENTIRE LIST
          //OF AGGREGATE PHRASE TEXTS AND NO ADJACENT PHRASE TEXTS WERE KNOWN.
          //SO, WE HAVE AGGREGATED AS MUCH AS WE CAN, AND WE ARE NO LONGER LOOKING.
          stillLooking = false;

        //ITERATION COUNTER FOR ESCAPING OUT OF THIS WHILE LOOP.
        iterations++;
      }

      //IF WE HAVE ONLY ONE AGGREGATE PHRASE TEXT, THEN THAT TEXT SHOULD BE OUR EXACT LINE TEXT.
      if (AggregatePhraseTexts.Count == 1)
      {
        AggregatePhraseTexts[0] = _Target.Phrase.Text;
      }
    }

    /// <summary>
    /// Searches within all KnownPhraseTexts for the given phraseText.  
    /// Returns true if known, else false.
    /// If you are asking about an aggregate phrase of which you know each of the pieces,
    /// BUT we have NOT marked the entire aggregate phrase as known, this will return false.
    /// Knowing the pieces is not necessarily knowing the whole.
    /// </summary>
    private bool IsPhraseKnown(string phraseText)
    {
      //FIRST CHECKS FOR ENTIRE PHRASE, THEN CONTAINMENT IN ANOTHER PHRASE.

      //ENTIRE PHRASE
      var isWholeTextKnown = (from knownPhraseText in KnownPhraseTexts
                              where knownPhraseText == phraseText
                              select knownPhraseText).Count() > 0;

      if (isWholeTextKnown)
        return true;

      //CONTAINED IN ANOTHER PHRASE
      var isKnownInAnotherPhrase = (from knownPhraseText in KnownPhraseTexts
                                    where knownPhraseText.Contains(phraseText)
                                    select knownPhraseText).Count() > 0;

      return isKnownInAnotherPhrase;
    }

    /// <summary>
    /// Marks this phraseText internally as known.  This does not mark any outside knowledge base.
    /// </summary>
    private void MarkPhraseKnown(string phraseText)
    {
      if (!IsPhraseKnown(phraseText))
        KnownPhraseTexts.Add(phraseText);
    }

    /// <summary>
    /// Marks this phraseText internally as UNknown.  This does not mark any outside knowledge base.
    /// </summary>
    private void MarkPhraseUnknown(string phraseText)
    {
      if (IsPhraseKnown(phraseText))
        KnownPhraseTexts.Remove(phraseText);
    }

    private static AutoResetEvent _AutoResetEvent = new AutoResetEvent(false);

    /// <summary>
    /// Calculates the PercentKnown of this line.
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetPercentKnownAsync()
    {
      if (_Target == null)
        return 0;// await new Task<double>(() => { return 0; });

      if (_PercentKnownIsUpToDate)
        return _CachedPercentKnownValue;

      var questionArgs =
        new Common.QuestionArgs(StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown, 
                                _Target.Phrase);
      
      var result = await DefaultPhrasePercentKnownAdvisor.Ton.AskAdviceAsync(questionArgs);

      _PercentKnownTaskVariable = (double)result.Object;

      _CachedPercentKnownValue = _PercentKnownTaskVariable;
      _PercentKnownIsUpToDate = true;
      return _PercentKnownTaskVariable;
    }

    private bool _PercentKnownIsUpToDate = false;
    private double _CachedPercentKnownValue = 0;

    private double _PercentKnownTaskVariable { get; set; }

    public async Task UpdateKnowledge()
    {
      //ASK ADVICE
      var questionArgs =
        new Common.QuestionArgs(StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown, _Target.Phrase);
      var result = await DefaultPhrasePercentKnownAdvisor.Ton.AskAdviceAsync(questionArgs);
     
      //INTERPRET ADVICE
      _CachedPercentKnownValue = (double)result.Object;
      _PercentKnownIsUpToDate = true;
      if (_CachedPercentKnownValue >= KnowledgeThreshold)
        MarkPhraseKnown(_Target.Phrase.Text);
      else
        MarkPhraseUnknown(_Target.Phrase.Text);
    }

    public void Handle(History.Events.ReviewedPhraseEvent message)
    {
      string phraseText = message.GetDetail<string>(History.HistoryResources.Key_PhraseText);
      double feedbackAsDouble = message.GetDetail<double>(History.HistoryResources.Key_FeedbackAsDouble);

      //IF OUR REVIEWED PHRASE IS RELATED TO OUR TARGET'S PHRASE TEXT, THEN OUR PERCENT KNOWN IS NOW OUT OF DATE.
      if (PhraseIsRelatedToLineText(phraseText))
        _PercentKnownIsUpToDate = false;

      if (feedbackAsDouble > KnowledgeThreshold)
        MarkPhraseKnown(phraseText);
      else
        MarkPhraseUnknown(phraseText);
    }

    private bool PhraseIsRelatedToLineText(string phraseText)
    {
      if (_Target.Phrase.Text.Contains(phraseText))
        return true;

      //check to see if any words of sufficient length are contained in this line text.  if so, then it's related.
      var words = phraseText.ParseIntoWords();
      List<string> wordsMoreThanTwoLettersLong = new List<string>();
      foreach (var word in words)
      {
        if (word.Length > 2)
          wordsMoreThanTwoLettersLong.Add(word);
      }

      foreach (var bigWord in wordsMoreThanTwoLettersLong)
      {
        if (_Target.Phrase.Text.Contains(bigWord))
          return true;
      }

      //IF WE MADE IT HERE, THEN WE FOUND NO RELATED WORDS
      return false;
    }

    public void Handle(NavigationRequestedEventMessage message)
    {
      AbortStudying();
    }

      #endregion

    private void AbortStudying()
    {
      _AbortIsFlagged = true;
    }

    private object _AbortLock = new object();
    private bool _abortIsFlagged = false;
    private bool _AbortIsFlagged
    {
      get
      {
        lock (_AbortLock)
        {
          return _abortIsFlagged;
        }
      }
      set
      {
        lock (_AbortLock)
        {
          _abortIsFlagged = value;
        }
      }
    }

  }
}
