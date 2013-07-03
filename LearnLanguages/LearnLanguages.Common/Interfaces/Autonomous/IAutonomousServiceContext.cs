using LearnLanguages.Common.Enums.Autonomous;
using System;
using System.Threading.Tasks;
namespace LearnLanguages.Common.Interfaces.Autonomous
{
  /// <summary>
  /// Contains environment within which a service will be run, including
  /// how long services can run, how long they have to stop running, and
  /// the ability to kill a service if it isn't being responsive enough.
  /// This means that these contexts should be creating the threads/tasks
  /// with timeouts of max allowed execution time. 
  /// </summary>
  public interface IAutonomousServiceContext : IDisposable
  {
    /// <summary>
    /// The autonomous service which this context has loaded.
    /// </summary>
    IAutonomousService Service { get; }

    /// <summary>
    /// Indicates the current state of the context.
    /// </summary>
    AutonomousServiceContextStates State { get; }

    /// <summary>
    /// Loads a service. For a service to be loaded, it must be
    /// Enabled and CanExecute must be true. This method must account for
    /// the good possibility that the service does NOT load successfully,
    /// as services will be, in effect, vying for more valuable contexts
    /// in which to execute, and may overextend their reach.
    /// </summary>
    /// <param name="service">service to load</param>
    bool TryLoadService(IAutonomousService service, int timeAllowedInMs);

    Task KillAsync(int timeAllowedInMs);

    /// <summary>
    /// Begins the execution of the service.
    /// </summary>
    Task ExecuteAsync(int timeAllowedInMs);

    /// <summary>
    /// Begins the abort of the service. 
    /// </summary>
    Task AbortAsync(int timeAllowedInMs);

    /// <summary>
    /// Amount of time allocated for loading of the service before 
    /// the load times out and the context is put into an error state.
    /// </summary>
    int AllowedLoadTime { get; set; }

    /// <summary>
    /// Amount of time allocated for aborting of the service before 
    /// the abort times out and the context is put into an error state.
    /// </summary>
    int AllowedAbortTime { get; set; }

    /// <summary>
    /// Amount of time allocated for execution of the service before 
    /// the execution times out and the context is put into an error state.
    /// </summary>
    int AllowedExecuteTime { get; set; }
  }
}
