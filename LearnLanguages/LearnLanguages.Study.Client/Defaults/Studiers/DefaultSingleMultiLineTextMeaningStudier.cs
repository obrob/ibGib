using System;
using System.Linq;
using LearnLanguages.Business;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using Caliburn.Micro;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Navigation.EventMessages;
using System.Threading.Tasks;
using LearnLanguages.Common;

namespace LearnLanguages.Study
{
  /// <summary>
  /// This studies the meaning of a single MultiLineTextEdit object.  It utilizes a LineMeaningStudier
  /// for each line in the MLT.  It uses its knowledge to determine the active number of lines to study.  
  /// Once it has this number, it cycles through the lines, studying each one until known, then de-activates
  /// that line, and activates another.    Once all lines are known, then StudyAgain() will return false.
  /// 
  /// **It does NOT aggregate Lines currently, though I should refactor the concept of a line into an 
  /// OrderedPhrase, but I'll have to do that some other time.  Once that happens, we can think of all 
  /// "MultiLineTexts" as "OrderedTexts", which is what all phrases are really anyway, with individual words
  /// as phrase with only one text to order.  I digress.  This should more than suffice for now.
  /// 
  /// **Also, the concept of active lines is not necessary as well.  This is a concession I'm making 
  /// for a strategy for choosing the next LineStudier.  Again, this would be better with an OrderedText,
  /// but the general idea is that choosing a line to study should be abstracted and the choice should be 
  /// reliant upon other factors, like when each was studied last, how well they are known, and 
  /// a whole slew of other factors...perfect for a neural net to figure out.  Alas, for now it must 
  /// just be active lines.
  /// </summary>
  public class DefaultSingleMultiLineTextMeaningStudier : StudierBase<MultiLineTextEdit>,
                                                          IHandle<NavigationRequestedEventMessage>
  {
    #region Ctors and Init
    
    public DefaultSingleMultiLineTextMeaningStudier()
      : base()
    {
      _LineStudiers = new Dictionary<int, DefaultLineMeaningStudier>();
      _LastActiveLineStudiedIndex = -1;
      _KnowledgeThreshold = double.Parse(StudyResources.DefaultKnowledgeThreshold);
      Services.EventAggregator.Subscribe(this);//navigation
    }

    public DefaultSingleMultiLineTextMeaningStudier(int startingAggregateSize)
      : this()
    {
      _AggregateSize = startingAggregateSize;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets and Sets the aggregate size for phrases in units of words per phrase.  So, 
    /// if AggregateSize == 2, then a phrase "the quick brown fox jumped over the moon"
    /// is parsed into "the quick" "brown fox" "jumped over" "the moon".  If 3, "the quick brown"
    /// "fox jumped over" "the moon", and so forth.  I may end up adding some noise to this, so it 
    /// will be something like 2 2 2 1 2 3 or something, but for now it's just this size or less 
    /// (as in the second example of 3, where the last phrase is just "the moon" (2).
    /// </summary>
    private int _AggregateSize { get; set; }

    /// <summary>
    /// How many lines to cycle through while studying.  Much like a buffer.  When a line is learned,
    /// then it gets removed from the ActiveLines and a different line replaces it.
    /// </summary>
    private int _ActiveLinesCount { get; set; }

    private int _LastActiveLineStudiedIndex { get; set; }

    private double _KnowledgeThreshold { get; set; }

    /// <summary>
    /// LineStudiers indexed by each's corresponding line number.
    /// </summary>
    private Dictionary<int, DefaultLineMeaningStudier> _LineStudiers { get; set; }
    
    #endregion

    #region Methods

    private DefaultLineMeaningStudier ChooseNextLineStudier(out int nextLineNumber)
    {
      List<int> unknownLineNumbers = new List<int>();
      nextLineNumber = -1;

      foreach (var studier in _LineStudiers)
      {
        if (_AbortIsFlagged)
          return null;

        var studierLineNumber = studier.Key;
        var task = studier.Value.GetPercentKnownAsync();
        var timeoutInMs = int.Parse(StudyResources.DefaultTimeoutGetPercentKnown);
        if (task.Wait(timeoutInMs))
        {
          var studierPercentKnown = task.Result;
          if (studierPercentKnown > this._KnowledgeThreshold)
          {
            //LINE IS KNOWN
            continue;
          }
          else
          {
            //LINE IS UNKNOWN
            unknownLineNumbers.Add(studierLineNumber);
            ////if line is lowest so far, then update lowestsofar to this line number
            //if (studierLineNumber < lowestSoFar)
            //  lowestSoFar = studierLineNumber;
          }
        }
        else
        {
          //TIMEOUT WAS REACHED. NEED TO ASSUME THIS WILL BE REACHED SOMETIMES
          //NEED TO DECIDE HOW THIS IS HANDLED.
          Services.PublishMessageEvent("GetPercentKnown timeout reached.", 
            MessagePriority.High, MessageType.Error);
          
          //i'm throwing this just temporarily. I"m not sure how to handle this situation
          throw new Exception();
        }
      }

      if (_AbortIsFlagged)
        return null;

      //ALL LINES ARE KNOWN IF COUNT == 0, SO PICK A LINE AT RANDOM
      if (unknownLineNumbers.Count == 0)
      {
        int randomLineNumber = Common.RandomPicker.Ton.NextInt(0, _Target.Lines.Count);
        nextLineNumber = randomLineNumber;
        var studier = _LineStudiers[nextLineNumber];
        return studier;
      }

      //THIS IS DIFFICULT, BECAUSE WE'RE HANDLING INDEXES OF INDEXES.
      //WE ARE LOOKING FOR THE LINE NUMBER OF THE NEXT STUDIER (THE INDEX FOR THAT STUDIER).
      //THIS LINE NUMBER IS IN THE UNKNOWN_LINE_NUMBERS, WHICH WE REFERENCE BY USING _*THAT*_
      //LINE NUMBER'S INDEX.  IF THAT INDEX IS GREATER THAN OUR ACTIVE LINE COUNT (WE CANNOT STUDY
      //MORE THAN ACTIVE_LINE_COUNT LINES AT A TIME)
      unknownLineNumbers.Sort();
      var nextLineNumberIndex = _LastActiveLineStudiedIndex + 1;
      if (nextLineNumberIndex >= unknownLineNumbers.Count ||
          nextLineNumberIndex >= _ActiveLinesCount)
        nextLineNumberIndex = 0;

      if (_AbortIsFlagged)
        return null;

      nextLineNumber = unknownLineNumbers[nextLineNumberIndex];
      var nextStudierToUse = _LineStudiers[nextLineNumber];

      return nextStudierToUse;
    }

    private void UpdateKnowledge()
    {
      if (_AbortIsFlagged)
        return;
      
      //todo: MLTMeaning...dynamically set active lines count from either some sort of calculator class
      _ActiveLinesCount = int.Parse(StudyResources.DefaultMeaningStudierActiveLinesCount);

      //todo: MLTMeaning...dynamically set knowledge threshold to user's specifications or other.
      _KnowledgeThreshold = double.Parse(StudyResources.DefaultKnowledgeThreshold);
    }

    protected void ChooseAggregateSize()
    {

      if (_AbortIsFlagged)
        return;

      //todo: MLTMeaning...dynamically choose aggregate size
      _AggregateSize = int.Parse(StudyResources.DefaultMeaningStudierAggregateSize);
    }

    protected virtual async Task PopulateLineStudiersAsync()
    {
      _LineStudiers.Clear();
      foreach (var line in _Target.Lines)
      {
        if (_AbortIsFlagged)
          return;
        
        var lineStudier = new DefaultLineMeaningStudier();
        _LineStudiers.Add(line.LineNumber, lineStudier);
      }

      //WE NOW HAVE LINE STUDIERS POPULATED WITH UNINITIALIZED LINE STUDIERS.
      //WE WILL NOW INITIALIZE EACH LINE STUDIER
      int linesInitializedCount = 0;
      foreach (var lineStudierEntry in _LineStudiers)
      {
        var line = _Target.Lines[lineStudierEntry.Key];
        if (_AbortIsFlagged)
          return;

        await lineStudierEntry.Value.InitializeForNewStudySessionAsync(line);
        linesInitializedCount++;

        if (_AbortIsFlagged)
          return;

        //IF WE HAVE INITIALIZED ALL OF OUR LINES, THEN OUR INITIALIZATION 
        //POPULATION IS COMPLETE
        if (linesInitializedCount == _Target.Lines.Count)
          return;
      }
    }

    /// <summary>
    /// Calculates the percent known of all lines of the MLT study target.
    /// </summary>
    /// <returns></returns>
    public double GetPercentKnown()
    {
      var lineCount = _Target.Lines.Count;
      double totalPercentKnownNonNormalized = 0.0d;
      double maxPercentKnownNonNormalized = lineCount;
      foreach (var lineInfo in _LineStudiers)
      {
        if (_AbortIsFlagged)
          return -1.0d;

        var task = lineInfo.Value.GetPercentKnownAsync();
        var timeoutInMs = int.Parse(StudyResources.DefaultTimeoutGetPercentKnown);
        if (task.Wait(timeoutInMs))
        {
          var linePercentKnown = task.Result;
          totalPercentKnownNonNormalized += linePercentKnown;
        }
        else
        {
          //TIMEOUT WAS REACHED. NEED TO ASSUME THIS WILL BE REACHED SOMETIMES
          //NEED TO DECIDE HOW THIS IS HANDLED.
          Services.PublishMessageEvent("GetPercentKnown timeout reached.",
            MessagePriority.High, MessageType.Error);

          //i'm throwing this just temporarily. I"m not sure how to handle this situation
          throw new Exception();
        }
      }

      var totalPercentKnownNormalized = totalPercentKnownNonNormalized / maxPercentKnownNonNormalized;
      return totalPercentKnownNormalized;
    }
    
    #endregion

    public async override Task InitializeForNewStudySessionAsync(MultiLineTextEdit target)
    {
      //USE POSTSHARP AOP FOR THINKING EVENTS
      var methodThinkId = target.Id;
      History.Events.ThinkingAboutTargetEvent.Publish(methodThinkId);

      _AbortIsFlagged = false;
      _Target = target;
      _LastActiveLineStudiedIndex = -1;

      //ACTIVATE EACH LINE FOR USE IN HISTORY PUBLISHER
      foreach (var line in target.Lines)
      {
        var activatedLineEvent = new History.Events.ActivatedLineEvent(line);
        History.HistoryPublisher.Ton.PublishEvent(activatedLineEvent);
      }

      #region Abort Check
      if (_AbortIsFlagged)
      {
        History.Events.ThinkedAboutTargetEvent.Publish(methodThinkId);
        return;
      }
      #endregion

      //EXECUTES CALLBACK WHEN POPULATE IS COMPLETED
      await PopulateLineStudiersAsync();

      History.Events.ThinkedAboutTargetEvent.Publish(methodThinkId);
    }

    public async override Task<ResultArgs<StudyItemViewModelArgs>> GetNextStudyItemViewModelAsync()
    {
      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();
      
      UpdateKnowledge();
      ChooseAggregateSize();
      var nextLineNumber = -1;
      var nextStudier = ChooseNextLineStudier(out nextLineNumber);
      if (nextStudier == null || nextLineNumber < 0)
        throw new Exception("todo: all lines are studied, publish completion event or something");

      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();

      var nextLineEdit = _Target.Lines[nextLineNumber];
      if (nextLineEdit.LineNumber != nextLineNumber)
      {
        var results = from line in _Target.Lines
                      where line.LineNumber == nextLineNumber
                      select line;

        nextLineEdit = results.FirstOrDefault();
        if (nextLineEdit == null)
          throw new Exception("cannot find line with corresponding line number");
      }

      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();

      _LastActiveLineStudiedIndex = nextLineEdit.LineNumber;
      return await nextStudier.GetNextStudyItemViewModelAsync();
    }

    public void Handle(NavigationRequestedEventMessage message)
    {
      AbortStudying();
    }

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
