using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ViewingPhrasePartOfLineOnScreenEvent : Bases.SinglePhraseEventBase
  {
    public ViewingPhrasePartOfLineOnScreenEvent()
      : base()
    {

    }

    public ViewingPhrasePartOfLineOnScreenEvent(PhraseEdit phrase, LineEdit line)
      : base(phrase)
    {
      AddLineDetails(line);
    }

    public ViewingPhrasePartOfLineOnScreenEvent(PhraseEdit phrase, LineEdit line, TimeSpan duration)
      : base(phrase, duration)
    {
      AddLineDetails(line);
    }

    protected virtual void AddLineDetails(LineEdit line)
    {
      AddDetail(HistoryResources.Key_LineId, line.Id);
    }
  }
}
