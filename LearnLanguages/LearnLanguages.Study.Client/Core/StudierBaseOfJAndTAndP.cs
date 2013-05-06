using System;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  public abstract class StudierBase<J, T, P> : IDo<J, T, P>
    where J : IJobInfo<T, P>
  {
    public StudierBase()
    {
      IsNotFirstRun = false;
      Id = Guid.NewGuid();
    }

    public Guid Id { get; protected set; }
   
    protected J _StudyJobInfo { get; set; }
    /// <summary>
    /// Todo: change IsNotFirstRun bad name, need to refactor to IsFirstRun and change logic.
    /// </summary>
    public bool IsNotFirstRun { get; protected set; }
    protected object _AbortLock = new object();
    protected bool _abortIsFlagged = false;
    protected bool _AbortIsFlagged
    {
      get
      {
        lock (_AbortLock)
        {
          return _abortIsFlagged;
        }
      }
      set
      {
        lock (_AbortLock)
        {
          _abortIsFlagged = value;
        }
      }
    }

    public void Do(J studyJobInfo)
    {
      if (studyJobInfo == null)
        throw new ArgumentNullException("target");

      _StudyJobInfo = studyJobInfo;

      DoImpl();
      IsNotFirstRun = true;
    }
    /// <summary>
    /// Should take into account if HasDoneThisBefore.
    /// </summary>
    protected abstract void DoImpl();
    public virtual void PleaseStopDoing(Guid jobInfoId)
    {
      if (_StudyJobInfo.Id == jobInfoId)
        _AbortIsFlagged = true;
    }
  }
}
