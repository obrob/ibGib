using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  //NOT NEEDED AT THE MOMENT. AM SAVING AUTOTRANSLATIONS AUTOMATICALLY WITH THE AZURE 
  //TRANSLATOR.

  ///// <summary>
  ///// Occurs when a phrase is automatically translated using an auto translate service.
  ///// </summary>
  //[Serializable]
  //public class PhraseAutoTranslatedEvent : Bases.HistoryEventBase
  //{
  //  public PhraseAutoTranslatedEvent(PhraseEdit sourcePhrase, PhraseEdit translatedPhrase)
  //    : base(TimeSpan.Zero)
  //  {
  //    SourcePhrase = sourcePhrase;
  //    TranslatedPhrase = translatedPhrase;
  //  }

  //  public PhraseEdit SourcePhrase { get; private set; }
  //  public PhraseEdit TranslatedPhrase { get; private set; }
  //}
}
