using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.Common.Exceptions
{
  [Serializable]
  public class UserNotAuthenticatedException : Exception
  {
    public UserNotAuthenticatedException()
      : base(CommonResources.ErrorMsgUserNotAuthenticatedException)
    {

    }

    public UserNotAuthenticatedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public UserNotAuthenticatedException(Exception innerException)
      : base(CommonResources.ErrorMsgUserNotAuthenticatedException, innerException)
    {

    }
  }
}
