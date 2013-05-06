using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;

namespace LearnLanguages.Common.Interfaces
{
  public interface INavigationSet
  {
    /// <summary>
    /// This text appears on the button for this
    /// navigation set.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// ToolTip for the navigation set's button
    /// </summary>
    string ToolTip { get; set; }

    /// <summary>
    /// These pages will have sub-buttons under 
    /// this navigation set's button.
    /// </summary>
    IList<IPage> Pages { get; }

    /// <summary>
    /// This is the order in the navigation panel 
    /// that this nav set prefers.
    /// This is zero-based (0 is first);
    /// </summary>
    int SortOrder { get; set; }

    /// <summary>
    /// Registers a page with this navigation set.
    /// </summary>
    void RegisterPage(IPage page);

    /// <summary>
    /// Unregisters page from this navigation set. 
    /// If the page is not found, throws an exception.
    /// </summary>
    void UnregisterPage(IPage page);

    ///// <summary>
    ///// The user must be in at least one of these roles.
    ///// If this is empty, then the user does not have to
    ///// be authenticated to see this NavSet
    ///// </summary>
    //IList<string> Roles { get; }
  }
}
