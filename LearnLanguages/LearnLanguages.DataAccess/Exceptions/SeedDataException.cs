using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class SeedDataException : Exception
  {
    public SeedDataException() 
      : base()
    {
    }

    public SeedDataException(string errorMsg)
      : base(errorMsg)
    {

    }

    public SeedDataException(Exception innerException)
      : base(DalResources.ErrorMsgSeedDataException, innerException)
    {

    }

  }
}
