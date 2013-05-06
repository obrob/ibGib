using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace LearnLanguages.Common.Interfaces
{
  public interface INavigationPanelViewModel : IViewModelBase
  {
    void PopulatePanel(IList<INavigationSet> navigationSets);
  }
}
