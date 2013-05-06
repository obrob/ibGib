using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class InvalidPasswordException : Exception
  {
    public InvalidPasswordException(string password)
      : base(string.Format(DalResources.ErrorMsgInvalidPasswordException, password))
    {
      Password = password; 
    }

    public InvalidPasswordException(string errorMsg, string password)
      : base(errorMsg)
    {
      Password = password;
    }

    public InvalidPasswordException(Exception innerException, string password)
      : base(string.Format(DalResources.ErrorMsgInvalidPasswordException, password), innerException)
    {
      Password = password;
    }

    public string Password { get; private set; }
  }
}
