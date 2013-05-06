using System;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using System.Threading.Tasks;

namespace LearnLanguages.History.Bases
{
  /// <summary>
  /// Records history events if should handle message.
  /// </summary>
  public abstract class HistoryRecorderBase<TMessage> : IHandle<TMessage>, IHaveId
  {
    public Guid Id { get; protected set; }

    protected bool _IsEnabled = false;
    /// <summary>
    /// Changing this affects whether this subscribes to HistoryPublisher events.
    /// </summary>
    public bool IsEnabled
    {
      get
      {
        return _IsEnabled;
      }
      set
      {
        if (value != _IsEnabled)
        {
          _IsEnabled = value;
          if (_IsEnabled)
            HistoryPublisher.Ton.SubscribeToEvents(this);
          else
            HistoryPublisher.Ton.UnsubscribeFromEvents(this);
        }
      }
    }

    public void Handle(TMessage message)
    {
      if (ShouldRecord(message))
        RecordAsync(message);
    }

    protected abstract bool ShouldRecord(TMessage message);
    protected abstract Task RecordAsync(TMessage message);
  }
}
