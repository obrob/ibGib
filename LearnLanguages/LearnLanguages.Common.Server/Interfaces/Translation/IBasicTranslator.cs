using System;
using System.Threading.Tasks;

namespace LearnLanguages.Common.Interfaces.Translation
{
  public interface IBasicTranslator
  {
    string AutoTranslateSync(string untranslatedPhraseText,
                                    string untranslatedPhraseLanguage,
                                    string targetLanguageText);

    Task<string> AutoTranslateAsync(string untranslatedPhraseText,
                                    string untranslatedPhraseLanguage,
                                    string targetLanguageText);
  }
}
