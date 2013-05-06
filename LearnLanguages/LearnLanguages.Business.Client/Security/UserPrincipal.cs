using System;
using System.Net;

using Csla.Security;
using Csla.Serialization;
using System.Threading.Tasks;

namespace LearnLanguages.Business.Security
{
  [Serializable]
  public class UserPrincipal : CslaPrincipal
  {
    /// <summary>
    /// UserPrincipal must have a default constructor for the data portal to serialize it with every data portal call.
    /// </summary>
    public UserPrincipal()
      : base()
    {

    }
    private UserPrincipal(UserIdentity identity)
      : base(identity)
    { }

    public static async Task<Exception> LoginAsync(string username, string clearUnsaltedPassword)
    {
      Exception retError = null;
      try
      {
        var userIdentity = await UserIdentity.GetUserIdentityAsync(username, clearUnsaltedPassword);
        if (userIdentity != null)
          Csla.ApplicationContext.User = new UserPrincipal(userIdentity);
        else
          Logout();
      }
      catch (Exception ex)
      {
        Logout();
        retError = ex;
      }

      return retError;

      //UserIdentity.GetUserIdentity(username, clearUnsaltedPassword, (s, r) =>
      //  {
      //    if (r.Error != null)
      //      Logout();
      //    else
      //      Csla.ApplicationContext.User = new UserPrincipal(r.Object); //r.Object is UserIdentity

      //    completed(r.Error);
      //  });
    }
#if !SILVERLIGHT
    public static void Login(string username, string clearUnsaltedPassword)
    {
      var identity = UserIdentity.GetUserIdentity(username, clearUnsaltedPassword); 
      //if credentials dont pass, identity will not be IsAuthenticated.
      Csla.ApplicationContext.User = new UserPrincipal(identity);
    }
    public static void Load(string username)
    {
      var identity = UserIdentity.GetUserIdentity(username);
      Csla.ApplicationContext.User = new UserPrincipal(identity);
    }
#endif
    public static void Logout()
    {
      Csla.ApplicationContext.User = new UnauthenticatedPrincipal();
    }
  }
}
