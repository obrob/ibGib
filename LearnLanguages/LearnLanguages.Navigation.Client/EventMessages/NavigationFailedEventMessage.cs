using LearnLanguages.Navigation.Interfaces;

namespace LearnLanguages.Navigation.EventMessages
{
  public class NavigationFailedEventMessage : NavigationEventMessage, 
                                              INavigationFailedEventMessage
  {
    public NavigationFailedEventMessage(NavigationInfo navigationInfo)
      : base(navigationInfo)
    {

    }

    ///// <summary>
    ///// Navigation requested event message, for navigating to view models using convention over configuration.
    ///// EXPECTED FORMAT: [CoreText]ViewModel
    ///// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    ///// </summary>
    ///// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    ///// <param name="queryString"></param>
    //public NavigationFailedEventMessage(string requestedViewModelCoreText, Guid navigationId, string errorInfo)
    //  : base(requestedViewModelCoreText, navigationId)
    //{
    //  ErrorInfo = errorInfo;
    //}

    //public NavigationFailedEventMessage(string requestedViewModelCoreText, 
    //                                    string query, 
    //                                    Guid navigationId, 
    //                                    string errorInfo)
    //  : base(requestedViewModelCoreText, query, navigationId)
    //{
    //}

    //public NavigationFailedEventMessage(string requestedViewModelCoreText, 
    //                                    IDictionary<string, string> queryEntries,
    //                                    Guid navigationId)
    //  : base(requestedViewModelCoreText, queryEntries, navigationId)
    //{
    //}

    //public string ErrorInfo { get; private set; }

    public static void Publish(NavigationInfo navInfo)
    {
      var eventMsg = new NavigationFailedEventMessage(navInfo);
      Services.EventAggregator.Publish(eventMsg);
    }
  }
}
