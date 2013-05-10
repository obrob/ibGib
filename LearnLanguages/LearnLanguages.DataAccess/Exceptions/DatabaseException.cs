using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class DatabaseException : Exception
  {
    public DatabaseException() 
      : base()
    {
    }

    public DatabaseException(string errorMsg)
      : base(errorMsg)
    {

    }

    public DatabaseException(Exception innerException)
      : base(DalResources.ErrorMsgDatabaseException, innerException)
    {

    }

  }
}
