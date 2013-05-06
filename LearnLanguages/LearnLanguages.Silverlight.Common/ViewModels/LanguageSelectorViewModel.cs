﻿using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Linq;
using LearnLanguages.Common.ViewModelBases;
using System.Threading.Tasks;
using LearnLanguages.Common.EventMessages;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.Common.ViewModels
{
  [Export(typeof(LanguageSelectorViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  //public class LanguageSelectorViewModel : Conductor<LanguageEditViewModel>.Collection.OneActive, Interfaces.IViewModelBase
  public class LanguageSelectorViewModel : ViewModelBase, 
                                           IHandle<LanguageEventMessage>
  {
    #region Ctors and Init

    public LanguageSelectorViewModel()
    {
      IsEnabled = true;
      Items = new BindableCollection<LanguageEditViewModel>();
      Services.EventAggregator.Subscribe(this);

      var suppress = ReloadLanguages();
    }

    #endregion

    #region Properties

    private BindableCollection<LanguageEditViewModel> _Items;
    public BindableCollection<LanguageEditViewModel> Items
    {
      get { return _Items; }
      set
      {
        if (value != _Items)
        {
          _Items = value;
          NotifyOfPropertyChange(() => Items);
        }
      }
    }

    private LanguageEditViewModel _SelectedItem;
    public LanguageEditViewModel SelectedItem
    {
      get { return _SelectedItem; }
      set
      {
        if (value != _SelectedItem)
        {
          _SelectedItem = value;
          NotifyOfPropertyChange(() => SelectedItem);
          if (SelectedItemChanged != null)
            SelectedItemChanged(_SelectedItem, EventArgs.Empty);
        }
      }
    }

    //private bool _IsEnabled = true;
    //public bool IsEnabled
    //{
    //  get { return _IsEnabled; }
    //  set
    //  {
    //    if (value != _IsEnabled)
    //    {
    //      _IsEnabled = value;
    //      NotifyOfPropertyChange(() => IsEnabled);
    //    }
    //  }
    //}

    public string AddLanguageToolTip { get { return ViewViewModelResources.ToolTipTextAddLanguage; } }

    #endregion

    #region Methods

    private async Task ReloadLanguages()
    {
      Items.Clear();

      var allLanguages = await LanguageList.GetAllAsync();
      for (int i = 0; i < allLanguages.Count; i++)
      {
        var languageEdit = allLanguages[i];
        var languageViewModel = Services.Container.GetExportedValue<LanguageEditViewModel>();
        languageViewModel.Model = languageEdit;
        Items.Add(languageViewModel);
      }
     
      if (SelectedItem != null && SelectedItem.Model != null)
      {
        //our currently selected item is not the actual item in the list, as it is being set
        //before our list is getting populated.
        SelectedItem = (from language in Items
                        where language.Model.Id == SelectedItem.Model.Id
                        select language).FirstOrDefault();
      }

      NotifyOfPropertyChange(() => Items);
      NotifyOfPropertyChange(() => SelectedItem);
      //SelectedItem = Items[0];
      //SelectedItem = Items[0];
      //foreach (var language in allLanguages)
      //{
      //  //var languageViewModel = Services.Container.GetExportedValue<LanguageEditViewModel>();
      //  //languageViewModel.Model = language;
      //  //Items.Add(languageViewModel);
      //  Items.Add(language);
      //}

      //ChangeActiveItem(Items[0], true);
    }
    public void AddLanguage()
    {
      var addLanguageViewModel = Services.Container.GetExportedValue<AddLanguageViewModel>();
      Services.WindowManager.ShowDialog(addLanguageViewModel);
    }

    public void Handle(LanguageEventMessage message)
    {
      var suppress = ReloadLanguages();
    }
    
    //public bool LoadFromUri(Uri uri)
    //{
    //  return true;
    //}
    //public void OnImportsSatisfied()
    //{
    //  //Navigation.Publish.PartSatisfied(ViewModelBase.GetCoreViewModelName(GetType()));
    //}
    //public bool ShowGridLines
    //{
    //  get { return bool.Parse(ViewViewModelResources.ShowGridLines); }
    //}


    
    #endregion

    #region Events
    
    public event EventHandler SelectedItemChanged;

    #endregion
  }
}
