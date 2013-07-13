//using ExampleServices;
using LearnLanguages.Autonomous.Loaders;
using LearnLanguages.Common;
using LearnLanguages.Common.Enums.Autonomous;
using LearnLanguages.Common.Interfaces.Autonomous;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LearnLanguages.Autonomous.Core
{
  [Serializable]
  [Export(typeof(IAutonomousServiceManager))]
  public class ServiceManager : IAutonomousServiceManager
  {
    public ServiceManager()
    {
      Configuration = ServiceManagerConfiguration.Ton;
      _Services = new List<IAutonomousService>();
    }

    public ServiceManagerConfiguration Configuration { get; set; }

    public Guid Id { get; set; }

    public async Task<bool> DisableAsync()
    {
      await KillAllContextsAsync(Configuration.AllowedKillAllContextsTimeInMs);
      return true;
    }

    public async Task<bool> EnableAsync()
    {
      var disabled = await DisableAsync();
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

    //[System.Security.SecurityCritical]
    private IAutonomousServiceContext CreateContext()
    {
      AppDomainContext retContext = null;
      try
      {
        
        IPrincipal user = null;
        CompositionContainer container = null;
        DoBeforeCrossAppDomainCommunication(out user, out container);
        try
        {
          var appDomainName = AutonomousResources.AppDomainNamePrefix +
                              DateTime.Now.ToLongTimeString() +
                              RandomPicker.Ton.PickLetters(Configuration.DefaultContextRandomSalt) +
                              AutonomousResources.AppDomainNameSuffix;

          AppDomainSetup ads = new AppDomainSetup();
          ads.ApplicationBase = System.Environment.CurrentDirectory;
          //ads.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
          //ads.PrivateBinPath = AppDomain.CurrentDomain.BaseDirectory;
          ads.DisallowBindingRedirects = false;
          ads.DisallowCodeDownload = true;
          ads.ConfigurationFile =
              AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

          AppDomain ad2 = AppDomain.CreateDomain(appDomainName, null);
          var contextType = typeof(AppDomainContext);
          var assembly = contextType.Assembly.FullName;
          var blah2 = ad2.CreateInstanceFromAndUnwrap(assembly,
                                                          typeof(AppDomainContext).FullName);

          //ad2.AssemblyResolve += ad2_AssemblyResolve;
          //ad2.AssemblyResolve += AssemblyResolve;
          //ad2.Load(GetType().Assembly.Location);
          //ad2.Load("LearnLanguages.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
          //ad2.AssemblyResolve += ad2_AssemblyResolve;
          //var contextType = typeof(AppDomainContext);
          //var assembly = contextType.Assembly.FullName;
          //retContext =
          //  (AppDomainContext)ad2.CreateInstanceAndUnwrap(assembly,
          //                                                typeof(AppDomainContext).FullName);


        }
        catch (Exception ex)
        {
          Services.PublishMessageEvent(ex.Message, MessagePriority.VeryHigh, MessageType.Error);
          throw;
        }
        finally
        {
          DoAfterCrossAppDomainCommunication(user, container);
        }
        return retContext;
      }
      catch (Exception ex)
      {
        Services.PublishMessageEvent(ex.Message, MessagePriority.VeryHigh, MessageType.Error);
        throw;
      }
    }

    private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
    {
      return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
    }

    private void DoBeforeCrossAppDomainCommunication(out IPrincipal user, out CompositionContainer container)
    {
      var blah = CallContext.GetData("Principal");

      user = Csla.ApplicationContext.User;
      Csla.ApplicationContext.User = null;

      container = Services.Container;
      Services.Container = null;

      var blah2 = CallContext.LogicalGetData("Principal");

    }

    private void DoAfterCrossAppDomainCommunication(IPrincipal user, CompositionContainer container)
    {
      Csla.ApplicationContext.User = user;
      Services.Container = container;
    }

    System.Reflection.Assembly ad2_AssemblyResolve(object sender, ResolveEventArgs args)
    {

      // get a list of all the assemblies loaded in our appdomain
      Assembly[] list = AppDomain.CurrentDomain.GetAssemblies();

      // search the list to find the assembly that was not found automatically
      // and return the assembly from the list

      foreach (Assembly asm in list)
        if (asm.FullName == args.Name)
          return asm;

      // if the assembly wasn't already in the appdomain, then try to load it.
      return Assembly.Load(args.Name);

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


    [ImportMany(typeof(IAutonomousService), AllowRecomposition = true)]
    private ICollection<IAutonomousService> _Services { get; set; }
  }
}