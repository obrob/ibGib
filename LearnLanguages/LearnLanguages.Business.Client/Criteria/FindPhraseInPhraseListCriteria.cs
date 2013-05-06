
using System;
using System.Net;
using Csla.Serialization;
using Csla;
using System.Collections.Generic;
using Csla.Core;

namespace LearnLanguages.Business.Criteria
{
  /// <summary>
  /// Use this when searching for a phrase text/language text in a given PhraseList list.
  /// </summary>
  [Serializable]
  public class FindPhraseInPhraseListCriteria : CriteriaBase<FindPhraseInPhraseListCriteria>
  {
    /// <summary>
    /// DO NOT USE.  FOR SERIALIZATION ONLY.
    /// </summary>
    public FindPhraseInPhraseListCriteria()
    {
      //ELIDED
    }

    /// <summary>
    /// This criteria is used when finding a phrase in a list of phrases.  
    /// 
    /// When you get this phrase from the PhraseList, however, that phrase will be a child phrase (of the PhraseList). 
    /// I am writing this criteria to help optimize DB calls, so the param 'getPhraseFromDB' refers to retrieving 
    /// the non-child version of the phrase (if found) from the DB.
    /// 
    /// If this were not included, it would take an additional round-trip data call to do this on the calling end. 
    /// The retriever is already working on the server side, so this will be faster.
    /// </summary>
    /// <param name="phraseText"></param>
    /// <param name="languageText"></param>
    /// <param name="phrases"></param>
    /// <param name="getPhraseFromDB"></param>
    public FindPhraseInPhraseListCriteria(string phraseText, string languageText, PhraseList phrases, bool getPhraseFromDB=true)
    {
      PhraseText = phraseText;
      LanguageText = languageText;
      Phrases = phrases;
      GetPhraseFromDB = getPhraseFromDB;
    }
        
    public static readonly PropertyInfo<PhraseList> PhrasesProperty = 
      RegisterProperty<PhraseList>(c => c.Phrases);
    public PhraseList Phrases
    {
      get { return ReadProperty(PhrasesProperty); }
      private set { LoadProperty(PhrasesProperty, value); }
    }

    public static readonly PropertyInfo<string> PhraseTextProperty =
      RegisterProperty<string>(c => c.PhraseText);
    public string PhraseText
    {
      get { return ReadProperty(PhraseTextProperty); }
      private set { LoadProperty(PhraseTextProperty, value); }
    }

    public static readonly PropertyInfo<string> LanguageTextProperty =
      RegisterProperty<string>(c => c.LanguageText);
    public string LanguageText
    {
      get { return ReadProperty(LanguageTextProperty); }
      private set { LoadProperty(LanguageTextProperty, value); }
    }

    public static readonly PropertyInfo<bool> GetPhraseFromDBProperty =
      RegisterProperty<bool>(c => c.GetPhraseFromDB);
    public bool GetPhraseFromDB
    {
      get { return ReadProperty(GetPhraseFromDBProperty); }
      private set { LoadProperty(GetPhraseFromDBProperty, value); }
    }
  }
}
