using LearnLanguages.Business.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.WebPages;

namespace LearnLanguages.Mvc4
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      ModelBinders.Binders.DefaultBinder = new Csla.Web.Mvc.CslaModelBinder();

      AreaRegistration.RegisterAllAreas();

      WebApiConfig.Register(GlobalConfiguration.Configuration);

      //These filters set up an "implicit deny" type of structure. 
      //Now, for any anonymous access, you have to add [AllowAnonymous]
      GlobalFilters.Filters.Add(new HandleErrorAttribute());
      GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
      //BundleMobileConfig.RegisterBundles(BundleTable.Bundles);

      //The Android view
      DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("android")
      {
        ContextCondition = Context => Context.Request.Browser.Platform == "Android"
      });

      //The iPhone view
      DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("iphone")
      {
        ContextCondition = Context => Context.Request.Browser.MobileDeviceModel == "iPhone"
      });

      //The mobile view
      //This has a lower priority than the other two so will only be used by a mobile device
      //that isn't Android or iPhone
      DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("mobile")
      {
        ContextCondition = Context => Context.Request.Browser.IsMobileDevice
      });

    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {
      if (Mvc4Helper.CurrentUserIsAuthenticated())
      {
        UserPrincipal.Load(Csla.ApplicationContext.User.Identity.Name);
      }
    }


  }
}