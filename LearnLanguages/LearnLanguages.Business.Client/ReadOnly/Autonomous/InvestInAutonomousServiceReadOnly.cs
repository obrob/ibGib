using System;
using System.Net;
using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess;
using System.Threading.Tasks;
using LearnLanguages.Business.Criteria;
using LearnLanguages.Common.Interfaces.Autonomous;
//For right now, have this compiler directive
#if !SILVERLIGHT
using LearnLanguages.Autonomous.Core;
#endif

namespace LearnLanguages.Business.ReadOnly.Autonomous
{
  /// <summary>
  /// This ReadOnly invests in a service and returns the receipt for the investment.
  /// </summary>
  [Serializable]
  public class InvestInAutonomousServiceReadOnly : Common.CslaBases.ReadOnlyBase<InvestInAutonomousServiceReadOnly>
  {
    #region Factory Methods

    public static async Task<InvestInAutonomousServiceReadOnly> CreateNewAsync(AutonomousServiceInvestmentCritera criteria)
    {
      var result = await DataPortal.CreateAsync<InvestInAutonomousServiceReadOnly>(criteria);
      return result;
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<IInvestmentInfo> InvestmentInfoProperty = 
      RegisterProperty<IInvestmentInfo>(c => c.InvestmentInfo);
    public IInvestmentInfo InvestmentInfo
    {
      get { return ReadProperty(InvestmentInfoProperty); }
      private set { LoadProperty(InvestmentInfoProperty, value); }
    }

    public static readonly PropertyInfo<IInvestmentReceipt> InvestmentReceiptProperty 
      = RegisterProperty<IInvestmentReceipt>(c => c.InvestmentReceipt);
    public IInvestmentReceipt InvestmentReceipt
    {
      get { return ReadProperty(InvestmentReceiptProperty); }
      private set { LoadProperty(InvestmentReceiptProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create(string username)
    {
      Id = Guid.NewGuid();
      //var blah = Services.ServiceManager;
    }
#endif

    #endregion
  }
}
