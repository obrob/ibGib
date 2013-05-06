using Caliburn.Micro;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.Core;
using LearnLanguages.Common.EventMessages;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation.EventMessages;
using LearnLanguages.Silverlight.Common;
using LearnLanguages.Silverlight.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.Pages
{
  [Export(typeof(IPage))]
  [Export(typeof(LogoutPage))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class LogoutPage : PageBase,
                            IHandle<NavigatingEventMessage>,
                            IHandle<NavigatedEventMessage>

  {
    public override void Initialize()
    {
      //NAVIGATION PROPERTIES
      NavSet = ViewViewModelResources.NavSetTextGeneral;
      NavButtonOrder = int.Parse(ViewViewModelResources.SortOrderLogoutPage);
      NavText = ViewViewModelResources.NavTextLogoutPage;
      IntendedUserAuthenticationState = 
        LearnLanguages.Common.Enums.IntendedUserAuthenticationState.Authenticated;

      //CONTENT VIEWMODEL
      ContentViewModel =
        Services.Container.GetExportedValue<LogoutViewModel>();

      //REGISTER WITH NAVIGATOR
      Navigation.Navigator.Ton.RegisterPage(this);
    }

    protected override void InitializeRoles()
    {
      _Roles = new List<string>()
      {
        DataAccess.DalResources.RoleAdmin,
        DataAccess.DalResources.RoleUser
      };
    }

    public void Handle(NavigatingEventMessage message)
    {
      if (message.NavigationInfo.TargetPage != this)
        return;

      ((LogoutViewModel)ContentViewModel).LogoutMessage = ViewViewModelResources.MsgLoggingOut;
    }

    public void Handle(NavigatedEventMessage message)
    {
      //HANDLE IF WE ARE THE TARGET PAGE
      if (message.NavigationInfo.TargetPage == this)
      {
        UserPrincipal.Logout();

        AuthenticationChangedEventMessage.Publish();

        ((LogoutViewModel)ContentViewModel).LogoutMessage = ViewViewModelResources.MsgLoggedOut;
      }
      else if (Navigation.Navigator.Ton.GetPreviousPage() == this)
      {
        //HANDLE IF WE ARE THE PREVIOUS PAGE (NAVIGATED AWAY FROM)
      }
    }

  }
}
