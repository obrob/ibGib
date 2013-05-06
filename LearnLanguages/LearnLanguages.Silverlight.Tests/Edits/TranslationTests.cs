using System;

using LearnLanguages.Business;

using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Linq;
using Csla;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using System.Threading.Tasks;


namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("translation")]
  public class TranslationTests : TestsBase
  {
    [TestMethod]
    [Asynchronous]
    public async Task CREATE_NEW()
    {
      TranslationEdit newTranslationEdit = null;
      
     var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
         newTranslationEdit = await TranslationEdit.NewTranslationEditAsync();
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newTranslationEdit),
                
                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }


    [TestMethod]
    [Asynchronous]
    public async Task GET()
    {
      Guid testId = Guid.Empty;
      TranslationEdit translationEdit = null;
      TranslationList allTranslations = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        allTranslations = await TranslationList.GetAllAsync();
        testId = allTranslations.First().Id;
        translationEdit = await TranslationEdit.GetTranslationEditAsync(testId);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(translationEdit),
                        () => Assert.IsTrue(translationEdit.Phrases.Count >= 2),
                        () => Assert.AreEqual(testId, translationEdit.Id),
                        
                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }   
    }

    [TestMethod]
    [Asynchronous]
    [Tag("current")]
    public async Task TRANSLATION_CREATOR_NEW()
    {
      
      TranslationEdit newTranslationEdit = null;
      TranslationEdit savedTranslationEdit = null;
      TranslationEdit confirmedSaveTranslationEdit = null;

      Business.TranslationCreator creator = null;

      string phraseAText = "Test Phrase A Text.  This is phrase A in English.";
      string phraseBText = "Test Phrase BBBB Text.  This is phrase B in Spanish.";
      //PhraseEdit phraseA = null;
      //PhraseEdit phraseB = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        ////CREATE THE TWO PHRASES
        //phraseA = await PhraseEdit.NewPhraseEditAsync(_ServerEnglishLang.Text);
        //phraseA.Text = phraseAText;
        //phraseA = await phraseA.SaveAsync();

        //phraseB = await PhraseEdit.NewPhraseEditAsync(_ServerSpanishLang.Text);
        //phraseB.Text = phraseBText;
        //phraseB = await phraseB.SaveAsync();

        //AT THIS POINT, WE HAVE TWO PHRASES THAT WOULD BE EQUIVALENT PHRASES (A TRANSLATION).

        //NEW
        //var criteria = new Business.Criteria.ListOfPhrasesCriteria(phraseA, phraseB);
        //var creator = await Business.TranslationCreator.CreateNewAsync(criteria);
        var A = new Tuple<string, string>();
        A.Item1 = phraseAText;
        A.Item2 = _ServerEnglishLang.Text;

        var B = new Tuple<string, string>();
        B.Item1 = phraseBText;
        B.Item2 = _ServerSpanishLang.Text;
        var criteria = new Business.Criteria.PhraseTextLanguageTextPairsCriteria(A, B);
        creator = await Business.TranslationCreator.CreateNewAsync(criteria);
        newTranslationEdit = creator.Translation;

        #region old
        //Business.TranslationCreator.CreateNewAsync(
        //    //EDIT
        //    newTranslationEdit.Phrases.AddedNew += (s5, r5) =>
        //      {
        //        if (phraseA == null)
        //        {
        //          phraseA = r5.NewObject;
        //          phraseA.Id = Guid.NewGuid();
        //          phraseA.Text = phraseAText;

        //          phraseA.LanguageId = SeedData.Ton.EnglishId;
        //        }
        //        else
        //        {
        //          phraseB = r5.NewObject;
        //          phraseB.Id = Guid.NewGuid();
        //          phraseB.Text = phraseBText;
        //          phraseB.LanguageId = SeedData.Ton.SpanishId;
        //        }
        //      };

        //newTranslationEdit.Phrases.AddNew();
        //newTranslationEdit.Phrases.AddNew();
        #endregion
        //SAVE
        savedTranslationEdit = await newTranslationEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        confirmedSaveTranslationEdit =
          await TranslationEdit.GetTranslationEditAsync(savedTranslationEdit.Id);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),

                        () => Assert.IsNotNull(newTranslationEdit),
                        () => Assert.IsNotNull(savedTranslationEdit),
                        () => Assert.IsNotNull(confirmedSaveTranslationEdit),
                        () => Assert.IsTrue(confirmedSaveTranslationEdit.Phrases.Count >= 2),
                        () => Assert.AreEqual(savedTranslationEdit.Id, confirmedSaveTranslationEdit.Id),
                        () => Assert.AreEqual(savedTranslationEdit.Phrases[0].Text, phraseAText),
                        () => Assert.AreEqual(savedTranslationEdit.Phrases[1].Text, phraseBText),

                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }
  }
}