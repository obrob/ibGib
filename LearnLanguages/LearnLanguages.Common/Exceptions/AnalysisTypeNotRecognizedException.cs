using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.Common.Exceptions
{
  [Serializable]
  public class AnalysisTypeNotRecognizedException : Exception
  {
    public AnalysisTypeNotRecognizedException(Type analysisType)
      : base(string.Format(CommonResources.ErrorMsgAnalysisTypeNotRecognizedException, 
                           analysisType))
    {

    }

    public AnalysisTypeNotRecognizedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public AnalysisTypeNotRecognizedException(Exception innerException,
                                              Type analysisType)
      : base(string.Format(CommonResources.ErrorMsgAnalysisTypeNotRecognizedException,
                           analysisType), 
             innerException)
    {

    }
  }
}
