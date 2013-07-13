using System;
using System.Linq;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using System.ComponentModel.Composition.Hosting;
using LearnLanguages.Common;
using LearnLanguages.Common.Interfaces.Autonomous;
using System.ComponentModel.Composition;

namespace LearnLanguages
{
  public static class Services
  {
    private static volatile object _initializationLock = new object();
    public static void Initialize(CompositionContainer container = null)
    {
      if (IsInitialized)
        return;

      lock (_initializationLock)
      {
        if (container == null)
          InitializeContainer();
        else
          _Container = container;

        //MUST SET BACKING FIELD (NOT PROPERTY) OR WILL DEADLOCK ON _INITIALIZATIONLOCK
        _IsInitialized = true;
      }
    }

    #region public bool IsInitialized

    private static bool _IsInitialized;
    public static bool IsInitialized
    {
      get
      {
        lock (_initializationLock)
        {
          return _IsInitialized;
        }
      }
      set
      {
        lock (_initializationLock)
        {
          _IsInitialized = value;
        }
      }
    }

    #endregion

    private static void InitializeContainer()
    {
      AssemblyCatalog catThis = new AssemblyCatalog(typeof(Services).Assembly);
      AggregateCatalog catAggregate = new AggregateCatalog(catThis);
      _Container = new CompositionContainer(catAggregate);

      //ADD BATCH FOR SERVICES TO CONTAINER, INCLUDING CONTAINER ITSELF, AND INITIALIZE SERVICES
      var batch = new CompositionBatch();
      _EventAggregator = new EventAggregator();
      batch.AddExportedValue<IEventAggregator>(_EventAggregator);
#if DEBUG
      _Logger = new DebugLogger();
      _EventAggregator.Subscribe(_Logger);
      batch.AddExportedValue<LearnLanguages.Common.Interfaces.ILogger>(_Logger);
#endif
      batch.AddExportedValue(_Container);
      _Container.Compose(batch);

      //_Container.ComposeParts();
    }

    #region public static CompositionContainer Container

    private static object __ContainerLock = new object();
    private static CompositionContainer _Container;
    public static CompositionContainer Container
    {
      get
      {
        lock (__ContainerLock)
        {
          if (!IsInitialized)
            Initialize();
          return _Container;
        }
      }
      set
      {
        lock (__ContainerLock)
        {
          _Container = value;
        }
      }
    }

    #endregion

    public static void AddCatalog(System.Reflection.Assembly assembly)
    {
      if (!IsInitialized)
        Initialize();
      var catAssembly = new AssemblyCatalog(assembly);
      ((AggregateCatalog)_Container.Catalog).Catalogs.Add(catAssembly);
    }

    public static void RemoveCatalog(System.Reflection.Assembly assembly)
    {
      if (!IsInitialized)
        Initialize();
      var catAggregate = (AggregateCatalog)_Container.Catalog;
      AssemblyCatalog catAssembly = null;
      if (ContainsAssemblyCatalog(assembly.FullName, out catAssembly))
        catAggregate.Catalogs.Remove(catAssembly);
      else
        PublishMessageEvent(string.Format("Tried to remove an assembly catalog (Assembly name = {0}) that was not in the container.", assembly.FullName),
                            MessagePriority.High,
                            MessageType.Error);
    }

    private static bool ContainsAssemblyCatalog(string assemblyName, out AssemblyCatalog catAssembly)
    {
      if (!IsInitialized)
        Initialize();
      var catAggregate = (AggregateCatalog)_Container.Catalog;
      catAssembly = (AssemblyCatalog)catAggregate.Catalogs
                                      .Where(cat => cat is AssemblyCatalog &&
                                                    ((AssemblyCatalog)cat).Assembly.FullName == assemblyName)
                                      .SingleOrDefault();

      return catAssembly != null;
    }

    public static bool ContainsAssemblyCatalog(string assemblyName)
    {
      if (!IsInitialized)
        Initialize();
      AssemblyCatalog catAssembly = null;
      return ContainsAssemblyCatalog(assemblyName, out catAssembly);
    }

    private static IEventAggregator _EventAggregator;
    public static IEventAggregator EventAggregator
    {
      get
      {
        if (!IsInitialized)
          Initialize();
        //if (_Container == null)
        //  throw new NullReferenceException(CommonResources.ErrorMsgNotInjected("Container"));
        //if (_EventAggregator == null)
        //  _EventAggregator = Container.GetExportedValue<IEventAggregator>();

        return _EventAggregator;
      }
    }
#if SILVERLIGHT
    private static IWindowManager _WindowManager;
    public static IWindowManager WindowManager
    {
      get
      {
        if (_Container == null)
          throw new NullReferenceException(CommonResources.ErrorMsgNotInjected("Container"));
        if (_WindowManager == null)
          _WindowManager = Container.GetExportedValue<IWindowManager>();

        return _WindowManager;
      }
    }
#endif

    private static ILogger _Logger;
    public static ILogger Logger
    {
      get
      {
        if (!IsInitialized)
          Initialize();

        if (_Logger == null)
          _Logger = Container.GetExportedValueOrDefault<ILogger>();
        return _Logger;
      }
    }

    public static void Log(string msg, LogPriority priority, LogCategory category)
    {
      if (Logger != null)
        Logger.Log(msg, priority, category);
    }

    //private static NavigationController _Navigator;
    //public static NavigationController Navigator
    //{
    //  get
    //  {
    //    if (_Navigator == null)
    //      _Navigator = Container.Resolve<NavigationController>();
    //    return _Navigator;
    //  }
    //}

    public static void PublishMessageEvent(string msgText,
                                           MessagePriority messagePriority,
                                           MessageType messageType)
    {
      if (!IsInitialized)
        Initialize();
      var msgEvent = new Common.Events.MessageEvent(msgText, messagePriority, messageType);
      EventAggregator.Publish(msgEvent);
    }

    //#region public IAutonomousServiceManager ServiceManager

    //private static object __ServiceManagerLock = new object();
    //[Import]
    //private static IAutonomousServiceManager _ServiceManager;
    //public static IAutonomousServiceManager ServiceManager
    //{
    //  get
    //  {
    //    lock (__ServiceManagerLock)
    //    {
    //      if (_ServiceManager == null)
    //      {
    //        if (Container == null)
    //          InitializeContainer();

    //        _ServiceManager = Container.GetExportedValue<IAutonomousServiceManager>();
    //      }
    //      return _ServiceManager;
    //    }
    //  }
    //  set
    //  {
    //    lock (__ServiceManagerLock)
    //    {
    //      _ServiceManager = value;
    //    }
    //  }
    //}

    //#endregion

  }
}
