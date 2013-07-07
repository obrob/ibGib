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
    public static void Initialize(CompositionContainer container = null)
    {
      if (IsInitialized)
        return;

      if (container == null)
        InitializeContainer();

      _Container = container;
      IsInitialized = true;
    }

    public static bool IsInitialized { get; private set; }

    private static void InitializeContainer()
    {
      AssemblyCatalog catThisAssembly = new AssemblyCatalog(typeof(Services).Assembly);
      AggregateCatalog catAggregate = new AggregateCatalog(catThisAssembly);
      _Container = new CompositionContainer(catAggregate);
      _Container.ComposeParts();
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
      var catAssembly = new AssemblyCatalog(assembly);
      ((AggregateCatalog)_Container.Catalog).Catalogs.Add(catAssembly);
    }

    public static void RemoveCatalog(System.Reflection.Assembly assembly)
    {
      var catAggregate = (AggregateCatalog)_Container.Catalog;
      var catAssembly = catAggregate.Catalogs
                                      .Where(cat => cat is AssemblyCatalog &&
                                                    ((AssemblyCatalog)cat).Assembly.FullName == assembly.FullName)
                                      .SingleOrDefault();
      if (catAssembly != null)
        catAggregate.Catalogs.Remove(catAssembly);
      else
        PublishMessageEvent(string.Format("Tried to remove an assembly catalog (Assembly name = {0}) that was not in the container.", assembly.FullName),
                            MessagePriority.High,
                            MessageType.Error);
    }

    private static IEventAggregator _EventAggregator;
    public static IEventAggregator EventAggregator
    {
      get
      {
        if (_Container == null)
          throw new NullReferenceException(CommonResources.ErrorMsgNotInjected("Container"));
        if (_EventAggregator == null)
          _EventAggregator = Container.GetExportedValue<IEventAggregator>();

        return _EventAggregator;
      }
    }

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

    private static ILogger _Logger;
    public static ILogger Logger
    {
      get
      {
        if (_Container == null)
          throw new NullReferenceException("Container has not been initialized");
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
