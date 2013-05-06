using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class GetAllFailedException : Exception
  {
    public GetAllFailedException()
      : base(DalResources.ErrorMsgGetAllFailedException)
    {
      
    }

    public GetAllFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public GetAllFailedException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgGetAllFailedException, innerException.Message), innerException)
    {

    }
  }
}
