using System;
using System.Linq;
using LearnLanguages.Business.Security;
using System.Collections.Generic;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Business
{
  public static class BusinessHelper
  {
    public static string GetCurrentUsername()
    {
      var currentUsername = ((UserIdentity)(Csla.ApplicationContext.User.Identity)).Name;
      return currentUsername;
    }

    public static Guid GetCurrentUserId()
    {
      var currentUserId = ((UserIdentity)(Csla.ApplicationContext.User.Identity)).UserId;
      return currentUserId;
    }

    /// <summary>
    /// Extracts the phrase (if found) in the given language.
    /// </summary>
    /// <param name="translation"></param>
    /// <param name="languageText"></param>
    /// <returns>If found, returns phrase in given language. Otherwise, returns null.</returns>
    public static PhraseEdit ExtractPhrase(TranslationEdit translation, 
                                           string languageText)
    {
      var results = (from phrase in translation.Phrases
                     where phrase.Language.Text == languageText
                     select phrase).FirstOrDefault();
      return results;
    }

    /// <summary>
    /// This takes a collection of MLT Dtos and puts their titles and ids into 
    /// a delimited string value to be stored in a single string. I'm doing this
    /// because the 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static string EncodeTitlesAndIdsAsString(ICollection<MultiLineTextDto> collection)
    {
      throw new NotImplementedException();
    }
  }
}
