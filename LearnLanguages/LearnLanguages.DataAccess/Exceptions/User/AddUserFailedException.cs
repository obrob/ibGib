using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class AddUserFailedException : Exception
  {
    public AddUserFailedException(string username)
      : base(string.Format(DalResources.ErrorMsgAddUserFailedException, username))
    {
      Username = username;
    }

    public AddUserFailedException(string errorMsg, string username)
      : base(errorMsg)
    {
      Username = username;
    }

    public AddUserFailedException(Exception innerException, string username)
      : base(DalResources.ErrorMsgAddUserFailedException, innerException)
    {
      Username = username;
    }

    public string Username { get; private set; }
  }
}
