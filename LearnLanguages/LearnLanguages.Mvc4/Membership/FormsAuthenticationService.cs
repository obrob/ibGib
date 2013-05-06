using LearnLanguages.Mvc4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LearnLanguages.Mvc4
{
  public class FormsAuthenticationService : IFormsAuthenticationService
  {
    public void SignIn(string username, bool createPersistentCookie)
    {
      string errorDescription = "";
      if (!Common.CommonHelper.UsernameIsValid(username, out errorDescription))
        throw new ArgumentException(errorDescription, "username");

      FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
    }

    public void SignOut()
    {
      FormsAuthentication.SignOut();
    }
  }
}