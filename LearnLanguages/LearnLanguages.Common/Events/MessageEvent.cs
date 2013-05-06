using System;
using Csla.Serialization;
using LearnLanguages.Common.MessageShower;

namespace LearnLanguages.Common.Events
{
  [Serializable]
  public class MessageEvent
  {
    public MessageEvent(string text, MessagePriority priority, MessageType type)
    {
      Text = text;
      Priority = priority;
      Type = type;
      Timestamp = DateTime.Now;
    }

    public string Text { get; private set; }
    public MessagePriority Priority { get; private set; }
    public MessageType Type { get; private set; }
    public DateTime Timestamp { get; private set; }
  }
}
