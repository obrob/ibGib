using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class VerifyUserFailedException : Exception
  {
    public VerifyUserFailedException(string username)
      : base(string.Format(DalResources.ErrorMsgVerifyUserFailedException, username))
    {
      Username = username;
    }

    public VerifyUserFailedException(string errorMsg, string username)
      : base(errorMsg)
    {
      Username = username;
    }

    public VerifyUserFailedException(Exception innerException, string username)
      : base(DalResources.ErrorMsgVerifyUserFailedException, innerException)
    {
      Username = username;
    }

    public string Username { get; private set; }
  }
}
