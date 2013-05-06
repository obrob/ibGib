using System;
using System.Linq;
using System.Net;
using Csla;
using Csla.Serialization;
using Csla.Core;
using LearnLanguages.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class creates a new translation, loads it with given phrases.  
  /// </summary>
  [Serializable]
  public class PhrasesExistRetriever : Common.CslaBases.ReadOnlyBase<PhrasesExistRetriever>
  {
    #region Factory Methods

    /// <summary>
    /// Creates a new existence object, filling ExistenceDictionary with key=phraseText, value=Exists entries.
    /// THIS IS NOT OPTIMIZED, AND IT DOES MOST OF THE WORK ON THE SERVER SIDE.  I MIGHT SHOULD TURN THIS INTO A COMMAND OBJECT
    /// AND HAVE THIS DONE ON THE CLIENT SIDE, ESPECIALLY SINCE CURRENTLY IT IS JUST USING PHRASELIST.GETALL() AND THEN 
    /// WORKING OFF OF THAT ANYWAY.
    /// </summary>
    /// <param name="criteria">phrase texts to be checked for existence in data store.</param>
    public static async Task<PhrasesExistRetriever> CreateNewAsync(Criteria.PhraseTextsCriteria criteria)
    {
      var result = await DataPortal.CreateAsync<PhrasesExistRetriever>(criteria);
      return result;
    }

#if !SILVERLIGHT
    /// <summary>
    /// Creates a new existence object, filling ExistenceDictionary with key=phraseText, value=Exists entries.
    /// THIS IS NOT OPTIMIZED, AND IT DOES MOST OF THE WORK ON THE SERVER SIDE.  I MIGHT SHOULD TURN THIS INTO A COMMAND OBJECT
    /// AND HAVE THIS DONE ON THE CLIENT SIDE, ESPECIALLY SINCE CURRENTLY IT IS JUST USING PHRASELIST.GETALL() AND THEN 
    /// WORKING OFF OF THAT ANYWAY.
    /// </summary>
    /// <param name="criteria">phrase texts to be checked for existence in data store.</param>
    public static PhrasesExistRetriever CreateNew(Criteria.PhraseTextsCriteria criteria)
    {
      return DataPortal.Create<PhrasesExistRetriever>(criteria);
    }
#endif

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return GetProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<MobileDictionary<string, bool>> ExistenceDictionaryProperty = 
      RegisterProperty<MobileDictionary<string, bool>>(c => c.ExistenceDictionary);
    public MobileDictionary<string, bool> ExistenceDictionary
    {
      get { return ReadProperty(ExistenceDictionaryProperty); }
      private set { LoadProperty(ExistenceDictionaryProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create(Criteria.PhraseTextsCriteria criteria)
    {
      RetrieverId = Guid.NewGuid();
      if (ExistenceDictionary == null)
        ExistenceDictionary = new MobileDictionary<string, bool>();

      PhraseList allPhrases = PhraseList.GetAll();
      var allPhrasesInLanguage = (from phrase in allPhrases
                                  where phrase.Language.Text == criteria.LanguageText
                                  select phrase);

      var phraseTextsNoDuplicates = criteria.PhraseTexts.Distinct();

      foreach (var phraseText in phraseTextsNoDuplicates)
      {
        var count = (from phrase in allPhrasesInLanguage
                     where phrase.Text == phraseText
                     select phrase).Count();
        var exists = count > 0;
        ExistenceDictionary.Add(phraseText, exists);
      }
    }
#endif

    #endregion
  }
}
