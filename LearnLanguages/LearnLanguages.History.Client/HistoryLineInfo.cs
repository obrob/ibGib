using System;
using Csla.Serialization;

namespace LearnLanguages.History
{
  [Serializable]
  public class HistoryLineInfo
  {
    public Guid LineId { get; set; }
    public Guid LanguageId { get; set; }
    public string LineText { get; set; }
    public string LanguageText { get; set; }
    public int LineNumber { get; set; }
  }
}
