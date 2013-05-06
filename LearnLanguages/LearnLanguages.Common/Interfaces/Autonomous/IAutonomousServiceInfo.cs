using System.Collections.Generic;

namespace LearnLanguages.Common.Interfaces.Autonomous
{
  /// <summary>
  /// This represents the state information of an autonomous 
  /// service. This is tightly coupled to that service, but 
  /// there is no reference to this info object on the Service
  /// object. The service should not know this information.
  /// 
  /// This is a major point for extensibility in future iterations
  /// of autonomous services. The more meaning information you can 
  /// gather about competing services, the more effectively you 
  /// can provide services to the user and ibgib.
  /// </summary>
  public interface IAutonomousServiceInfo : IHaveId
  {
    /// <summary>
    /// The Service that this info is tightly coupled to.
    /// </summary>
    IAutonomousService Service { get; set; }
    
    /// <summary>
    /// The cost of running this service.
    /// </summary>
    int Cost { get; set; }
    
    /// <summary>
    /// The balance for this service.
    /// </summary>
    int Balance { get; }

    /// <summary>
    /// Amount to add to the balance. This should return this info,
    /// as per fluent-style coding.
    /// </summary>
    /// <param name="amount">amount to add to the balance</param>
    /// <returns>Should return this info object, as per fluent-style coding.</returns>
    IAutonomousServiceInfo ApplyInvestment(int amount);

    /// <summary>
    /// Amount to subtract from the balance. This should return this info,
    /// as per fluent-style coding.
    /// </summary>
    /// <param name="amount">amount to subtract from the balance</param>
    /// <returns>Should return this info object, as per fluent-style coding.</returns>
    IAutonomousServiceInfo ApplyCost(int amount);

    /// <summary>
    /// Gets the level of trust for the service via parsing
    /// the service history.
    /// </summary>
    double GetTrustLevel();

    /// <summary>
    /// History for the 
    /// </summary>
    IList<Events.AutonomousEvent> ServiceEventHistory { get; }

  }
}
