using System;
namespace LearnLanguages.Common.Interfaces.Autonomous
{
  public interface IAutonomousServiceManager : IHaveId
  {
    void InvestIn(Type serviceType, int amountToInvest, DateTime expirationDate);
    void InvestIn(string serviceName, int amountToInvest, DateTime expirationDate);
    bool Enable();
    bool Disable();
  }
}
