using System;
using System.Net;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Linq;

namespace LearnLanguages.Silverlight.ViewModels
{
  /// <summary>
  /// This ViewModel represents a single Phrase inside of a Translation.Phrases in the use case of 'ViewTranslations'.
  /// </summary>
  [Export(typeof(ViewTranslationsPhrasesItemViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  //public class ViewTranslationsTranslationPhrasesItemViewModel : ViewModelBase<PhraseEdit, PhraseDto>
  public class ViewTranslationsPhrasesItemViewModel : TranslationPhrasesItemViewModelBase
  {
    public ViewTranslationsPhrasesItemViewModel()
      : base()
    {
      _IsChecked = false;
    }

    protected bool _IsChecked;
    public bool IsChecked
    {
      get { return _IsChecked; }
      set
      {
        if (value != _IsChecked)
        {
          _IsChecked = value;
          NotifyOfPropertyChange(() => IsChecked);
        }
      }
    }


    //#region Ctors and Init
    //public ViewTranslationsTranslationPhrasesItemViewModel()
    //{
    //  _Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
    //  HookInto(_Languages);
    //  IsChecked = false;
    //}

    //#endregion

    //#region Fields and Properties

    //private bool _SettingModel { get; set; }

    //private LanguageSelectorViewModel _Languages;
    //public LanguageSelectorViewModel Languages
    //{
    //  get { return _Languages; }
    //  set
    //  {
    //    if (value != _Languages)
    //    {
    //      _Languages = value;
    //      NotifyOfPropertyChange(() => Languages);
    //    }
    //  }
    //}

    //private bool _IsChecked;
    //public bool IsChecked
    //{
    //  get { return _IsChecked; }
    //  set
    //  {
    //    if (value != _IsChecked)
    //    {
    //      _IsChecked = value;
    //      NotifyOfPropertyChange(() => IsChecked);
    //    }
    //  }
    //}

    //#endregion

    //#region Methods

    //private void HookInto(LanguageSelectorViewModel _Languages)
    //{
    //  _Languages.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_Languages_PropertyChanged);
    //}

    //void _Languages_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //  if (!_SettingModel)
    //  {
    //    Model.BeginEdit();
    //    Model.Language = Languages.SelectedItem.Model;
    //    Model.ApplyEdit();
    //    NotifyOfPropertyChange(() => Model);
    //    //Model.BeginSave((s, r) =>
    //    //  {
    //    //    if (r.Error != null)
    //    //      throw r.Error;
    //    //    Model = (PhraseEdit)(r.NewObject);
    //    //  });
    //  }
    //}

    //public override void SetModel(PhraseEdit model)
    //{
    //  _SettingModel = true;
    //  {
    //    base.SetModel(model);
    //    if (model != null)
    //    {
    //      var languageViewModel = Services.Container.GetExportedValue<LanguageEditViewModel>();
    //      languageViewModel.Model = model.Language;
    //      Languages.SelectedItem = (from l in Languages.Items
    //                                where l.Model.Id == model.LanguageId
    //                                select l).FirstOrDefault();
    //      Languages.SelectedItem = languageViewModel;
    //    }
    //    else
    //    {
    //      Languages.SelectedItem = null;
    //    }
    //  }
    //  _SettingModel = false;
    //}
    //#endregion
  }
}
