using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(StudySessionCompleteViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudySessionCompleteViewModel : ViewModelBase
  {
    #region Ctors and Init

    public StudySessionCompleteViewModel()
    {
    }

    #endregion

    #region Properties


    #endregion

    #region Methods

   
    #endregion
  }
}
