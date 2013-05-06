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
  [Export(typeof(ManageUsersPage))]
  public class ManageUsersPage : PageBase
  {
    public override void Initialize()
    {
      //NAVIGATION PROPERTIES
      NavSet = ViewViewModelResources.NavSetTextAdmin;
      NavButtonOrder = int.Parse(ViewViewModelResources.SortOrderManageUsersPage);
      NavText = ViewViewModelResources.NavTextManageUsersPage;

      //CONTENT VIEWMODEL
      ContentViewModel =
        Services.Container.GetExportedValue<ManageUsersViewModel>();

      //REGISTER WITH NAVIGATOR
      Navigation.Navigator.Ton.RegisterPage(this);

      Services.EventAggregator.Subscribe(this);
    }

    protected override void InitializeRoles()
    {
      _Roles = new List<string>()
      {
        DataAccess.DalResources.RoleAdmin
      };
    }
    
  }
}
