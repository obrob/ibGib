using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AuthenticationStatusNavigationButtonViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class AuthenticationStatusNavigationButtonViewModel : NavigationButtonViewModelBase
  {
    public override bool CanNavigateImpl()
    {
      return Csla.ApplicationContext.User.Identity.IsAuthenticated;
    }
  }
}
