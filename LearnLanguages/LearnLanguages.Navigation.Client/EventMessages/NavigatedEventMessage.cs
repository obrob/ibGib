using System;
using System.Collections.Generic;
using LearnLanguages.Navigation.Interfaces;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Navigation.EventMessages
{
  public class NavigatedEventMessage : NavigationEventMessage, 
                                       INavigatedEventMessage
  {
    public NavigatedEventMessage(NavigationInfo navigationInfo)
      : base(navigationInfo)
    {
    }

    public static void Publish(NavigationInfo navInfo)
    {
      var eventMsg = new NavigatedEventMessage(navInfo);
      Services.EventAggregator.Publish(eventMsg);
    }
  }
}
