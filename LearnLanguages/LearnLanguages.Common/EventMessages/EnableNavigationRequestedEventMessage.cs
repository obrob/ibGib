namespace LearnLanguages.Common.EventMessages
{
  public class EnableNavigationRequestedEventMessage
  {
    public EnableNavigationRequestedEventMessage()
    {
    }

    public static void Publish()
    {
      Services.EventAggregator.Publish(new EnableNavigationRequestedEventMessage());
    }
  }
}
