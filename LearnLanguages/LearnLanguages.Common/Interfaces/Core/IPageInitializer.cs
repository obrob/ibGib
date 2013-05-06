using System;
using System.Collections.Generic;

namespace LearnLanguages.Common.Interfaces
{
  /// <summary>
  /// Put one of these in your project in which you 
  /// want to initiale pages. This is where you 
  /// should register your pages with the Navigator.
  /// </summary>
  public interface IPageInitializer : IHaveId
  {
    void InitializePages();
  }
}
