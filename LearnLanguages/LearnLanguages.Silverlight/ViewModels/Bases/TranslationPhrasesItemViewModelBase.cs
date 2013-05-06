using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Linq;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  
  //[Export(typeof(TranslationPhrasesItemViewModelBase))]
  //[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  /// <summary>
  /// This is the ViewModelBase for any PhrasesItem in a TranslationViewModel.  So this represents a 
  /// Phrase contained in a Translation.  It provides a LanguageSelectorViewModel and wirings, common to LanguageEdits.
  /// </summary>
  public class TranslationPhrasesItemViewModelBase : ViewModelBase<PhraseEdit, PhraseDto>
  {
    #region Ctors and Init

    public TranslationPhrasesItemViewModelBase()
    {
      _Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
      HookInto(_Languages);
    }

    #endregion

    #region Fields and Properties

    protected bool _SettingModel { get; set; }

    protected LanguageSelectorViewModel _Languages;
    public LanguageSelectorViewModel Languages
    {
      get { return _Languages; }
      set
      {
        if (value != _Languages)
        {
          _Languages = value;
          NotifyOfPropertyChange(() => Languages);
        }
      }
    }

    
    #endregion

    #region Methods

    protected void HookInto(LanguageSelectorViewModel _Languages)
    {
      _Languages.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_Languages_PropertyChanged);
    }

    protected void _Languages_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (!_SettingModel)
      {
        Model.BeginEdit();
        Model.Language = Languages.SelectedItem.Model;
        Model.ApplyEdit();
        NotifyOfPropertyChange(() => Model);
        //Model.BeginSave((s, r) =>
        //  {
        //    if (r.Error != null)
        //      throw r.Error;
        //    Model = (PhraseEdit)(r.NewObject);
        //  });
      }
    }

    public override void SetModel(PhraseEdit model)
    {
      _SettingModel = true;
      {
        base.SetModel(model);
        if (model != null)
        {
          var languageViewModel = Services.Container.GetExportedValue<LanguageEditViewModel>();
          languageViewModel.Model = model.Language;
          Languages.SelectedItem = (from l in Languages.Items
                                    where l.Model.Id == model.LanguageId
                                    select l).FirstOrDefault();
          Languages.SelectedItem = languageViewModel;
        }
        else
        {
          Languages.SelectedItem = null;
        }
      }
      _SettingModel = false;
    }
    #endregion
  }
}
