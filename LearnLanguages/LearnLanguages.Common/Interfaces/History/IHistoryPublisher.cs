using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IHistoryPublisher
  {
    Guid Id { get; }
    void PublishEvent(IHistoryEvent historyEvent);
    void SubscribeToEvents(object subscriber);
    void UnsubscribeFromEvents(object subscriber);
  }
}
