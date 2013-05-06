using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  /// <summary>
  /// This event occurs when you are studying a line.  I make this for the intent
  /// of knowing which phrases are entire lines, versus partial lines, or even just 
  /// solo phrases.
  /// </summary>
  [Serializable]
  public class StartingStudySessionEvent : Bases.HistoryEventBase
  {
    public StartingStudySessionEvent()
      : base(TimeSpan.Zero)
    {
    }

  }
}
