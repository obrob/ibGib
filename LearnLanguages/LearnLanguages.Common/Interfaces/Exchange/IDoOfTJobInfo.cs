using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IDo<TJobInfo, TTarget, TProduct>
    where TJobInfo : IJobInfo<TTarget, TProduct>
  {
    /// <summary>
    /// Id unique to this studier's configuration.  For now, this is just a random Guid.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Performs a study iteration using the job info and the offerExchange given.
    /// </summary>
    void Do(TJobInfo studyJobInfo);

    /// <summary>
    /// Id of the job you wish to cancel.  "Please" because this is an async operation and it may not be successful.
    /// </summary>
    void PleaseStopDoing(Guid jobInfoId);
  }
}
