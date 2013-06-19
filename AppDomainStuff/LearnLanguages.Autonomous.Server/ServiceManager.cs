using LearnLanguages.Common.Enums.Autonomous;
using LearnLanguages.Common.Interfaces.Autonomous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnLanguages.Autonomous.Server
{
    public class ServiceManager : IAutonomousServiceManager
    {
      public ServiceManager()
      {
        Configuration = ServiceManagerConfiguration.Ton;
      }

      public ServiceManagerConfiguration Configuration { get; set; }

      public Guid Id { get; set; }
     
      public async Task<bool> Disable()
      {
        await StopAndDisableAllContextsAsync();
        return true;
      }

      public async Task<bool> Enable()
      {
        var disabled = await Disable();
        if (!disabled)
          return false;

        await InitializeContextsAsync();
        return true;
      }

      private async Task InitializeContextsAsync()
      {
        _Contexts = new List<Context>();
        for (int i = 0; i < Configuration.ConcurrentContexts; i++)
        {
          var context = new Context()
            {
              AllowedAbortTime = Configuration.DefaultContextAllowedAbortTime,
              AllowedExecuteTime = Configuration.DefaultContextAllowedExecuteTime,
              AllowedLoadTime = Configuration.DefaultContextAllowedLoadTime
            };
        }
      }

      private List<Context> _Contexts { get; set; }

      private async Task StopAndDisableAllContextsAsync()
      {
        //
      }

      public IObservable<IInvestmentReceipt> Invest(IInvestmentInfo investmentInfo)
      {
        throw new NotImplementedException();
      }

    }
}
