using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Interfaces;
using Caliburn.Micro;
using System;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(StudyASongItemViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyASongItemViewModel : ViewModelBase<MultiLineTextEdit, MultiLineTextDto>
  {
    public StudyASongItemViewModel()
    {

    }

    private bool _IsChecked;
    public bool IsChecked
    {
      get { return _IsChecked; }
      set
      {
        if (value != _IsChecked)
        {
          _IsChecked = value;
          NotifyOfPropertyChange(() => IsChecked);
        }
      }
    }
  }
}
