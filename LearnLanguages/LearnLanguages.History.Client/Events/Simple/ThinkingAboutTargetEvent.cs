using System;


namespace LearnLanguages.History.Events
{
  public class ThinkingAboutTargetEvent : Bases.ThinkingEventBase
  {

    public ThinkingAboutTargetEvent(Guid targetId)
      : base(targetId)
    {
    }

    public Guid TargetId
    {
      get
      {
        return GetDetail<Guid>(HistoryResources.Key_TargetId);
      }
    }

    public static void Publish(Guid targetId)
    {
      History.HistoryPublisher.Ton.PublishEvent(new ThinkingAboutTargetEvent(targetId));
    }
  }
}
