using LearnLanguages.Navigation.Interfaces;

namespace LearnLanguages.Navigation.EventMessages
{
  public abstract class NavigationEventMessage : Interfaces.INavigationEventMessage
  {
    public NavigationEventMessage(NavigationInfo navigationInfo)
    {
      NavigationInfo = navigationInfo;
    }

    public NavigationInfo NavigationInfo { get; private set; }
  }
}
