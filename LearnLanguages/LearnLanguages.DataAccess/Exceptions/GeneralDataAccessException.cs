using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class GeneralDataAccessException : Exception
  {
    public GeneralDataAccessException() 
      : base()
    {
    }

    public GeneralDataAccessException(string errorMsg)
      : base(errorMsg)
    {

    }

    public GeneralDataAccessException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgGeneralDataAccessException, innerException.Message), innerException)
    {

    }

  }
}
