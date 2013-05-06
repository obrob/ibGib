using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Linq;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewPhrasesItemViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewPhrasesItemViewModel : ViewModelBase<PhraseEdit, PhraseDto>
  {
    #region Ctors and Init
    public ViewPhrasesItemViewModel()
    {
      _Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
      HookInto(_Languages);
      IsChecked = false;
    }

    #endregion

    #region Fields and Properties

    private bool _SettingModel { get; set; }

    private LanguageSelectorViewModel _Languages;
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

    private bool _IsFilteredOut = false;
    public bool IsFilteredOut
    {
      get { return _IsFilteredOut; }
      set
      {
        if (value != _IsFilteredOut)
        {
          _IsFilteredOut = value;
          NotifyOfPropertyChange(() => IsFilteredOut);
        }
      }
    }

    #endregion

    #region Methods

    private void HookInto(LanguageSelectorViewModel _Languages)
    {
      _Languages.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_Languages_PropertyChanged);
    }

    void _Languages_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (!_SettingModel && (Languages.SelectedItem != null))
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
