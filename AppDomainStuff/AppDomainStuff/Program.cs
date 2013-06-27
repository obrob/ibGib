using ExampleServices;
using LearnLanguages.Autonomous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppDomainStuff
{
  class Program
  {
    static void Main(string[] args)
    {

      Pause("Starting services.");
      StartAutonomousServiceManager();
      bool shouldEnd = false;
      do
      {
        shouldEnd = AskForEndProgram();
      } while (!shouldEnd);
      //AppDomainTestStuff();
      //Thread.Sleep(15000);
    }

    private static bool AskForEndProgram()
    {
      //just being silly
      var input = Console.ReadLine();
      switch (input.ToLower())
      {
        case "exit":
          return true;
        case "q":
          return true;
        case "quit":
          return true;
        case "end":
          return true;

        default:
          Console.WriteLine("What'd you say to me?");
          return false;
      }
    }

    private static ServiceManager _ServiceManager { get; set; }
    private static async void StartAutonomousServiceManager()
    {
      _ServiceManager = new ServiceManager();
      var isEnabled = await _ServiceManager.Enable();
    }

    private static async Task AppDomainTestStuff()
    {
      // Get and display the friendly name of the default AppDomain. 
      string callingDomainName = Thread.GetDomain().FriendlyName;
      MyWriteLine(callingDomainName);
      //Pause();

      // Get and display the full name of the EXE assembly.
      //string exeAssembly = Assembly.GetEntryAssembly().FullName;
      string mbrtAssembly = typeof(MarshalByRefType).Assembly.FullName;

      MyWriteLine(mbrtAssembly);
      //Pause();

      // Construct and initialize settings for a second AppDomain.
      AppDomainSetup ads = new AppDomainSetup();
      ads.ApplicationBase =
          System.Environment.CurrentDirectory;
      ads.DisallowBindingRedirects = false;
      ads.DisallowCodeDownload = true;
      ads.ConfigurationFile =
          AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

      AppDomain ad2 = AppDomain.CreateDomain("AD #2", null, ads);

      //Pause();

      // Create an instance of MarshalbyRefType in the second AppDomain.  
      // A proxy to the object is returned.
      List<MarshalByRefType> mbrts = new List<MarshalByRefType>();
      for (int i = 0; i < 3000; i++)
      {
        try
        {

          MarshalByRefType mbrt =
              (MarshalByRefType)ad2.CreateInstanceAndUnwrap(
                  mbrtAssembly,
                  typeof(MarshalByRefType).FullName
              );

          mbrts.Add(mbrt);

          // Call a method on the object via the proxy, passing the  
          // default AppDomain's friendly name in as a parameter.
          var returnedInfo = await mbrt.SomeMethod(callingDomainName, (s) =>
          {
            MyWriteLine(string.Format("This is in the callback. {0}", s), true);
            return s + "  " + DateTime.Now.Millisecond.ToString();
          });


          //Pause();

          MyWriteLine(string.Format("This is run on the calling domain {0}", callingDomainName));
          MyWriteLine("The following line is the info returned from AD #2");
          MyWriteLine(returnedInfo);
          //Pause();

          MyWriteLine("The following line is the state property of the actual MarshalByRef object after its method is invoked.");
          MyWriteLine(mbrt.SomeState);
          //Pause();

        }
        catch (Exception ex)
        {
          var blah = i;
          throw;
        }
      }

      var a = mbrts[0].MyState;
      var b = mbrts[0].SomeState;

      Pause();
      MyWriteLine("Press enter to unload the second app domain.", true);
      Pause();

      AppDomain.Unload(ad2);
      mbrts.Clear();
      mbrts = null;
      ad2 = null;

      GC.Collect();

      MyWriteLine("App domain 2 unloaded.", true);
      Pause();
    }

    private static void MyWriteLine(string line, bool forceWrite = false)
    {
//      Console.WriteLine(line);
      if (forceWrite)
        Console.WriteLine(line);
    }

    public static void Pause(string msg = "")
    {
      Console.WriteLine();
      if (msg != "")
        Console.WriteLine(msg);
      Console.WriteLine("Paused. Press Enter to continue.");
      Console.ReadLine();
    }

    

  }
}
