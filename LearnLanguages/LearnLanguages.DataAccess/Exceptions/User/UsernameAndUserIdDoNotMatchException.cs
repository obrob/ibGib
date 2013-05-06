using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class UsernameAndUserIdDoNotMatchException : Exception
  {
    public UsernameAndUserIdDoNotMatchException(string username, Guid userId)
      : base(string.Format(DalResources.ErrorMsgUsernameAndUserIdDoNotMatchException, username, userId))
    {
      Username = username;
      UserId = userId;
    }

    public UsernameAndUserIdDoNotMatchException(string errorMsg, string username, Guid userId)
      : base(errorMsg)
    {
      Username = username;
      UserId = userId;
    }

    public UsernameAndUserIdDoNotMatchException(Exception innerException, string username, Guid userId)
      : base(string.Format(DalResources.ErrorMsgUsernameAndUserIdDoNotMatchException, username, userId), innerException)
    {
      Username = username;
      UserId = userId;
    }

    public string Username { get; private set; }
    public Guid UserId { get; private set; }
  }
}
