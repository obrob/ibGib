using System;

namespace LearnLanguages.Common.Interfaces
{
  /// <summary>
  /// Special kind of viewmodel that corresponds to a page.
  /// This is tightly coupled with IPage.
  /// </summary>
  public interface IPageViewModel : IViewModelBase
  {
    string Title { get; set; }
    string Description { get; set; }
    string Instructions { get; set; }
    IPage Page { get; set; }
  }
}
