using Caliburn.Micro;
using LearnLanguages.Common.Core;
using LearnLanguages.Common.EventMessages;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation.EventMessages;
using LearnLanguages.Study.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
namespace LearnLanguages.Study.Pages
{
  [Export(typeof(IPage))]
  [Export(typeof(SimpleProgressPage))]
  public class SimpleProgressPage : PageBase, 
                                    IHandle<NavigatedEventMessage>,
                                    IHandle<NavigatingEventMessage>
  {
    public override void Initialize()
    {
      //NAVIGATION PROPERTIES
      NavSet = StudyResources.NavSetTextLanguages;
      NavButtonOrder = int.Parse(StudyResources.SortOrderSimpleProgressPage);
      NavText = StudyResources.NavTextSimpleProgressPage;

      //CONTENT VIEWMODEL
      ContentViewModel =
        Services.Container.GetExportedValue<SimpleProgressViewModel>();

      //REGISTER WITH NAVIGATOR
      Navigation.Navigator.Ton.RegisterPage(this);

      Services.EventAggregator.Subscribe(this);
    }

    protected override void InitializeRoles()
    {
      _Roles = new List<string>()
      {
        DataAccess.DalResources.RoleAdmin,
        DataAccess.DalResources.RoleUser
      };
    }

    public void Handle(NavigatedEventMessage message)
    {
      if (message.NavigationInfo.TargetPage == this)
      {
        PageIsActive = true;

        var backgroundTask = new Task(LoadDataAsync, TaskCreationOptions.LongRunning);
        backgroundTask.Start();
      }
    }

    private async void LoadDataAsync()
    {
      ///FIRST GET THE ANALYSIS
      var vm = (SimpleProgressViewModel)ContentViewModel;

      var targetId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      var description = "SimpleProgressPage.LoadDataAsync";
      IncrementContentBusyEventMessage.Publish(description);
      //DisableNavigationRequestedEventMessage.Publish();
      try
      {
        await vm.InitializeData();
      }
      finally
      {
        DecrementContentBusyEventMessage.Publish(description);
        //EnableNavigationRequestedEventMessage.Publish();
        History.Events.ThinkedAboutTargetEvent.Publish(targetId);
      }

      ///WHILE THE USER IS ON THIS PAGE, CONTINUE TO REFINE IT
      while (PageIsActive)
      {
        //#region Thinking (try..)
        //targetId = Guid.NewGuid();
        //History.Events.ThinkingAboutTargetEvent.Publish(targetId);
        //try
        //{
        //#endregion
          await vm.RefineAnalysis(0);
        //#region (...finally) Thinked
        //}
        //finally
        //{
        //  History.Events.ThinkedAboutTargetEvent.Publish(targetId);
        //}
        //  #endregion
      }
    }

    #region private bool PageIsActive

    private object __PageIsActiveLock = new object();
    private bool _PageIsActive;
    private bool PageIsActive
    {
      get
      {
        lock (__PageIsActiveLock)
        {
          return _PageIsActive;
        }
      }
      set
      {
        lock (__PageIsActiveLock)
        {
          _PageIsActive = value;
        }
      }
    }

    #endregion
    
    public void Handle(NavigatingEventMessage message)
    {
      if (message.NavigationInfo.TargetPage != this)
        PageIsActive = false;
    }
  }
}

