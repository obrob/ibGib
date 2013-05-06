using System;
using Csla.Serialization;
using LearnLanguages.Business;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ReviewingPhraseEvent : Bases.SinglePhraseEventBase
  {
    public ReviewingPhraseEvent()
      : base()
    {

    }

    public ReviewingPhraseEvent(PhraseEdit phrase, Guid reviewMethodId)
      : base(phrase)
    {
      AddReviewMethodId(reviewMethodId);
    }

    public ReviewingPhraseEvent(Guid phraseId, Guid languageId, Guid reviewMethodId)
      : base(phraseId, languageId)
    {
      AddReviewMethodId(reviewMethodId);
    }

    private void AddReviewMethodId(Guid reviewMethodId)
    {
      AddDetail(HistoryResources.Key_ReviewMethodId, reviewMethodId);
    }
  }
}
