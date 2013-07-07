using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
using System.Linq;
using LearnLanguages.Business.Security;
using LearnLanguages.Business.ReadOnly.Autonomous;

namespace LearnLanguages.Silverlight.Tests
{
  //THE SEEDDATA INSTANCE IS NOT UPDATED ON THE CLIENT.  WE CANNOT TEST AGAINST SeedData.Ton IDS
  //BECAUSE THESE IDS WERE NOT UPDATED WHEN THE DB WAS SEEDED, THE SeedData.Ton ON THE SERVER 
  //WAS UPDATED.  THE RELATIONSHIPS SHOULD BE VALID HOWEVER.
  [TestClass]
  [Tag("svcmgr")]
  public class ServiceManagerTests : TestsBase
  {
    
    [TestMethod]
    [Asynchronous]
    public async Task ENABLE()
    {
      var isComplete = false;
      EnqueueConditional(() => isComplete);
      try
      {
        await Setup();
        var result = await ChangeEnableAutonomousServiceManagerReadOnly.CreateNewAsync(true);
        
        EnqueueCallback(() => Assert.IsTrue(result.WasSuccessful.Value));

        EnqueueTestComplete();
      }
      finally
      {
        Teardown();
      }
    }

  }
}