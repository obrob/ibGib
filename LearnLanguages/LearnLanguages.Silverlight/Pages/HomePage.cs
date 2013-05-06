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
  [Export(typeof(HomePage))]
  public class HomePage : PageBase,
                          IHandle<AuthenticationChangedEventMessage>
  {
    public override void Initialize()
    {
      //NAVIGATION PROPERTIES
      NavSet = ViewViewModelResources.NavSetTextHome;
      NavButtonOrder = int.Parse(ViewViewModelResources.SortOrderHomePage);
      NavText = ViewViewModelResources.NavTextHomePage;

      //CONTENT VIEWMODEL
      ContentViewModel =
        Services.Container.GetExportedValue<HomeViewModel>();

      //REGISTER WITH NAVIGATOR
      Navigation.Navigator.Ton.RegisterPage(this);

      Services.EventAggregator.Subscribe(this);
    }

    protected override void InitializeRoles()
    {
      _Roles = new List<string>()
      {
        DataAccess.DalResources.RoleAdmin,
        DataAccess.DalResources.RoleUser
      };
    }

    public void Handle(AuthenticationChangedEventMessage message)
    {
      if (message.IsAuthenticated)
        Navigation.Navigator.Ton.NavigateTo(this);
    }
  }
}
