using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class GetRolesFailedException : Exception
  {
    public GetRolesFailedException(string username)
      : base(string.Format(DalResources.ErrorMsgGetRolesFailedException, username))
    {
      Username = username;
    }

    public GetRolesFailedException(string errorMsg, string username)
      : base(errorMsg)
    {
      Username = username;
    }

    public GetRolesFailedException(Exception innerException, string username)
      : base(string.Format(DalResources.ErrorMsgGetRolesFailedException, username), innerException)
    {
      Username = username;
    }

    public string Username { get; private set; }
  }
}
