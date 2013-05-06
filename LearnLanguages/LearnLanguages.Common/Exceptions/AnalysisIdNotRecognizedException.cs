using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.Common.Exceptions
{
  [Serializable]
  public class AnalysisIdNotRecognizedException : Exception
  {
    /// <summary>
    /// dummy is to change signature to differentiate this from the 
    /// standard Exception(errormsg) signature.
    /// </summary>
    /// <param name="analysisId"></param>
    /// <param name="dummy"></param>
    public AnalysisIdNotRecognizedException(string analysisId, int dummy = 0)
      : base(string.Format(CommonResources.ErrorMsgAnalysisIdNotRecognizedException, 
                           analysisId))
    {

    }

    public AnalysisIdNotRecognizedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public AnalysisIdNotRecognizedException(Exception innerException,
                                            string analysisId)
      : base(string.Format(CommonResources.ErrorMsgAnalysisIdNotRecognizedException,
                           analysisId), 
             innerException)
    {

    }
  }
}
