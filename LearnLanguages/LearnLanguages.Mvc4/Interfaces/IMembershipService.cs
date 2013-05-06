using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LearnLanguages.Mvc4.Interfaces
{
  public interface IMembershipService
  {
    bool ValidateUser(string username, string password);
    MembershipCreateStatus CreateUser(string username, string password, string email);
    bool ChangePassword(string username, string oldPassword, string newPassword);
  }
}