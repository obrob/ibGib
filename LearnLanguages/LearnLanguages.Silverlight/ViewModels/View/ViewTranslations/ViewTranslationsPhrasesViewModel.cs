using System;
using System.Linq;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using Caliburn.Micro;
using System.Collections.Specialized;
using System.Windows;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Silverlight.ViewModels
{
  /// <summary>
  /// This ViewModel represents the Phrases inside of a translation in the use case of 'ViewTranslations'.
  /// </summary>
  [Export(typeof(ViewTranslationsPhrasesViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class 
    ViewTranslationsPhrasesViewModel 
      : Conductor<ViewTranslationsPhrasesItemViewModel>.Collection.AllActive, 
        IViewModelBase,
        IHaveModelList<PhraseList>
  {
    #region Ctors and Init

    public ViewTranslationsPhrasesViewModel()
    {
      _InitiateDeleteVisibility = Visibility.Visible;
      _FinalizeDeleteVisibility = Visibility.Collapsed;
    }

    #endregion

    #region Properties

    private PhraseList _ModelList;
    public PhraseList ModelList
    {
      get { return _ModelList; }
      set
      {
        if (value != _ModelList)
        {
          UnhookFrom(_ModelList);
          _ModelList = value;
          HookInto(_ModelList);
          PopulateViewModels(_ModelList);
          NotifyOfPropertyChange(() => ModelList);
        }
      }
    }

    private Visibility _InitiateDeleteVisibility;
    public Visibility InitiateDeleteVisibility
    {
      get { return _InitiateDeleteVisibility; }
      set
      {
        if (value != _InitiateDeleteVisibility)
        {
          _InitiateDeleteVisibility = value;
          NotifyOfPropertyChange(() => InitiateDeleteVisibility);
        }
      }
    }

    private Visibility _FinalizeDeleteVisibility;
    public Visibility FinalizeDeleteVisibility
    {
      get { return _FinalizeDeleteVisibility; }
      set
      {
        if (value != _FinalizeDeleteVisibility)
        {
          _FinalizeDeleteVisibility = value;
          NotifyOfPropertyChange(() => FinalizeDeleteVisibility);
        }
      }
    }

    private Visibility _ViewModelVisibility;
    public Visibility ViewModelVisibility
    {
      get { return _ViewModelVisibility; }
      set
      {
        if (value != _ViewModelVisibility)
        {
          _ViewModelVisibility = value;
          NotifyOfPropertyChange(() => ViewModelVisibility);
        }
      }
    }


    #endregion

    #region Methods

    public void AddPhrase(PhraseEdit phrase)
    {
      ModelList.Add(phrase);
    }

    public bool LoadFromUri(Uri uri)
    {
      return true;
    }

    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }

    private void PopulateViewModels(PhraseList phrases)
    {
      Items.Clear();
      foreach (var phraseEdit in phrases)
      {
        var itemViewModel = Services.Container.GetExportedValue<ViewTranslationsPhrasesItemViewModel>();
        itemViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleItemViewModelChanged);
        itemViewModel.Model = phraseEdit;
        Items.Add(itemViewModel);
      }
    }

    protected virtual void HookInto(PhraseList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void UnhookFrom(PhraseList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    
    #endregion

    #region Events

    void HandleItemViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }
    protected virtual void HandleChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      //Model.BeginSave((s2, r2) =>
      //{
      //  if (r2.Error != null)
      //    throw r2.Error;
      //  Model = (PhraseList)r2.NewObject;
      //});
      //NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);

      //NotifyOfPropertyChange(() => CanCancelEdit);
    }
    protected virtual void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      //NotifyOfPropertyChange(() => CanSave);
      //NotifyOfPropertyChange(() => CanCancelEdit);
    }
    public void OnImportsSatisfied()
    {
      //Publish.PartSatisfied("ViewPhrases");
    }

    #endregion

    #region Commands

    //public bool CanSave
    //{
    //  get
    //  {
    //    return (ModelList != null && ModelList.IsSavable);
    //  }
    //}
    //public virtual void Save()
    //{
    //  ModelList.BeginSave((s, r) =>
    //  {
    //    if (r.Error != null)
    //      throw r.Error;

    //    ModelList = (PhraseList)r.NewObject;
    //    //propagate new PhraseEdits to their ViewModels
    //    PopulateViewModels(ModelList);

    //    NotifyOfPropertyChange(() => CanSave);
    //  });
    //}

    public bool CanInitiateDeleteChecked
    {
      get
      {
        bool somethingIsChecked = (from viewModel in Items
                                   where viewModel.IsChecked
                                   select viewModel).Count() > 0;

        //return somethingIsChecked && CanSave;
        return somethingIsChecked;
      }
    }
    public void InitiateDeleteChecked()
    {
      InitiateDeleteVisibility = Visibility.Collapsed;
      FinalizeDeleteVisibility = Visibility.Visible;
    }

    public void FinalizeDeleteChecked()
    {
      var checkedForDeletion = from viewModel in Items
                               where viewModel.IsChecked
                               select viewModel;

      foreach (var toDelete in checkedForDeletion)
      {
        ModelList.Remove(toDelete.Model);
      }

      PopulateViewModels(ModelList);

      //NotifyOfPropertyChange(() => CanSave);
      //if (CanSave)
      //  Save();

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
    }

    public void CancelDeleteChecked()
    {
      foreach (var item in Items)
      {
        var vm = item;
        vm.IsChecked = false;
      }

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }

    public bool CanAdd
    {
      get
      {
        //for now, this always returns true.  in the future, we may check to see if user has the privelages to add more than
        //two phrases or something similar
        return true;
      }
    }
    /// <summary>
    /// Adds a blank phrase to Phrases.
    /// </summary>
    public void Add()
    {
      ModelList.AddedNew += ModelList_AddedNew;
      ModelList.AddNew();
      ModelList.AddedNew -= ModelList_AddedNew;
      //NotifyOfPropertyChange(() => CanSave);
    }

    private async void ModelList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<PhraseEdit> e)
    {
      var phrase = e.NewObject;
      var cmd = await PhraseDefaultSetterCommand.ExecuteAsync(phrase);
      phrase = cmd.Phrase;
      PopulateViewModels(ModelList);
    }

    #endregion

    public string ToolTip
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }
  }
}
