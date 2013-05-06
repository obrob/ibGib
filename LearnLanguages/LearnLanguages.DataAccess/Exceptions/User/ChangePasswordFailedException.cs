using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class ChangePasswordFailedException : Exception
  {
    public ChangePasswordFailedException() 
      : base()
    {
    }

    public ChangePasswordFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public ChangePasswordFailedException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgChangePasswordFailedException, innerException.Message), innerException)
    {

    }

  }
}
