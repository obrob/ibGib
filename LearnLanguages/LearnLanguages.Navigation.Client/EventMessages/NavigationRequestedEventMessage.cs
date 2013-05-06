using System;
using System.Collections.Generic;
using LearnLanguages.Navigation.Interfaces;

namespace LearnLanguages.Navigation.EventMessages
{
  /// <summary>
  /// These Request messages generate their own NavigationId in ctor because a navigation request is the 
  /// first step in the navigation process.  All proceeding navigation event messages will reference this
  /// generated NavigationId.
  /// </summary>
  public class NavigationRequestedEventMessage : NavigationEventMessage,
                                                 INavigationRequestedEventMessage
  {
    public NavigationRequestedEventMessage(NavigationInfo navigationInfo)
      : base(navigationInfo)
    {

    }

    public static void Publish(NavigationInfo navInfo)
    {
      var eventMsg = new NavigationRequestedEventMessage(navInfo);
      Services.EventAggregator.Publish(eventMsg);
    }

    ///// <summary>
    ///// Navigation requested event message, for navigating to view models using convention over configuration.
    ///// EXPECTED FORMAT: [CoreText]ViewModel
    ///// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    ///// </summary>
    ///// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    ///// <param name="queryString"></param>
    //public NavigationRequestedEventMessage(string requestedViewModelCoreText)
    //  : base(requestedViewModelCoreText, Guid.NewGuid())
    //{
    //}

    ///// <summary>
    ///// Navigation requested event message, for navigating to view models using convention over configuration.
    ///// EXPECTED FORMAT: [CoreText]ViewModel
    ///// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    ///// </summary>
    ///// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    ///// <param name="queryString">EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)</param>
    //public NavigationRequestedEventMessage(string requestedViewModelCoreText, string query)
    //  : base(requestedViewModelCoreText, query, Guid.NewGuid())
    //{
    //}

    ///// <summary>
    ///// Navigation requested event message, for navigating to view models using convention over configuration.
    ///// EXPECTED FORMAT: [CoreText]ViewModel
    ///// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    ///// </summary>
    ///// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    ///// <param name="queryEntries">EXPECTED FORMAT: [key=PropertyName, value=PropertyValueAsString] (not implemented yet)</param>
    //public NavigationRequestedEventMessage(string requestedViewModelCoreText, 
    //                                       IDictionary<string, string> queryEntries)
    //  : base(requestedViewModelCoreText, queryEntries, Guid.NewGuid())
    //{
    //}

    
  }
}
