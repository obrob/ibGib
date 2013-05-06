using LearnLanguages.Common.Interfaces.Translation;
using System;
using System.Threading.Tasks;

namespace LearnLanguages.Common.Translation.Bing
{
  //public class BingBasicTranslator : IBasicTranslator
  //{
  //  public Task<string> AutoTranslate(string untranslatedPhraseText,
  //                                    string untranslatedPhraseLanguage, 
  //                                    string targetLanguageText)
  //  {
  //    throw new NotImplementedException();
  //    //BingTranslatorService.LanguageServiceClient client = new BingTranslatorService.LanguageServiceClient();

  //    //client.TranslateCompleted += (s, r) =>
  //    //{
  //    //  if (r.Error != null)
  //    //  {
  //    //    callback(this, new ResultArgs<PhraseEdit>(r.Error));
  //    //    return;
  //    //  }

  //    //  var translatedText = r.Result;
  //    //  PhraseEdit.NewPhraseEdit(targetLanguageText, (s2, r2) =>
  //    //  {
  //    //    if (r2.Error != null)
  //    //    {
  //    //      callback(this, new ResultArgs<PhraseEdit>(r.Error));
  //    //      return;
  //    //    }

  //    //    var translatedPhrase = r2.Object;
  //    //    translatedPhrase.Text = translatedText;
  //    //    callback(this, new ResultArgs<PhraseEdit>(translatedPhrase));
  //    //    return;
  //    //  });
  //    //};

  //    //var untranslatedPhraseLanguageCode = BingTranslateHelper.GetLanguageCode(untranslatedPhrase.Language.Text);
  //    //var targetLanguageCode = BingTranslateHelper.GetLanguageCode(targetLanguageText);

  //    //try
  //    //{
  //    //  client.TranslateAsync(StudyResources.BingAppId,
  //    //                        untranslatedPhrase.Text,
  //    //                        untranslatedPhraseLanguageCode,
  //    //                        targetLanguageCode,
  //    //                        @"text/plain", //this as opposed to "text/html"
  //    //                        "general"); //only supported category is "general"
  //    //  //GOTO ABOVE TRANSLATECOMPLETED HANDLER
  //    //}
  //    //catch (Exception ex)
  //    //{
  //    //  callback(this, new ResultArgs<PhraseEdit>(ex));
  //    //}
  //  }
  //}
}
