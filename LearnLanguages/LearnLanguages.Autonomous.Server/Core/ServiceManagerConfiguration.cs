using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnLanguages.Autonomous.Core
{
  public class ServiceManagerConfiguration
  {
    //public static int Concurrent
    #region Singleton Pattern Members
    private static volatile ServiceManagerConfiguration _Ton;
    private static object _Lock = new object();
    public static ServiceManagerConfiguration Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new ServiceManagerConfiguration();
          }
        }

        return _Ton;
      }
    }
    #endregion

    public ServiceManagerConfiguration()
    {
      //Assign defaults
      DefaultConcurrentContexts = 
        int.Parse(ServiceManagerConfigurationResources.DefaultConcurrentContexts);
      DefaultAllowedKillAllContextsTimeInMs = 
        int.Parse(ServiceManagerConfigurationResources.DefaultKillAllContextsAllowedTimeInMs);
      DefaultContextAllowedAbortTimeInMs = 
        int.Parse(ServiceManagerConfigurationResources.DefaultContextAllowedAbortTimeInMs);
      DefaultContextAllowedExecuteTimeInMs =
        int.Parse(ServiceManagerConfigurationResources.DefaultContextAllowedExecuteTimeInMs);
      DefaultContextAllowedLoadTimeInMs =
        int.Parse(ServiceManagerConfigurationResources.DefaultContextAllowedLoadTimeInMs);
      DefaultContextRandomSalt =
        int.Parse(ServiceManagerConfigurationResources.DefaultContextRandomSalt);

      //Assign properties according to the defaults
      ConcurrentContexts = DefaultConcurrentContexts;
      AllowedKillAllContextsTimeInMs = DefaultAllowedKillAllContextsTimeInMs;
      ContextAllowedAbortTimeInMs = DefaultContextAllowedAbortTimeInMs;
      ContextAllowedExecuteTimeInMs = DefaultContextAllowedExecuteTimeInMs;
      ContextAllowedLoadTimeInMs = DefaultContextAllowedLoadTimeInMs;
    }

    public int ConcurrentContexts { get; set; }
    public int AllowedKillAllContextsTimeInMs { get; private set; }
    public int ContextAllowedAbortTimeInMs { get; private set; }
    public int ContextAllowedExecuteTimeInMs { get; private set; }
    public int ContextAllowedLoadTimeInMs { get; private set; }


    public int DefaultConcurrentContexts { get; set; }
    public int DefaultAllowedKillAllContextsTimeInMs { get; private set; }
    public int DefaultContextAllowedAbortTimeInMs { get; private set; }
    public int DefaultContextAllowedExecuteTimeInMs { get; private set; }
    public int DefaultContextAllowedLoadTimeInMs { get; private set; }
    public int DefaultContextRandomSalt { get; private set; }
  }
}
