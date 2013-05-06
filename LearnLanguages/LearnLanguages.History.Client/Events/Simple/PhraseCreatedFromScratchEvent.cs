using System;
using System.Collections.Generic;
using LearnLanguages.Business;
using Csla.Serialization;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class PhraseCreatedFromScratchEvent : Bases.SinglePhraseEventBase
  {
    public PhraseCreatedFromScratchEvent(PhraseEdit phrase)
      : base(phrase) 

    {

    } 
  }
}
