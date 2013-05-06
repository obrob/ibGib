using System;
using LearnLanguages.Common;
using LearnLanguages.History;
using LearnLanguages.Business;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;
using LearnLanguages.History.Events;
using LearnLanguages.History.Bases;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  public class DefaultReviewedWordInPhraseRecorder : HistoryRecorderBase<ReviewedLineEvent>
  {
    public DefaultReviewedWordInPhraseRecorder()
    {
      Id = Guid.Parse(StudyResources.DefaultWordInPhraseReviewedRecorderId);
    }

    /// <summary>
    /// Always returns true.  This records all LineReviewedEvent's.
    /// </summary>
    protected override bool ShouldRecord(History.Events.ReviewedLineEvent message)
    {
      return true;
    }

    protected async override Task RecordAsync(History.Events.ReviewedLineEvent message)
    {
//CHANGE DEFAULT PHRASE RECORDER RECORDASYNC TO USE TASKS
      /// DETAILS: LINE ID
      /// REVIEWMETHODID
      /// LINETEXT
      /// LINENUMBER
      /// PHRASEID
      /// LANGUAGEID
      /// LANGUAGETEXT
      /// FEEDBACKASDOUBLE
      /// DURATION

      var lineId = message.GetDetail<Guid>(HistoryResources.Key_LineId);
      var reviewMethodId = message.GetDetail<Guid>(HistoryResources.Key_ReviewMethodId);
      var lineText = message.GetDetail<string>(HistoryResources.Key_LineText);
      var lineNumber = message.GetDetail<int>(HistoryResources.Key_LineNumber);
      var phraseId = message.GetDetail<Guid>(HistoryResources.Key_PhraseId);
      var languageId = message.GetDetail<Guid>(HistoryResources.Key_LanguageId);
      var languageText = message.GetDetail<string>(HistoryResources.Key_LanguageText);
      var feedback = message.GetDetail<double>(HistoryResources.Key_FeedbackAsDouble);
      var duration = message.Duration;
      
      #region PARSE TEXT INTO WORDS

      var words = lineText.ParseIntoWords();

      #region INITIALIZE BELIEF PROPERTIES COMMON TO ALL WORDS

      //THE TIMESTAMP OF RECORDER'S BELIEF IS RIGHT NOW
      var beliefTimeStamp = DateTime.Now;

      //RECORDER USES TEXT PROPERTY FOR DETAILS ABOUT REVIEW IN FORM OF 
      //QUERY STRING, INCLUDING LINE ID/NUMBER AND DURATION
      var beliefText = @"?";
      #region beliefText += ... (DURATION, LINE ID, LINE NUMBER)
      //DURATION
      beliefText += HistoryResources.Key_DurationInMilliseconds + "=" + duration.Milliseconds.ToString();
      //LINE ID
      beliefText += @"&" + HistoryResources.Key_LineId + "=" + lineId.ToString();
      //LINE NUMBER
      beliefText += @"&" + HistoryResources.Key_LineNumber + "=" + lineNumber.ToString();
      #endregion

      //FEEDBACK STRENGTH.  RECORDER IS PASSTHROUGH IN THIS CASE.  RECORDER HAS NO ALTERATION
      //TO THE STRENGTH OF THE FEEDBACK.
      var beliefStrength = feedback;

      //THE RECORDER IS THE BELIEVER IN THIS CASE
      var beliefBelieverId = Id;

      //REVIEWMETHOD ID
      var beliefReviewMethodId = reviewMethodId;

      #endregion

      #region Action<object> createAndSaveBelief THIS IS THE ACTION THAT HAPPENS IN THE FOR LOOP.  IT USES _CURRENTWORDPHRASEID AND COMMON BELIEF PROPERTIES
      Action<object> createAndSaveBelief = async (object state) =>
        {
          AutoResetEvent autoResetEvent = (AutoResetEvent)state;

          //CREATE BELIEF
          var belief = await PhraseBeliefEdit.NewPhraseBeliefEditAsync(_CurrentWordPhraseId);

          if (belief.Phrase.Language.Text != languageText)
            throw new Exception("languagetext does not match message");

          belief.TimeStamp = beliefTimeStamp;
          belief.Text = beliefText;
          belief.Strength = beliefStrength;
          belief.BelieverId = beliefBelieverId;
          belief.ReviewMethodId = beliefReviewMethodId;

          //SAVE BELIEF
          var savedBelief = await belief.SaveAsync();
          autoResetEvent.Set();
        };

      #endregion
      foreach (var word in words)
      {
        ThreadPool.QueueUserWorkItem(new WaitCallback(createAndSaveBelief), _AutoResetEvent);
        _AutoResetEvent.WaitOne();
      }

      #endregion
    }

    private static AutoResetEvent _AutoResetEvent = new AutoResetEvent(false);
    private static Guid _CurrentWordPhraseId = Guid.Empty;
  }
}
