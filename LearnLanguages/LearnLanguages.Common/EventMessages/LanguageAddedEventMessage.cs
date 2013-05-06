namespace LearnLanguages.Common.EventMessages
{
  public class LanguageAddedEventMessage : LanguageEventMessage
  {
    public LanguageAddedEventMessage(string newLanguageText)
    {
      NewLanguageText = newLanguageText;
    }

    public string NewLanguageText { get; private set; }
  }
}
