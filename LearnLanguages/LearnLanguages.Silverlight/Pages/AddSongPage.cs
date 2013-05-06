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
  [Export(typeof(AddSongPage))]
  public class AddSongPage : PageBase
  {
    public override void Initialize()
    {
      //NAVIGATION PROPERTIES
      NavSet = ViewViewModelResources.NavSetTextAddSong;
      NavButtonOrder = int.Parse(ViewViewModelResources.SortOrderAddSongPage);
      NavText = ViewViewModelResources.NavTextAddSongPage;

      //CONTENT VIEWMODEL
      ContentViewModel =
        Services.Container.GetExportedValue<AddSongViewModel>();

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

    
  }
}
