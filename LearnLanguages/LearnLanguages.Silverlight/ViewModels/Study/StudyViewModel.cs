using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Interfaces;
using Caliburn.Micro;
using System;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(StudyViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class StudyViewModel : ViewModelBase
  {
    public StudyViewModel()
    {
    }

    private IViewModelBase _StudyScreen;
    public IViewModelBase StudyScreen
    {
      get { return _StudyScreen; }
      set
      {
        if (value != _StudyScreen)
        {
          _StudyScreen = value;
          NotifyOfPropertyChange(() => StudyScreen);
        }
      }
    }
  }
}
