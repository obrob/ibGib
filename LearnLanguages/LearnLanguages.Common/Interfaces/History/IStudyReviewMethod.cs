using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IStudyReviewMethod
  {
    /// <summary>
    /// Id of this HistoryEvent being published
    /// </summary>
    Guid ReviewMethodId { get; }
  }
}
