using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ViewingPhraseOnScreenEvent : Bases.SinglePhraseEventBase
  {
    public ViewingPhraseOnScreenEvent()
      : base()
    {

    }

    public ViewingPhraseOnScreenEvent(PhraseEdit phrase)
      : base(phrase)
    {

    }

    public ViewingPhraseOnScreenEvent(Guid phraseId, Guid languageId)
      : base(phraseId, languageId)
    {

    }

  }
}
