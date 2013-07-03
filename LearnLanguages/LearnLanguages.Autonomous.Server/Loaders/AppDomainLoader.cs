using LearnLanguages.Common.Interfaces.Autonomous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnLanguages.Autonomous.Loaders
{
  public class AppDomainLoader
  {
    #region Singleton Pattern Members
    private static volatile AppDomainLoader _Ton;
    private static object _Lock = new object();
    public static AppDomainLoader Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new AppDomainLoader();
          }
        }

        return _Ton;
      }
    }
    #endregion
    
    public async Task<IAutonomousServiceContext> LoadServiceAsync(IAutonomousService service, int timeAllowedInMs)
    {
      throw new NotImplementedException();
      //We will create an AppDomainContext, which descends from MarshalByRefObject.
      //This will actually return a proxy.
      //This context must load the service in that appdomain
      //But first, let's just get a context that does anything at all in a second
      //appdomain.
      
      var serviceType = service.GetType();

      //AppDomainSetup ads = new AppDomainSetup();
      //ads.ApplicationBase =
      //    System.Environment.CurrentDirectory;
      //ads.DisallowBindingRedirects = false;
      //ads.DisallowCodeDownload = true;
      //ads.ConfigurationFile =
      //    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

      //var appDomainName = AutonomousResources.AppDomainNamePrefix + 
      //                    service.Name + 
      //                    AutonomousResources.AppDomainNameSuffix;
      //AppDomain ad2 = AppDomain.CreateDomain(appDomainName, null, ads);
      //var contextType = typeof(AppDomainContext);
      //var assembly = contextType.Assembly.FullName;
      //var contextProxy =
      //       (AppDomainContext)ad2.CreateInstanceAndUnwrap(
      //           assembly,
      //           typeof(AppDomainContext).FullName
      //       );

    //  var beforeState = contextProxy.State;
    //  var success = contextProxy.TryLoadService(service, timeAllowedInMs);
    //  var afterState = contextProxy.State;
    //  return contextProxy;
    }
  }
}
