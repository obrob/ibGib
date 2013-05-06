using LearnLanguages.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LearnLanguages.Mvc4.Controllers
{
  public class MultiLineTextListController : Csla.Web.Mvc.Controller, Csla.Web.Mvc.IModelCreator
  {
    public object CreateModel(Type modelType)
    {
      return null;
      // this CreateModel method is entirely optional, and
      // exists to demonstrate how you implement the
      // IModelCreator interface
      //if (modelType.Equals(typeof(ProjectEdit)))
      //  return ProjectEdit.NewProject();
      //else
      //  return Activator.CreateInstance(modelType);
      if (modelType.Equals(typeof(MultiLineTextList)))
        return new MultiLineTextList();
      else
        return Activator.CreateInstance(modelType);
    }

    //
    // GET: /MultiLineTextList/

    public async Task<ActionResult> Index()
    {
      ViewData.Model = await MultiLineTextList.GetAllAsync();
      return View();
    }

    //
    // GET: /MultiLineTextList/Details/5

    public ActionResult Details(int id)
    {
      return View();
    }

    //
    // GET: /MultiLineTextList/Create

    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /MultiLineTextList/Create

    [HttpPost]
    public ActionResult Create(FormCollection collection)
    {
      try
      {
        // TODO: Add insert logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    //
    // GET: /MultiLineTextList/Edit/5

    public ActionResult Edit(int id)
    {
      return View();
    }

    //
    // POST: /MultiLineTextList/Edit/5

    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add update logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    //
    // GET: /MultiLineTextList/Delete/5

    public ActionResult Delete(int id)
    {
      return View();
    }

    //
    // POST: /MultiLineTextList/Delete/5

    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add delete logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}
