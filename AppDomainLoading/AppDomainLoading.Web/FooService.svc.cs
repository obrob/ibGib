using AppDomainLoading.Autonomous;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace AppDomainLoading.Web
{
  [ServiceContract(Namespace = "")]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class FooService
  {
    [OperationContract]
    public void Foo()
    {
      ServiceManager svcManager = new ServiceManager();
      svcManager.LoadAppDomain();
    }

    // Add more operations here and mark them with [OperationContract]
  }
}
