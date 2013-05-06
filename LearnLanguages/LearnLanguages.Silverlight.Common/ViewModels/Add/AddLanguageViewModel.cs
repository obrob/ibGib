using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModelBases;
using System.Threading.Tasks;

namespace LearnLanguages.Silverlight.Common.ViewModels
{
  [Export(typeof(AddLanguageViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddLanguageViewModel : ViewModelBase
  {
    public AddLanguageViewModel()
    {
      DisplayName = ViewViewModelResources.TitleAddLanguage;
      var suppress = InitializeModelAsync();
    }

    private async Task InitializeModelAsync()
    {
      LanguageEdit newLanguage = null;
      #region Thinking
      var thinkId = System.Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkId);
      try
      {
      #endregion
        newLanguage = await LanguageEdit.NewLanguageEditAsync();
      #region Thinked
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(thinkId);
      }
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
