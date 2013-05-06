using Caliburn.Micro;
using LearnLanguages.Common.Core;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation.EventMessages;
using LearnLanguages.Silverlight.Common;
using LearnLanguages.Silverlight.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.Pages
{
  [Export(typeof(IPage))]
  [Export(typeof(LoginPage))]
  public class LoginPage : PageBase,
                           IHandle<NavigatedEventMessage>
  {
    public override void Initialize()
    {
      //NAVIGATION PROPERTIES
      NavSet = ViewViewModelResources.NavSetTextGeneral;
      NavButtonOrder = int.Parse(ViewViewModelResources.SortOrderLoginPage);
      NavText = ViewViewModelResources.NavTextLoginPage;
      IntendedUserAuthenticationState = 
        LearnLanguages.Common.Enums.IntendedUserAuthenticationState.NotAuthenticated;

      //CONTENT VIEWMODEL
      ContentViewModel =
        Services.Container.GetExportedValue<LoginViewModel>();

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


    public void Handle(NavigatedEventMessage message)
    {
      if (Navigation.Navigator.Ton.GetPreviousPage() == this)
      {
        //IF WE ARE MOVING AWAY, THEN GET RID OF OUR CONTENT VIEWMODEL
        var loginVM = (LoginViewModel)ContentViewModel;
        loginVM.Username = "";
        loginVM.Password = "";
      }
    }

  }
}
