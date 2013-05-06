using System;
using LearnLanguages.History;
using LearnLanguages.Business;
using LearnLanguages.History.Events;
using LearnLanguages.History.Bases;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  public class DefaultReviewedLineRecorder : HistoryRecorderBase<ReviewedLineEvent>
  {
    public DefaultReviewedLineRecorder()
    {
      Id = Guid.Parse(StudyResources.DefaultPhraseReviewedRecorderId);
    }

    /// <summary>
    /// Always returns true.  This records all LineReviewedEvent's.
    /// </summary>
    protected override bool ShouldRecord(History.Events.ReviewedLineEvent message)
    {
      return true;
    }

    protected override async Task RecordAsync(History.Events.ReviewedLineEvent message)
    {
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

      #region CREATE BELIEF
      var belief = await PhraseBeliefEdit.NewPhraseBeliefEditAsync(phraseId);

      if (belief.Phrase.Text != lineText ||
          belief.Phrase.Language.Text != languageText)
        throw new Exception("phrasetext or languagetext does not match message");

      #region EDIT BELIEF
      //THE TIMESTAMP OF RECORDER'S BELIEF IS RIGHT NOW
      belief.TimeStamp = DateTime.Now;

      //RECORDER USES TEXT PROPERTY FOR DETAILS ABOUT REVIEW IN FORM OF 
      //QUERY STRING, INCLUDING LINE ID/NUMBER AND DURATION
      var beliefText = @"?";
      #region Build beliefText
      beliefText += HistoryResources.Key_DurationInMilliseconds + "=" + duration.Milliseconds.ToString();
      beliefText += @"&" + HistoryResources.Key_LineId + "=" + lineId.ToString();
      beliefText += @"&" + HistoryResources.Key_LineNumber + "=" + lineNumber.ToString();
      #endregion
      belief.Text = beliefText;

      //FEEDBACK STRENGTH.  RECORDER IS PASSTHROUGH IN THIS CASE.  RECORDER HAS NO ALTERATION
      //TO THE STRENGTH OF THE FEEDBACK.
      belief.Strength = feedback;

      //THE RECORDER IS THE BELIEVER IN THIS CASE
      belief.BelieverId = Id;

      //REVIEWMETHOD ID
      belief.ReviewMethodId = reviewMethodId;

      //PHRASE IS ALREADY SET
      #endregion

      #region SAVE BELIEF
      await belief.SaveAsync();
      #endregion

      #endregion
    }
  }
}
