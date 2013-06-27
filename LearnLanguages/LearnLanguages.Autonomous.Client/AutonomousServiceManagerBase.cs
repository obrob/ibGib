using LearnLanguages.Common.Interfaces.Autonomous;
using System;

namespace LearnLanguages.Autonomous.Client
{
  public class AutonomousServiceManagerBase : IAutonomousServiceManager
  {
    public IObservable<IInvestmentReceipt> Invest(IInvestmentInfo investmentInfo)
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


    System.Threading.Tasks.Task<bool> IAutonomousServiceManager.Enable()
    {
      throw new NotImplementedException();
    }

    System.Threading.Tasks.Task<bool> IAutonomousServiceManager.Disable()
    {
      throw new NotImplementedException();
    }
  }
}
