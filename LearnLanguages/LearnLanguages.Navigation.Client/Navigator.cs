using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation.EventMessages;
using LearnLanguages.Navigation.Interfaces;
using LearnLanguages.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace LearnLanguages.Navigation
{
  /// <summary>
  /// To use this, just call RegisterPage. This automatically does
  /// the following:
  /// 1) Associates the page to the proper NavigationSet. If this 
  ///    does not exist, it first creates the NavigationSet.
  /// 2) Associates the NavigationPanel to the ShellViewModel.
  /// 3) Populates the NavigationPanel with the proper ViewModels.
  /// 4) Handles NavigationRequestedEventMessages by injecting the 
  ///    proper page's viewmodel into the shellviewmodel.
  /// </summary>
  public class Navigator //: IHandle<INavigationRequestedEventMessage>
  {
    #region Ctors and Init

    public Navigator()
    {
      Pages = new Dictionary<string, IPage>();
      NavigationSets = new Dictionary<string, INavigationSet>();
      PageHistory = new List<IPage>();
    }

    #region Singleton Pattern Members
    private static volatile Navigator _Ton;
    private static object _Lock = new object();
    public static Navigator Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new Navigator();
          }
        }

        return _Ton;
      }
    }
    #endregion

    #endregion

    #region Properties

    private Dictionary<string, IPage> Pages { get; set; }
    private Dictionary<string, INavigationSet> NavigationSets { get; set; }
    public List<IPage> PageHistory { get; set; }

    //private INavigationPanelViewModel _NavigationPanel;
    //[Import]
    //public INavigationPanelViewModel NavigationPanel
    //{
    //  get { return _NavigationPanel; }
    //  set
    //  {
    //    if (value != _NavigationPanel)
    //    {
    //      _NavigationPanel = value;
    //      NotifyOfPropertyChange(() => NavigationPanel);
    //    }
    //  }
    //}

    #endregion

    #region Methods

    public void RegisterPage(IPage page)
    {
      //CHECK FOR NULL PAGE
      if (page == null)
        throw new ArgumentNullException("page");

      //GET UNIQUE PAGE ID TEXT (NAVSET + NAVTEXT)
      string uniquePageIdText = GetUniqueIdText(page);

      //CHECK TO MAKE SURE WE DON'T ALREADY HAVE THIS UNIQUE PAGE ID TEXT
      if (Pages.ContainsKey(uniquePageIdText))
        throw new Exceptions.NavigationTextAlreadyExistsException(uniquePageIdText);

      //ADD THE PAGE TO OUR COLLECTION
      Pages.Add(uniquePageIdText, page);

      ////REGISTER PAGE WITH EVENT AGGREGATOR. IF HAS VIEWMODEL, REGISTER THAT TOO
      
      Services.EventAggregator.Subscribe(page);
      
      ////if (page.ContentViewModel != null)
      ////  Services.EventAggregator.Subscribe(page.ContentViewModel);

      //GET THE NAVSET
      INavigationSet navSet = null;
      if (NavigationSets.ContainsKey(page.NavSet))
      {
        //NAVSET ALREADY EXISTS
        navSet = NavigationSets[page.NavSet];
      }
      else
      {
        //DOESN'T EXIST, SO CREATE A NEW NAVSET AND REGISTER IT
        NavigationSet newSet = new NavigationSet();
        newSet.Text = page.NavSet;
        NavigationSets.Add(newSet.Text, newSet);
        navSet = newSet;
      }

      //ASSOCIATE THE PAGE TO THE NAVSET
      navSet.RegisterPage(page);

      //REFRESH THE NAVIGATION PANEL
      RefreshNavigationPanel();
    }

    public void UnregisterPage(IPage page)
    {
      var results = from navSet in NavigationSets
                    where navSet.Value.Pages.Contains(page)
                    select navSet.Value;
      var relevantNavSet = results.FirstOrDefault();
      if (relevantNavSet == null)
        throw new Exception("Page not found in navigation set");
      relevantNavSet.UnregisterPage(page);
      RefreshNavigationPanel();
    }

    private IShellViewModel GetShellViewModel()
    {
      var shellViewModel = Services.Container.GetExportedValue<IShellViewModel>();
      return shellViewModel;
    }

    public string GetUniqueIdText(IPage page)
    {
      var text = page.NavSet + 
                 NavigationResources.PageUniqueIdStringSeparator + 
                 page.NavText;
      return text;
    }

    /// <summary>
    /// Returns the current page. Can return null if the current page has been freed
    /// or if there is no current page (though this would be odd, if not an exception).
    /// </summary>
    public IPage GetCurrentPage()
    {
      if (PageHistory.Count != 0)
      {
        return PageHistory[PageHistory.Count - 1];
      }
      else
      {
        //ZERO PAGES IN HISTORY, SO NO CURRENT PAGE
        return null;
      }
    }

    public IPage GetPreviousPage()
    {
      //WE ONLY HAVE A PREVIOUS PAGE IF OUR PAGE HISTORY IS AT LEAST 2
      if (PageHistory.Count >= 2)
      {
        return PageHistory[PageHistory.Count - 2];
      }
      else
      {
        //ZERO PAGES IN HISTORY, SO NO PREVIOUS PAGE
        return null;
      }
    }

    public IPage GetNavigationPage(string pageIdString)
    {
      var page = Pages[pageIdString];
      return page;
    }

    public bool NavigateTo(string targetPageUniqueIdText)
    {
      var page = Pages[targetPageUniqueIdText];
      return NavigateTo(page);
    }

    /// <summary>
    /// Injects the page.ContentViewModel into the
    /// ShellViewModel, publishing event messages
    /// along the way.
    /// </summary>
    public bool NavigateTo(IPage targetPage)
    {
      var navWasSuccessful = false;

      //PUBLISH THE REQUEST EVENT MESSAGE
      var navId = Guid.NewGuid();
      var navInfo = new NavigationInfo(navId, targetPage);
      NavigationRequestedEventMessage.Publish(navInfo);

      //PUBLISH THE NAVIGATING EVENT MESSAGE
      NavigatingEventMessage.Publish(navInfo);
      try
      {
        //INJECT THE PAGE VIEWMODEL INTO THE SHELL
        var shellVM = GetShellViewModel();
        shellVM.Main = targetPage.ContentViewModel;

        //PUBLISH THE NAVIGATED EVENT MESSAGE
        NavigatedEventMessage.Publish(navInfo);

        //ADD THE PAGE TO THE HISTORY
        PageHistory.Add(targetPage);

        //SET THE RETURN VARIABLE
        navWasSuccessful = true;


      }
      catch
      {
        //PUBLISH THE FAILED EVENT MESSAGE
        NavigationFailedEventMessage.Publish(navInfo);

        //SET THE RETURN VARIABLE
        navWasSuccessful = false;
      }

      //RETURN OUR SUCCESS
      return navWasSuccessful;
    }

    //public void Handle(INavigationRequestedEventMessage message)
    //{
    //  Publish.Navigating(message.NavigationInfo);

    //  //EXTRACT THE TYPE FROM THE NAVIGATION MESSAGE
    //  string requestedContractName = "";
    //  Type requestedViewModelType = ExtractType(message);
    //  if (requestedViewModelType != null)
    //  {
    //    requestedContractName = requestedViewModelType.FullName;
    //  }
    //  else
    //  {
    //    //Services.Container.
    //    //requestedContractName = "LearnLanguages.Study.ViewModels." + message.NavigationInfo.ViewModelCoreNoSpaces + "ViewModel";
    //    ////Publish.NavigationFailed<LoginViewModel>(message.NavigationInfo.NavigationId, AppResources.BaseAddress);
    //    //Publish.NavigationFailed(message.NavigationInfo);
    //    //return;
    //  }

    //  var importDef =
    //    new ContractBasedImportDefinition(requestedContractName,
    //                                      AttributedModelServices.GetTypeIdentity(requestedViewModelType),
    //                                      null,
    //                                      ImportCardinality.ExactlyOne,
    //                                      true,
    //                                      false,
    //                                      CreationPolicy.Any);

    //  var exports = Services.Container.GetExports(importDef);
    //  //GET THE VIEWMODEL FROM THE CONTAINER
    //  //var exports = Services.Container.GetExports(requestedViewModelType, typeof(ViewModelMetadataAttribute), requestedContractName);
    //  var exportFound = false;
    //  IViewModelBase requestedViewModel = null;
    //  requestedViewModel = exports.FirstOrDefault().Value as IViewModelBase;
    //  exportFound = requestedViewModel != null;
    //  //foreach (var export in exports)
    //  //{
    //  //  requestedViewModel = export.Value as IViewModelBase;
    //  //  if (requestedViewModel != null)
    //  //    exportFound = true;

    //  //  break; //breaks out of the foreach after the first iteration.
    //  //}

    //  //IF WE FOUND A VIEWMODEL, INJECT IT IN THE SHELLVIEWMODEL
    //  if (exportFound)
    //  {
    //    var shellViewModel = GetShellViewModel();
    //    //var shellViewModel = Services.Container.GetExportedValue<IShellViewModel>();
    //    shellViewModel.Main = requestedViewModel;
    //    Navigation.Publish.Navigated(message.NavigationInfo, requestedViewModel);
    //  }
    //  else
    //  {
    //    Publish.NavigationFailed(message.NavigationInfo);
    //  }
    //}

    public void RefreshNavigationPanel()
    {
      var shellVM = GetShellViewModel();
      if (shellVM.NavigationPanelViewModel == null)
      {
        shellVM.NavigationPanelViewModel = new NavigationPanelViewModel();
      }
      var navSets = GetSortedNavigationSets();
      shellVM.NavigationPanelViewModel.PopulatePanel(navSets);
    }

    private IList<INavigationSet> GetSortedNavigationSets()
    {
      var retSortedList = new List<INavigationSet>();
      foreach (var navSetEntry in NavigationSets)
        retSortedList.Add(navSetEntry.Value);
      retSortedList.Sort((a, b) =>
        {
          //if the comparison is 0, then they are equal.
          //if a is before b, then it should be negative
          //otherwise, it should be positive
          int comparison = 0;
          comparison = a.SortOrder - b.SortOrder;
          return comparison;
        });
      return retSortedList;
    }

    #endregion

    #region Events

    #endregion


    
  }
}
