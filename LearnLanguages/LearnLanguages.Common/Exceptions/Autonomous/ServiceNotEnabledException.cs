using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.Common.Exceptions
{
  [Serializable]
  public class ServiceNotEnabledException : Exception
  {
    public ServiceNotEnabledException(string serviceName, object sigOverloadDummy)
      : base(string.Format(CommonResources.ErrorMsgServiceNotEnabledException, serviceName))
    {

    }

    public ServiceNotEnabledException(string errorMsg)
      : base(errorMsg)
    {

    }

    public ServiceNotEnabledException(Exception innerException)
      : base(CommonResources.ErrorMsgServiceNotEnabledException, innerException)
    {

    }
  }
}
