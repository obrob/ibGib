using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class LanguageTextNotFoundException : Exception
  {
    public LanguageTextNotFoundException(string languageText)
      //: base(string.Format(DalResources.ErrorMsgLanguageTextNotFoundException, languageText))
      : base(GetDefaultErrorMessage(languageText))
    {
      LanguageText = languageText; 
    }

    public LanguageTextNotFoundException(string errorMsg, string languageText)
      : base(errorMsg)
    {
      LanguageText = languageText;
    }

    public LanguageTextNotFoundException(Exception innerException, string languageText)
      //: base(string.Format(DalResources.ErrorMsgLanguageTextNotFoundException, languageText), innerException)
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
      return string.Format(DalResources.ErrorMsgLanguageTextNotFoundException, languageText);
    }
  }
}
