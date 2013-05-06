using Caliburn.Micro;
using System.Diagnostics;
//using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Navigation.Interfaces;

namespace LearnLanguages.Silverlight
{
  public class DebugEventMessageListener : IHandle<INavigationEventMessage>
  {
    public DebugEventMessageListener()
    {
      Services.EventAggregator.Subscribe(this);
    }

    public void Handle(INavigationEventMessage message)
    {
      //Debug.WriteLine("NavigationEventMessage Start");
      Debug.WriteLine(message.GetType().Name + " Start");
      Debug.WriteLine("NavigationId: " + message.NavigationInfo.NavigationId.ToString());
      Debug.WriteLine(message.NavigationInfo.TargetPage.NavSet);
      Debug.WriteLine(message.NavigationInfo.TargetPage.NavText);
      foreach (var role in message.NavigationInfo.TargetPage.Roles)
        Debug.WriteLine("TargetPage role: " + role);
      if (message.NavigationInfo.Uri == null)
        Debug.WriteLine("NavigationInfo.Uri == null");
      else
        Debug.WriteLine(message.NavigationInfo.Uri.ToString());
      Debug.WriteLine("\r\n");
    }
  }
}
