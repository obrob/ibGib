using System;
using Csla.Serialization;
using LearnLanguages.Business;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ReviewingLineEvent : Bases.SingleLineEventBase
  {
    public ReviewingLineEvent()
      : base()
    {

    }

    public ReviewingLineEvent(LineEdit line, Guid reviewMethodId)
      : base(line)
    {
      AddReviewMethodId(reviewMethodId);
    }

    //public ReviewingLineEvent(Guid lineId, Guid phraseId, Guid languageId, Guid reviewMethodId)
    //  : base(lineId, languageId)
    //{
    //  AddReviewMethodId(reviewMethodId);
    //}

    private void AddReviewMethodId(Guid reviewMethodId)
    {
      AddDetail(HistoryResources.Key_ReviewMethodId, reviewMethodId);
    }
  }
}
