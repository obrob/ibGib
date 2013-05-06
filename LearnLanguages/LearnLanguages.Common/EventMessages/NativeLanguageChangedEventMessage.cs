using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.EventMessages
{
  public class NativeLanguageChangedEventMessage
  {
    public static void Publish(string newNativeLanguageText)
    {
      var msg = new NativeLanguageChangedEventMessage(newNativeLanguageText);
      Services.EventAggregator.Publish(msg);
    }

    public NativeLanguageChangedEventMessage(string newNativeLanguageText)
    {
      NewNativeLanguageText = newNativeLanguageText;
    }

    public string NewNativeLanguageText { get; private set; }
  }
}
