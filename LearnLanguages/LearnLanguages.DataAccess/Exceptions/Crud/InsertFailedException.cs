using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class InsertFailedException : Exception
  {
    public InsertFailedException()
      : base(DalResources.ErrorMsgInsertFailedException)
    {

    }

    public InsertFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public InsertFailedException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgInsertFailedException, innerException.Message), innerException)
    {

    }
  }
}
