using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(StudyASongNavigationButtonViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class StudyASongNavigationButtonViewModel : NavigationButtonViewModelBase
  {
    protected override string GetLabelText()
    {
      return ViewViewModelResources.NavigationLabelStudyASong;
    }
    public override bool CanNavigateImpl()
    {
      var identity = Csla.ApplicationContext.User.Identity;
      return identity.IsAuthenticated;
    }
  }
}
