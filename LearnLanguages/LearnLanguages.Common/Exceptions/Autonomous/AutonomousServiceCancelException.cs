using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.Common.Exceptions
{
  [Serializable]
  public class AutonomousServiceCancelException : Exception
  {
    public AutonomousServiceCancelException(string serviceName, Exception innerException)
      : base(string.Format(CommonResources.ErrorMsgAutonomousServiceCancelException, 
                           serviceName, 
                           innerException.Message),
             innerException)
    {

    }

    public AutonomousServiceCancelException(string errorMsg)
      : base(errorMsg)
    {

    }

    public AutonomousServiceCancelException(Exception innerException)
      : base(string.Format(CommonResources.ErrorMsgAutonomousServiceCancelException, 
                           "SvcName not given", 
                           innerException.Message), 
             innerException)
    {

    }
  }
}
