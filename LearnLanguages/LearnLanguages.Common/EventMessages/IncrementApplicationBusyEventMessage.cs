namespace LearnLanguages.Common.EventMessages
{
  public class IncrementApplicationBusyEventMessage
  {
    public IncrementApplicationBusyEventMessage(string description)
    {
      Description = description;
    }

    public string Description { get; private set; }

    public static void Publish(string description)
    {
      Services.EventAggregator.Publish(new IncrementApplicationBusyEventMessage(description));
    }
  }
}
