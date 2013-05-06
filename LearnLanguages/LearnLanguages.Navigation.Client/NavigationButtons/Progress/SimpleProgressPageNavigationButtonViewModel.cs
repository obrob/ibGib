using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(SimpleProgressPageNavigationButtonViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class SimpleProgressPageNavigationButtonViewModel : NavigationButtonViewModelBase
  {
    protected override string GetLabelText()
    {
      return ViewViewModelResources.NavigationLabelSimpleProgressPage;
    }
    public override bool CanNavigateImpl()
    {
      var identity = Csla.ApplicationContext.User.Identity;
      return identity.IsAuthenticated;
    }
  }
}
