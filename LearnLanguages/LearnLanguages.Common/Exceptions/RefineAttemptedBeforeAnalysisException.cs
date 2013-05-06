using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.Common.Exceptions
{
  [Serializable]
  public class RefineAttemptedBeforeAnalysisException : Exception
  {
    public RefineAttemptedBeforeAnalysisException()
      : base(CommonResources.ErrorMsgRefineAttemptedBeforeAnalysisException)
    {

    }

    public RefineAttemptedBeforeAnalysisException(string errorMsg)
      : base(errorMsg)
    {

    }

    public RefineAttemptedBeforeAnalysisException(Exception innerException)
      : base(CommonResources.ErrorMsgRefineAttemptedBeforeAnalysisException,
             innerException)
    {

    }
  }
}
