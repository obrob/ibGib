using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class InvalidUsernameException : Exception
  {
    public InvalidUsernameException(string username)
      : base(string.Format(DalResources.ErrorMsgInvalidUsernameException, username))
    {
      Username = Username; 
    }

    public InvalidUsernameException(string errorMsg, string username)
      : base(errorMsg)
    {
      Username = username;
    }

    public InvalidUsernameException(Exception innerException, string username)
      : base(string.Format(DalResources.ErrorMsgInvalidUsernameException, username), innerException)
    {
      Username = username;
    }

    public string Username { get; private set; }
  }
}
