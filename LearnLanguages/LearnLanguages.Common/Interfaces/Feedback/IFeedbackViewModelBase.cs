using System;
using Csla.Serialization;

namespace LearnLanguages.Common.Interfaces
{
  public interface IFeedbackViewModelBase : IViewModelBase, IGetFeedback, IHaveFeedback
  {
    //bool IsEnabled { get; set; }

    //EXPOSED IN IGETFEEDBACK, WRITTEN HERE FOR CONVENIENCE
    //IFeedback GetFeedback(int timeoutMilliseconds);
    //Task<IFeedback> GetFeedbackAsync(int timeoutMilliseconds);
  }
}
