using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDomainLoading.Autonomous
{
  public static class AssemblyResolverHelper
  {
    public static System.Reflection.Assembly ad2_AssemblyResolve(object sender, ResolveEventArgs args)
    {
      Debug.WriteLine(System.Environment.NewLine);
      Debug.WriteLine("---------- [AssemblyResolverHelper.ad2_AssemblyResolve()] ----------");
      Debug.WriteLine(string.Format("------------- args.Name = \"{0}\"", args.Name));
      Debug.WriteLine(string.Format("------------- args.RequestingAssembly = \"{0}\"", args.RequestingAssembly.FullName));

      throw new NotImplementedException();
    }
  }
}
