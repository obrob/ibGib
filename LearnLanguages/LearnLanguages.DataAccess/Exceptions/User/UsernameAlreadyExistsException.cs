using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class UsernameAlreadyExistsException : Exception
  {
    public UsernameAlreadyExistsException(string username)
      : base(string.Format(DalResources.ErrorMsgUsernameAlreadyExistsException, username))
    {
      Username = username;
    }

    public UsernameAlreadyExistsException(string errorMsg, string username)
      : base(errorMsg)
    {
      Username = username;
    }

    public UsernameAlreadyExistsException(Exception innerException, string username)
      : base(string.Format(DalResources.ErrorMsgUsernameAlreadyExistsException, username), innerException)
    {
      Username = username;
    }

    public string Username { get; private set; }
  }
}
