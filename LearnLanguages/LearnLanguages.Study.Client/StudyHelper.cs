using LearnLanguages.Common;
using System;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  public static class StudyHelper
  {
    public static async Task<ResultArgs<StudyItemViewModelArgs>> GetAbortedAsync()
    {
      var abortTask = new Task<ResultArgs<StudyItemViewModelArgs>>(
          () =>
          {
            var abortResult = 
              new ResultArgs<StudyItemViewModelArgs>(StudyItemViewModelArgs.Aborted);
            return abortResult;
          });
      return await abortTask;
    }

    /// <summary>
    /// I DONT THINK THIS WORKS
    /// This wraps an object inside of a . It then creates
    /// a task whose function will be to return this ResultArgs wrapper.
    /// This function is async and you can just await this function. It is just a simpler
    /// way to return an object inside of a task.
    /// </summary>
    /// <typeparam name="T">Type of result object</typeparam>
    /// <param name="objectToWrap">result object to be wrapped</param>
    /// <returns></returns>
    public static async Task<T> WrapInTask<T>(T objectToWrap) 
    {
      var retTask = new Task<T>(() => { return objectToWrap; });
      retTask.Start();
      return await retTask;
    }
  }
}
