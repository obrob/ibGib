using LearnLanguages.Business.Security;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Common;
using Caliburn.Micro;
using LearnLanguages.Navigation.EventMessages;
using LearnLanguages.Common.EventMessages;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LogoutViewModel))]
  [ViewModelMetadata("Logout")]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class LogoutViewModel : PageViewModelBase
  {
    private string _LogoutMessage = AppResources.LogoutMessage;
    public string LogoutMessage
    {
      get { return _LogoutMessage; }
      set
      {
        if (value != _LogoutMessage)
        {
          _LogoutMessage = value;
          NotifyOfPropertyChange(() => LogoutMessage);
        }
      }
    }

    //Logout() functionality is included in the LogoutNavigationButtonViewModel

    //public void Logout()
    //{
    //  UserPrincipal.Logout();
    //  Services.EventAggregator.Publish(new Events.AuthenticationChangedEventMessage());
    //}

    //public bool CanLogout
    //{
    //  get
    //  {
    //    return (Csla.ApplicationContext.User.Identity.IsAuthenticated);
    //  }
    //}

    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = ViewViewModelResources.InstructionsLogoutPage;
      Title = ViewViewModelResources.TitleLogoutPage;
      Description = ViewViewModelResources.DescriptionLogoutPage;
      ToolTip = ViewViewModelResources.ToolTipLogoutPage;
    }


  }
}
