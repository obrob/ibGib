using System;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.Args
{
  public class QuestionAnswerArgs
  {
    public QuestionAnswerArgs() { }
    public QuestionAnswerArgs(PhraseEdit question, PhraseEdit answer)
    {
      Question = question;
      Answer = answer;
      Error = null;
    }
    public QuestionAnswerArgs(Exception error)
    {
      Question = null;
      Answer = null;
      Error = error;
    }

    public PhraseEdit Question { get; set; }
    public PhraseEdit Answer { get; set; }
    public Exception Error { get; set; }
  }
}
