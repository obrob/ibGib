using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class LoadUserDataException : Exception
  {
    public LoadUserDataException(string username)
      : base(string.Format(DalResources.ErrorMsgLoadUserDataException, username))
    {
      Username = username;
    }

    public LoadUserDataException(string errorMsg, string username)
      : base(errorMsg)
    {
      Username = username;
    }

    public LoadUserDataException(Exception innerException, string username)
      : base(string.Format(DalResources.ErrorMsgLoadUserDataException, username), innerException)
    {
      Username = username;
    }

    public string Username { get; private set; }
  }
}
