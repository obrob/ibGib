#if SILVERLIGHT
using Csla.Serialization;
#else
using System;
#endif

using LearnLanguages.Common.Interfaces.Autonomous;

namespace LearnLanguages.Common.Events
{
  [Serializable]
  public class AutonomousEvent : MessageEvent
  {
    public AutonomousEvent(IAutonomousService service, string text, MessagePriority priority, MessageType type)
      : base(text, priority, type)
    {
      Service = service;
    }

    public IAutonomousService Service { get; private set; }
  }
}
