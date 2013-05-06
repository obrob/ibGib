using System;
using System.Threading.Tasks;

namespace LearnLanguages.Common.Interfaces
{
  public interface IGetFeedback
  {
    IFeedback GetFeedback(int timeoutMilliseconds);
    Task<IFeedback> GetFeedbackAsync(int timeoutMilliseconds);
  }
}
