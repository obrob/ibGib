using System;
using Csla.Serialization;
using LearnLanguages.Business;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ReviewedPhraseEvent : Bases.SinglePhraseEventBase
  {
    public ReviewedPhraseEvent()
      : base()
    {

    }

    public ReviewedPhraseEvent(PhraseEdit phrase, Guid reviewMethodId, TimeSpan duration)
      : base(phrase, duration)
    {
      AddReviewMethodId(reviewMethodId);
    }

    public ReviewedPhraseEvent(double feedbackAsDouble, Guid phraseId, string phraseText, Guid languageId, 
      string languageText, Guid reviewMethodId, TimeSpan duration)
      : base(phraseId, phraseText, languageId, languageText, duration)
    {
      AddReviewMethodId(reviewMethodId);
      AddFeedbackAsDouble(feedbackAsDouble);
    }

    private void AddFeedbackAsDouble(double feedbackAsDouble)
    {
      AddDetail(HistoryResources.Key_FeedbackAsDouble, feedbackAsDouble);
    }

    private void AddReviewMethodId(Guid reviewMethodId)
    {
      AddDetail(HistoryResources.Key_ReviewMethodId, reviewMethodId);
    }
  }
}
