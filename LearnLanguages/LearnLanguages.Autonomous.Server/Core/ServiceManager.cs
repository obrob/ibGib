//using ExampleServices;
using LearnLanguages.Autonomous.Loaders;
using LearnLanguages.Common;
using LearnLanguages.Common.Enums.Autonomous;
using LearnLanguages.Common.Interfaces.Autonomous;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnLanguages.Autonomous.Core
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
        await KillAllContextsAsync(Configuration.AllowedKillAllContextsTimeInMs);
        return true;
      }

      public async Task<bool> Enable()
      {
        var disabled = await Disable();
        if (!disabled)
          return false;

        await InitializeContextsAsync();
        BeginLoadingServicesAsync();
        return true;
      }

      private Task BeginLoadingServicesAsync()
      {
        throw new NotImplementedException();
      }

      private async Task InitializeContextsAsync()
      {
        //First, kill any and all contexts that may be living. 
        await KillAllContextsAsync(ServiceManagerConfiguration.Ton.AllowedKillAllContextsTimeInMs);
        
        //Now spin up new contexts, according to the configuration's ConcurrentContexts property.
        _Contexts = new List<IAutonomousServiceContext>();
        for (int i = 0; i < Configuration.ConcurrentContexts; i++)
        {
          var context = CreateContext();
          _Contexts.Add(context);
        }
      }

      private IAutonomousServiceContext CreateContext()
      {
        AppDomainContext retContext = null;

        AppDomainSetup ads = new AppDomainSetup();
        ads.ApplicationBase =
            System.Environment.CurrentDirectory;
        ads.DisallowBindingRedirects = false;
        ads.DisallowCodeDownload = true;
        ads.ConfigurationFile =
            AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

        var appDomainName = AutonomousResources.AppDomainNamePrefix +
                            DateTime.Now.ToLongTimeString() + 
                            RandomPicker.Ton.PickLetters(Configuration.DefaultContextRandomSalt) +
                            AutonomousResources.AppDomainNameSuffix;
        AppDomain ad2 = AppDomain.CreateDomain(appDomainName, null, ads);
        var contextType = typeof(AppDomainContext);
        var assembly = contextType.Assembly.FullName;
        retContext = 
          (AppDomainContext)ad2.CreateInstanceAndUnwrap(assembly, 
                                                        typeof(AppDomainContext).FullName);

        return retContext;
      }

      private List<IAutonomousServiceContext> _Contexts { get; set; }

      private async Task KillAllContextsAsync(int allowedTimeInMs)
      {
        if (_Contexts == null)
          return;

        if (_Contexts.Count == 0)
        {
          _Contexts = null;
          return;
        }

        //Tell the contexts that we're killing them in allowedTimeInMs time.
        var killTasks = new List<Task>();
        for (int i = 0; i < _Contexts.Count; i++)
        {
          var context = _Contexts[i];
          var killTask = context.KillAsync(allowedTimeInMs);
          killTasks.Add(killTask);
        }

        //Wait for the timeout period of time or when all of the kill tasks return
        await Task.WhenAny(Task.WhenAll(killTasks), Task.Delay(allowedTimeInMs));

        //Dispose of each context (not sure if necessary)
        for (int i = 0; i < _Contexts.Count; i++)
        {
          var context = _Contexts[i];
          context.Dispose();
          _Contexts.RemoveAt(i);
        }

        //Removes what should be last remnant of contexts, to prepare
        //garbage collection.
        _Contexts = null;

        //To clean up the AppDomainContexts, we need to do a GC.Collect
        //Otherwise, the resources may not be cleaned up for awhile.
        //This was determined through AppDomainStuff testing.
        GC.Collect();

        Contract.Ensures(_Contexts == null);
      }

      public IObservable<IInvestmentReceipt> Invest(IInvestmentInfo investmentInfo)
      {
        throw new NotImplementedException();
      }



      [ImportMany(typeof(IAutonomousService))]
      private ICollection<IAutonomousService> _Services { get; set; }
    }
}
