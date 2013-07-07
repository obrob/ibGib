
using System;
using System.Net;
using Csla.Serialization;
using Csla;
using LearnLanguages.Common.Interfaces.Autonomous;
using LearnLanguages.Common.Autonomous;

namespace LearnLanguages.Business.Criteria
{
  [Serializable]
  public class AutonomousServiceInvestmentCritera : CriteriaBase<AutonomousServiceInvestmentCritera>
  {
    /// <summary>
    /// DON'T USE THIS CTOR.  THIS IS A READ ONLY CRITERIA CLASS.  THIS CTOR IS ONLY HERE
    /// BECAUSE SERIALIZATION REQUIRES IT.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public AutonomousServiceInvestmentCritera()
    {
      //required for serialization
    }
    
    public AutonomousServiceInvestmentCritera(string serviceName, ulong amountToInvest)
    {
      var investmentInfo = new InvestmentInfo(serviceName, amountToInvest);
      LoadProperty<IInvestmentInfo>(InvestmentInfoProperty, investmentInfo);
    }

    public AutonomousServiceInvestmentCritera(IInvestmentInfo investmentInfo)
    {
      LoadProperty<IInvestmentInfo>(InvestmentInfoProperty, investmentInfo);
    }

    public static readonly PropertyInfo<IInvestmentInfo> InvestmentInfoProperty = 
      RegisterProperty<IInvestmentInfo>(c => c.InvestmentInfo);
    public IInvestmentInfo InvestmentInfo
    {
      get { return ReadProperty(InvestmentInfoProperty); }
      private set { LoadProperty(InvestmentInfoProperty, value); }
    }

  }
}
