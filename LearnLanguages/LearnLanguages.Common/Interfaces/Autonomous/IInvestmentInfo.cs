using System;

namespace LearnLanguages.Common.Interfaces.Autonomous
{
  /// <summary>
  /// This should encapsulate the necessary data to convey an 
  /// investment in a service. In a simple transaction, you just tell 
  /// it an amount and service. In a more complex transaction, some of
  /// these fields could be unused, and the real details are encapsulated
  /// in the AdditionalState object. So, this is a convenience parameter 
  /// class, that has the AdditionalState property as a fallback.
  /// 
  /// The 
  /// 
  /// More simply, this is the argument that is passed to the 
  /// service manager, i.e. Invest(IInvestmentInfo)
  /// </summary>
  public interface IInvestmentInfo : IHaveId
  {
    /// <summary>
    /// The name of the service. (Optional)
    /// </summary>
    string ServiceName { get; }
    /// <summary>
    /// Type of service to invest in. (Optional)
    /// </summary>
    Type ServiceType { get; }
    /// <summary>
    /// Id of service to invest in. (Optional)
    /// </summary>
    Guid ServiceId { get; }
    /// <summary>
    /// Positive whole number amount to invest. (Optional)
    /// 
    /// My intent by making it ulong is that there are only 
    /// investments, no decrements. 
    /// </summary>
    ulong AmountToInvest { get; }
    /// <summary>
    /// Flag to indicate if the investment has an
    /// expiration date. If it does, then the investment
    /// is considered invalid after that date. (Optional)
    /// </summary>
    bool InvestmentWillExpire { get; }
    /// <summary>
    /// Date of expiration of the investment. After this date,
    /// the investment is considered invalid and should not
    /// be used. (Optional)
    /// 
    /// Of course, enforcing this depends on the receiver of 
    /// investment.
    /// </summary>
    DateTime InvestmentExpirationDate { get; }
    /// <summary>
    /// Object to be used for confidential transactions. (Optional)
    /// I don't really know how this would be implemented,
    /// but I figured I should include it. Shouldn't hurt if
    /// it remains unused.
    /// </summary>
    object InvestmentSecret { get; }
    /// <summary>
    /// Fallback state property to include any details not listed
    /// explicitly as properties. (Optional)
    /// </summary>
    object AdditionalState { get; }
  }
}
