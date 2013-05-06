using LearnLanguages.Common.Enums;
using LearnLanguages.Common.Interfaces;
using System;
using System.Collections.Generic;
namespace LearnLanguages.Common.Core
{
  public abstract class PageBase : IPage
  {
    public PageBase()
    {
      InitializeRoles();

      //BY DEFAULT, ONLY AUTHENTICATED USERS CAN SEE PAGES
      IntendedUserAuthenticationState = 
        Enums.IntendedUserAuthenticationState.Authenticated;

      HideNavButton = false;
    }

    public Guid Id { get; protected set; }

    public string NavSet { get; set; }
    public int NavButtonOrder { get; set; }
    public string NavText { get; set; }
    public bool HideNavButton { get; set; }
    
    public IPageViewModel ContentViewModel { get; set; }

    public IntendedUserAuthenticationState IntendedUserAuthenticationState { get; set; }
    protected IList<string> _Roles { get; set; }
    public IList<string> Roles
    {
      get { return GetRolesImpl(); }
    }
    protected virtual IList<string> GetRolesImpl()
    {
      return _Roles;
    }

    public abstract void Initialize();
    protected abstract void InitializeRoles();
  }
}
