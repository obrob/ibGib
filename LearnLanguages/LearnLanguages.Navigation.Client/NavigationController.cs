//using System;
//using System.Linq;
//using System.ComponentModel.Composition;
//using LearnLanguages.Navigation;
//using LearnLanguages.Navigation.Interfaces;
//using LearnLanguages.Common.Interfaces;
//using Caliburn.Micro;
//using System.ComponentModel.Composition.Primitives;
//using LearnLanguages.Common.EventMessages;

//namespace LearnLanguages.Silverlight
//{
//  /// <summary>
//  /// This controller listens only to Authentication and Navigation_***REQUESTED***_EventMessages.  It then publishes
//  /// Navigating, Navigated, NavigationFailed events when appropriate, each of which this controller does NOT listen to.  
//  /// Each is published with the same, essentially readonly NavigationInfo object.
//  /// </summary>
//  [Export(typeof(INavigationController))]
//  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
//  public class NavigationController : INavigationController,
//                                      IHandle<AuthenticationChangedEventMessage>//,
//                                      //IPartImportsSatisfiedNotification
//  {
//    public NavigationController()
//    {
//      Services.EventAggregator.Subscribe(this);
//    }
    
//    //public void OnImportsSatisfied()
//    //{
//    //  Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage("NavigationController"));
//    //  //Services.EventAggregator.Publish(
//    //  //  (Interfaces.INavigationRequestedEventMessage)(new Events.NavigationRequestedEventMessage("Login")));
//    //}
    
//    #region Handle Event Messages

//    public void Handle(AuthenticationChangedEventMessage message)
//    {
//      if (message.IsAuthenticated)
//        //Navigation.Publish.NavigationRequest<AuthenticationStatusViewModel>(AppResources.BaseAddress);
//        Navigation.Publish.NavigationRequest<HomeViewModel>(AppResources.BaseAddress);
//      else
//        Navigation.Publish.NavigationRequest<LoginViewModel>(AppResources.BaseAddress);
//    }

//    public void Handle(INavigationRequestedEventMessage message)
//    {
//      Publish.Navigating(message.NavigationInfo);

//      //EXTRACT THE TYPE FROM THE NAVIGATION MESSAGE
//      string requestedContractName = "";
//      Type requestedViewModelType = ExtractType(message);
//      if (requestedViewModelType != null)
//      {
//        requestedContractName = requestedViewModelType.FullName;
//      }
//      else
//      {
//        //Services.Container.
//        //requestedContractName = "LearnLanguages.Study.ViewModels." + message.NavigationInfo.ViewModelCoreNoSpaces + "ViewModel";
//        ////Publish.NavigationFailed<LoginViewModel>(message.NavigationInfo.NavigationId, AppResources.BaseAddress);
//        //Publish.NavigationFailed(message.NavigationInfo);
//        //return;
//      }

//      var importDef = 
//        new ContractBasedImportDefinition(requestedContractName, 
//                                          AttributedModelServices.GetTypeIdentity(requestedViewModelType),
//                                          null, 
//                                          ImportCardinality.ExactlyOne, 
//                                          true, 
//                                          false, 
//                                          CreationPolicy.Any);

//      var exports = Services.Container.GetExports(importDef);
//      //GET THE VIEWMODEL FROM THE CONTAINER
//      //var exports = Services.Container.GetExports(requestedViewModelType, typeof(ViewModelMetadataAttribute), requestedContractName);
//      var exportFound = false;
//      IViewModelBase requestedViewModel = null;
//      requestedViewModel = exports.FirstOrDefault().Value as IViewModelBase;
//      exportFound = requestedViewModel != null;
//      //foreach (var export in exports)
//      //{
//      //  requestedViewModel = export.Value as IViewModelBase;
//      //  if (requestedViewModel != null)
//      //    exportFound = true;

//      //  break; //breaks out of the foreach after the first iteration.
//      //}

//      //IF WE FOUND A VIEWMODEL, INJECT IT IN THE SHELLVIEWMODEL
//      if (exportFound)
//      {
//        var shellViewModel = Services.Container.GetExportedValue<ViewModels.ShellViewModel>();
//        shellViewModel.Main = requestedViewModel;
//        Navigation.Publish.Navigated(message.NavigationInfo, requestedViewModel);
//      }
//      else
//      {
//        Publish.NavigationFailed(message.NavigationInfo);
//      }
//    }

//    #endregion

//    #region Helper Methods

//    private string ExtractContractName(INavigationRequestedEventMessage message)
//    {
//      return message.NavigationInfo.ViewModelCoreNoSpaces + @"ViewModel";
//    }
//    private Type ExtractType(INavigationRequestedEventMessage message)
//    {
//      //FORMAT: [message]ViewModel
//      var retType = Type.GetType(@"LearnLanguages.Silverlight.ViewModels." + message.NavigationInfo.ViewModelCoreNoSpaces + @"ViewModel");

//      return retType;
//    }

//    #endregion
//  }
//}
