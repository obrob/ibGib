using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModelBases;
using System.Threading.Tasks;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddLanguageViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddLanguageViewModel : ViewModelBase
  {
    public AddLanguageViewModel()
    {
      InitializeModelAsync();
    }

    private async Task InitializeModelAsync()
    {
      #region Thinking
      var thinkId = System.Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkId);
      #endregion
      var newLanguage = await LanguageEdit.NewLanguageEditAsync();
      #region Thinked
      History.Events.ThinkedAboutTargetEvent.Publish(thinkId);
      #endregion

      var languageViewModel = Services.Container.GetExportedValue<AddLanguageLanguageEditViewModel>();
      languageViewModel.Model = newLanguage;
      LanguageViewModel = languageViewModel;
    }

    private AddLanguageLanguageEditViewModel _LanguageViewModel;
    public AddLanguageLanguageEditViewModel LanguageViewModel
    {
      get { return _LanguageViewModel; }
      set
      {
        if (value != _LanguageViewModel)
        {
          _LanguageViewModel = value;
          NotifyOfPropertyChange(() => LanguageViewModel);
        }
      }
    }
  }
}
