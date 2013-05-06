using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewPhrasesNavigationButtonViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class ViewPhrasesNavigationButtonViewModel : NavigationButtonViewModelBase
  {
    public override bool CanNavigateImpl()
    {
      var identity = Csla.ApplicationContext.User.Identity;
      return identity.IsAuthenticated;
    }
  }
}
