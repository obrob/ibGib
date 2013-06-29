using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.Common.Exceptions
{
  [Serializable]
  public class AutonomousServiceExecuteException : Exception
  {
    public AutonomousServiceExecuteException(string serviceName, Exception innerException)
      : base(string.Format(CommonResources.ErrorMsgAutonomousServiceExecuteException, 
                           serviceName, 
                           innerException.Message),
             innerException)
    {

    }

    public AutonomousServiceExecuteException(string errorMsg)
      : base(errorMsg)
    {

    }

    public AutonomousServiceExecuteException(Exception innerException)
      : base(string.Format(CommonResources.ErrorMsgAutonomousServiceExecuteException, 
                           "SvcName not given", 
                           innerException.Message), 
             innerException)
    {

    }
  }
}
