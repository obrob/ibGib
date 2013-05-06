using System;
using LearnLanguages.Common.Delegates;
using System.Threading.Tasks;

namespace LearnLanguages.Common.Interfaces
{
  /// <summary>
  /// Can this advisor answer this type of question.  if Yes, return answer async.
  /// </summary>
  public interface IAnalyzer : IHaveId
  {
    /// <summary>
    /// Flag to indicate if the analysis has been built. We should
    /// only be able to refine an analysis if it has been built.
    /// </summary>
    bool AnalysisHasBeenBuilt { get; }

    /// <summary>
    /// Gets the analysis with the given id string (name, id, whatever) 
    /// for this analyzer.
    /// </summary>
    /// <typeparam name="T">IAnalysis class</typeparam>
    /// <param name="recordInDb">true if you want to persist this analysis in the Db</param>
    /// <returns>IAnalysis descendant</returns>
    Task<T> GetAnalysis<T>(string analysisIdString = "", bool recordInDb = false)
      where T : IAnalysis;

    /// <summary>
    /// This is the method that should let the analyzer chug.
    /// It should run in the background.
    /// </summary>
    Task RefineAnalysis(int timeAllottedInMs);

  }
}
