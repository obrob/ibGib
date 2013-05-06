using System;
using System.Net;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Navigation.EventMessages;
using Caliburn.Micro;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  public abstract class StudyItemViewModelBase : ViewModelBase, 
                                                 Interfaces.IStudyItemViewModelBase,
                                                 IHandle<NavigationRequestedEventMessage>
  {
    #region Ctors and Init

    public StudyItemViewModelBase()
    {
      Services.EventAggregator.Subscribe(this);//navigation
    }

    #endregion

    #region Properties

    /// <summary>
    /// Convenience property.
    /// </summary>
    protected DateTime _DateTimeQuestionShown { get; set; }
    /// <summary>
    /// Convenience property.
    /// </summary>
    protected DateTime _DateTimeAnswerShown { get; set; }
    /// <summary>
    /// Id should be unique identifier for either an instance or an entire class.
    /// </summary>
    public Guid ReviewMethodId
    {
      get { return GetReviewMethodId(); }
    }

    private string _StudyItemTitle;
    /// <summary>
    /// Title for the current study item for the viewmodel.
    /// </summary>
    public string StudyItemTitle
    {
      get { return _StudyItemTitle; }
      set
      {
        if (value != _StudyItemTitle)
        {
          _StudyItemTitle = value;
          NotifyOfPropertyChange(() => StudyItemTitle);
        }
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// By default, this awaits the ShowAsyncImpl method, then fires the 
    /// Shown event. This can be overridden.
    /// </summary>
    /// <returns>Task (async void)</returns>
    public virtual async Task ShowAsync()
    {
      await ShowAsyncImpl();
      if (Shown != null)
        Shown(this, EventArgs.Empty);
    }

    //protected void DispatchShown()
    //{
    //  if (Shown != null)
    //    Shown(this, EventArgs.Empty);
    //}

    /// <summary>
    /// Implementation for actual showing of viewmodel. This show 
    /// is not like old winforms days when a form pops up. When this is called, 
    /// the viewmodel should already be "on screen" able to display stuff, and 
    /// this method should toggle visibilities of individual pieces, and initiate 
    /// whatever process it does. I'm just writing this because to me, I used to 
    /// associate Show() to "pop up a window or modal", and that just isn't the 
    /// case anymore.
    /// </summary>
    /// <returns></returns>
    protected abstract Task ShowAsyncImpl();

    /// <summary>
    /// Should set in motion aborting the showing of this viewmodel.
    /// </summary>
    public abstract void Abort();

    /// <summary>
    /// Implement this to get the unique identifier for either this class
    /// or this instance. For beginners, I'm using this as a class id.
    /// </summary>
    /// <returns></returns>
    protected abstract Guid GetReviewMethodId();

    /// <summary>
    /// This by default calls the Abort() method, if for some reason the 
    /// user chooses to navigate to somewhere else while showing this viewmodel.
    /// </summary>
    public virtual void Handle(NavigationRequestedEventMessage message)
    {
      Abort();
    }

    #endregion

    #region Events

    /// <summary>
    /// Fired after the viewmodel is done with its Show() method. The Show()
    /// method could be a long running process
    /// </summary>
    public event EventHandler Shown;

    #endregion
  }
}
