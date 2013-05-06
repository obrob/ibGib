using System;

namespace LearnLanguages.Common
{
  public class QuestionArgs
  {
    public QuestionArgs(string question, object state)
    {
      Question = question;
      State = state;
    }

    public string Question { get; private set; }
    public object State { get; private set; }
  }
}
