using LearnLanguages.Business.Security;
using LearnLanguages.Mvc4.Interfaces;
using LearnLanguages.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LearnLanguages.Mvc4.Controllers
{
  public class LoginController : Csla.Web.Mvc.Controller
  {

    public IFormsAuthenticationService FormsService { get; set; }
    public IMembershipService MembershipService { get; set; }

    protected override void Initialize(RequestContext requestContext)
    {
      if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
      if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

      base.Initialize(requestContext);
    }

    //
    // GET: /Login/

    [AllowAnonymous]
    public ActionResult Index()
    {
      var triggerAuthenticatedHack = ModelState.IsValid;

      if (Mvc4Helper.CurrentUserIsAuthenticated())
        return RedirectToAction("Index", "Home");
      else
        return View();

      
    }

    //public async Task<ActionResult> Login()
    //{
    //  await UserPrincipal.LoginAsync("user", "password");
      
    //  return View();
    //}

    [HttpPost]
    [AllowAnonymous]
    public ActionResult Index(LoginViewModel model, string returnUrl)
    {
      var triggerAuthenticateHack = ModelState.IsValid;
      if (Mvc4Helper.CurrentUserIsAuthenticated())
      {
        if (Url.IsLocalUrl(returnUrl))
        {
          return Redirect(returnUrl);
        }
        else
        {
          return RedirectToAction("Index", "Home");
        }
      }
      else
      {
        if (ModelState.IsValid)
        {
          if (MembershipService.ValidateUser(model.Username, model.Password))
          {
            FormsService.SignIn(model.Username, model.RememberMe);
            if (Url.IsLocalUrl(returnUrl))
            {
              return Redirect(returnUrl);
            }
            else
            {
              return RedirectToAction("Index", "Home");
            }
          }
          else
          {
            ModelState.AddModelError("", "The username or password provided is incorrect.");
          }
        }
      }
      // If we got this far, something failed, redisplay form
      return View(model);
    }

    public ActionResult Logout()
    {
      UserPrincipal.Logout();

      return View();
    }

    ////
    //// GET: /Login/Details/5

    //public ActionResult Details(int id)
    //{
    //  return View();
    //}

    ////
    //// GET: /Login/Create

    //public ActionResult Create()
    //{
    //  return View();
    //}

    ////
    //// POST: /Login/Create

    //[HttpPost]
    //public ActionResult Create(FormCollection collection)
    //{
    //  try
    //  {
    //    // TODO: Add insert logic here

    //    return RedirectToAction("Index");
    //  }
    //  catch
    //  {
    //    return View();
    //  }
    //}

    ////
    //// GET: /Login/Edit/5

    //public ActionResult Edit(int id)
    //{
    //  return View();
    //}

    ////
    //// POST: /Login/Edit/5

    //[HttpPost]
    //public ActionResult Edit(int id, FormCollection collection)
    //{
    //  try
    //  {
    //    // TODO: Add update logic here

    //    return RedirectToAction("Index");
    //  }
    //  catch
    //  {
    //    return View();
    //  }
    //}

    ////
    //// GET: /Login/Delete/5

    //public ActionResult Delete(int id)
    //{
    //  return View();
    //}

    ////
    //// POST: /Login/Delete/5

    //[HttpPost]
    //public ActionResult Delete(int id, FormCollection collection)
    //{
    //  try
    //  {
    //    // TODO: Add delete logic here

    //    return RedirectToAction("Index");
    //  }
    //  catch
    //  {
    //    return View();
    //  }
    //}
  }
}
