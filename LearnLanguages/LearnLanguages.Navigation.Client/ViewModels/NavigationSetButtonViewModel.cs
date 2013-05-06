using System;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Navigation;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.EventMessages;
using System.ComponentModel.Composition;

namespace LearnLanguages.Navigation.ViewModels
{
  [Export(typeof(NavigationSetButtonViewModel))]
  public class NavigationSetButtonViewModel : ViewModelBase,
                                              ICanExpand
  {
    public NavigationSetButtonViewModel(INavigationSet navigationSet)
    {
      NavigationSet = navigationSet;
      _IsExpanded = false;
    }

    private INavigationSet _NavigationSet;
    /// <summary>
    /// Binding should bind to NavigationSet_Text 
    /// and NavigationSet_ToolTip.
    /// </summary>
    public INavigationSet NavigationSet
    {
      get { return _NavigationSet; }
      set
      {
        if (value != _NavigationSet)
        {
          _NavigationSet = value;
          NotifyOfPropertyChange(() => NavigationSet);
          if (_NavigationSet != null)
            Text = _NavigationSet.Text;
          else
            Text = "Null NavigationSet";
        }
      }
    }

    private string _Text;
    public string Text
    {
      get { return _Text; }
      set
      {
        if (value != _Text)
        {
          _Text = value;
          NotifyOfPropertyChange(() => Text);
        }
      }
    }

    public virtual void ToggleExpanded()
    {
      IsExpanded = !IsExpanded;
      ExpandedChangedEventMessage.Publish(this);
    }

    private bool _IsExpanded;
    public bool IsExpanded
    {
      get { return _IsExpanded; }
      set
      {
        if (value != _IsExpanded)
        {
          _IsExpanded = value;
          NotifyOfPropertyChange(() => IsExpanded);
        }
      }
    }
  }
}
