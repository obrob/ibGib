using System;
using Csla.Serialization;

namespace LearnLanguages.Common.Feedback
{
  [Serializable]
  public class Feedback<T> : Interfaces.IFeedback
  {
    public T Value { get; set; }
  }
}
