namespace LearnLanguages.Common.EventMessages
{
  public class DecrementContentBusyEventMessage
  {
    public DecrementContentBusyEventMessage(string description)
    {
      Description = description;
    }

    public string Description { get; private set; }

    public static void Publish(string description)
    {
      Services.EventAggregator.Publish(new DecrementContentBusyEventMessage(description));
    }

  }
}
