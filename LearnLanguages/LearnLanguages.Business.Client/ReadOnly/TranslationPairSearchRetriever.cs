using System;
using System.Linq;
using System.Collections.Generic;

using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class searches for a translation containing a pair of phrases.  If it finds the translation 
  /// that contains exactly these two phrases, then it returns with the translation.  
  /// If it finds no translation, then it returns null as translation.
  /// If it finds more than one translation, then it checks for context, breaking the tie that way,
  /// and assigns the translation.  If no context is found, it returns the first translation it finds.
  /// 
  /// This does NOT search through all translations looking for containment of the phrases.  So, if you 
  /// search for "the", it will look for the translation that contains the phrase.Text=="the".  It will
  /// not return a translation such that it has a Phrase.Text="the quick brown fox".
  /// 
  /// Right now, this is very slow.  It downloads a list of all phrases (on the server side), and searches 
  /// through them manually.  It also does some other non-optimal things.
  /// </summary>
  [Serializable]
  public class TranslationPairSearchRetriever : Common.CslaBases.ReadOnlyBase<TranslationPairSearchRetriever>
  {
    #region Factory Methods

    /// <summary>
    /// Creates a new translation, making child-copies of the phrasesCriteria.Phrases and adding these
    /// children to translation.Phrases.
    /// </summary>
    /// <param name="phrasesCriteria">collection of PhraseEdits, do not have to be marked as children.</param>
    public static async Task<TranslationPairSearchRetriever> CreateNewAsync(
      Criteria.TranslationPairSearchCriteria criteria)
    {
      if (criteria == null)
        throw new ArgumentNullException("criteria");
      if (criteria.PhraseA == null)
        throw new ArgumentException("criteria.PhraseA == null");
      if (criteria.PhraseB == null)
        throw new ArgumentException("criteria.PhraseB == null");

      var result = await DataPortal.CreateAsync<TranslationPairSearchRetriever>(criteria);
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
    public void DataPortal_Create(Criteria.TranslationPairSearchCriteria criteria)
    {
      RetrieverId = Guid.NewGuid();
      Translation = null;
      //Translation = TranslationEdit.NewTranslationEdit();

      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        Result<ICollection<PhraseDto>> result = phraseDal.GetAll();
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }

        var allPhraseDtos = result.Obj;


        //WE HAVE A LIST OF ALL THE PHRASES.  NOW SEARCH THROUGH FOR OUR PHRASES.

        //PHRASE A
        var foundPhraseADto = (from phraseDto in allPhraseDtos
                               where phraseDto.Text == criteria.PhraseA.Text &&
                                     phraseDto.LanguageId == criteria.PhraseA.LanguageId
                               select phraseDto).FirstOrDefault();

        if (foundPhraseADto == null)
        {
          //PHRASE A WAS NOT FOUND IN DB, SO THERE IS NO TRANSLATION.
          return;
        }

        //PHRASE B
        var foundPhraseBDto = (from phraseDto in allPhraseDtos
                               where phraseDto.Text == criteria.PhraseB.Text &&
                                     phraseDto.LanguageId == criteria.PhraseB.LanguageId
                               select phraseDto).FirstOrDefault();

        if (foundPhraseBDto == null)
        {
          //PHRASE A WAS NOT FOUND IN DB, SO THERE IS NO TRANSLATION.
          return;
        }


        //WE FOUND BOTH PHRASES, NOW WE LOOK FOR TRANSLATION WITH PHRASE A.  WE WILL THEN
        //LOOK WITHIN THOSE RESULTS FOR PHRASE B.  WE WILL THEN NARROW ANY POSSIBLE MULTIPLE 
        //TRANSLATIONS WITH CONTEXT TEXT.


        //LOOK FOR TRANSLATION WITH PHRASE A
        var phraseAEdit = DataPortal.Fetch<PhraseEdit>(foundPhraseADto.Id);
        var translationsContainingPhraseA = TranslationList.GetAllTranslationsContainingPhraseById(phraseAEdit);

        //IF WE FIND NO TRANSLATIONS WITH PHRASE A, THEN WE RETURN WITH NO TRANSLATION FOUND
        if (translationsContainingPhraseA.Count == 0)
          return;

        //WE FOUND TRANSLATIONS WITH PHRASE A
        //NOW LOOK WITHIN RESULTS FOR PHRASE B
        var translationsWithBothPhrases = (from translation in translationsContainingPhraseA
                                            where (from phrase in translation.Phrases
                                                   where phrase.Id == foundPhraseBDto.Id
                                                   select phrase).Count() > 0
                                            select translation);

        //THERE ARE NO TRANSLATIONS WITH BOTH PHRASES, SO RETURN WITH TRANSLATION == NULL
        if (translationsWithBothPhrases.Count() == 0)
          return;


        //WE FOUND TRANSLATIONS CONTAINING BOTH PHRASES, 
        //AND WE MUST NOW CHECK AGAINST CONTEXT (IF PROVIDED)
        if (!string.IsNullOrEmpty(criteria.ContextText))
        {
          //CONTEXT TEXT HAS BEEN PROVIDED
          //WE CHOOSE [SQL].FIRSTORDEFAULT() BECAUSE IF WE CANNOT MATCH THE CONTEXT, THEN OUR SEARCH 
          //SHOULD RETURN NULL
          Translation = (from t in translationsWithBothPhrases
                         where t.ContextPhrase != null &&
                               t.ContextPhrase.Text == criteria.ContextText
                         select t).FirstOrDefault();
        }
        else
        {
          //CONTEXT TEXT HAS NOT BEEN PROVIDED
          //WE KNOW WE HAVE FOUND AT LEAST ONE, AND THIS SETS TRANSLATION TO THE FIRST TRANSLATION FOUND.  
          Translation = translationsWithBothPhrases.First();
        }
      }
    }

#endif

    #endregion
  }
}
