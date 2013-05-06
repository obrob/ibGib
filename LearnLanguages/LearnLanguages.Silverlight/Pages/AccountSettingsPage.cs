using Caliburn.Micro;
using LearnLanguages.Common.Core;
using LearnLanguages.Common.EventMessages;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Silverlight.Common;
using LearnLanguages.Silverlight.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.Pages
{
  [Export(typeof(IPage))]
  [Export(typeof(AccountSettingsPage))]
  public class AccountSettingsPage : PageBase
  {
    public override void Initialize()
    {
      //NAVIGATION PROPERTIES
      NavSet = ViewViewModelResources.NavSetTextAccountSettings;
      NavButtonOrder = int.Parse(ViewViewModelResources.SortOrderAccountSettingsPage);
      NavText = ViewViewModelResources.NavTextAccountSettingsPage;

      //CONTENT VIEWMODEL
      ContentViewModel =
        Services.Container.GetExportedValue<AccountSettingsViewModel>();

      //REGISTER WITH NAVIGATOR
      Navigation.Navigator.Ton.RegisterPage(this);

      Services.EventAggregator.Subscribe(this);
    }

    protected override void InitializeRoles()
    {
      _Roles = new List<string>()
      {
        DataAccess.DalResources.RoleUser,
        DataAccess.DalResources.RoleAdmin
      };
    }
    
  }
}
