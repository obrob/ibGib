using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace AppDomainLoading.Autonomous
{
  [Serializable]
  public class ServiceManager : MarshalByRefObject
  {
    public void LoadAppDomain()
    {
      try
      {
        var adCurrent = AppDomain.CurrentDomain;

        LogAppDomain(adCurrent, "Current");

        //var appDomainName = AutonomousResources.AppDomainNamePrefix +
        //                        DateTime.Now.ToLongTimeString() +
        //                        RandomPicker.Ton.PickLetters(Configuration.DefaultContextRandomSalt) +
        //                        AutonomousResources.AppDomainNameSuffix;
        var ad2Name = "blahblahdomain";

        var adSetup = new AppDomainSetup()
        {
          //ApplicationBase = System.Environment.CurrentDirectory,
          ApplicationBase = adCurrent.BaseDirectory,
          //PrivateBinPath = adCurrent.BaseDirectory,
          PrivateBinPath = Path.Combine(adCurrent.BaseDirectory, "bin"),
          DisallowBindingRedirects = false,
          DisallowCodeDownload = true,
          ConfigurationFile = adCurrent.SetupInformation.ConfigurationFile
        };

        LogAppDomainSetup(adSetup, "ad2");

        //var ad2 = AppDomain.CreateDomain(ad2Name, null);
        var ad2 = AppDomain.CreateDomain(ad2Name, null, adSetup);

        LogAppDomain(ad2, "ad2");

        var contextType = typeof(AppDomainContext);
        var assembly = contextType.Assembly; //.FullName;

        //ad2.AssemblyResolve += ad2_AssemblyResolve;

        //var blah2 = ad2.CreateInstanceFromAndUnwrap(
        //  //assembly.FullName,
        //  assembly.Location,
        //  contextType.FullName);
        //var foo = (AppDomainContext)blah2;

        Debug.WriteLine(string.Format("-------------- Creating instance from assembly \"{0}\" of type \"{1}\"", assembly.FullName, contextType.FullName));
        var obj = ad2.CreateInstance(assembly.FullName, contextType.FullName);
        var adContext = (AppDomainContext)obj.Unwrap();

        Debug.WriteLine("---------- [LoadAppDomain()] adContext after creation: " + adContext);

        adContext.Foo();
        Debug.WriteLine("---------- [LoadAppDomain()] adContext after Foo(): " + adContext);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("[ LoadAppDomain() ] EXCEPTION: " + ex);
        throw;
      }
    }

    private void LogAppDomainSetup(AppDomainSetup adSetup, string context)
    {
      Debug.WriteLine(System.Environment.NewLine);
      Debug.WriteLine(string.Format("---------- [AppDomainSetup  \"{0}\"] ----------", context));
      Debug.WriteLine(string.Format("------------- ApplicationName = \"{0}\"", adSetup.ApplicationName ?? "<null>"));
      Debug.WriteLine(string.Format("------------- ApplicationBase = \"{0}\"", adSetup.ApplicationBase ?? "<null>"));
      Debug.WriteLine(string.Format("------------- DisallowBindingRedirects = \"{0}\"", adSetup.DisallowBindingRedirects));
      Debug.WriteLine(string.Format("------------- DisallowCodeDownload = \"{0}\"", adSetup.DisallowCodeDownload));
    }

    private void LogAppDomain(AppDomain ad, string context)
    {
      Debug.WriteLine(System.Environment.NewLine);
      Debug.WriteLine(string.Format("---------- [AppDomain  \"{0}\"] ----------", context));
      Debug.WriteLine(string.Format("------------- FriendlyName = \"{0}\"", ad.FriendlyName ?? "<null>"));
      Debug.WriteLine(string.Format("------------- BaseDirectory = \"{0}\"", ad.BaseDirectory ?? "<null>"));
      Debug.WriteLine(string.Format("------------- RelativeSearchPath = \"{0}\"", ad.RelativeSearchPath ?? "<null>"));
      Debug.WriteLine(string.Format("------------- PrivateBinDirectory = \"{0}\\{1}\"", 
        ad.BaseDirectory ?? "<BaseDirectory=null>", 
        ad.RelativeSearchPath ?? "<RelativeSearchPath=null>"));
    }  

    System.Reflection.Assembly ad2_AssemblyResolve(object sender, ResolveEventArgs args)
    {
      Debug.WriteLine("---------- [ad2_AssemblyResolve()]");
      Debug.WriteLine(string.Format("------------- args.Name = \"{0}\"", args.Name));
      Debug.WriteLine(string.Format("------------- args.RequestingAssembly = \"{0}\"", args.RequestingAssembly.FullName));

      throw new NotImplementedException();
    }
  }
}
