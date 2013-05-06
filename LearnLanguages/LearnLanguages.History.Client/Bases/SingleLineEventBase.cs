using System;
using System.Collections.Generic;
using LearnLanguages.Business;
using Csla.Serialization;

namespace LearnLanguages.History.Bases
{
  [Serializable]
  public abstract class SingleLineEventBase : HistoryEventBase
  {
    //private Guid lineId;
    //private Guid reviewMethodId;
    //private string lineText;
    //private int lineNumber;
    //private double feedbackAsDoubleGiven;
    //private int duration;

    public SingleLineEventBase()
      : base()
    {

    }

    public SingleLineEventBase(Guid lineId)
      : base(TimeSpan.MinValue)
    {
      AddLineId(lineId);
    }

    public SingleLineEventBase(Guid lineId, Guid languageId)
      : base(TimeSpan.MinValue)
    {
      AddLineId(lineId);
      AddLanguageId(languageId);
    }

    public SingleLineEventBase(Guid lineId, TimeSpan duration)
      : base(duration)
    {
      AddLineId(lineId);
    }

    public SingleLineEventBase(Guid lineId, Guid languageId, TimeSpan duration)
      : base(duration)
    {
      AddLineId(lineId);
      AddLanguageId(languageId);
    }

    public SingleLineEventBase(Guid lineId, string lineText, Guid languageId, string languageText, TimeSpan duration)
      : base(duration)
    {
      AddLineId(lineId);
      AddLineText(lineText);
      AddLanguageId(languageId);
      AddLanguageText(languageText);
    }

    public SingleLineEventBase(Guid lineId, string lineText, int lineNumber, Guid phraseId, Guid languageId, string languageText, TimeSpan duration)
      : base(duration)
    {
      AddLineId(lineId);
      AddLineText(lineText);
      AddLanguageId(languageId);
      AddLanguageText(languageText);
      AddLineNumber(lineNumber);
      AddPhraseId(phraseId);
    }

    public SingleLineEventBase(LineEdit line)
      : base(TimeSpan.MinValue)
    {
      AddLineDetails(line);
    }

    public SingleLineEventBase(LineEdit line, TimeSpan duration)
      : base(duration)
    {
      AddLineDetails(line);
    }

    public SingleLineEventBase(LineEdit line, params KeyValuePair<string, object>[] details)
      : base(TimeSpan.MinValue, details)
    {
      AddLineDetails(line);
    }

    public SingleLineEventBase(LineEdit line, TimeSpan duration, params KeyValuePair<string, object>[] details)
      : base(duration, details)
    {
      AddLineDetails(line);
    }

    public SingleLineEventBase(Guid lineId, Guid reviewMethodId, string lineText, int lineNumber, 
      double feedbackAsDoubleGiven, TimeSpan duration)
      : base(duration)
    {
      AddLineId(lineId);
      AddDetail(HistoryResources.Key_ReviewMethodId, reviewMethodId);
      AddLineText(lineText);
      AddLineNumber(lineNumber);
      AddDetail(HistoryResources.Key_FeedbackAsDouble, feedbackAsDoubleGiven);
    }

    protected virtual void AddLineDetails(LineEdit line)
    {
      AddLineId(line.Id);
      AddLineText(line.Phrase.Text);
      AddPhraseId(line.PhraseId);
      AddLanguageId(line.Phrase.LanguageId);
      AddLanguageText(line.Phrase.Language.Text);
      AddLineNumber(line.LineNumber);
    }

    protected virtual void AddLineId(Guid lineId)
    {
      AddDetail(HistoryResources.Key_LineId, lineId);
    }

    private void AddLineNumber(int lineNumber)
    {
      AddDetail(HistoryResources.Key_LineNumber, lineNumber);
    }

    protected virtual void AddLanguageId(Guid languageId)
    {
      AddDetail(HistoryResources.Key_LanguageId, languageId);
    }

    protected virtual void AddLineText(string text)
    {
      AddDetail(HistoryResources.Key_LineText, text);
    }

    protected virtual void AddLanguageText(string text)
    {
      AddDetail(HistoryResources.Key_LanguageText, text);
    }

    private void AddPhraseId(Guid phraseId)
    {
      AddDetail(HistoryResources.Key_PhraseId, phraseId);
    }
  }
}
