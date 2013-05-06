using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class BadCriteriaException : Exception
  {
    public BadCriteriaException(string correctCriteriaDescription)
      : base(string.Format(DalResources.ErrorMsgBadCriteriaException, correctCriteriaDescription))
    {

    }

    public BadCriteriaException(string errorMsg, object dummyForDifferentSignature = null)
      : base(errorMsg)
    {

    }

    public BadCriteriaException(Exception innerException, string correctCriteriaDescription)
      : base(string.Format(DalResources.ErrorMsgBadCriteriaException, correctCriteriaDescription), innerException)
    {

    }
  }
}
