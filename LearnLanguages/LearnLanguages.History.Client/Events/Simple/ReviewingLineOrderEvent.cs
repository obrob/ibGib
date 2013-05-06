using System;
using Csla.Serialization;
using LearnLanguages.Business;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ReviewingLineOrderEvent : Bases.SingleLineEventBase
  {
    public ReviewingLineOrderEvent()
      : base()
    {

    }

    public ReviewingLineOrderEvent(LineEdit line, MultiLineTextEdit multiLineText, Guid reviewMethodId)
      : base(line)
    {
      AddReviewMethodId(reviewMethodId);
      AddDetail(HistoryResources.Key_MultiLineTextId, multiLineText.Id);
    }

    //public ReviewingLineOrderEvent(Guid lineId, Guid phraseId, Guid languageId, Guid reviewMethodId)
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
