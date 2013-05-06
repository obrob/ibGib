using LearnLanguages.Common.Interfaces.Translation;
using System;
using System.Threading.Tasks;

using Microsoft;
using System.Net;
using System.Linq;
using System.ComponentModel.Composition;

namespace LearnLanguages.Common.Translation.Azure
{
  /// <summary>
  /// This basic translator can auto-translate using the MS Azure Translator API service.
  /// The sync version of the AutoTranslate BLOCKS while it is talking to the MS service.
  /// </summary>
  [Export(typeof(IBasicTranslator))]
  public class AzureBasicTranslator : IBasicTranslator
  {
    public string AutoTranslateSync(string untranslatedPhraseText, string untranslatedPhraseLanguage, string targetLanguageText)
    {
      //PREPARE THE TRANSLATOR CONTAINER (TRANSLATION SERVICE CLIENT)
      var serviceRootUri = new Uri(CommonResources.AzureServiceRootUriAddress);
//#if DEBUG
      var accountKey = CommonResources.LearnLanguagesAccountKey;
//#else
//      string accountKey = (string)System.Windows.Application.Current.Resources[CommonResources.AzureLearnLanguagesAccountKey];
//#endif
      TranslatorContainer translatorContainer = new TranslatorContainer(serviceRootUri);
      translatorContainer.Credentials = new NetworkCredential(accountKey, accountKey);

      //TURN THE LANGUAGE TEXTS INTO LANGUAGE CODES THAT SERVICE UNDERSTANDS
      var fromLanguageCode = TranslationHelper.GetLanguageCode(untranslatedPhraseLanguage);
      var toLanguageCode = TranslationHelper.GetLanguageCode(targetLanguageText);

      //BUILDS THE TRANSLATION QUERY
      var translationQuery = translatorContainer.Translate(untranslatedPhraseText,
                                                           toLanguageCode,
                                                           fromLanguageCode);

      //ACTUALLY EXECUTE THE TRANSLATION QUERY
      var translations = translationQuery.Execute().ToList();

      //INTERPRET THE TRANSLATION RESULTS
      if (translations.Count > 0)
      {
        var translationText = translations.First().Text;
        //RETURN TRANSLATION (FIRST TRANSLATION, IF MULTIPLE EXIST)
        return translationText;
      }
      else
      {
        //RETURN EMPTY STRING
        return string.Empty;
      }
    }

    public async Task<string> AutoTranslateAsync(string untranslatedPhraseText, 
                                                 string untranslatedPhraseLanguage, 
                                                 string targetLanguageText)
    {
      var translateTask = new Task<string>(() => 
        { 
          var translationText = AutoTranslateSync(untranslatedPhraseText, 
                                                  untranslatedPhraseLanguage,
                                                  targetLanguageText);
          return translationText;
        });

      translateTask.Start();
      return await translateTask;
    }
  }
}
