using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class CreateFailedException : Exception
  {
    public CreateFailedException()
      : base(DalResources.ErrorMsgCreateFailedException)
    {

    }

    public CreateFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public CreateFailedException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgCreateFailedException, innerException.Message), innerException)
    {

    }
  }
}
