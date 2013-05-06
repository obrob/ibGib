using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(ExecuteStudySongsViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class ExecuteStudySongsViewModel : PageViewModelBase
  {
    public ExecuteStudySongsViewModel()
    {
    }

    private IViewModelBase _StudyScreen;
    public IViewModelBase StudyScreen
    {
      get { return _StudyScreen; }
      set
      {
        if (value != _StudyScreen)
        {
          _StudyScreen = value;
          NotifyOfPropertyChange(() => StudyScreen);
        }
      }
    }

    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = StudyResources.InstructionsExecuteStudySongsPage;
      Title = StudyResources.TitleExecuteStudySongsPage;
      Description = StudyResources.DescriptionExecuteStudySongsPage;
      ToolTip = StudyResources.ToolTipExecuteStudySongsPage;
    }
  }
}
