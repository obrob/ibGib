using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDomainLoading.Autonomous
{
  [Serializable]
  public class ServiceManager : MarshalByRefObject
  {
    public void LoadAppDomain()
    {
      //var appDomainName = AutonomousResources.AppDomainNamePrefix +
      //                        DateTime.Now.ToLongTimeString() +
      //                        RandomPicker.Ton.PickLetters(Configuration.DefaultContextRandomSalt) +
      //                        AutonomousResources.AppDomainNameSuffix;
      var appDomainName = "blahblahdomain";

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
      ad2.AssemblyResolve += ad2_AssemblyResolve;
      var blah2 = ad2.CreateInstanceFromAndUnwrap(assembly,
                                                      typeof(AppDomainContext).FullName);
    }

    System.Reflection.Assembly ad2_AssemblyResolve(object sender, ResolveEventArgs args)
    {
      throw new NotImplementedException();
    }
  }
}
