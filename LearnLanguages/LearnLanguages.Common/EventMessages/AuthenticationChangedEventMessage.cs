namespace LearnLanguages.Common.EventMessages
{
  public class AuthenticationChangedEventMessage 
  {
    public string CurrentPrincipalName
    {
      get { return Csla.ApplicationContext.User.Identity.Name; }
    }
    public bool IsAuthenticated
    {
      get { return Csla.ApplicationContext.User.Identity.IsAuthenticated; }
    }
    public bool IsInRole(string role)
    {
      bool isInRole = Csla.ApplicationContext.User.IsInRole(role);
      return isInRole;
    }

    public static void Publish()
    {
      Services.EventAggregator.Publish(new AuthenticationChangedEventMessage());
    }
  }
}
