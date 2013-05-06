using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System.Windows;
using LearnLanguages.Common.EventMessages;

namespace LearnLanguages.Navigation.ViewModels
{
  [Export(typeof(NavigationSetViewModel))]

  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class NavigationSetViewModel : Conductor<IViewModelBase>.Collection.AllActive,
                                        IViewModelBase,
                                        IHandle<ExpandedChangedEventMessage>
  {
    #region Ctors and Init

    public NavigationSetViewModel()
    {
      Services.EventAggregator.Subscribe(this);

      //ALL NAVIGATION SETS ARE VISIBLE
      _ViewModelVisibility = Visibility.Visible;

      //SETTING PRIVATE BACKING FIELD, SO NO PROP CHANGE NOTIFIED, 
      //NO REFRESHITEMSVISIBILITY
      _ItemsIsVisible = false;
      //RefreshItemsVisibility();
    }

    #endregion

    #region Properties

    private bool _IsEnabled = true;
    public bool IsEnabled
    {
      get { return _IsEnabled; }
      set
      {
        if (value != _IsEnabled)
        {
          _IsEnabled = value;
          NotifyOfPropertyChange(() => IsEnabled);
        }
      }
    }

    private bool _IsBusy;
    public bool IsBusy
    {
      get { return _IsBusy; }
      set
      {
        if (value != _IsBusy)
        {
          _IsBusy = value;
          NotifyOfPropertyChange(() => IsBusy);
        }
      }
    }

    private INavigationSet _NavigationSet;
    public INavigationSet NavigationSet
    {
      get { return _NavigationSet; }
      set
      {
        if (value != _NavigationSet)
        {
          _NavigationSet = value;
          NotifyOfPropertyChange(() => NavigationSet);
          UpdateTitleControl();
          PopulateNavigationButtonViewModels();
        }
      }
    }

    private NavigationSetButtonViewModel _TitleControl;
    /// <summary>
    /// The navigation set's button viewmodel.
    /// </summary>
    public NavigationSetButtonViewModel TitleControl
    {
      get { return _TitleControl; }
      set
      {
        if (value != _TitleControl)
        {
          _TitleControl = value;
          NotifyOfPropertyChange(() => TitleControl);
        }
      }
    }

    private bool _ItemsIsVisible;
    /// <summary>
    /// Setting this refreshes the items visibility
    /// </summary>
    public bool ItemsIsVisible
    {
      get { return _ItemsIsVisible; }
      set
      {
        if (value != _ItemsIsVisible)
        {
          _ItemsIsVisible = value;
          NotifyOfPropertyChange(() => ItemsIsVisible);
          //RefreshItemsVisibility();
        }
      }
    }

    #endregion

    #region Methods

    private void UpdateTitleControl()
    {
      TitleControl = new NavigationSetButtonViewModel(NavigationSet);
    }

    /// <summary>
    /// Clears the child Page NavigationButtonViewModels, then populates
    /// them with pages according to the current user's roles and the 
    /// page's required roles.
    /// </summary>
    public void PopulateNavigationButtonViewModels()
    {
      Items.Clear();
      if (NavigationSet == null)
        return;

      ///ADD ANY INDIVIDUAL PAGE THAT THE USER HAS A
      ///REQUIRED ROLE.
      for (int i = 0; i < NavigationSet.Pages.Count; i++)
      {
        var page = NavigationSet.Pages[i];
        if (NavigationHelper.CurrentUserCanAccessPage(page)
            &&
            !page.HideNavButton)
        {
          var navButtonVM = new NavigationButtonViewModel(page);
          ///THESE PAGES ARE ALREADY SORTED, SO WE CAN JUST
          ///ADD THEM
          Items.Add(navButtonVM);
        }
      }

      //RefreshItemsVisibility();
    }

    /// <summary>
    /// Refreshes the Visibility of each of the navigation
    /// button viewmodels.
    /// </summary>
    //private void RefreshItemsVisibility()
    //{
    //  if (ItemsIsVisible)
    //    ItemsVisibility = Visibility.Visible;
    //  else
    //    ItemsVisibility = Visibility.Collapsed;

    //  foreach (var viewModelControl in Items)
    //  {
    //    if (ItemsIsVisible)
    //      viewModelControl.ViewModelVisibility = Visibility.Visible;
    //    else
    //      viewModelControl.ViewModelVisibility = Visibility.Collapsed;
    //  }
    //}


    #endregion

    #region Events

    #endregion


    //private Visibility _ItemsVisibility;
    //public Visibility ItemsVisibility
    //{
    //  get { return Visibility.Visible; }// _ItemsVisibility; }
    //  set
    //  {
    //    if (value != _ItemsVisibility)
    //    {
    //      _ItemsVisibility = value;
    //      NotifyOfPropertyChange(() => ItemsVisibility);
    //    }
    //  }
    //}

   #region From Base

    public void OnImportsSatisfied()
    {
      //var coreViewModelName = ViewModelBase.GetCoreViewModelName(typeof(NavigationSetViewModel));
      //Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage(coreViewModelName));
    }
    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(NavigationResources.ShowGridLines); }
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

    private string _ToolTip = "NavigationSet!";
    public string ToolTip
    {
      get { return _ToolTip; }
      set
      {
        if (value != _ToolTip)
        {
          _ToolTip = value;
          NotifyOfPropertyChange(() => ToolTip);
        }
      }
    }

    private string _BusyContent;
    public string BusyContent
    {
      get { return _BusyContent; }
      set
      {
        if (value != _BusyContent)
        {
          _BusyContent = value;
          NotifyOfPropertyChange(() => BusyContent);
        }
      }
    }
    #endregion

    public void Handle(ExpandedChangedEventMessage message)
    {
      //TRIGGERS REFRESH VISIBILITY...I DON'T KNOW WHY AND I DON'T CARE AT THIS POINT.
      ItemsIsVisible = TitleControl.IsExpanded;
    }
  }
}
