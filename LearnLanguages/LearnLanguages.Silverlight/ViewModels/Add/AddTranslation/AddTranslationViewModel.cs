using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModelBases;
using System;
using System.Threading.Tasks;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddTranslationViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddTranslationViewModel : ViewModelBase
  {
    #region Ctors and Init

    public AddTranslationViewModel()
    {
      InitializeModelAsync();
    }

    private async Task InitializeModelAsync()
    {
      var thinkId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkId);
      var blankTranslationCreator = await BlankTranslationCreator.CreateNewAsync();
      History.Events.ThinkedAboutTargetEvent.Publish(thinkId);

      TranslationViewModel = Services.Container.GetExportedValue<AddTranslationTranslationEditViewModel>();
      TranslationViewModel.Model = blankTranslationCreator.Translation;
      HookInto(TranslationViewModel);
    }

    #endregion

    #region Properties

    private AddTranslationTranslationEditViewModel _TranslationViewModel;
    public AddTranslationTranslationEditViewModel TranslationViewModel
    {
      get { return _TranslationViewModel; }
      set
      {
        if (value != _TranslationViewModel)
        {
          _TranslationViewModel = value;
          NotifyOfPropertyChange(() => TranslationViewModel);
        }
      }
    }

    #endregion

    #region Methods

    private void HookInto(AddTranslationTranslationEditViewModel translationViewModel)
    {
      translationViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(translationViewModel_PropertyChanged);
    }

    #endregion

    #region Events

    void translationViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => CanSave);
    }

    #endregion

    #region Commands

    public bool CanSave
    {
      get
      {
        return (TranslationViewModel != null &&
                TranslationViewModel.Model != null &&
                TranslationViewModel.Model.IsSavable);
      }
    }
    public void Save()
    {
      TranslationViewModel.Model.BeginSave((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          TranslationViewModel.Model = (TranslationEdit)r.NewObject;
          //NotifyOfPropertyChange(() => CanSave);
        });
    }

    #endregion
  }
}
