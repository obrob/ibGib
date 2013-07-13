using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDomainLoading.Autonomous
{
  [Serializable]
  public class AppDomainContext : MarshalByRefObject
  {
    public void Foo()
    {
      MyStringProperty = "Foo done did.";
    }

    public string MyStringProperty { get; set; }
  }
}
