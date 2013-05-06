using LearnLanguages.Business.Security;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Common;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(HomeViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class HomeViewModel : PageViewModelBase
  {
    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = ViewViewModelResources.InstructionsHomePage;
      Title = ViewViewModelResources.TitleHomePage;
      Description = ViewViewModelResources.DescriptionHomePage;
      ToolTip = ViewViewModelResources.ToolTipHomePage;
    }
  }
}
