namespace LearnLanguages.Common.EventMessages
{
  public class ExpandedChangedEventMessage 
  {
    public static void Publish(Interfaces.ICanExpand expandableViewModel)
    {
      var msg = new ExpandedChangedEventMessage(expandableViewModel);
      Services.EventAggregator.Publish(msg);
    }

    public ExpandedChangedEventMessage(Interfaces.ICanExpand expandableViewModel)
    {
      ViewModel = expandableViewModel;
    }

    public Interfaces.ICanExpand ViewModel { get; private set; }
  }
}
