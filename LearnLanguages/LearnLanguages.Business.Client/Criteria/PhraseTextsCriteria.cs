
using System;
using System.Net;
using Csla.Serialization;
using Csla;
using System.Collections.Generic;
using Csla.Core;

namespace LearnLanguages.Business.Criteria
{
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public class PhraseTextsCriteria : CriteriaBase<PhraseTextsCriteria>
  {
    public PhraseTextsCriteria()
    {
      //required for serialization
    }
    public PhraseTextsCriteria(string languageText, params string[] phraseTexts)
    {
      LanguageText = languageText;
      PhraseTexts = new MobileList<string>(phraseTexts);
    }

    public PhraseTextsCriteria(string languageText, IList<string> phraseTexts)
    {
      LanguageText = languageText;
      PhraseTexts = new MobileList<string>(phraseTexts);
    }

    public static readonly PropertyInfo<IList<string>> PhraseTextsProperty = 
      RegisterProperty<IList<string>>(c => c.PhraseTexts);
    public IList<string> PhraseTexts
    {
      get { return ReadProperty(PhraseTextsProperty); }
      private set { LoadProperty(PhraseTextsProperty, value); }
    }

    public static readonly PropertyInfo<string> LanguageTextProperty = RegisterProperty<string>(c => c.LanguageText);
    public string LanguageText
    {
      get { return ReadProperty(LanguageTextProperty); }
      private set { LoadProperty(LanguageTextProperty, value); }
    }
  }
}
