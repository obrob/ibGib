using System;
using LearnLanguages.Common.Interfaces;
using System.Diagnostics;
using System.ComponentModel.Composition;

namespace LearnLanguages
{
  //[Export(typeof(ILogger))]
  //I manually add this in the bootstrapper
  public class DebugLogger : ILogger
  {
    public void Log(string msg, LogPriority priority, LogCategory category)
    {
      string padding = "------";

      Debug.WriteLine(padding);

      Debug.WriteLine(category.ToString());
      Debug.WriteLine(priority.ToString());
      Debug.WriteLine(msg);

      Debug.WriteLine(padding);
    }

    public void Handle(Common.Events.MessageEvent message)
    {
      //HANDLE MESSAGE EVENT IMPLEMENTATION IN DEBUGLOGGER (AND CREATE PRIORITY/TYPE CONVERTER)
      //var logPriority = LogPriority.High
      //throw new NotImplementedException();
    }
  }
}
