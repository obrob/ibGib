using LearnLanguages.Common.Enums;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Navigation
{
  public static class NavigationHelper
  {
    public static bool CurrentUserCanAccessPage(IPage page)
    {
      //CHECK IF THE PAGE.INTENDEDUSERAUTHENTICATIONSTATE IS CORRECT
      //FOR CURRENT USER
      var authState = page.IntendedUserAuthenticationState;
      if (authState != IntendedUserAuthenticationState.Either)
      {
        if (
            //USER IS AUTHENTICATED, AND STATE IS NOT AUTHENTICATED
            (
             Csla.ApplicationContext.User.Identity.IsAuthenticated 
             && 
             authState != IntendedUserAuthenticationState.Authenticated
            )

            //OR
            ||

            (
            //USER IS NOT AUTHENTICATED, AND STATE IS NOT "NOT AUTHENTICATED"
             !Csla.ApplicationContext.User.Identity.IsAuthenticated 
             && 
             authState != IntendedUserAuthenticationState.NotAuthenticated
            )
          )
          return false;
      }


      //IF THE ROLES IS EMPTY, THEN ANY USER CAN ACCESS THE PAGE
      if (page.Roles.Count == 0 
        || 
        //OR IF THE PAGE IS INTENDED FOR NONAUTHENTICATED USERS
          page.IntendedUserAuthenticationState == 
            Common.Enums.IntendedUserAuthenticationState.NotAuthenticated
        ||
        //OR IF THE PAGE IS INTENDED FOR EITHER NONAUTHENTICATED AND AUTHENTICATED USERS
          page.IntendedUserAuthenticationState == 
            Common.Enums.IntendedUserAuthenticationState.Either)
        return true;

      //ITERATE THROUGH PAGE ROLES, CHECKING AGAINST USER.ISINROLE
      //AND PAGE.INTENDEDUSERAUTHENTICATIONSTATE
      for (int i = 0; i < page.Roles.Count; i++)
      {
        var pageRole = page.Roles[i];
        if (Csla.ApplicationContext.User.IsInRole(pageRole))
          //USER ROLE MATCHES, SO RETURN TRUE
          return true;
      }

      //NO MATCHES FOUND, SO USER DOES NOT HAVE THE CORRECT ROLE
      return false;
    }

  }
}
