using LearnLanguages.Common.Core;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;
namespace LearnLanguages.Study.Pages
{
  [Export(typeof(IPage))]
  [Export(typeof(StudySongsPage))]
  public class StudySongsPage : PageBase
  {
    public override void Initialize()
    {
      //NAVIGATION PROPERTIES
      NavSet = StudyResources.NavSetTextLanguages;
      NavButtonOrder = int.Parse(StudyResources.SortOrderStudySongs);
      NavText = StudyResources.NavTextStudySongs;

      //CONTENT VIEWMODEL
      ContentViewModel =
        Services.Container.GetExportedValue<StudySongsViewModel>();

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

