using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class LanguageTextAlreadyExistsException : Exception
  {
    public LanguageTextAlreadyExistsException(string languageText)
      //: base(string.Format(DalResources.ErrorMsgLanguageTextAlreadyExistsException, languageText))
      : base(GetDefaultErrorMessage(languageText))
    {
      LanguageText = languageText; 
    }

    public LanguageTextAlreadyExistsException(string errorMsg, string languageText)
      : base(errorMsg)
    {
      LanguageText = languageText;
    }

    public LanguageTextAlreadyExistsException(Exception innerException, string languageText)
      //: base(string.Format(DalResources.ErrorMsgLanguageTextAlreadyExistsException, languageText), innerException)
      : base(GetDefaultErrorMessage(languageText), innerException)
    {
      LanguageText = languageText;
    }

    public string LanguageText { get; private set; }

    /// <summary>
    /// This returns the default error message for this exception class.
    /// </summary>
    /// <param name="languageText"></param>
    /// <returns></returns>
    public static string GetDefaultErrorMessage(string languageText)
    {
      return string.Format(DalResources.ErrorMsgLanguageTextAlreadyExistsException, languageText);
    }
  }
}
