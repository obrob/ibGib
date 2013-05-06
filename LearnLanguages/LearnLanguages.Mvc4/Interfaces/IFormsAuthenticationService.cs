﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LearnLanguages.Mvc4.Interfaces
{
  public interface IFormsAuthenticationService
  {
    void SignIn(string userName, bool createPersistentCookie);
    void SignOut();
  }
}