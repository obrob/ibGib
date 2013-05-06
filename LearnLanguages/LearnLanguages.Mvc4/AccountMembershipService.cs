using LearnLanguages.Mvc4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LearnLanguages.Mvc4
{
  public class AccountMembershipService : IMembershipService
  {
    private readonly MembershipProvider _provider;

    public AccountMembershipService()
      : this(null)
    {
    }

    public AccountMembershipService(MembershipProvider provider)
    {
      _provider = provider ?? Membership.Provider;
    }

    public int MinPasswordLength
    {
      get
      {
        return _provider.MinRequiredPasswordLength;
      }
    }

    public bool ValidateUser(string username, string password)
    {
      CheckUsernamePassword(username, password);
      return _provider.ValidateUser(username, password);
    }

    private static void CheckUsernamePassword(string username, string password)
    {
      var errorDescription = "";
      if (!Common.CommonHelper.UsernameIsValid(username, out errorDescription))
        throw new ArgumentException(errorDescription, "username");
      if (!Common.CommonHelper.PasswordIsValid(password, out errorDescription))
        throw new ArgumentException(errorDescription, "password");

    }

    public MembershipCreateStatus CreateUser(string username, string password, string email)
    {
      throw new NotSupportedException("CreateUser is not supported yet");
      //CheckUsernamePassword(username, password);
      //if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

      //MembershipCreateStatus status;
      //_provider.CreateUser(username, password, email, null, null, true, null, out status);
      //return status;
    }

    public bool ChangePassword(string username, string oldPassword, string newPassword)
    {
      throw new NotSupportedException("ChangePassword is not supported yet");

      //if (String.IsNullOrEmpty(username)) throw new ArgumentException("Value cannot be null or empty.", "username");
      //if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
      //if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

      //// The underlying ChangePassword() will throw an exception rather
      //// than return false in certain failure scenarios.
      //try
      //{
      //  MembershipUser currentUser = _provider.GetUser(username, true /* userIsOnline */);
      //  return currentUser.ChangePassword(oldPassword, newPassword);
      //}
      //catch (ArgumentException)
      //{
      //  return false;
      //}
      //catch (MembershipPasswordException)
      //{
      //  return false;
      //}
    }
  }
}