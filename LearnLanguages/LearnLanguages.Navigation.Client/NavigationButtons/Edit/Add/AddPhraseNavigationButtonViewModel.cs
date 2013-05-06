using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddPhraseNavigationButtonViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class AddPhraseNavigationButtonViewModel : NavigationButtonViewModelBase
  {
    public override bool CanNavigateImpl()
    {
      var identity = Csla.ApplicationContext.User.Identity;
      return identity.IsAuthenticated;
    }
  }
}
