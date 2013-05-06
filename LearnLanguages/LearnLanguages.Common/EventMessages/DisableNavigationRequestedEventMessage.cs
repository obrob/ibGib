namespace LearnLanguages.Common.EventMessages
{
  public class DisableNavigationRequestedEventMessage
  {
    public DisableNavigationRequestedEventMessage()
    {
    }

    public static void Publish()
    {
      Services.EventAggregator.Publish(new DisableNavigationRequestedEventMessage());
    }
  }
}
