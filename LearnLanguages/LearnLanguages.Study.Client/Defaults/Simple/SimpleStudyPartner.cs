using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.Composition;
using System.Reflection;

using LearnLanguages.Business;
using LearnLanguages.Common;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.History.Events;
using LearnLanguages.Navigation.EventMessages;
using LearnLanguages.Offer;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.EventMessages;

using Caliburn.Micro;
using Csla.Core;
using System.Collections.Generic;

namespace LearnLanguages.Study.Defaults.Simple
{
  [Export(typeof(IStudyPartner))]
  public class SimpleStudyPartner : MultiLineTextListStudyPartnerBase,
                                    IHandle<ReviewedPhraseEvent>
  {

    #region Ctors & Init

    public SimpleStudyPartner()
    {
      _ReviewedLineRecorder = new DefaultReviewedLineRecorder();
      _ReviewedLineRecorder.IsEnabled = true;
      _ReviewedPhraseRecorder = new ReviewedPhraseRecorder();
      _ReviewedPhraseRecorder.IsEnabled = true;
      _ReviewedWordInPhraseRecorder = new DefaultReviewedWordInPhraseRecorder();
      _ReviewedWordInPhraseRecorder.IsEnabled = false;
      //_PhraseAutoTranslatedRecorder = new DefaultPhraseAutoTranslatedRecorder();
      //_PhraseAutoTranslatedRecorder.IsEnabled = true;
      History.HistoryPublisher.Ton.SubscribeToEvents(this);
    }

    #endregion
    
    #region Recorders

    private DefaultReviewedLineRecorder _ReviewedLineRecorder { get; set; }
    private ReviewedPhraseRecorder _ReviewedPhraseRecorder { get; set; }
    private DefaultReviewedWordInPhraseRecorder _ReviewedWordInPhraseRecorder { get; set; }
    //private DefaultPhraseAutoTranslatedRecorder _PhraseAutoTranslatedRecorder { get; set; }

    #endregion

    #region Properties & Fields
    
    private string _CurrentUserNativeLanguageText { get; set; }
    private PhraseBeliefList _AllPhraseBeliefsCache { get; set; }
    private PhraseBeliefList _MostRecentPhraseBeliefsCache { get; set; }
    /// <summary>
    /// key = phraseText
    /// Value.Item1 = languageText
    /// value.Item2 = belief strength
    /// This is a temporary measure to address storing recently reviewed
    /// phrases. This study partner gets a list of the most recent
    /// beliefs about all of the current user's phrases. Any phrases that
    /// are reviewed after this retrieval will create pseudo beliefs stored
    /// in here. This means I'm just keeping track of a phrase text (string)
    /// to a Tuple composed of the 
    /// {languageText (string), belief strength (double)}.
    /// </summary>
    private MobileDictionary<string, Tuple<string, double>> _PseudoBeliefsCache { get; set; }

    #endregion

    #region Methods

    protected override async Task InitializeForNewStudySessionAsync(
      MultiLineTextList multiLineTexts)
    {
      #region DisableNav/Thinking
      DisableNavigationRequestedEventMessage.Publish();
      var targetId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      #endregion
      try
      {
        #region ThinkPing
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
        #endregion
        var retrieverStudyData = await Business.StudyDataRetriever.CreateNewAsync();
        _CurrentUserNativeLanguageText = retrieverStudyData.StudyData.NativeLanguageText;
        #region ThinkPing
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
        #endregion
        var retrieverRecentBeliefs = await MostRecentPhraseDataBeliefsRetriever.CreateNewAsync();
        _MostRecentPhraseBeliefsCache = retrieverRecentBeliefs.MostRecentPhraseBeliefs;
        #region ThinkPing
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
        #endregion
      }
      catch (Exception ex)
      {
        Services.PublishMessageEvent(string.Format(StudyResources.ErrorMsgInitializeForNewStudySession, ex.Message), 
                                     MessagePriority.High, 
                                     MessageType.Error);
      }
      finally
      {
        #region EnableNav/Thinked
        EnableNavigationRequestedEventMessage.Publish();
        History.Events.ThinkedAboutTargetEvent.Publish(targetId);
        #endregion
      }
    }

    protected override async Task<StudyItemViewModelBase> GetNextStudyItemViewModelAsync()
    {
      StudyItemViewModelBase qaViewModel = null;
      PhraseEdit phraseToStudy = null;

      DisableNavigationRequestedEventMessage.Publish();
      try
      {
        ///THINGS THAT MUST HAPPEN:
        ///CHOOSE A PHRASE/LINE TO STUDY FROM THE TARGET
        ///GET A VIEWMODEL FOR THAT PHRASE/LINE

        #region Thinking (try..)
        var targetId = Guid.NewGuid();
        History.Events.ThinkingAboutTargetEvent.Publish(targetId);
        try
        {
        #endregion
          ///SO, THE SIMPLEST FORM OF CHOOSING A PHRASE/LINE TO STUDY IS JUST GETTING
          ///CHOOSING FROM THE MLT LIST AT RANDOM.
          //PhraseEdit phraseToStudy = GetRandomPhrase(GetCurrentTarget());
          phraseToStudy = await GetRandomUnknownPhrase(GetCurrentTarget());
          
        #region (...finally) Thinked
        }
        catch (Exception ex)
        {
          Services.PublishMessageEvent(string.Format(StudyResources.ErrorMsgGetNextStudyItemViewModel, ex.Message), 
                                       MessagePriority.High,
                                       MessageType.Error);
        }
        finally
        {
          History.Events.ThinkedAboutTargetEvent.Publish(targetId);
        }
        #endregion

        if (phraseToStudy == null)
          phraseToStudy = GetRandomPhrase(GetCurrentTarget());

        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
        {
          qaViewModel = await StudyItemViewModelFactory.Ton.ProcureAsync(phraseToStudy,
            _CurrentUserNativeLanguageText);
        }
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);

        //PUBLISH A REVIEWING PHRASE EVENT MESSAGE.
        History.Events.ReviewingPhraseEvent reviewingEvent =
          new ReviewingPhraseEvent(phraseToStudy, qaViewModel.ReviewMethodId);
        History.HistoryPublisher.Ton.PublishEvent(reviewingEvent);
      }
      catch (Exception ex)
      {
        Services.PublishMessageEvent(string.Format(StudyResources.ErrorMsgGetNextStudyItemViewModel, ex.Message),
                                     MessagePriority.High,
                                     MessageType.Error);
      }
      finally
      {
        EnableNavigationRequestedEventMessage.Publish();
      }

      return qaViewModel;
    }

    /// <summary>
    /// Simply gets the phrase from a random line from any of the MultiLineTexts 
    /// in the given multiLineTextList parameter.
    /// </summary>
    /// <returns></returns>
    private PhraseEdit GetRandomPhrase(MultiLineTextList multiLineTextList)
    {
      var relevantBeliefs = 
        PhraseBeliefList.GetBeliefsAboutPhrasesInMultiLineTextsAsync(multiLineTextList);

      var randomMlt = RandomPicker.Ton.PickOne<MultiLineTextEdit>(multiLineTextList);
      var randomLine = RandomPicker.Ton.PickOne<LineEdit>(randomMlt.Lines);
      return randomLine.Phrase;
    }

    /// <summary>
    /// Gets the most recent phrase beliefs for all of the current user's phrases.
    /// </summary>
    /// <returns></returns>
    private async Task<PhraseEdit> GetRandomUnknownPhrase(MultiLineTextList multiLineTextList)
    {
      History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);

      if (_MostRecentPhraseBeliefsCache == null)
        throw new Exception("MostRecentPhraseBeliefsCache should not be null");
      if (_PseudoBeliefsCache == null)
        _PseudoBeliefsCache = new MobileDictionary<string,Tuple<string,double>>();

      PhraseEdit retPhrase = null;

      //GET OUR UNKNOWNS OUT OF OUR CACHES
      var threshold = double.Parse(StudyResources.DefaultKnowledgeThreshold);
      ////debug
      //var debug = _MostRecentPhraseBeliefsCache.First();
      //try
      //{
      //  PhraseEdit debugPhrase = debug.Phrase;
      //}
      //catch (Exception ex)
      //{
      //  System.Windows.MessageBox.Show(ex.Message);
      //}
      ////debug
      try
      {
        var unknownBeliefsFromCache = (from belief in _MostRecentPhraseBeliefsCache
                                       where belief.Strength < threshold &&
                                             PhraseTextIsInMultiLineTextList(belief.Phrase.Text, multiLineTextList)
                                       select belief).ToList();
        var unknownPseudoBeliefsFromCache = (from entry in _PseudoBeliefsCache
                                             where entry.Value.Item2 < threshold &&
                                                   PhraseTextIsInMultiLineTextList(entry.Key, multiLineTextList)
                                             select entry).ToList();

      //GET OUR UNKNOWN PHRASES THAT DONT HAVE A BELIEF OR PSUEDO BELIEF REGISTERED WITH THEM YET
      var unregisteredUnknownPhrases = GetUnregisteredUnknownPhraseBeliefs(multiLineTextList, 
                                                                           unknownBeliefsFromCache, 
                                                                           unknownPseudoBeliefsFromCache);

      var indexToPick = -1;
      //TOTAL COUNT = MOST RECENT PHRASE BELIEFS + PSEUDO BELIEFS COUNT + PHRASES WITHOUT BELIEFS
      var totalCountUnknown = 0;
      totalCountUnknown += unknownBeliefsFromCache.Count;
      if (_PseudoBeliefsCache != null)
        totalCountUnknown += unknownPseudoBeliefsFromCache.Count;
      totalCountUnknown += unregisteredUnknownPhrases.Count;

      if (totalCountUnknown == 0)
      {
        //EVERYTHING IS KNOWN, SO RETURN NULL
        return null;
      }

      //IF WE TRY TO PICK FROM THE PSEUDO CACHE, THEN IT IS POSSIBLE
      //THAT OUR PHRASETEXT NO LONGER MATCHES A PHRASE. THEREFORE, WE
      //SET UP A LOOP TO KEEP TRYING TO PICK A RANDOM PHRASE FROM THE
      //ENTIRE LIST OF BOTH CACHES. IF WE FAIL TO DO THIS, THEN WE WILL
      //PICK FROM JUST THE ACTUAL BELIEF CACHE (NOT THE PSEUDO).
      var maxTries = int.Parse(StudyResources.MaxTriesToPickRandomFromEntireList);
      for (int i = 0; i < maxTries; i++)
      {
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);

        indexToPick = RandomPicker.Ton.NextInt(0, totalCountUnknown);

        if (indexToPick < unknownBeliefsFromCache.Count)
        {
          //WE PICK FROM THE PHRASE BELIEF CACHE
          var belief = unknownBeliefsFromCache[indexToPick];
          return belief.Phrase;
        }
        else if (indexToPick < (unknownBeliefsFromCache.Count + unknownPseudoBeliefsFromCache.Count))
        {
          #region //WE _TRY_ TO PICK FROM THE PSUEDO CACHE
          indexToPick -= unknownBeliefsFromCache.Count;
          var beliefEntry = unknownPseudoBeliefsFromCache[indexToPick];
          var phraseText = beliefEntry.Key;
          var languageText = beliefEntry.Value.Item1;
          #endregion
          var phrase = await PhraseEdit.NewPhraseEditAsync(languageText);
          phrase.Text = phraseText;
          var phraseId = Guid.NewGuid();
          phrase.Id = phraseId;
          var retriever = await PhrasesByTextAndLanguageRetriever.CreateNewAsync(phrase);
          retPhrase = retriever.RetrievedSinglePhrase;
          if (retPhrase != null)
            //WE HAVE FOUND A RETURN PHRASE, SO BREAK OUT OF OUR ATTEMPT LOOP
            return retPhrase;
        }
        else
        {
          //WE PICK FROM THE UNKNOWN PHRASES WITHOUT BELIEFS I'M CONVERTING THIS 
          //TO LIST THIS LATE BECAUSE WE DON'T NEED IT AS A LIST UNTIL NOW.
          var asList = unregisteredUnknownPhrases.ToList();
          indexToPick -= (unknownBeliefsFromCache.Count + unknownPseudoBeliefsFromCache.Count);
          retPhrase = unregisteredUnknownPhrases[indexToPick];
          return retPhrase;
        }
      }
      

      //IF WE'VE GOTTEN THIS FAR, THEN WE COULDN'T FIND AN UNKNOWN PHRASE
      Services.PublishMessageEvent("Couldn't retrieve unknown phrase.", MessagePriority.Medium, MessageType.Warning);
      return null;
      }
      catch (Exception ex)
      {
        Services.PublishMessageEvent(string.Format(StudyResources.ErrorMsgGetRandomUnknownPhrase, ex.Message),
                                     MessagePriority.High,
                                     MessageType.Error);
        return null;
      }
    }

    /// <summary>
    /// Unregistered means not registered in either beliefs from cache or pseudo beliefs from cache.
    /// </summary>
    /// <param name="multiLineTextList"></param>
    /// <param name="unknownBeliefsFromCache"></param>
    /// <param name="unknownPseudoBeliefsFromCache"></param>
    /// <returns></returns>
    private List<PhraseEdit> GetUnregisteredUnknownPhraseBeliefs(MultiLineTextList multiLineTextList, 
      List<PhraseBeliefEdit> unknownBeliefsFromCache, List<KeyValuePair<string, Tuple<string, double>>> unknownPseudoBeliefsFromCache)
    {
      List<PhraseEdit> retPhrases = new List<PhraseEdit>();
      foreach (var mlt in multiLineTextList)
      {
        var phrasesInNeitherCache = from line in mlt.Lines
                                    where //LINE IS NOT IN EITHER CACHE (THUS EXCLUDING ASSOCIATED UNKNOWN BELIEFS)
                                          (!(from keyIsPhraseText in unknownPseudoBeliefsFromCache
                                             select keyIsPhraseText.Key).Union(
                                             from belief in unknownBeliefsFromCache
                                             select belief.Phrase.Text).Contains(line.Phrase.Text))
                                             
                                             &&

                                          //AND PHRASE HAS NO ASSOCIATED BELIEF (THUS EXCLUDING KNOWN BELIEFS)
                                          (!(from keyIsPhraseText in _PseudoBeliefsCache
                                             select keyIsPhraseText.Key).Union(
                                             from belief in _MostRecentPhraseBeliefsCache
                                             select belief.Phrase.Text).Contains(line.Phrase.Text))
                                    select line.Phrase;
        retPhrases.AddRange(phrasesInNeitherCache);
      }

      return retPhrases;
    }

    private bool PhraseTextIsInMultiLineTextList(string phraseText, MultiLineTextList multiLineTextList)
    {
      var exists = (from mlt in multiLineTextList
                    where (from line in mlt.Lines
                           where line.Phrase.Text == phraseText
                           select line).Count() > 0
                    select mlt).Count() > 0;
      return exists;
    }

    public void Handle(ReviewedPhraseEvent message)
    {
      History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);

      //GET RELEVANT INFO FROM THE EVENT MESSAGE
      var phraseId = message.GetDetail<Guid>(History.HistoryResources.Key_PhraseId);
      var phraseText = message.GetDetail<string>(History.HistoryResources.Key_PhraseText);
      var languageText = message.GetDetail<string>(History.HistoryResources.Key_LanguageText);
      var feedbackAsDouble = message.GetDetail<double>(History.HistoryResources.Key_FeedbackAsDouble);

      //WE ONLY WANT FOREIGN PHRASES
      if (languageText.ToLower() == _CurrentUserNativeLanguageText.ToLower())
        return;

      //CHECK TO SEE IF WE ALREADY HAVE A BELIEF ABOUT THIS PHRASE
      var cachedBelief = (from belief in _MostRecentPhraseBeliefsCache
                          where belief.PhraseId == phraseId ||
                                belief.Phrase.Text == phraseText
                          select belief).FirstOrDefault();

      if (cachedBelief != null)
      {
        //WE HAVE A CACHED BELIEF FOR THIS PHRASE ALREADY
        //SO JUST REPLACE THE STRENGTH WITH THE NEW ONE, BUT
        //DO *NOT* SAVE THE BELIEF. THIS IS JUST A CACHE.
        cachedBelief.Strength = feedbackAsDouble;
      }
      else
      {
        if (_PseudoBeliefsCache == null)
          _PseudoBeliefsCache = new MobileDictionary<string, Tuple<string, double>>();
        var languageAndFeedbackTuple = new Tuple<string, double>() { Item1 = languageText, Item2 = feedbackAsDouble };

        //WE DO *NOT* HAVE A CACHED BELIEF FOR THIS PHRASE ALREADY
        //BUT WE NEED TO CHECK IF WE ALREADY HAVE A PSEUDO BELIEF
        //CACHED FIRST.
        if (_PseudoBeliefsCache.ContainsKey(phraseText))
        {
          //WE'VE REVIEWED THIS PHRASE BEFORE, SO REPLACE THE 
          //BELIEF STRENGTH
          _PseudoBeliefsCache[phraseText] = languageAndFeedbackTuple;
        }
        else
        {
          //THIS IS THE FIRST TIME WE'VE REVIEWED THE PHRASE
          _PseudoBeliefsCache.Add(phraseText, languageAndFeedbackTuple);
        }
      }
    }
  
    #endregion

  }
}
