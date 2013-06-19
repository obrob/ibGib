using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleServices
{
    public class SimpleService
    {
      public void Foo()
      {
        Console.WriteLine("Foo executed in SimpleService.");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
      }
    }
}
