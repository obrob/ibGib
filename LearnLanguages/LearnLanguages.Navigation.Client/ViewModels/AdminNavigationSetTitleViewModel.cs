using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AdminNavigationSetTitleViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class AdminNavigationSetTitleViewModel : NavigationSetTitleViewModelBase
  {
    public override string GetLabelText()
    {
      return ViewViewModelResources.TitleLabelTextAdmin;
    }
  }
}
