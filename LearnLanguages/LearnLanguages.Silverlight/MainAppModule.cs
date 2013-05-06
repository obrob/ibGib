using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight
{
  [Export(typeof(IModule))]
  public class MainAppModule : IModule
  {
    
    public void Initialize()
    {
      //REGISTER PAGES WITH NAVIGATOR
      var pages = new List<IPage>();
      var pageExports = Services.Container.GetExports<IPage>();
      foreach (var pageExport in pageExports)
        pages.Add(pageExport.Value);

      for (int i = 0; i < pages.Count; i++)
      {
        var page = pages[i];
        //THIS REGISTERS THE PAGE WITH THE NAVIGATOR
        page.Initialize();
      }
    }
  }
}
