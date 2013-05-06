using System;
using Csla.Serialization;

namespace LearnLanguages.History
{
  [Serializable]
  public class HistoryException : Exception
  {
    public HistoryException()
      : base()
    {

    }

    public HistoryException(string errorMsg)
      : base(errorMsg)
    {

    }

    public HistoryException(string errorMsg, Exception innerException)
      : base(errorMsg, innerException)
    {

    }
  }
}
