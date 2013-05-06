using System;
using System.Net;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Linq;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewTranslationsItemViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewTranslationsItemViewModel : TranslationEditViewModelBase<ViewTranslationsPhrasesViewModel>
  {
    #region Ctors and Init

    public ViewTranslationsItemViewModel()
    {
      //_Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
      //HookInto(_Languages);
      IsChecked = false;
    }

    #endregion

    #region Fields and Properties

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

    private bool _IsChecked;
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

    #endregion

    #region Methods

    //private void HookInto(LanguageSelectorViewModel languages)
    //{
    //  languages.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_Languages_PropertyChanged);
    //}

    //void _Languages_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //  //if (!_SettingModel)
    //  //{
    //  //  Model.BeginEdit();
    //  //  Model.Language = Languages.SelectedItem.Model;
    //  //  Model.ApplyEdit();
    //  //  NotifyOfPropertyChange(() => Model);
    //    //Model.BeginSave((s, r) =>
    //    //  {
    //    //    if (r.Error != null)
    //    //      throw r.Error;
    //    //    Model = (PhraseEdit)(r.NewObject);
    //    //  });
    //  //}
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



    #endregion
  }
}
