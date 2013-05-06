using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(StudyNavigationButtonViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class StudyNavigationButtonViewModel : NavigationButtonViewModelBase
  {
    public override bool CanNavigateImpl()
    {
      var identity = Csla.ApplicationContext.User.Identity;
      return identity.IsAuthenticated;
    }
  }
}
