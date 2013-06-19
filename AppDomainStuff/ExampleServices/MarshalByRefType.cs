using SomeOtherLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleServices
{
  // Because this class is derived from MarshalByRefObject, a proxy  
  // to a MarshalByRefType object can be returned across an AppDomain  
  // boundary. 
  public class MarshalByRefType : MarshalByRefObject
  {
    public string SomeState { get; set; }

    public MyOwnAwesomeClass MyState { get; set; }
    private MyOwnAwesomeClass _MyPrivateState { get; set; }
    private MyOwnAwesomeClass _myPrivateField;

    //  Call this method via a proxy. 
    public Task<string> SomeMethod(string callingDomainName, Func<string, string> callback)
    {
      // Display the name of the calling AppDomain and the name  
      // of the second domain. 
      // NOTE: The application's thread has transitioned between  
      // AppDomains.
      MyWriteLine(string.Format("Calling from '{0}' to '{1}'.",
                        callingDomainName,
                        Thread.GetDomain().FriendlyName));

      // Get this AppDomain's settings and display some of them.
      AppDomainSetup ads = AppDomain.CurrentDomain.SetupInformation;
      //MyWriteLine(string.Format("This WriteLine is run from within MarshalByRefType in the domain: {0}", 
      //                    AppDomain.CurrentDomain.FriendlyName));
      var info = string.Format("AppName={0}, AppBase={1}, ConfigFile={2}",
          ads.ApplicationName,
          ads.ApplicationBase,
          ads.ConfigurationFile);
      MyWriteLine(info);

      SomeState = info;

      var sb = new StringBuilder();
      for (int i = 0; i < 100000; i++)
      {
        sb.Append("a");
      }

      MyState = new MyOwnAwesomeClass("it", "doesn't", "matter", "what");
      _MyPrivateState = new MyOwnAwesomeClass("I", "put", "in", "here");
      _myPrivateField = new MyOwnAwesomeClass("Cuz", "any", "thing", "'ll do" + sb.ToString());

      if (callback != null)
      {
        var fromCallingAppDomain = callback("This I don't think we can do.");
        
        Console.WriteLine(string.Format("This was funcy {0}", fromCallingAppDomain));
      }

      return Task.FromResult<string>(info);
    }

    private void MyWriteLine(string line)
    {
      //Console.WriteLine(line);
    }
  }
}
