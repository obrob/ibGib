using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace LearnLanguages.Common.Interfaces
{
  public interface IViewModelBase : IPartImportsSatisfiedNotification 
  {
    string ToolTip { get; set; }
    bool LoadFromUri(Uri uri);
    //string GetCoreViewModelName(bool withSpaces = false);
    bool ShowGridLines { get; }
    Visibility ViewModelVisibility { get; set; }
    bool IsEnabled { get; set; }
    bool IsBusy { get; set; }
    string BusyContent { get; set; }
  }
}
