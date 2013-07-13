using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDomainLoading.Autonomous
{
  [Serializable]
  public class AppDomainContext : MarshalByRefObject
  {
    public AppDomainContext()
    {
      Debug.WriteLine("\r\n---------- [AppDomainContext() constructor]");

      this.MyCtorSetProperty = "I'm alive!";

      Debug.WriteLine("------------- [AppDomainContext() constructor] this: " + this);
    }

    public void Foo()
    {
      Debug.WriteLine("\r\n---------- [Foo()]");
      
      this.MyFooSetProperty = "Foo done did.";

      Debug.WriteLine("------------- [Foo()] this: " + this);
    }

    public string MyCtorSetProperty { get; set; }
    public string MyFooSetProperty { get; set; }

    public override string ToString()
    {
      return string.Format("MyCtorSetProperty=\"{0}\", MyFooSetProperty=\"{1}\"",
        this.MyCtorSetProperty ?? "<null>",
        this.MyFooSetProperty ?? "<null>");
    }
  }
}
