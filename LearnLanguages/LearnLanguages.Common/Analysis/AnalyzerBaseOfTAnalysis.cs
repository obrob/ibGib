using LearnLanguages.Common.Exceptions;
using LearnLanguages.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace LearnLanguages.Common.Analysis
{
  /// <summary>
  /// You can utilize this base class if your analyzer returns
  /// just one type of analysis. It can still return multiple 
  /// variations using that type, using the analysisIdString, 
  /// but it can only be of one type. 
  /// The main function of this base class is just to do some 
  /// error handling for the GetAnalysis{T}. If it isn't the 
  /// understood type, then it throws an exception that says
  /// this. It also checks for AnalysisHasBeenBuilt before
  /// running RefineAnalysisImpl. If not, throws an exception.
  /// </summary>
  /// <typeparam name="TAnalysis">The IAnalysis Type that this Analyzer knows how to produce.</typeparam>
  public abstract class AnalyzerBase<TAnalysis> : IAnalyzer
    where TAnalysis : IAnalysis
  {
    public Guid Id { get; protected set; }

    public bool AnalysisHasBeenBuilt { get; protected set; }

    public virtual async Task<T> GetAnalysis<T>(string analysisIdString = "",
                                                bool recordInDb = false)
      where T : IAnalysis
    {
      if (typeof(T) != typeof(TAnalysis))
        throw new Exceptions.AnalysisTypeNotRecognizedException(typeof(T));

      IAnalysis retAnalysis = null;
      retAnalysis = await GetAnalysisImpl(analysisIdString, recordInDb);
      AnalysisHasBeenBuilt = true;
      return (T)retAnalysis;
    }

    protected abstract Task<TAnalysis> GetAnalysisImpl(string analysisIdString = "",
                                                       bool recordInDb = false);

    public virtual async Task RefineAnalysis(int timeAllottedInMs)
    {
      if (!AnalysisHasBeenBuilt)
        return;
        //throw new RefineAttemptedBeforeAnalysisException();

      await RefineAnalysisImpl(timeAllottedInMs);
    }

    protected abstract Task RefineAnalysisImpl(int timeAllottedInMs);

  }
}
