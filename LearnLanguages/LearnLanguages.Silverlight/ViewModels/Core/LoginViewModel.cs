using System.ComponentModel.Composition;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.ViewModelBases;
using System.ComponentModel;
using System.Threading.Tasks;
using LearnLanguages.Common.EventMessages;
using LearnLanguages.Silverlight.Common;
using Caliburn.Micro;
using System.Windows.Input;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LoginViewModel))]
  [ViewModelMetadata("Login")]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class LoginViewModel : PageViewModelBase
  {
    private string _Username;
    public string Username
    {
      get { return _Username; }
      set
      {
        if (value != _Username)
        {
          _Username = value;
          NotifyOfPropertyChange(() => Username);
          NotifyOfPropertyChange(() => CanLogin);
        }
      }
    }

    private string _Password;
    public string Password
    {
      get { return _Password; }
      set
      {
        if (value != _Password)
        {
          _Password = value;
          NotifyOfPropertyChange(() => Password);
          NotifyOfPropertyChange(() => CanLogin);
        }
      }
    }

    public async Task Login()
    {
      if (!CanLogin)
        return;

      LoggingIn = true;
#if DEBUG
      //var allEdits = await Business.PhraseList.GetAllAsync();
#endif

      IncrementApplicationBusyEventMessage.Publish("LoginViewModel.Login");
      try
      {
        var result = await UserPrincipal.LoginAsync(Username, Password);
        AuthenticationChangedEventMessage.Publish();
      }
      finally
      {
        DecrementApplicationBusyEventMessage.Publish("LoginViewModel.Login");
      }

      if (Csla.ApplicationContext.User.Identity.IsAuthenticated)
      {
        //LOGIN SUCCESSFUL
        LoggingIn = false;
      }
      else
      {
        //LOGIN UNSUCCESSFUL
        //PENALIZE FOR NOT LOGGING IN PROPERLY.
        //QUICK AND DIRTY PENALTY.
        //HACK: LOGIN FAILED PENALTY EASILY CIRCUMVENTED.  THIS REALLY NEEDS TO BE DONE ON THE SERVER SIDE.
        System.Windows.MessageBox.Show(AppResources.LoginFailedMessage);
        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (sender, eventArg) =>
          {
            System.Threading.Thread.Sleep(int.Parse(AppResources.LoginFailedPenaltyInMilliseconds));
            LoggingIn = false;
          };
        worker.RunWorkerAsync();
      }
    }

    public bool CanLogin
    {
      get
      {
        return (!string.IsNullOrEmpty(Username) && 
                !string.IsNullOrEmpty(Password) &&
                !LoggingIn);
      }
    }

    private bool _LoggingIn = false;
    public bool LoggingIn
    {
      get { return _LoggingIn; }
      set
      {
        if (value != _LoggingIn)
        {
          _LoggingIn = value;
          NotifyOfPropertyChange(() => LoggingIn);
          NotifyOfPropertyChange(() => CanLogin);
        }
      }
    }

    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = ViewViewModelResources.InstructionsLoginPage;
      Title = ViewViewModelResources.TitleLoginPage;
      Description = ViewViewModelResources.DescriptionLoginPage;
      ToolTip = ViewViewModelResources.ToolTipLoginPage;
    }

    /// <summary>
    /// Logs in when the user presses enter. Should refactor. But need
    /// to change the view as well.
    /// </summary>
    public async void ExecuteAction(ActionExecutionContext context)
    {
      var eventArgs = (KeyEventArgs)context.EventArgs;
      if (eventArgs.Key != Key.Enter) return;

      IncrementApplicationBusyEventMessage.Publish("LoginViewModel.ExecuteAction");
      try
      {
        await Login();
      }
      finally
      {
        DecrementApplicationBusyEventMessage.Publish("LoginViewModel.ExecuteAction");
      }
    }
  }
}
