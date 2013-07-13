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
      // Add your operation implementation here
      return;
    }

    // Add more operations here and mark them with [OperationContract]
  }
}
