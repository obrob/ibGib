using LearnLanguages.Common.Enums.Autonomous;
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
  public interface IAutonomousServiceContext
  {
    /// <summary>
    /// The autonomous service which this context has loaded.
    /// </summary>
    IAutonomousService LoadedService { get; }

    /// <summary>
    /// Indicates the current state of the context.
    /// </summary>
    AutonomousServiceContextStates CurrentState { get; }

    /// <summary>
    /// Loads a service. For a service to be loaded, it must be
    /// Enabled and CanExecute must be true. This method must account for
    /// the good possibility that the service does NOT load successfully,
    /// as services will be, in effect, vying for more valuable contexts
    /// in which to execute, and may overextend their reach.
    /// </summary>
    /// <param name="service">service to load</param>
    Task<bool> TryLoadAsync(IAutonomousService service);
    void Execute();
    void Abort();
    int AllowedLoadTime { get; set; }
    int AllowedAbortTime { get; set; }
    int MinExecutionTime { get; set; }
    int MaxExecutionTime { get; set; }
  }
}
