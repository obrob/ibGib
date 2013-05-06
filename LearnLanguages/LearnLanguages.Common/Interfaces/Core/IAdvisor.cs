using System;
using LearnLanguages.Common.Delegates;
using System.Threading.Tasks;

namespace LearnLanguages.Common.Interfaces
{
  /// <summary>
  /// Can this advisor answer this type of question.  if Yes, return answer async.
  /// </summary>
  public interface IAdvisor
  {
    Guid Id { get; }
    /// <summary>
    /// Can this advisor answer this type of question.  if Yes, return answer async.
    /// </summary>
    Task<ResultArgs<object>> AskAdviceAsync(QuestionArgs questionArgs);
  }
}
