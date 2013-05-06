using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnLanguages.Mvc4.Controllers
{
  [AllowAnonymous]
  public class HomeController : Controller
  {
    [AllowAnonymous]
    public ActionResult Index()
    {

      //ViewBag.Message = "ibHome!";

      return View();
    }

    [AllowAnonymous]
    public ActionResult About()
    {
      //ViewBag.Message = "aboutGib!";

      return View();
    }

    [AllowAnonymous]
    public ActionResult Contact()
    {
      //ViewBag.Message = "contactGib!";

      return View();
    }

    [AllowAnonymous]
    public ActionResult Vision()
    {
      //ViewBag.Message = "ibVision!";

      return View();
    }

    
  }
}
