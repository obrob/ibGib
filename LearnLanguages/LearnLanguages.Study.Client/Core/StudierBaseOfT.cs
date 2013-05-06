using System;
using System.Net;
using LearnLanguages.Common.Delegates;
using System.Threading.Tasks;
using LearnLanguages.Common;

namespace LearnLanguages.Study
{
  public abstract class StudierBase<T>
  {
    //public abstract void SetTarget(T target);
    protected T _Target { get; set; }
    public abstract Task InitializeForNewStudySessionAsync(T target);
    public abstract Task<ResultArgs<StudyItemViewModelArgs>> GetNextStudyItemViewModelAsync();
  }
}
