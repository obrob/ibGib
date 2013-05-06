using System;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using System.Windows;

namespace LearnLanguages.Common.ViewModelBases
{
  public abstract class PageViewModelBase : ViewModelBase, 
                                            IPageViewModel
  {
    public PageViewModelBase()
    {
      InitializePageViewModelPropertiesImpl();
    }

    /// <summary>
    /// Method holder to be implemented to set Title, Description,
    /// Instructions, and Page.
    /// </summary>
    protected abstract void InitializePageViewModelPropertiesImpl();

    private string _Title;
    public string Title
    {
      get { return _Title; }
      set
      {
        if (value != _Title)
        {
          _Title = value;
          NotifyOfPropertyChange(() => Title);
        }
      }
    }

    private string _Description;
    public string Description
    {
      get { return _Description; }
      set
      {
        if (value != _Description)
        {
          _Description = value;
          NotifyOfPropertyChange(() => Description);
        }
      }
    }

    private string _Instructions;
    public string Instructions
    {
      get { return _Instructions; }
      set
      {
        if (value != _Instructions)
        {
          _Instructions = value;
          NotifyOfPropertyChange(() => Instructions);
        }
      }
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
          NotifyOfPropertyChange(() => Page);
        }
      }
    }
  }
}
