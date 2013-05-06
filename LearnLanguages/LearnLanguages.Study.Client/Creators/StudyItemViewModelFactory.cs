using System;
using System.Linq;

using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common;
using LearnLanguages.Study.ViewModels;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The purpose of this class is to produce viewmodels given some input.
  /// </summary>
  public class StudyItemViewModelFactory
  {
    #region Singleton Pattern Members
    private static volatile StudyItemViewModelFactory _Ton;
    private static object _Lock = new object();
    public static StudyItemViewModelFactory Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new StudyItemViewModelFactory();
          }
        }

        return _Ton;
      }
    }
    #endregion

    /// <summary>
    /// Creates a study item using the phrase and nativeLanguageText.  If the phrase.Language
    /// is the same as the native language, then this will be a study item related to studying
    /// the meaning of the phrase, all in the native language.  If these two languages are 
    /// different, then it will procure a translation study item.
    /// This prepares the view model so that all that is necessary for the caller is to call
    /// viewmodel.Show(...);
    /// </summary>
    public async Task<StudyItemViewModelBase> ProcureAsync(PhraseEdit phrase,
      string nativeLanguageText)
    //public async Task<ResultArgs<StudyItemViewModelBase>> ProcureAsync(PhraseEdit phrase, 
    {
      //FOR NOW, THIS WILL JUST CREATE ONE TYPE OF STUDY ITEM VIEW MODEL: STUDY QUESTION ANSWER VIEWMODEL.
      //IT WILL SHOW THE QUESTION AND HIDE THE ANSWER FOR VARIOUS AMOUNTS OF TIME, DEPENDING ON QUESTION
      //LENGTH, AND OTHER FACTORS.
      //IN THE FUTURE, THIS WILL BE EXTENSIBILITY POINT WHERE WE CAN SELECT DIFFERENT VARIETIES OF Q AND A 
      //TRANSLATION VIEW MODELS BASED ON HOW SUCCESSFUL THEY ARE.  WE CAN ALSO HAVE DIFFERENT CONFIGURATIONS
      //OF VARIETIES BE SELECTED HERE.

      //ViewModels.StudyTimedQuestionAnswerViewModel viewModel = new ViewModels.StudyTimedQuestionAnswerViewModel();

      var languageText = phrase.Language.Text;

      //IF THE TARGET LANGUAGE IS DIFFERENT FROM THE PHRASE LANGUAGE, THEN WE CREATE A TRANSLATION Q & A.
      //IF THE TWO LANGUAGES ARE THE SAME, THEN WE CREATE A STUDY NATIVE LANGUAGE PHRASE Q & A.
        
      bool phraseIsInForeignLanguage = (languageText != nativeLanguageText);
      if (phraseIsInForeignLanguage)
      {
        //DO A TRANSLATION Q & A
        //WE NEED TO FIND A TRANSLATION FOR THIS FOREIGN LANGUAGE PHRASE.
        PhraseEdit translatedPhrase = null;
        #region Thinking (try..)
        var targetId = Guid.NewGuid();
        History.Events.ThinkingAboutTargetEvent.Publish(targetId);
        try
        {
        #endregion
          var result = await GetTranslatedPhrase(phrase, nativeLanguageText);
          translatedPhrase = result.Object;
        #region (...finally) Thinked
        }
        finally
        {
          History.Events.ThinkedAboutTargetEvent.Publish(targetId);
        }
        #endregion

        //RIGHT NOW, WE HAVE A MANUAL AND A TIMED QA.
        var timedQA = new ViewModels.StudyPhraseTimedQuestionAnswerViewModel();
        var manualQA = new ViewModels.StudyPhraseTimedQuestionAnswerViewModel();
        //hack: I'm just setting these both to timed, because that is what I've corrected
        //for the async stuff and I want to make sure that it works.
        //var manualQA = new ViewModels.StudyPhraseManualQuestionAnswerViewModel();

        //PICK A RANDOM VIEW MODEL, EITHER TIMED OR MANUAL
        StudyItemViewModelBase dummy = null;
        var qaViewModel = RandomPicker.Ton.PickOne<StudyItemViewModelBase>(timedQA, manualQA, out dummy);

        //ASSIGN QUESTION/ANSWER RANDOMLY FROM PHRASE AND TRANSLATED PHRASE
        PhraseEdit answer = null;
        var question = RandomPicker.Ton.PickOne(phrase, translatedPhrase, out answer);

        //INITIALIZE VIEWMODEL WITH Q & A
        if (qaViewModel is ViewModels.StudyPhraseTimedQuestionAnswerViewModel)
          ((ViewModels.StudyPhraseTimedQuestionAnswerViewModel)qaViewModel).Initialize(question, answer);//, (e1) =>
        else
          ((ViewModels.StudyPhraseTimedQuestionAnswerViewModel)qaViewModel).Initialize(question, answer);
          //((ViewModels.StudyPhraseManualQuestionAnswerViewModel)qaViewModel).Initialize(question, answer);

        return qaViewModel;
        //var resultQAViewModel = new ResultArgs<StudyItemViewModelBase>(qaViewModel);
        //return await StudyHelper.WrapInTask<ResultArgs<StudyItemViewModelBase>>(resultQAViewModel);
      }
      else
      {
        //DO A NATIVE LANGUAGE Q & A
        throw new NotImplementedException();
      }
    }

    ///// <summary>
    ///// Creates a study item using the line and MLT. 
    ///// This prepares the view model so that all that is necessary for the caller is to call
    ///// viewmodel.Show(...);
    ///// </summary>
    //public async Task<ResultArgs<StudyItemViewModelBase>> Procure(LineEdit line,
    //  MultiLineTextEdit multiLineText)
    //{
    //  //FOR NOW, THIS WILL JUST CREATE ONE OF TWO STUDY ITEM VIEW MODELS: 
    //  //STUDY LINE ORDER VIEWMODEL (TIMED OR MANUAL).
    //  //IT WILL SHOW THE QUESTION AND HIDE THE ANSWER FOR VARIOUS AMOUNTS OF TIME, DEPENDING ON QUESTION
    //  //LENGTH, AND OTHER FACTORS.
    //  //IN THE FUTURE, THIS WILL BE EXTENSIBILITY POINT WHERE WE CAN SELECT DIFFERENT VARIETIES OF Q AND A 
    //  //TRANSLATION VIEW MODELS BASED ON HOW SUCCESSFUL THEY ARE.  WE CAN ALSO HAVE DIFFERENT CONFIGURATIONS
    //  //OF VARIETIES BE SELECTED HERE.

    //  var manualViewModel = new StudyLineOrderManualQuestionAnswerViewModel();
    //  var timedViewModel = new StudyLineOrderTimedQuestionAnswerViewModel();
    //  StudyItemViewModelBase dummy = null;
    //  var viewModel = RandomPicker.Ton.PickOne<StudyItemViewModelBase>(manualViewModel, timedViewModel, out dummy);
    //  if (viewModel is StudyLineOrderManualQuestionAnswerViewModel)
    //    manualViewModel.Initialize(line, multiLineText);
    //  else
    //    timedViewModel.Initialize(line, multiLineText);
    //  return await new Task<ResultArgs<StudyItemViewModelBase>>(
    //    () =>
    //    {
    //      var result = new ResultArgs<StudyItemViewModelBase>(viewModel);
    //      return result;
    //    });
    //}

    /// <summary>
    /// Uses auto translation service (as of writing, this is BingTranslatorService) to translate phrase
    /// into another phrase, which is returned in the callback.
    /// </summary>
    private async Task<PhraseEdit> AutoTranslate(PhraseEdit untranslatedPhrase,
                                                 string targetLanguageText)
    {
      throw new NotImplementedException();
      //todo: implement this using common.translation.translator
      //BingTranslatorService.LanguageServiceClient client = new BingTranslatorService.LanguageServiceClient();

      //client.TranslateCompleted += (s, r) =>
      //{
      //  if (r.Error != null)
      //  {
      //    callback(this, new ResultArgs<PhraseEdit>(r.Error));
      //    return;
      //  }

      //  var translatedText = r.Result;
      //  PhraseEdit.NewPhraseEdit(targetLanguageText, (s2, r2) =>
      //    {
      //      if (r2.Error != null)
      //      {
      //        callback(this, new ResultArgs<PhraseEdit>(r.Error));
      //        return;
      //      }

      //      var translatedPhrase = r2.Object;
      //      translatedPhrase.Text = translatedText;
      //      callback(this, new ResultArgs<PhraseEdit>(translatedPhrase));
      //      return;
      //    });
      //};

      //var untranslatedPhraseLanguageCode = BingTranslateHelper.GetLanguageCode(untranslatedPhrase.Language.Text);
      //var targetLanguageCode = BingTranslateHelper.GetLanguageCode(targetLanguageText);

      //try
      //{
      //  client.TranslateAsync(StudyResources.BingAppId,
      //                        untranslatedPhrase.Text,
      //                        untranslatedPhraseLanguageCode,
      //                        targetLanguageCode,
      //                        @"text/plain", //this as opposed to "text/html"
      //                        "general"); //only supported category is "general"
      //  //GOTO ABOVE TRANSLATECOMPLETED HANDLER
      //}
      //catch (Exception ex)
      //{
      //  callback(this, new ResultArgs<PhraseEdit>(ex));
      //}
    }

    /// <summary>
    /// First searches DB for translated phrase.  If not found there, uses AutoTranslate.
    /// </summary>
    private async Task<ResultArgs<PhraseEdit>> GetTranslatedPhrase(PhraseEdit phrase, 
      string targetLanguageText)
    {
      var criteria = new Business.Criteria.TranslationSearchCriteria(phrase, targetLanguageText, true);
      var retriever = await TranslationSearchRetriever.CreateNewAsync(criteria);

      if (retriever.Translation != null)
      {
        var translatedPhrase = BusinessHelper.ExtractPhrase(retriever.Translation, 
                                                            targetLanguageText);
        return new ResultArgs<PhraseEdit>(translatedPhrase);
      }
      else
      {
        return null;
      }


      //////THIS IS THE PHRASE WE'RE LOOKING FOR
      //PhraseEdit translatedPhrase = null;

      ////FIRST LOOK IN DB.  IF NOT, THEN AUTO TRANSLATE.
      //#region Thinking (try..)
      //var targetId = Guid.NewGuid();
      //History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      //try
      //{
      //#endregion
      //  var searchCriteria = new Business.Criteria.TranslationSearchCriteria(phrase, targetLanguageText);
      //  var retriever = await TranslationSearchRetriever.CreateNewAsync(searchCriteria);
      //#region (...finally) Thinked
      //}
      //finally
      //{
      //  History.Events.ThinkedAboutTargetEvent.Publish(targetId);
      //}
      //  #endregion
      ////    callback(this, new ResultArgs<PhraseEdit>(r.Error));
      ////    return;
      ////  }

      ////  var retriever = r.Object;
      ////  var foundTranslation = retriever.Translation;
      ////  if (foundTranslation != null)
      ////  {
      ////    //FOUND TRANSLATED PHRASE IN DATABASE.
      ////    translatedPhrase = (from p in foundTranslation.Phrases
      ////                        where p.Language.Text == targetLanguageText
      ////                        select p).First();
      ////    callback(this, new ResultArgs<PhraseEdit>(translatedPhrase));
      ////    return;
      ////  }
      ////  else
      ////  {
      //    //NO TRANSLATION FOUND IN DB, WILL NEED TO AUTO TRANSLATE
      //    var translatedPhrase = await AutoTranslate(phrase, targetLanguageText);//, (s2, r2) =>
      //    //{
      //    //  if (r2.Error != null)
      //    //  {
      //    //    callback(this, new ResultArgs<PhraseEdit>(r2.Error));
      //    //    return;
      //    //  }

      //    //  //GOT TRANSLATED PHRASE VIA AUTOTRANSLATE.  NEED TO CHECK IF WE ALREADY HAVE THIS
      //    //  //PHRASE IN OUR DB, AND USE THAT OBJECT INSTEAD IF WE DO.
      //    //  translatedPhrase = r2.Object;
      //    //  var translatedPhraseLanguageText = translatedPhrase.Language.Text;
      //    //  var criteriaPhrase = new Business.Criteria.ListOfPhrasesCriteria(translatedPhrase);
      //    //  Business.PhrasesByTextAndLanguageRetriever.CreateNew(criteriaPhrase, (s3, r3) =>
      //    //    {
      //    //      if (r3.Error != null)
      //    //      {
      //    //        callback(this, new ResultArgs<PhraseEdit>(r2.Error));
      //    //        return;
      //    //      }

      //    //      //CHECK OUR RETRIEVER IF IT SUCCESSFULLY FOUND A PHRASE EDIT IN THE DB.
      //    //      //IF IT DID FIND ONE (NOT NULL), THEN ASSIGN OUR TRANSLATEDPHRASE VAR TO THE DB VERSION.
      //    //      //THIS WAY, WE WON'T DUPLICATE PHRASES IN OUR DB.
      //    //      var translatedPhraseRetriever = r3.Object;
      //    //      var dbPhraseEdit = translatedPhraseRetriever.RetrievedPhrases[translatedPhrase.Id];
      //    //      if (dbPhraseEdit != null)
      //    //        translatedPhrase = dbPhraseEdit;

      //    //      //FIRE EVENT THAT WE HAVE AUTOTRANSLATED A PHRASE
      //    //      var autoTranslatedEvent =
      //    //        new History.Events.PhraseAutoTranslatedEvent(phrase, translatedPhrase);
      //    //      History.HistoryPublisher.Ton.PublishEvent(autoTranslatedEvent);

      //    //      //CALLBACK
      //    //      callback(this, new ResultArgs<PhraseEdit>(translatedPhrase));
      //    //      return;

      //    //    });

      //    //});
      ////  }
      ////});
    }
  }


}
