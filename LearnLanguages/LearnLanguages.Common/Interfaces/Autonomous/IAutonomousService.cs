namespace LearnLanguages.Common.Interfaces.Autonomous
{
  /// <summary>
  /// External classes (manager, context, possibly others in the future) 
  /// use this as a dumb executor in a controlled environment. This 
  /// things job is to let others know if it can do anything (if it is
  /// flagged as enabled), and if it can, then others can use it to
  /// execute.
  /// </summary>
  public interface IAutonomousService : IHaveId
  {
    /// <summary>
    /// The services name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Indicates whether or not the service is able to run.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Indicates if the service is currently executing
    /// </summary>
    bool IsExecuting { get; }

    /// <summary>
    /// Turn on the service, putting it in a state to be able to execute.
    /// Returns true if enable was successful, false otherwise.
    /// </summary>
    /// <returns>true if enable was successful, false otherwise</returns>
    bool Enable();
    /// <summary>
    /// Turn off the service, putting it in a state to NO LONGER be able to execute.
    /// Returns true if enable was successful, false otherwise.
    /// </summary>
    /// <returns>true if enable was successful, false otherwise</returns>
    bool Disable();

    /// <summary>
    /// Can this object perform an iteration of execution? Is it ready to go?
    /// </summary>
    bool CanExecute { get; }

    /// <summary>
    /// Executes the behavior of this service. At the most granular level,
    /// this should be a single iteration of execution. Or, IOW, this service
    /// will be measured/controlled by external things, so it should provide
    /// as stable behavior as possible, and that is most easily achieved 
    /// through as small and manageable chunks of execution as possible.
    /// </summary>
    void Execute();

    /// <summary>
    /// Number of iterations that this service has completed during
    /// this service's lifespan (since its constructor was called).
    /// </summary>
    int NumIterationsCompletedThisLifetime { get; }

    /// <summary>
    /// Aborts the service's execution within the given timeAllowed. 
    /// This is NOT an async timeout. This value is how long the service is being 
    /// given before that service's executing thread is no longer guaranteed. After
    /// that time, the thread may be stopped dead, and the service will be 
    /// marked as an untrustworthy service. 
    /// </summary>
    void Abort(int timeAllowed);
  }
}
