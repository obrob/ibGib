using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class UsernameNotFoundException : Exception
  {
    public UsernameNotFoundException(string username)
      : base(string.Format(DalResources.ErrorMsgUsernameNotFoundException, username))
    {
      Username = username;
    }

    public UsernameNotFoundException(string errorMsg, string username)
      : base(errorMsg)
    {
      Username = username;
    }

    public UsernameNotFoundException(Exception innerException, string username)
      : base(string.Format(DalResources.ErrorMsgUsernameNotFoundException, username), innerException)
    {
      Username = username;
    }

    public string Username { get; private set; }
  }
}
