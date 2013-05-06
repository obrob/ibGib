using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System.Windows;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(IWantToLearnViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class IWantToLearnViewModel : Conductor<ViewModelBase>.Collection.AllActive,
                                       IViewModelBase
  {
    public IWantToLearnViewModel()
    {
      PopulateButtons();
    }

    private void PopulateButtons()
    {
      Items.Clear();

      //WILL USE THIS SORTED LIST
      var sortedItems = new List<ViewModelBase>();

      //ADD THESE BUTTONS
      var iWantToLearnASongNavButtonViewModel = Services.Container.GetExportedValue<IWantToLearnASongNavigationButtonViewModel>();
      sortedItems.Add(iWantToLearnASongNavButtonViewModel);

      //SORT THE BUTTONS COMPARER
      Comparison<ViewModelBase> comparison = (a, b) =>
        {
          string prefix = "IWantToLearn";
          string aName = a.GetType().Name;
          string bName = b.GetType().Name;
          int sortByIndex = 0;
          if (aName.Contains(prefix) && aName.Length > prefix.Length &&
              bName.Contains(prefix) && bName.Length > prefix.Length)
            sortByIndex = prefix.Length; //we want to sort by the next letter after the prefix

          if (aName[sortByIndex] < bName[sortByIndex])
            return -1;
          else
            return 1;
        };

      //DO THE SORT
      sortedItems.Sort(comparison);

      //INSERT SORTED ITEMS INTO ITEMS LIST
      Items.Clear();
      for (int i = 0; i < sortedItems.Count; i++)
      {
        Items.Insert(i, sortedItems[i]);
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

    public void OnImportsSatisfied()
    {
      //var coreViewModelName = ViewModelBase.GetCoreViewModelName(typeof(IWantToLearnViewModel));
      //Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage(coreViewModelName));
    }
    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }

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
