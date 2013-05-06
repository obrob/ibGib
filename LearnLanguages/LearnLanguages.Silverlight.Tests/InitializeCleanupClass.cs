//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;

//using LearnLanguages.Business.Security;
//using LearnLanguages.DataAccess;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Threading.Tasks;


//namespace LearnLanguages.Silverlight.Tests
//{
//  /// <summary>
//  /// Logs in as the valid user with roles Admin and User.
//  /// </summary>
//  [TestClass]
//  public class InitializeCleanupClass : Microsoft.Silverlight.Testing.SilverlightTest
//  {
//    [AssemblyInitialize]
//    public async Task InitializeAllTesting()
//    {
//      var task = UserPrincipal.LoginAsync(SeedData.Ton.TestValidUsername, SeedData.Ton.TestValidPassword);
//      task.Wait(5000);
//      Assert.IsInstanceOfType(Csla.ApplicationContext.User.Identity, typeof(UserIdentity));
//      Assert.IsTrue(Csla.ApplicationContext.User.Identity.IsAuthenticated);
//    }

//    [AssemblyCleanup]
//    public void CleanUpAllTesting()
//    {
//      UserPrincipal.Logout();
//    }
//  }
//}
