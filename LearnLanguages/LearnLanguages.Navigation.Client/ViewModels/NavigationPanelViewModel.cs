using System;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System.Windows;
using LearnLanguages.Common.EventMessages;
using LearnLanguages.Common.Enums;

namespace LearnLanguages.Navigation.ViewModels
{
  [Export(typeof(NavigationPanelViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class NavigationPanelViewModel : Conductor<IViewModelBase>.Collection.AllActive,
                                          IHandle<AuthenticationChangedEventMessage>,
                                          IHandle<ExpandedChangedEventMessage>,
                                          INavigationPanelViewModel
  {
    #region Ctors & Init

    public NavigationPanelViewModel()
      : base()
    {
      Services.EventAggregator.Subscribe(this);
      //PopulatePanel();
      IsEnabled = false;
    }

    #endregion

    #region Properties

    private IList<INavigationSet> _NavigationSetsCache { get; set; }

    private bool _IsEnabled;
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
    #endregion

    #region Methods

    public void PopulatePanel(IList<INavigationSet> navigationSets)
    {
      Items.Clear();
      var user = Csla.ApplicationContext.User.Identity;
      _NavigationSetsCache = navigationSets;

      if (!user.IsAuthenticated)
        AddUnauthenticatedItems(navigationSets);
      else
        AddAuthenticatedItems(navigationSets);
    }
    
    private void AddAuthenticatedItems(IList<INavigationSet> navigationSets)
    {
      ///ITERATE THROUGH THE NAV SETS, ADDING THE SETS AND 
      ///THEIR CORRESPONDING PAGES WHICH THE CURRENT USER HAS
      ///THE ROLES TO USE.
      for (int i = 0; i < navigationSets.Count; i++)
      {
        var navigationSet = navigationSets[i];
        var navSetContainsPageToShow =
          //ANY PAGE IN NAVSET.PAGES
          (from pge in navigationSet.Pages
           where //WHERE...

                 //USER CAN ACCESS PAGE
                 NavigationHelper.CurrentUserCanAccessPage(pge)
                 &&
                 //PAGE.BUTTON ISN'T HIDDEN
                 !pge.HideNavButton

           //IF THE COUNT IS > 0, THEN NAV SET CONTAINS A PAGE TO SHOW
           select pge).Count() > 0;

        if (navSetContainsPageToShow)
        {
          //ADD THE NAVSET TO THE PANEL
          AddNavigationSetViewModelToPanel(navigationSet);
        }
        else
        {
          //IF NO ROLES MATCH ANY PAGES IN THIS NAVSET
          //THEN CONTINUE TO THE NEXT NAVSET ITERATION
          continue;
        }
      }
    }

    private void AddUnauthenticatedItems(IList<INavigationSet> navigationSets)
    {
      ///ITERATE THROUGH THE NAV SETS, LOOKING FOR ONES THAT
      ///HAVE PAGES REQUIRING ZERO ROLES
      for (int i = 0; i < navigationSets.Count; i++)
      {
        var navigationSet = navigationSets[i];
        ///IF THE NAVIGATION SET CONTAINS ANY PAGES FOR UNAUTHENTICATED USERS
        ///THEN ADD BOTH THE NAVSET AND EACH OF ITS PAGES THAT ALLOWS FOR 
        ///THIS.
        var navigationSetContainsUnauthenticatedItems =
          (from p in navigationSet.Pages
           where
                 //USER CAN ACCESS PAGE
                 NavigationHelper.CurrentUserCanAccessPage(p)
                 &&
                 //PAGE.BUTTON ISN'T HIDDEN
                 !p.HideNavButton

           select p).Count() > 0;
        if (navigationSetContainsUnauthenticatedItems)
        {
          //ADD THE NAVSET TO THE PANEL
          AddNavigationSetViewModelToPanel(navigationSet);
        }
        else
        {
          //CONTINUE ITERATING THROUGH NAV SETS
          continue;
        }
      }
    }

    private void AddNavigationSetViewModelToPanel(INavigationSet navigationSet)
    {
      var navSetViewModel = new NavigationSetViewModel();
      navSetViewModel.NavigationSet = navigationSet;
      navSetViewModel.ToolTip = navigationSet.ToolTip;
      Items.Add(navSetViewModel);
    }

    #endregion

    public void Handle(AuthenticationChangedEventMessage message)
    {
      PopulatePanel(_NavigationSetsCache);
    }

    public void Handle(ExpandedChangedEventMessage message)
    {
      RefreshPanelVisibility();
      //PopulatePanel(_NavigationSetsCache);
    }

    private void RefreshPanelVisibility()
    {
      //ITERATE THROUGH THE NAVIGATIONSET VIEWMODELS
      for (int i = 0; i < Items.Count; i++)
      {
        var vm = (NavigationSetViewModel)Items[i];
        vm.ItemsIsVisible = vm.TitleControl.IsExpanded;

        ////ITERATE THROUGH EACH OF THE NAVIGATIONSETBUTTONVIEWMODELS,
        ////SETTING ITS VISIBILITY TO THE NAVSET VISIBILITY
        ////ITEMS = NAVIGATION BUTTON VIEWMODELS
        //for (int j = 0; j < vm.Items.Count; j++)
        //{
        //  var navButton = (NavigationButtonViewModel)vm.Items[j];
        //  navButton.ViewModelVisibility = navSetVisibility;
        //}
      }
    }

    #region From Base

    public void OnImportsSatisfied()
    {
      //var coreViewModelName = ViewModelBase.GetCoreViewModelName(typeof(NavigationPanelViewModel));
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
    private string _ToolTip;
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
  }
}
