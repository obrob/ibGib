using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class VeryBadException : Exception
  {
    public VeryBadException()
      : base(string.Format(DalResources.ErrorMsgVeryBadException, "no detail provided"))
    {
    }

    public VeryBadException(string errorMsg)
      : base(errorMsg)
    {

    }

    public VeryBadException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgVeryBadException, innerException.Message), innerException)
    {

    }

  }
}
