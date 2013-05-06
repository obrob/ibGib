using LearnLanguages.Common.Enums;
using System;
using System.Collections.Generic;

namespace LearnLanguages.Common.Interfaces
{
  /// <summary>
  /// Behavior for a page. The app is composed of
  /// a navigation panel and content. You use a 
  /// page for the concept of a location in the 
  /// app. It has a corresponding navigation 
  /// button and navigation set on the navigation 
  /// panel. It also has a ContentViewModel, 
  /// of type IPageViewModel, that it is tightly
  /// coupled with.
  /// Also, a page is associated with Roles.
  /// This means that you must be in ONE of these
  /// roles to view the page (You do NOT have to
  /// be in ALL of the roles).
  /// </summary>
  public interface IPage : IHaveId
  {
    /// <summary>
    /// Do initialization code for this page.
    /// </summary>
    void Initialize();

    /// <summary>
    /// If true, this page will not have a navigation button on the navigation
    /// panel.
    /// </summary>
    bool HideNavButton { get; set; }

    /// <summary>
    /// Text of NavSet. Needs to be refactored
    /// </summary>
    string NavSet { get; set; }

    /// <summary>
    /// This is the order in the navigation panel 
    /// that this page prefers.
    /// This is zero-based (0 is first);
    /// </summary>
    int NavButtonOrder { get; set; }

    /// <summary>
    /// This is the text that will appear on the navigation button. 
    /// This does not have to be unique, but the combination 
    /// NavSet+NavText must be unique.
    /// </summary>
    string NavText { get; set; }

    /// <summary>
    /// Is the page only for authenticated users, non-authenticated users,
    /// or either?
    /// </summary>
    IntendedUserAuthenticationState IntendedUserAuthenticationState { get; }

    /// <summary>
    /// If the RequiredAuthenticationState is set to Authenticated, then
    /// the user must be in at least one of these roles.
    /// </summary>
    IList<string> Roles { get; }

    /// <summary>
    /// This is the content viewmodel, ie the "meat" of the page
    /// </summary>
    IPageViewModel ContentViewModel { get; set; }
  }
}
