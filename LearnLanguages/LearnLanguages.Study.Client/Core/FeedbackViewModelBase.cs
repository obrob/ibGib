using System;
using System.Net;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common;
using LearnLanguages.Navigation.EventMessages;
using Caliburn.Micro;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  public abstract class FeedbackViewModelBase : ViewModelBase, 
                                                IFeedbackViewModelBase,
                                                IHandle<NavigationRequestedEventMessage>
  {
    public FeedbackViewModelBase()
    {
      Services.EventAggregator.Subscribe(this);//navigation
    }
    public abstract IFeedback GetFeedback(int timeoutMilliseconds);
    public abstract Task<IFeedback> GetFeedbackAsync(int timeoutMilliseconds);

    private IFeedback _Feedback;
    public IFeedback Feedback
    {
      get { return _Feedback; }
      set
      {
        if (value != _Feedback)
        {
          _Feedback = value;
          NotifyOfPropertyChange(() => Feedback);
        }
      }
    }

    //private bool _IsEnabled;
    //public bool IsEnabled
    //{
    //  get { return _IsEnabled; }
    //  set
    //  {
    //    if (value != _IsEnabled)
    //    {
    //      _IsEnabled = value;
    //      NotifyOfPropertyChange(() => IsEnabled);
    //    }
    //  }
    //}

    public virtual void Handle(NavigationRequestedEventMessage message)
    {
      IsEnabled = false;
    }
  }
}
