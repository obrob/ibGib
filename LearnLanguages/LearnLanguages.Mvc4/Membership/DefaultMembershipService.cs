using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LearnLanguages.Mvc4.Interfaces;

namespace LearnLanguages.Mvc4
{
  public class DefaultMembershipService : IMembershipService
  {
    public DefaultMembershipService()
    {
      _Provider = new CustomMembershipProvider();
    }

    private CustomMembershipProvider _Provider { get; set; }


    public bool ValidateUser(string username, string password)
    {
      return _Provider.ValidateUser(username, password);
    }

    public System.Web.Security.MembershipCreateStatus CreateUser(string username, string password, string email)
    {
      throw new NotImplementedException();
    }

    public bool ChangePassword(string username, string oldPassword, string newPassword)
    {
      throw new NotImplementedException();
    }
  }
}