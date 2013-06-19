using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleServices
{
  public class SleepService : LearnLanguages.Common.Interfaces.Autonomous.IAutonomousService
  {
    public SleepService(int timeToSleepInMs, bool finishWithException)
    {
      TimeToSleepInMs = timeToSleepInMs;
      FinishWithException = finishWithException;
    }

    public Guid Id { get; set; }
    public int TimeToSleepInMs { get; set; }
    public bool FinishWithException { get; set; }

    public void Execute()
    {
      Thread.Sleep(TimeToSleepInMs);
      if (FinishWithException)
        throw new Exception("My service sucks. I am throwing an exception.");

      NumIterationsCompletedThisLifetime += 1;
    }

    public void Abort(int timeAllowed)
    {
      //Does nothing. In a real service, this would set a flag.
    }

    public bool CanExecute
    {
      get { return true; }
    }

    public bool Disable()
    {
      return true;
    }

    public bool Enable()
    {
      return true;
    }

    public bool IsEnabled { get; set; }

    public bool IsExecuting { get; set; }

    public string Name { get; set; }

    public int NumIterationsCompletedThisLifetime { get; set; }

  }
}
