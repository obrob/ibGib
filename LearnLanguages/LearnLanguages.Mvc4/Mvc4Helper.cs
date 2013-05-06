using LearnLanguages.Business.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LearnLanguages.Mvc4
{
  public static class Mvc4Helper
  {
    public static bool CurrentUserIsAuthenticated()
    {
      return Csla.ApplicationContext.User != null &&
             Csla.ApplicationContext.User.Identity.IsAuthenticated;
    }
  }
}