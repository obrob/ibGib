using System;
using Csla.Serialization;

namespace LearnLanguages.Study
{
  [Serializable]
  public class StudyException : Exception
  {
    public StudyException() 
      : base()
    {
    }

    public StudyException(string errorMsg)
      : base(errorMsg)
    {

    }

    public StudyException(Exception innerException)
      : base(StudyResources.ErrorMsgStudyException, innerException)
    {

    }

  }
}
