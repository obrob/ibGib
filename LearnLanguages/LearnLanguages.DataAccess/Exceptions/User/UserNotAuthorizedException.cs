using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class UserNotAuthorizedException : Exception
  {
    public UserNotAuthorizedException()
      : base(DalResources.ErrorMsgUserNotAuthorizedException)
    {

    }

    public UserNotAuthorizedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public UserNotAuthorizedException(string descriptionOfAttempt, int signatureDummy)
      : base(string.Format(DalResources.ErrorMsgUserNotAuthorizedToDoException, descriptionOfAttempt))
    {
      
    }

    public UserNotAuthorizedException(Exception innerException)
      : base(DalResources.ErrorMsgUserNotAuthorizedException, innerException)
    {

    }
  }
}
