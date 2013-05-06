using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(EditNavigationSetTitleViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class EditNavigationSetTitleViewModel : NavigationSetTitleViewModelBase
  {
    public override string GetLabelText()
    {
      return ViewViewModelResources.TitleLabelTextEdit;
    }
  }
}
