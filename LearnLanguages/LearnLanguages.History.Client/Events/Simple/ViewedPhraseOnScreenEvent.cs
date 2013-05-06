using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ViewedPhraseOnScreenEvent : Bases.SinglePhraseEventBase
  {
    public ViewedPhraseOnScreenEvent()
      : base()
    {

    }

    public ViewedPhraseOnScreenEvent(PhraseEdit phrase)
      : base(phrase)
    {

    }

    public ViewedPhraseOnScreenEvent(PhraseEdit phrase, TimeSpan duration)
      : base(phrase, duration)
    {

    }

    public ViewedPhraseOnScreenEvent(Guid phraseId, Guid languageId)
      : base(phraseId, languageId)
    {

    }
  }
}
