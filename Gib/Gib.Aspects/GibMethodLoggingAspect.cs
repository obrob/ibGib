using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Extensibility;

namespace Gib.Aspects
{
    [MulticastAttributeUsage(MulticastTargets.Method, 
                             TargetMemberAttributes = MulticastAttributes.Instance | MulticastAttributes.Static)]
    [Serializable]
    public class GibMethodLoggingAspect : OnMethodBoundaryAspect
    {
      public override void OnEntry(MethodExecutionArgs args)
      {
        Debug.WriteLine(string.Format("---------- [GibMethodLoggingAspect.OnEntry()] {0}.{1}() ----------", 
          args.Method.DeclaringType.Name, args.Method.Name));
      }

      public override void OnException(MethodExecutionArgs args)
      {
        Debug.WriteLine(string.Format("---------- [GibMethodLoggingAspect.OnException()] {0}.{1}() ----------", 
          args.Method.DeclaringType.Name, args.Method.Name));
      }

      public override void OnSuccess(MethodExecutionArgs args)
      {
        Debug.WriteLine(string.Format("---------- [GibMethodLoggingAspect.OnSuccess()] {0}.{1}() ----------", 
          args.Method.DeclaringType.Name, args.Method.Name));
      }
    }
}
