using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnLanguages.Autonomous
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
      ConcurrentContexts = int.Parse(ServiceManagerConfigurationResources.ConcurrentContexts);
    }

    public int ConcurrentContexts { get; set; }

    public int DefaultContextAllowedAbortTimeInMs { get; set; }

    public int DefaultContextAllowedExecuteTimeInMs { get; set; }

    public int DefaultContextAllowedLoadTimeInMs { get; set; }
  }
}
