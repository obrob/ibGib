using System;
using LearnLanguages.History;
using LearnLanguages.Business;
using LearnLanguages.History.Events;
using LearnLanguages.History.Bases;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  public class ReviewedPhraseRecorder : HistoryRecorderBase<ReviewedPhraseEvent>
  {
    public ReviewedPhraseRecorder()
    {
      Id = Guid.Parse(StudyResources.RecorderIdReviewedPhrase);
    }

    /// <summary>
    /// Always returns true.  This records all PhraseReviewedEvent's.
    /// </summary>
    protected override bool ShouldRecord(History.Events.ReviewedPhraseEvent message)
    {
      return true;
    }

    protected override async Task RecordAsync(ReviewedPhraseEvent message)
    {
      var reviewMethodId = message.GetDetail<Guid>(HistoryResources.Key_ReviewMethodId);
      var phraseText = message.GetDetail<string>(HistoryResources.Key_PhraseText);
      var phraseId = message.GetDetail<Guid>(HistoryResources.Key_PhraseId);
      var languageId = message.GetDetail<Guid>(HistoryResources.Key_LanguageId);
      var languageText = message.GetDetail<string>(HistoryResources.Key_LanguageText);
      var feedback = message.GetDetail<double>(HistoryResources.Key_FeedbackAsDouble);
      var duration = message.Duration;
      var phraseWasModifiedDuringReview = false;

      //CREATE BELIEF
      var belief = await PhraseBeliefEdit.NewPhraseBeliefEditAsync(phraseId);

      if (belief.Phrase.Text != phraseText)
        //THIS ONLY HAPPENS WHEN THE PHRASE WAS MODIFIED DURING ITS REVIEW
        phraseWasModifiedDuringReview = true;
      
      if (belief.Phrase.Language.Text != languageText)
        throw new Exception("languagetext does not match message");

      //EDIT BELIEF
      //THE TIMESTAMP OF RECORDER'S BELIEF IS RIGHT NOW
      belief.TimeStamp = DateTime.Now;

      //RECORDER USES TEXT PROPERTY FOR DETAILS ABOUT REVIEW IN FORM OF 
      //QUERY STRING, INCLUDING PHRASE TEXT AND DURATION
      //BUILD BELIEFTEXT
      var beliefText = BeliefHelper.GetInitialEmptyBeliefText();
      beliefText = BeliefHelper.AppendDurationInMs(beliefText, duration.Milliseconds);
      beliefText = BeliefHelper.AppendPhraseText(beliefText, phraseText);
      beliefText = BeliefHelper.AppendLanguageText(beliefText, languageText);
      //ONLY APPEND THAT IT WAS MODIFIED DURING REVIEW IF IT WAS. MOST WILL
      //NOT HAVE THIS VALUE.
      if (phraseWasModifiedDuringReview)
        beliefText = BeliefHelper.AppendPhraseWasModifiedDuringReview(beliefText);

      belief.Text = beliefText;

      //FEEDBACK STRENGTH.  RECORDER IS PASSTHROUGH IN THIS CASE.  RECORDER HAS NO ALTERATION
      //TO THE STRENGTH OF THE FEEDBACK.
      belief.Strength = feedback;

      //THE RECORDER IS THE BELIEVER IN THIS CASE
      belief.BelieverId = Id;

      //REVIEWMETHOD ID
      belief.ReviewMethodId = reviewMethodId;

      //SAVE BELIEF
      await belief.SaveAsync();
    }
  }
}
