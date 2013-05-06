using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ViewedPhrasePartOfLineOnScreenEvent : Bases.SinglePhraseEventBase
  {
    public ViewedPhrasePartOfLineOnScreenEvent()
      : base()
    {

    }

    public ViewedPhrasePartOfLineOnScreenEvent(PhraseEdit phrase, LineEdit line)
      : base(phrase)
    {
      AddLineDetails(line);
    }

    public ViewedPhrasePartOfLineOnScreenEvent(PhraseEdit phrase, LineEdit line, TimeSpan duration)
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
