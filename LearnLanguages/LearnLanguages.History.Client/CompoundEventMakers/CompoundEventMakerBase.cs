using System;
using LearnLanguages.Common.Interfaces;
using Caliburn.Micro;

namespace LearnLanguages.History.CompoundEventMakers
{
  public abstract class CompoundEventMakerBase : ICompoundEventMaker, IHandle<Events.StartingStudySessionEvent>
  {
    protected abstract void Reset();

    public Guid Id { get { return GetId(); } }

    protected abstract Guid GetId();

    public virtual void Enable()
    {
      Reset();
      HistoryPublisher.Ton.SubscribeToEvents(this);
    }

    public virtual void Disable()
    {
      HistoryPublisher.Ton.UnsubscribeFromEvents(this);
      Reset();
    }

    public virtual void Handle(Events.StartingStudySessionEvent message)
    {
      Reset();
    }
  }
}
