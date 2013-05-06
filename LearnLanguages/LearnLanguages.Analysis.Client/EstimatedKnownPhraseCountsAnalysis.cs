using LearnLanguages.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LearnLanguages.Analysis
{
  public class EstimatedKnownPhraseCountsAnalysis : IAnalysis
  {
    public EstimatedKnownPhraseCountsAnalysis(Guid id,
      ObservableCollection<KeyValuePair<string, int>> data)
    {
      Id = id;
      Data = data;
    }

    public EstimatedKnownPhraseCountsAnalysis(
      ObservableCollection<KeyValuePair<string, int>> data)
    {
      Id = Guid.NewGuid();
      Data = data;
    }

    public Guid Id { get; private set; }

    /// <summary>
    /// key (string) = Language Text
    /// value (int)  = Estimated count of known phrases
    /// </summary>
    public ObservableCollection<KeyValuePair<string, int>> Data { get; private set; }
  }
}
