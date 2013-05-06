using System;
using System.ComponentModel.Composition;
using System.Windows;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common.Feedback;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LearnLanguages.Study.ViewModels
{
  /// <summary>
  /// Stateless viewmodel, just used for providing an interface to the user with
  /// providing feedback as to how much of something should be considered "known" 
  /// by the user.
  /// </summary>
  [Export(typeof(PercentKnownFeedbackViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class PercentKnownFeedbackViewModel : FeedbackViewModelBase
                                               
  {
    #region Ctors and Init

    public PercentKnownFeedbackViewModel()
    {
      Services.EventAggregator.Subscribe(this);
    }

    #endregion

    #region Properties

    private string _Instructions = StudyResources.InstructionsSelectFeedbackNoneSomeMostAll;
    public string Instructions
    {
      get { return _Instructions; }
      set
      {
        if (value != _Instructions)
        {
          _Instructions = value;
          NotifyOfPropertyChange(() => Instructions);
        }
      }
    }

    private string _NoneInstructions = StudyResources.InstructionsSelectFeedbackNone;
    public string NoneInstructions
    {
      get { return _NoneInstructions; }
      set
      {
        if (value != _NoneInstructions)
        {
          _NoneInstructions = value;
          NotifyOfPropertyChange(() => NoneInstructions);
        }
      }
    }

    private string _SomeInstructions = StudyResources.InstructionsSelectFeedbackSome;
    public string SomeInstructions
    {
      get { return _SomeInstructions; }
      set
      {
        if (value != _SomeInstructions)
        {
          _SomeInstructions = value;
          NotifyOfPropertyChange(() => SomeInstructions);
        }
      }
    }

    private string _MostInstructions = StudyResources.InstructionsSelectFeedbackMost;
    public string MostInstructions
    {
      get { return _MostInstructions; }
      set
      {
        if (value != _MostInstructions)
        {
          _MostInstructions = value;
          NotifyOfPropertyChange(() => MostInstructions);
        }
      }
    }

    private string _AllInstructions = StudyResources.InstructionsSelectFeedbackAll;
    public string AllInstructions
    {
      get { return _AllInstructions; }
      set
      {
        if (value != _AllInstructions)
        {
          _AllInstructions = value;
          NotifyOfPropertyChange(() => AllInstructions);
        }
      }
    }

    private Visibility _InstructionsVisibility = Visibility.Visible;
    public Visibility InstructionsVisibility
    {
      get { return _InstructionsVisibility; }
      set
      {
        if (value != _InstructionsVisibility)
        {
          _InstructionsVisibility = value;
          NotifyOfPropertyChange(() => InstructionsVisibility);
        }
      }
    }

    private DateTime _FeedbackAsked { get; set; }
    private DateTime _FeedbackGiven { get; set; }

    #endregion

    #region Methods

    public override IFeedback GetFeedback(int timeoutMilliseconds)
    {
      //INITIALIZE VARIABLES
      Feedback = new Feedback<double>();
      SetFeedback(-1);
      TimeSpan timeoutTimeSpan = new TimeSpan(0, 0, 0, 0, timeoutMilliseconds);
      DateTime timeoutDateTime = DateTime.Now + timeoutTimeSpan;
      IsEnabled = true;
      bool feedbackIsProvided = false;

      //TIMESTAMP WHEN FEEDBACK IS ASKED
      _FeedbackAsked = DateTime.Now;
      do
      {
        //SLEEP A CERTAIN AMOUNT OF TIME BETWEEN CHECKING FOR FEEDBACK VALUE
        System.Threading.Thread.Sleep(int.Parse(StudyResources.DefaultFeedbackCheckIntervalMilliseconds));
        var feedbackValue = ((Feedback<double>)Feedback).Value;

        //IF FEEDBACK IS PROVIDED, IT WILL BE EQUAL TO OR GREATER THAN 0
        //NEGATIVE FEEDBACK IS NOT ALLOWED (IT SHOULD INITIALIZE TO A NEGATIVE 
        //VALUE, E.G. -1)
        feedbackIsProvided = feedbackValue >= 0;
      }
      while (DateTime.Now < timeoutDateTime && !feedbackIsProvided);
      IsEnabled = false;

      _FeedbackGiven = DateTime.Now;
      PublishFeedback();
      return Feedback;
    }

    //CONVERT FEEDBACK VIEWMODEL GETFEEDBACKASYNC TO ASYNC/AWAIT KEYWORD PATTERN
    public override async Task<IFeedback> GetFeedbackAsync(int timeoutMilliseconds)
    {
      var getFeedbackTask = new Task<IFeedback>(() => GetFeedback(timeoutMilliseconds));
      getFeedbackTask.Start();
      return await getFeedbackTask;

      ////ENABLE THE FEEDBACK CONTROLS (E.G. BUTTONS)
      //IsEnabled = true;

      //BackgroundWorker worker = new BackgroundWorker();
      //worker.DoWork += (s, r) =>
      //  {
      //    //RUNS ON A BACKGROUND THREAD
      //    try
      //    {
      //      //var threadIdDoWork = System.Threading.Thread.CurrentThread.ManagedThreadId;
      //      TimeSpan timeoutTimeSpan = new TimeSpan(0, 0, 0, 0, timeoutMilliseconds);
      //      DateTime timeoutDateTime = DateTime.Now + timeoutTimeSpan;
      //      bool feedbackIsProvided = false;
      //      _FeedbackAsked = DateTime.Now;
      //      do
      //      {
      //        //DO THIS WHILE WE ARE WAITING FOR FEEDBACK FROM A CONTROL OR A TIMEOUT
      //        var feedbackValue = ((Feedback<double>)Feedback).Value;
      //        feedbackIsProvided = feedbackValue != -1;
      //        System.Threading.Thread.Sleep(int.Parse(StudyResources.DefaultFeedbackCheckIntervalMilliseconds));
      //      }
      //      while (DateTime.Now < timeoutDateTime && !feedbackIsProvided);

      //    }
      //    catch (Exception ex)
      //    {
      //      IsEnabled = false;
      //      callback(this, new ResultArgs<IFeedback>(ex));
      //    }
      //  };
      //worker.RunWorkerCompleted += (s2, r2) =>
      //  {
      //    //RUNS ON THE MAIN THREAD
      //    //var threadIdCompleted = System.Threading.Thread.CurrentThread.ManagedThreadId;
      //    _FeedbackGiven = DateTime.Now;
      //    PublishFeedback();
      //    IsEnabled = false;
      //    callback(this, new ResultArgs<IFeedback>(Feedback));
      //  };

      //worker.RunWorkerAsync();
    }

    private void PublishFeedback()
    {
      if (Feedback == null)
        return;

      var duration = _FeedbackGiven - _FeedbackAsked;
      var feedbackAsDouble = ((Feedback<double>)Feedback).Value;
      History.HistoryPublisher.Ton.PublishEvent(
        new History.Events.FeedbackAsDoubleGivenEvent(feedbackAsDouble, duration));
    }

    private void SetFeedback(double feedbackValue)
    {
      ((Feedback<double>)Feedback).Value = feedbackValue;
    }

    public void None()
    {
      SetFeedback(double.Parse(StudyResources.PercentKnownNone));
    }

    public void Some()
    {
      SetFeedback(double.Parse(StudyResources.PercentKnownSome));
    }

    public void Most()
    {
      SetFeedback(double.Parse(StudyResources.PercentKnownMost));
    }

    public void All()
    {
      SetFeedback(double.Parse(StudyResources.PercentKnownAll));
    }

    

    #endregion

    
  }
}
