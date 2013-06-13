using System;
namespace LearnLanguages.Common.Interfaces.Autonomous
{
  public interface IAutonomousServiceManager : IHaveId
  {
    IObservable<IInvestmentReceipt> Invest(IInvestmentInfo investmentInfo);
    bool Enable();
    bool Disable();
  }
}
