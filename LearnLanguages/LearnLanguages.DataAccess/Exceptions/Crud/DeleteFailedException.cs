using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class DeleteFailedException : Exception
  {
    public DeleteFailedException()
      : base(DalResources.ErrorMsgDeleteFailedException)
    {

    }

    public DeleteFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public DeleteFailedException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgDeleteFailedException, innerException.Message), innerException)
    {

    }
  }
}
