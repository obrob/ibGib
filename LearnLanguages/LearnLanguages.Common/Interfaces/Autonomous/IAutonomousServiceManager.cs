using System;
using System.Threading.Tasks;
namespace LearnLanguages.Common.Interfaces.Autonomous
{
  public interface IAutonomousServiceManager : IHaveId
  {
    IObservable<IInvestmentReceipt> Invest(IInvestmentInfo investmentInfo);
    Task<bool> EnableAsync();
    Task<bool> DisableAsync();
  }
}
