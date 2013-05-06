using System;
using System.Net;
using System.Linq;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearnLanguages.DataAccess.Exceptions;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class creates a new translation, loads it with given phrases.  
  /// </summary>
  [Serializable]
  public class TranslationCreator : Common.CslaBases.ReadOnlyBase<TranslationCreator>
  {
    #region Factory Methods

    /// <summary>
    /// Creates a new translation, making child-copies of the phrasesCriteria.Phrases and adding these
    /// children to translation.Phrases.
    /// </summary>
    /// <param name="phrasesCriteria">collection of PhraseEdits, do not have to be marked as children.</param>
    public static async Task<TranslationCreator> CreateNewAsync(
      Criteria.ListOfPhrasesCriteria phrasesCriteria)
    {
      var result = await DataPortal.CreateAsync<TranslationCreator>(phrasesCriteria);
      return result;
    }

    public static async Task<TranslationCreator> CreateNewAsync(
      Criteria.PhraseTextLanguageTextPairsCriteria criteria)
    {
      var result = await DataPortal.CreateAsync<TranslationCreator>(criteria);
      return result;
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return GetProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<TranslationEdit> TranslationProperty = RegisterProperty<TranslationEdit>(c => c.Translation);
    public TranslationEdit Translation
    {
      get { return GetProperty(TranslationProperty); }
      private set { LoadProperty(TranslationProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create(Criteria.ListOfPhrasesCriteria phrasesCriteria)
    {
      //INITIALIZE TRANSLATION CREATOR
      RetrieverId = Guid.NewGuid();
      Translation = TranslationEdit.NewTranslationEdit();

      //WILL USE THIS TO POPULATE THE TRANSLATION
      List<PhraseEdit> phrasesToUse = new List<PhraseEdit>(phrasesCriteria.Phrases);
      
      //FILL TRANSLATION.PHRASES WITH EMPTY PHRASES
      for (int i = 0; i < phrasesToUse.Count; i++)
      {
        Translation.Phrases.AddNew();
      }
      
      //RETRIEVE ANY ALREADY-EXISTING PHRASES IN DB
      var retriever = PhrasesByTextAndLanguageRetriever.CreateNew(phrasesCriteria);
      for (int i = 0; i < phrasesToUse.Count; i++)
      {
        var phraseToAdd = phrasesToUse[i];

        //IF THE RETRIEVEDPHRASES CONTAINS TO THE KEY (WHICH IT SHOULD) 
        //AND THE RETRIEVER FOUND AN ALREADY EXISTING PHRASE IN DB,
        //THEN REPLACE PHRASETOADD WITH THAT DB PHRASE
        if (retriever.RetrievedPhrases.ContainsKey(phraseToAdd.Id) &&
            retriever.RetrievedPhrases[phraseToAdd.Id] != null)
          phraseToAdd = retriever.RetrievedPhrases[phraseToAdd.Id];

        var translationChildPhrase = Translation.Phrases[i];
        var dto = phraseToAdd.CreateDto();
        translationChildPhrase.LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    public void DataPortal_Create(Criteria.PhraseTextLanguageTextPairsCriteria criteria)
    {
      //INITIALIZE TRANSLATION CREATOR
      RetrieverId = Guid.NewGuid();
      Translation = TranslationEdit.NewTranslationEdit();

      //FILL TRANSLATION.PHRASES WITH EMPTY PHRASES
      for (int i = 0; i < criteria.PhraseTextLanguageTextPairs.Count; i++)
      {
        Translation.Phrases.AddNew();
      }

      //FILL IN THE PHRASE DATA
      for (int j = 0; j < criteria.PhraseTextLanguageTextPairs.Count; j++)
      {
        //EXTRACT DATA FROM TUPLE
        var pair = criteria.PhraseTextLanguageTextPairs[j];
        var phraseText = pair.Item1;
        var languageText = pair.Item2;

        //ENTER DATA INTO THE BLANK PHRASES
        var phraseEdit = Translation.Phrases[j];
        phraseEdit.Text = phraseText;

        //DO THE LANGUAGE
        //TRY TO GET LANGUAGE EDIT FROM DB
        var allLanguages = LanguageList.GetAll();
        var results = from languageEdit in allLanguages
                      where languageEdit.Text == languageText
                      select languageEdit;
        if (results.Count() > 0)
        {
          //THE LANGUAGE ALREADY EXISTS (OMG I'M TIRED AND THIS IS TERRIBLE)
          var childLanguageEdit = results.First();
          phraseEdit.Language = childLanguageEdit;
        }
        else
        {
          //THE LANGUAGE DOES NOT EXIST YET
          phraseEdit.Language.Id = Guid.NewGuid();
          phraseEdit.Language.Text = languageText;
        }
      }

      //SAVE THE TRANSLATION (SYNC CUZ WE'RE ON THE SERVER)
      Translation = Translation.Save();
    }

#endif

    #endregion
  }
}
