using System;
using System.Threading.Tasks;

namespace LearnLanguages.Common.Interfaces.Translation
{
  public interface IBasicTranslator
  {
    Task<string> AutoTranslate(string untranslatedPhraseText,
                               string untranslatedPhraseLanguage,
                               string targetLanguageText);
  }
}
