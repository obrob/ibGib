using System;
using System.Collections.Generic;
using LearnLanguages.Business;
using Csla.Serialization;

namespace LearnLanguages.History.Bases
{
  [Serializable]
  public abstract class SinglePhraseEventBase : HistoryEventBase
  {
    public SinglePhraseEventBase()
      : base()
    {

    }

    public SinglePhraseEventBase(Guid phraseId)
      : base(TimeSpan.MinValue)
    {
      AddPhraseId(phraseId);
    }

    public SinglePhraseEventBase(Guid phraseId, Guid languageId)
      : base(TimeSpan.MinValue)
    {
      AddPhraseId(phraseId);
      AddLanguageId(languageId);
    }

    public SinglePhraseEventBase(Guid phraseId, TimeSpan duration)
      : base(duration)
    {
      AddPhraseId(phraseId);
    }

    public SinglePhraseEventBase(Guid phraseId, Guid languageId, TimeSpan duration)
      : base(duration)
    {
      AddPhraseId(phraseId);
      AddLanguageId(languageId);
    }

    public SinglePhraseEventBase(Guid phraseId, string phraseText, Guid languageId, string languageText, TimeSpan duration)
      : base(duration)
    {
      AddPhraseId(phraseId);
      AddPhraseText(phraseText);
      AddLanguageId(languageId);
      AddLanguageText(languageText);
    }

    public SinglePhraseEventBase(Guid phraseId, string phraseText, Guid languageId, string languageText)
      : base(TimeSpan.MinValue)
    {
      AddPhraseId(phraseId);
      AddPhraseText(phraseText);
      AddLanguageId(languageId);
      AddLanguageText(languageText);
    }

    public SinglePhraseEventBase(PhraseEdit phrase)
      : base(TimeSpan.MinValue) 
    {
      AddPhraseDetails(phrase);
    }

    public SinglePhraseEventBase(PhraseEdit phrase, TimeSpan duration)
      : base(duration)
    {
      AddPhraseDetails(phrase);
    }

    public SinglePhraseEventBase(PhraseEdit phrase, params KeyValuePair<string, object>[] details)
      : base(TimeSpan.MinValue, details)
    {
      AddPhraseDetails(phrase);
    }

    public SinglePhraseEventBase(PhraseEdit phrase, TimeSpan duration, params KeyValuePair<string, object>[] details)
      : base(duration, details)
    {
      AddPhraseDetails(phrase);
    }

    protected virtual void AddPhraseDetails(PhraseEdit phrase)
    {
      AddPhraseId(phrase.Id);
      AddPhraseText(phrase.Text);
      AddLanguageId(phrase.LanguageId);
      AddLanguageText(phrase.Language.Text);
    }

    protected virtual void AddPhraseId(Guid phraseId)
    {
      AddDetail(HistoryResources.Key_PhraseId, phraseId);
    }

    protected virtual void AddLanguageId(Guid languageId)
    {
      AddDetail(HistoryResources.Key_LanguageId, languageId);
    }

    protected virtual void AddPhraseText(string text)
    {
      AddDetail(HistoryResources.Key_PhraseText, text);
    }

    protected virtual void AddLanguageText(string text)
    {
      AddDetail(HistoryResources.Key_LanguageText, text);
    }
  }
}
