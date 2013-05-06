using System;
using System.Net;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.Delegates
{
  //public delegate void QuestionAnswerCallback(PhraseEdit question, PhraseEdit answer, Exception exception);
  public delegate void QuestionAnswerCallback(object sender, Args.QuestionAnswerArgs result);
}
