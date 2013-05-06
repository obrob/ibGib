using System.Collections.Generic;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation.Interfaces;
using System.Linq;
using System;

namespace LearnLanguages.Navigation
{
  public class NavigationSet : INavigationSet
  {
    public NavigationSet()
    {
      Pages = new List<IPage>();
    }

    public int SortOrder { get; set; }
    public string Text { get; set; }
    public string ToolTip { get; set; }
    public IList<IPage> Pages { get; private set; }

    public void RegisterPage(IPage page)
    {
      //CHECK FOR DUPLICATE
      if (Pages.Contains(page))
        throw new Exceptions.PageAlreadyRegisteredException(page);

      //ADD THE PAGE
      Pages.Add(page);

      //HARD-CAST AS LIST TO USE THE SORT FUNCTION
      List<IPage> pages = (List<IPage>)Pages;

      //SORT
      pages.Sort((a, b) =>
        {
          //if the comparison is 0, then they are equal.
          //if a is before b, then it should be negative
          //otherwise, it should be positive
          int comparison = 0;
          comparison = a.NavButtonOrder - b.NavButtonOrder;
          return comparison;
        });

    }
    public void UnregisterPage(IPage page)
    {
      if (Pages.Contains(page))
        Pages.Remove(page);
    }
  }
}
