using System;
using System.Linq;
using System.Threading;

using LearnLanguages.Common;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Business;
using Caliburn.Micro;
using Csla.Core;
using LearnLanguages.History;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  /// <summary>
  /// This advisor expects a question StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown with 
  /// State == PhraseEdit, answer is type double between 0.0 and 1.0.
  /// </summary>
  public class DefaultPhrasePercentKnownAdvisor : IAdvisor, 
                                                  //IHandle<History.Events.ReviewedLineEvent>,
                                                  IHandle<History.Events.ReviewedPhraseEvent>
  {
    #region Singleton Pattern Members
    private static volatile DefaultPhrasePercentKnownAdvisor _Ton;
    private static object _Lock = new object();
    public static DefaultPhrasePercentKnownAdvisor Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new DefaultPhrasePercentKnownAdvisor();
          }
        }

        return _Ton;
      }
    }
    #endregion

    #region Ctors and Init

    public DefaultPhrasePercentKnownAdvisor()
    {
      Cache = new MobileDictionary<PhraseTextLanguageTextPair, double>();
      History.HistoryPublisher.Ton.SubscribeToEvents(this);
    }

    #endregion

    #region Properties

    public Guid Id { get { return Guid.Parse(StudyResources.AdvisorIdPhrasePercentKnownStudyAdvisor); } }

    private static AutoResetEvent _AutoResetEvent = new AutoResetEvent(false);
    private static Dictionary<Guid, AutoResetEvent> _AutoResetEvents = new Dictionary<Guid, AutoResetEvent>();
    private PhraseEdit _CurrentWordPhrase { get; set; }
    private int _CountWordsKnown { get; set; }

    private class PhraseTextLanguageTextPair
    {
      public PhraseTextLanguageTextPair(string phraseText, string languageText)
      {
        PhraseText = phraseText;
        LanguageText = languageText;
      }

      public string PhraseText { get; set; }
      public string LanguageText { get; set; }
    }
    private MobileDictionary<PhraseTextLanguageTextPair, double> Cache { get; set; }

    #endregion

    #region Methods

    public async Task<ResultArgs<object>> AskAdviceAsync(QuestionArgs questionArgs)
    {
      if (questionArgs == null)
        throw new ArgumentNullException("questionArgs");

      //WE ONLY KNOW HOW TO DEAL WITH PHRASE EDIT QUESTIONS ABOUT PERCENT KNOWN.
      if (!(questionArgs.State is PhraseEdit) ||
            questionArgs.Question != StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown)
        return await new Task<ResultArgs<object>>(() => 
        {
          var errorMsg = string.Format("Unknown question for advice.\n" +
                                       "questionArgs.State: {0}\n" +
                                       "questionArgs.Question: {1}", 
                                       questionArgs.State.ToString(), questionArgs.Question);
          var error = new Study.StudyException(errorMsg);
          return new ResultArgs<object>(error);
        });

      var phrase = (PhraseEdit)questionArgs.State;

      var result = await GetPercentKnownAsync(phrase);
      //THE RESULT IS CASTED ALREADY AS A DOUBLE. WE NEED TO WRAP IT AS A DOUBLE
      var percentKnownObjectWrapper = new ResultArgs<object>(result.Object);
      

      return await StudyHelper.WrapInTask<ResultArgs<object>>(percentKnownObjectWrapper);
      //var answerTask = new Task<ResultArgs<object>>(() => { return new ResultArgs<object>(percentKnown); });
      //return await answerTask;
    }

    private async Task<ResultArgs<double>> GetPercentKnownAboutPhraseWithBeliefs(PhraseEdit phrase,
                                                                                 PhraseBeliefList beliefs)
    {
//#if DEBUG
//        if (phrase.Text.Contains("amour") && phrase.Text.Contains("mon"))
//          System.Diagnostics.Debugger.Break();
//#endif
        if (beliefs.Count < 1)
          throw new ArgumentException("beliefs.Count < 1. This method assumes that there is at least one belief about the phrase \n\n" +
                                      "Phrase.Text: " + phrase.Text);

        ///FOR NOW, THIS JUST GETS THE MOST RECENT BELIEF AND GOES WITH THE STRENGTH OF THAT BELIEF.
        ///THIS IS ABSOLUTELY NOT HOW IT SHOULD BE BUT THAT IS WHY WE HAVE A "DEFAULT" ADVISOR AND
        ///EXTENSIBILITY FOR THIS.

        var results = from belief in beliefs
                      orderby belief.TimeStamp
                      select belief;

        var mostRecentBelief = results.Last();
        var percentKnown = mostRecentBelief.Strength;
        return await new Task<ResultArgs<double>>(() => { return new ResultArgs<double>(percentKnown); });
      
    }

    //public void Handle(History.Events.ReviewedLineEvent message)
    //{
    //  //THIS MESSAGE IS GOING TO BE RECORDED BY A DIFFERENT RECORDER OBJECT, BUT 
    //  //WE WANT TO PUT IT INTO OUR CACHE FOR QUICK ADVISING

    //  //LINETEXT
    //  string lineText = "";
    //  var messageHasLineText = message.TryGetDetail<string>(History.HistoryResources.Key_LineText, out lineText);
    //  if (!messageHasLineText)
    //    return;

    //  //LANGUAGE TEXT
    //  string languageText = "";
    //  var messageHasLanguageText =
    //    message.TryGetDetail<string>(History.HistoryResources.Key_LanguageText, out languageText);
    //  if (!messageHasLanguageText)
    //    return;

    //  //FEEDBACK
    //  double feedbackAsDouble = -1;
    //  var messageHasFeedbackAsDouble =
    //    message.TryGetDetail<double>(History.HistoryResources.Key_FeedbackAsDouble, out feedbackAsDouble);
    //  if (!messageHasFeedbackAsDouble)
    //    return;

    //  //ADD/REPLACE ENTRY
    //  var keyPhraseInfo = new PhraseTextLanguageTextPair(lineText, languageText);
    //  var containsLinePhrase = Cache.ContainsKey(keyPhraseInfo);
    //  if (!containsLinePhrase)
    //    Cache.Add(keyPhraseInfo, feedbackAsDouble);
    //  else
    //    Cache[keyPhraseInfo] = feedbackAsDouble;
    //}

    public void Handle(History.Events.ReviewedPhraseEvent message)
    {
      var phraseText = message.GetDetail<string>(HistoryResources.Key_PhraseText);
      var languageText = message.GetDetail<string>(HistoryResources.Key_LanguageText);
      var feedbackAsDouble = message.GetDetail<double>(HistoryResources.Key_FeedbackAsDouble);

      var results = from entry in Cache
                    where entry.Key.PhraseText == phraseText &&
                          entry.Key.LanguageText == languageText
                    select entry;

      if (results.Count() == 1)
      {
        //WE ARE REPLACING EXISTING PHRASE/LANGUAGE ENTRY'S FEEDBACK WITH MOST RECENT SCORE
        var entryKey = results.First().Key;
        Cache[entryKey] = feedbackAsDouble;
      }
      else
      {
        //WE ARE ADDING NEW PHRASE/LANGUAGE ENTRY WITH FEEDBACK SCORE
        var key = new PhraseTextLanguageTextPair(phraseText, languageText);
        Cache.Add(key, feedbackAsDouble);
      }
    }

    private async Task<ResultArgs<double>> GetPercentKnownAsync(PhraseEdit phrase)
    {
      #region CHECK CACHE

      var results = from entry in Cache
                    where entry.Key.PhraseText == phrase.Text &&
                          entry.Key.LanguageText == phrase.Language.Text
                    select entry;

      if (results.Count() == 1)
      {
        #region GIVE ANSWER FROM CACHE

        var entry = results.First();

        var percentKnown = entry.Value;
        return await new Task<ResultArgs<double>>(() => { return new ResultArgs<double>(percentKnown); });

        #endregion
      }

      #endregion

      #region CHECK BELIEFS IN DB

      var beliefs = await PhraseBeliefList.GetBeliefsAboutPhraseAsync(phrase.Id);

      if (beliefs.Count <= 0)
      {
        //WE HAVE NO PRIOR BELIEFS ABOUT THIS PHRASE
        var percentKnownTask = GetPercentKnownAboutPhraseWithNoPriorBeliefsAsync(phrase);
        return await percentKnownTask;
      }
      else
      {
        //WE HAVE BELIEFS ABOUT THIS PHRASE
        var percentKnownTask = GetPercentKnownAboutPhraseWithBeliefs(phrase, beliefs);
        return await percentKnownTask;
      }

      #endregion
    }

    private async Task<ResultArgs<double>> GetPercentKnownAboutPhraseWithNoPriorBeliefsAsync(PhraseEdit phrase)
    {
      //FOR RIGHT NOW, THIS IS JUST GOING TO RETURN ZERO. IT SUCKS BUT OH WELL.
      //TODO: IMPLEMENT GETPERCENTKNOWNABOUTPHRASEWITHNOPRIORBELIEFS
      double percentKnown = 0;
      var result = new ResultArgs<double>(percentKnown);
      return result;

      //return await StudyHelper.WrapInTask<double>(percentKnown);
      //return await new Task<ResultArgs<double>>(() => { return new ResultArgs<double>(percentKnown); });

//      //WE HAVE NO PRIOR BELIEFS ABOUT THIS PHRASE, SO WE WILL CHECK THE SUM OF THE BELIEFS OF THE INDIVIDUAL WORDS
//      var words = phrase.Text.ParseIntoWords();

//      #region ONLY ONE WORD IN PHRASE (RETURN WITH ZERO PERCENT KNOWN)
//      //IF WE ONLY HAVE A SINGLE WORD IN OUR PHRASE, THEN WE HAVE ALREADY CHECKED THIS SINGLE WORD FOR BELIEF 
//      //AND CAME UP EMPTY.  SO WE HAVE NO PERCENT KNOWN ABOUT THIS PHRASE.
//      if (words.Count < 2)
//      {
//        percentKnown = 0;
//        callback(this, new ResultArgs<double>(percentKnown));
//        return;
//      }
//      #endregion

//      #region MULTIPLE WORDS IN PHRASE (SUM PERCENT KNOWNS OF EACH WORD)

//      #region FIRST, GET THE PHRASES FOR EACH OF THE INDIVIDUAL WORDS
//      var phraseTextsCriteria = new Business.Criteria.PhraseTextsCriteria(phrase.Language.Text, words);
//      PhraseList.NewPhraseList(phraseTextsCriteria, (s, r) =>
//      {
//        //TODO: CHECK TO SEE WHAT HAPPENS IF WE DELETE A PHRASE THAT IS AN INDIVIDUAL WORD AND WE DO A FUNCTION (LIKE PERCENT KNOWN ABOUT PHRASE WITH NO PRIOR BELIEFS) THAT ASSUMES ALL INDIVIDUAL WORDS EXIST IN DATABASE.

//        if (r.Error != null)
//        {
//          callback(this, new ResultArgs<double>(r.Error));
//          return;
//        }

//        var wordPhrases = r.Object;

//        #region SECOND, CALL THIS ADVISOR'S GETPERCENTKNOWN RECURSIVELY FOR EACH INDIVIDUAL WORD'S PERCENT KNOWN

//        #region DECLARE ACTION THAT WILL USE WAITONE IN UPCOMING ASYNC FOR LOOP
//        Action<object> getPercentKnownWord = (object state) =>
//        {
//          //AutoResetEvent autoResetEvent = (AutoResetEvent)state;
//          Guid id = (Guid)state;
//          //IF THE ID IS NOT THERE, THEN THIS AUTORESETEVENT HAS ALREADY BEEN REMOVED FROM THE LIST
//          //IE, IT HAS TIMED OUT.
//          if (!_AutoResetEvents.ContainsKey(id))
//            return;

//          var autoResetEvent = _AutoResetEvents[(Guid)state];
////#if DEBUG
////          if (_CurrentWordPhrase.Text.Contains("Dans"))
////            System.Diagnostics.Debugger.Break();
////#endif 

//          #region PhrasePercentKnownAdvisor.Ton.GetPercentKnown(_CurrentWordPhrase, (s2, r2) =>
//          DefaultPhrasePercentKnownAdvisor.Ton.GetPercentKnown(_CurrentWordPhrase, (s2, r2) =>
//          {
//            if (r2.Error != null)
//            {
//              callback(this, new ResultArgs<double>(r2.Error));
//              return;
//            }

//            //WE ASSUME A SINGLE WORD IS EITHER KNOWN OR UNKNOWN.
//            if (r2.Object > 0)
//              _CountWordsKnown++;

//            autoResetEvent.Set();
//          });
//          #endregion
//        };

//        #endregion

//        #region EXECUTE ASYNC FOR LOOP EXECUTING ABOVE ACTION
//        _CountWordsKnown = 0;
//        var localAutoResetEvent = new AutoResetEvent(false);
//        var areID = Guid.NewGuid();
//        _AutoResetEvents.Add(areID, localAutoResetEvent);
//        foreach (var wordPhrase in wordPhrases)
//        {
//          _CurrentWordPhrase = wordPhrase;
//          int dummyWorker = -1;
//          int dummyCompletionPort = -1;
//          ThreadPool.GetMaxThreads(out dummyWorker, out dummyCompletionPort);
//          ThreadPool.QueueUserWorkItem(new WaitCallback(getPercentKnownWord), areID);

//          //ThreadPool.QueueUserWorkItem(new WaitCallback(getPercentKnownWord), _AutoResetEvent);
//          localAutoResetEvent.WaitOne(500);

//        }
//        #endregion

//        #region FINALLY, GET THE PERCENT KNOWN = COUNT WORDS KNOWN / COUNT ALL WORDS (AND RETURN)
//        var countAllWords = wordPhrases.Count;
//        percentKnown = ((double)_CountWordsKnown) / ((double)countAllWords);

//        callback(this, new ResultArgs<double>(percentKnown));
//        return;
//        #endregion

//        #endregion
//      });

//      #endregion

//      #endregion
    }

    #endregion

    #region Events

    #endregion
  }
}
