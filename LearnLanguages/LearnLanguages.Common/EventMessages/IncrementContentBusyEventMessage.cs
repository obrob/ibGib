using System.Diagnostics;
namespace LearnLanguages.Common.EventMessages
{
  public class IncrementContentBusyEventMessage
  {
    public IncrementContentBusyEventMessage(string description)
    {
      Description = description;
    }

    public string Description { get; private set; }

    public static void Publish(string description)
    {
      Services.EventAggregator.Publish(new IncrementContentBusyEventMessage(description));
    }

  }
}
