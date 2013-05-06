using System;
using System.Net;
using Csla;
using Csla.Serialization;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class creates a new translation, loads it with two blank phrases, each with the default language.
  /// </summary>
  [Serializable]
  public class BlankTranslationCreator : Common.CslaBases.ReadOnlyBase<BlankTranslationCreator>
  {
    #region Factory Methods

    public static async Task<BlankTranslationCreator> CreateNewAsync()
    {
      var result = await DataPortal.CreateAsync<BlankTranslationCreator>();
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
    public void DataPortal_Create()
    {
      RetrieverId = Guid.NewGuid();

      Translation = TranslationEdit.NewTranslationEdit();
      
      var phraseA = Translation.Phrases.AddNew();
      phraseA.LanguageId = LanguageEdit.GetDefaultLanguageId();
      //phraseA.Language = LanguageEdit.GetLanguageEdit(phraseA.LanguageId);
      phraseA.Language = DataPortal.FetchChild<LanguageEdit>(phraseA.LanguageId);
      
      var phraseB = Translation.Phrases.AddNew();
      phraseB.LanguageId = LanguageEdit.GetDefaultLanguageId();
      //phraseB.Language = LanguageEdit.GetLanguageEdit(phraseB.LanguageId);
      phraseB.Language = DataPortal.FetchChild<LanguageEdit>(phraseB.LanguageId);
    }
#endif

    #endregion
  }
}
