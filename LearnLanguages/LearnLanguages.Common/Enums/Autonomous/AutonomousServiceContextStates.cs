namespace LearnLanguages.Common.Enums.Autonomous
{
  public enum AutonomousServiceContextStates
  {
    Loading,
    Loaded,
    LoadError,
    Executing,
    Executed,
    ExecuteError,
    Canceling, //change to cancel
    Canceled,
    CancelError,
    Killing, //change to abort?? (thread vocab google)
    Killed,
    KillError,
    Unloading,
    Unloaded,
    UnloadError
  }
}
