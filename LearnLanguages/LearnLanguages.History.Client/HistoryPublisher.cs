using System;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace LearnLanguages.History
{
  public class HistoryPublisher : IHistoryPublisher
  {
    public HistoryPublisher()
    {
      _CompoundEventMakers = new List<ICompoundEventMaker>();
    }

    [ImportMany]
    public ICollection<ICompoundEventMaker> _CompoundEventMakers { get; set; }

    public Guid Id { get { return Guid.Parse(HistoryResources.HistoryPublisherId); } }

    #region Singleton Pattern Members
    private static volatile HistoryPublisher _Ton;
    private static object _Lock = new object();
    /// <summary>
    /// Singleton instance.  Named "The" for esthetic reasons.
    /// </summary>
    public static HistoryPublisher Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
            {
              _Ton = new HistoryPublisher();
              Services.Container.SatisfyImportsOnce(_Ton);
              ConsoleHistoryListener.Ton.GetType();//hack
            }
          }
        }

        return _Ton;
      }
    }
    #endregion

    #region protected static IEventAggregator _EventAggregator
    private static volatile IEventAggregator _eventAggregator = new EventAggregator();
    private static object _EventAggregatorLock = new object();
    protected static IEventAggregator _EventAggregator 
    {
      get
      {
        lock (_EventAggregatorLock)
        {
          return _eventAggregator;
        }
      }
    }
    #endregion

    public void PublishEvent(IHistoryEvent historyEvent)
    {
      _EventAggregator.Publish(historyEvent);
    }
    public void SubscribeToEvents(object subscriber)
    {
      _EventAggregator.Subscribe(subscriber);
    }
    public void UnsubscribeFromEvents(object subscriber)
    {
      _EventAggregator.Unsubscribe(subscriber);
    }
  }
}
