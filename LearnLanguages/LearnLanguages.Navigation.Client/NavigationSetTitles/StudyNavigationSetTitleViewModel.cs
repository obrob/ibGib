using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(StudyNavigationSetTitleViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class StudyNavigationSetTitleViewModel : NavigationSetTitleViewModelBase
  {
    public override string GetLabelText()
    {
      return ViewViewModelResources.TitleLabelTextStudy;
    }
  }
}
