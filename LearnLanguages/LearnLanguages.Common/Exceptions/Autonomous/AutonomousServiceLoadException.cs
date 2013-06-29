using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.Common.Exceptions
{
  [Serializable]
  public class AutonomousServiceLoadException : Exception
  {
    public AutonomousServiceLoadException(string serviceName, Exception innerException)
      : base(string.Format(CommonResources.ErrorMsgAutonomousServiceLoadException, 
                           serviceName, 
                           innerException.Message),
             innerException)
    {

    }

    public AutonomousServiceLoadException(string errorMsg)
      : base(errorMsg)
    {

    }

    public AutonomousServiceLoadException(Exception innerException)
      : base(string.Format(CommonResources.ErrorMsgAutonomousServiceLoadException, 
                           "SvcName not given", 
                           innerException.Message), 
             innerException)
    {

    }
  }
}
