using LearnLanguages.Common.Core;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;
namespace LearnLanguages.Study.Pages
{
  [Export(typeof(IPage))]
  [Export(typeof(ExecuteStudySongsPage))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class ExecuteStudySongsPage : PageBase
  {
    public override void Initialize()
    {
      //NAVIGATION PROPERTIES
      HideNavButton = true;
      NavSet = StudyResources.NavSetTextLanguages;
      //NavButtonOrder = int.Parse(StudyResources.SortOrderExecuteStudySongs);
      //NavText = StudyResources.NavTextExecuteStudySongs;

      //CONTENT VIEWMODEL
      ContentViewModel =
        Services.Container.GetExportedValue<ExecuteStudySongsViewModel>();

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
  }
}

