using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class UpdateFailedException : Exception
  {
    public UpdateFailedException()  
      : base(DalResources.ErrorMsgUpdateFailedException)
    {

    }

    public UpdateFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public UpdateFailedException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgUpdateFailedException, innerException.Message), innerException)
    {

    }
  }
}
