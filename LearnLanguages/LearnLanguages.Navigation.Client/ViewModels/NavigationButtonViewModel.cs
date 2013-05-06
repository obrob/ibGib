using System;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Navigation;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation.EventMessages;

namespace LearnLanguages.Navigation.ViewModels
{
  public class NavigationButtonViewModel : ViewModelBase
  {
    public NavigationButtonViewModel()
    {

    }

    public NavigationButtonViewModel(IPage page)
    {
      Page = page;
    }

    private IPage _Page;
    public IPage Page
    {
      get { return _Page; }
      set
      {
        if (value != _Page)
        {
          _Page = value;
          Text = value.NavText;
          NotifyOfPropertyChange(() => Page);
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

    /// <summary>
    /// Publishes a NavigationRequest to the Page.
    /// </summary>
    public virtual void Navigate()
    {
      Navigator.Ton.NavigateTo(Page);
    }
  }
}
