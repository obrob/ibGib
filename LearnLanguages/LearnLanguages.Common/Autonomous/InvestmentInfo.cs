using System;
using LearnLanguages.Common.Interfaces.Autonomous;
using Csla.Serialization;

namespace LearnLanguages.Common.Autonomous
{
  /// <summary>
  /// Simple implementation of IInvestmentInfo. See the
  /// interface for more details.
  /// </summary>
  [Serializable]
  public class InvestmentInfo : IInvestmentInfo
  {
    public InvestmentInfo()
    {
      Id = Guid.NewGuid();
    }

    public InvestmentInfo(string serviceName, ulong amountToInvest)
      : this()
    {
      ServiceName = serviceName;
      AmountToInvest = amountToInvest;
    }

    public InvestmentInfo(string serviceName, ulong amoungToInvest, Type serviceType,
      Guid serviceId, bool investmentWillExpire, DateTime investmentExpirationDate, 
      object investmentSecret, object additionalState)
      : this(serviceName, amoungToInvest)
    {
      ServiceType = serviceType;
      ServiceId = serviceId;
      InvestmentWillExpire = investmentWillExpire;
      InvestmentExpirationDate = investmentExpirationDate;
      InvestmentSecret = investmentSecret;
      AdditionalState = additionalState;
    }

    public Guid Id { get; private set; }

    public string ServiceName { get; private set; }
    public Type ServiceType { get; private set; }
    public Guid ServiceId { get; private set; }
    
    public ulong AmountToInvest { get; private set; }
    public bool InvestmentWillExpire { get; private set; }
    public DateTime InvestmentExpirationDate { get; private set; }
    public object InvestmentSecret { get; private set; }
    public object AdditionalState { get; private set; }
    
  }
}
