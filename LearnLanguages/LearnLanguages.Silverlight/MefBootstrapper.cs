using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Caliburn.Micro;
using LearnLanguages.Study.Defaults.Simple;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Silverlight.ViewModels;
using LearnLanguages.Silverlight.Common.ViewModels;
using LearnLanguages.Analysis;
using LearnLanguages.Navigation;
using LearnLanguages.History;
//using LearnLanguages.DataAccess;

//THIS CODE IS BASED ON THE CODE MADE BY ROB EISENBERG AT http://caliburnmicro.codeplex.com/discussions/218561?ProjectName=caliburnmicro

namespace LearnLanguages.Silverlight
{
  public class MefBootstrapper : Bootstrapper<IShellViewModel>//, IHandle<Interfaces.IPartSatisfiedEventMessage>
  //public class MefBootstrapper : Bootstrapper<ViewModels.ShellViewModel>//, IHandle<Interfaces.IPartSatisfiedEventMessage>
  {
    private CompositionContainer _Container;

#if DEBUG
    private DebugEventMessageListener _Listener;
#endif
    
    protected override void Configure()
    {
      //CREATE ASSEMBLY CATALOGS FOR COMPOSITION OF APPLICATION (WPF)
      AssemblyCatalog catThis = new AssemblyCatalog(typeof(MefBootstrapper).Assembly);
      AssemblyCatalog catNavigation = new AssemblyCatalog(typeof(Navigator).Assembly);
      AssemblyCatalog catHistory = new AssemblyCatalog(typeof(HistoryPublisher).Assembly);
      AssemblyCatalog catStudy = new AssemblyCatalog(typeof(SimpleStudyPartner).Assembly);
      AssemblyCatalog catAnalysis = new AssemblyCatalog(typeof(SimplePhraseBeliefAnalyzer).Assembly);
      AssemblyCatalog catSilverlightCommon = new AssemblyCatalog(typeof(LanguageEditViewModel).Assembly);
      AggregateCatalog catAll = new AggregateCatalog(
                                                     catThis, 
                                                     catNavigation,
                                                     catHistory, 
                                                     catStudy,
                                                     catAnalysis,
                                                     catSilverlightCommon 
                                                     );
      //AggregateCatalog catAll = new AggregateCatalog(catThis);

      //NEW UP CONTAINER WITH CATALOGS
      _Container = new CompositionContainer(catAll);

      //ADD BATCH FOR SERVICES TO CONTAINER, INCLUDING CONTAINER ITSELF, AND INITIALIZE SERVICES
      var batch = new CompositionBatch();
      batch.AddExportedValue<IWindowManager>(new WindowManager());
      var eventAggregator = new EventAggregator();
      batch.AddExportedValue<IEventAggregator>(eventAggregator);
#if DEBUG
      var debugLogger = new DebugLogger();
      eventAggregator.Subscribe(debugLogger);
      batch.AddExportedValue<LearnLanguages.Common.Interfaces.ILogger>(debugLogger);
#endif
      batch.AddExportedValue(_Container);
      _Container.Compose(batch);

      //INITIALIZE SERVICES WITH THE CONTAINER
      Services.Initialize(_Container);

      //INITIALIZE CSLA DATA PORTAL
      //Csla.DataPortalClient.WcfProxy.DefaultUrl = "http://localhost:50094/SlPortal.svc";
      Csla.DataPortal.ProxyTypeName = 
        typeof(Compression.CompressedProxy).AssemblyQualifiedName;
#if DEBUG
      Csla.DataPortalClient.WcfProxy.DefaultUrl = 
        DataAccess.DalResources.WcfProxyDefaultUrl;
#elif STAGING
      Csla.DataPortalClient.WcfProxy.DefaultUrl =
        DataAccess.DalResources.WcfProxyReleaseUrl;
#else
      Csla.DataPortalClient.WcfProxy.DefaultUrl =
        DataAccess.DalResources.WcfProxyReleaseUrl;
#endif

      //COMMENTING OUT THIS FOLLOWING LINE GIVES ME THE NON-NEGATIVE NUMBER REQUIRED ERROR
      Csla.ApplicationContext.DataPortalProxy = 
        typeof(Csla.DataPortalClient.WcfProxy).AssemblyQualifiedName;
      
      //INITIALIZE DALMANAGER
      //DalManager.Initialize(_Container);

#if DEBUG
      //SUBSCRIBE TO EVENTAGGREGATOR
      Services.EventAggregator.Subscribe(this);
      _Listener = new DebugEventMessageListener();
#endif

      //INITIALIZE MODULES
      InitializeModules();
    }

    private void InitializeModules()
    {
      var moduleExports = _Container.GetExports<IModule>();
      foreach (var moduleExport in moduleExports)
        moduleExport.Value.Initialize();
    }

    protected override IEnumerable<System.Reflection.Assembly> SelectAssemblies()
    {
      var assemblies = new List<System.Reflection.Assembly>();
      assemblies.Add(typeof(SimpleStudyPartner).Assembly);
      assemblies.Add(typeof(LanguageEditViewModel).Assembly);
      assemblies.Add(typeof(Navigator).Assembly);
      assemblies.Add(typeof(SimplePhraseBeliefAnalyzer).Assembly);
      assemblies.AddRange(base.SelectAssemblies());

      return assemblies;
    }
    protected override object GetInstance(Type serviceType, string key)
    {
      string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
      var exports = _Container.GetExportedValues<object>(contract);

      if (exports.Count() > 0)
        return exports.First();

      throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
    }
    protected override IEnumerable<object> GetAllInstances(Type serviceType)
    {
      return _Container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
    }
    protected override void BuildUp(object instance)
    {
      _Container.SatisfyImportsOnce(instance);
    }

    protected override void OnUnhandledException(object sender, System.Windows.ApplicationUnhandledExceptionEventArgs e)
    {
      System.Windows.MessageBox.Show("unhandled exception: " + e.ExceptionObject.Message);
      e.Handled = true;
      base.OnUnhandledException(sender, e);
    }

    //private bool ShellModelSatisfied = false;
    //private bool NavigationControllerSatisfied = false;

    //public void Handle(Interfaces.IPartSatisfiedEventMessage message)
    //{
    //  if (message.Part == "Shell")
    //    ShellModelSatisfied = true;
    //  else if (message.Part == "NavigationController")
    //    NavigationControllerSatisfied = true;

    //  if (ShellModelSatisfied && NavigationControllerSatisfied)
    //  {
    //    Services.EventAggregator.Unsubscribe(this);
    //    //Services.EventAggregator.Publish(new Events.NavigationRequestedEventMessage("Login"));
    //    Navigation.Publish.NavigationRequest<ViewModels.LoginViewModel>(AppResources.BaseAddress);
    //  }
    //}
  }
}
