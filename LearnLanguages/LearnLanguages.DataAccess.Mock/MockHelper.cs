using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Mock
{
  public static class MockHelper
  {
    public static Guid GetCurrentUserId()
    {
      string currentUsername = Csla.ApplicationContext.User.Identity.Name;
      var results = from u in SeedData.Ton.Users
                    where u.Username == currentUsername
                    select u;
      if (results.Count() == 1)
      {
        return results.First().Id;
      }
      else if (results.Count() == 0)
      {
        throw new Exceptions.UsernameNotFoundException(currentUsername);
      }
      else
      {
        //WTH? VERY BAD EXCEPTION. COUNT > 1 OR NEGATIVE. MULTIPLE USERS?
        throw new Exceptions.VeryBadException(DataAccess.DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
      }

    }
    public static void CheckAuthentication()
    {
      if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
        throw new Common.Exceptions.UserNotAuthenticatedException();
    }
  }
}
