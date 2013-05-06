using LearnLanguages.Common.Interfaces.Autonomous;
using System;

namespace LearnLanguages.Autonomous.Client
{
  public class AutonomousServiceManagerBase : IAutonomousServiceManager
  {

    public void InvestIn(Type serviceType, int amountToInvest, DateTime expirationDate)
    {
      throw new NotImplementedException();
    }

    public void InvestIn(string serviceName, int amountToInvest, DateTime expirationDate)
    {
      throw new NotImplementedException();
    }

    public bool Enable()
    {
      throw new NotImplementedException();
    }

    public bool Disable()
    {
      throw new NotImplementedException();
    }

    public Guid Id
    {
      get { throw new NotImplementedException(); }
    }
  }
}
