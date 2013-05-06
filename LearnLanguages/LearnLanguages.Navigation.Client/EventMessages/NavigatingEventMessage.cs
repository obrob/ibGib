using System;
using System.Collections.Generic;
using LearnLanguages.Navigation.Interfaces;

namespace LearnLanguages.Navigation.EventMessages
{
  public class NavigatingEventMessage : NavigationEventMessage, 
                                        INavigatingEventMessage
  {
    public NavigatingEventMessage(NavigationInfo navigationInfo)
      : base(navigationInfo)
    {

    }

    public static void Publish(NavigationInfo navigationInfo)
    {
      var eventMsg = new NavigatingEventMessage(navigationInfo);
      Services.EventAggregator.Publish(eventMsg);
    }

    ///// <summary>
    ///// Navigation requested event message, for navigating to view models using convention over configuration.
    ///// EXPECTED FORMAT: [CoreText]ViewModel
    ///// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    ///// </summary>
    ///// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    ///// <param name="queryString"></param>
    //public NavigatingEventMessage(string requestedViewModelCoreText, Guid navigationId)
    //  : base(requestedViewModelCoreText, navigationId)
    //{
    //}

    //public NavigatingEventMessage(string requestedViewModelCoreText, string query, Guid navigationId)
    //  : base(requestedViewModelCoreText, query, navigationId)
    //{
    //}

    //public NavigatingEventMessage(string requestedViewModelCoreText, 
    //                              IDictionary<string, string> queryEntries,
    //                              Guid navigationId)
    //  : base(requestedViewModelCoreText, queryEntries, navigationId)
    //{
    //}
  }
}
