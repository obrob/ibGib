using System;
using System.Net;
using LearnLanguages.Study.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Study
{
  [Serializable]
  public class StudyItemViewModelArgs
  {
    /// <summary>
    /// This is for serialization.  Do not use this ctor.
    /// </summary>
    public StudyItemViewModelArgs()
    {
    }

    public StudyItemViewModelArgs(IStudyItemViewModelBase viewModel)
    {
      ViewModel = viewModel;
    }

    /// <summary>
    /// This ctor always sets aborted to true and VM to null.
    /// </summary>
    private StudyItemViewModelArgs(bool abortedDummyParam)
    {
      ViewModel = null;
      IsAborted = true;
    }

    public IStudyItemViewModelBase ViewModel { get; private set; }

    public static StudyItemViewModelArgs Aborted
    {
      get
      {
        return new StudyItemViewModelArgs(true);
      }
    }

    public bool IsAborted { get; private set; }
  }
}
