using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class FetchFailedException : Exception
  {
    public FetchFailedException()
      : base(DalResources.ErrorMsgFetchFailedException)
    {
      
    }

    public FetchFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public FetchFailedException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgFetchFailedException, innerException.Message), innerException)
    {

    }
  }
}
