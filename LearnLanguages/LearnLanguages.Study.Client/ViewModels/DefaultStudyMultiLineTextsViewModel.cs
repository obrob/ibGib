using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModels;
using LearnLanguages.DataAccess;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(DefaultStudyMultiLineTextsViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class DefaultStudyMultiLineTextsViewModel : 
    ViewModelBase<MultiLineTextList, MultiLineTextEdit, MultiLineTextDto>
  {
    #region Ctors and Init

    public DefaultStudyMultiLineTextsViewModel()
    {

    }

    #endregion

    #region Properties

    private string _StudyTitle = StudyResources.DefaultStudyTitle;
    public string StudyTitle
    {
      get { return _StudyTitle; }
      set
      {
        if (value != _StudyTitle)
        {
          _StudyTitle = value;
          NotifyOfPropertyChange(() => StudyTitle);
        }
      }
    }

    private IViewModelBase _StudyItemViewModel;
    public IViewModelBase StudyItemViewModel
    {
      get { return _StudyItemViewModel; }
      set
      {
        if (value != _StudyItemViewModel)
        {
          _StudyItemViewModel = value;
          NotifyOfPropertyChange(() => StudyItemViewModel);
        }
      }
    }

    #endregion

    #region Commands

    public bool CanGo
    {
      get
      {
        
        return true;
      }
    }
    public void Go()
    {

    }

    #endregion
  }
}
