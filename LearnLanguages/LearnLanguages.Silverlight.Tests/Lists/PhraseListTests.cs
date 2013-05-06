using System;

using LearnLanguages.Business;

using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using System.Threading.Tasks;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("phraselist")]
  public class PhraseListTests : TestsBase
  {
    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL()
    {
      PhraseList allPhrases = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        allPhrases = await PhraseList.GetAllAsync();
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(allPhrases),
                        () => Assert.IsTrue(allPhrases.Count > 0),
                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }
    
    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL_EDIT_SAVE()
    {

      var allPhrasesCount = -1;
      bool phrasesCountStaysTheSame = false;
      var text0 = "This is edited Text00000._save";
      var text1 = "This is edited Text11111._save";
      var text2 = "This is edited Text22222222222222222222222._save";
      var containsText0 = false;
      var containsText1 = false;
      var containsText2 = false;  

      PhraseList allPhrases = null;
      PhraseList savedPhrases = null;
      
      
var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        allPhrases = await PhraseList.GetAllAsync();

        allPhrases[0].Text = text0;
        allPhrases[1].Text = text1;
        allPhrases[2].Text = text2;

        allPhrasesCount = allPhrases.Count;

        savedPhrases = await allPhrases.SaveAsync();

        phrasesCountStaysTheSame = allPhrasesCount == savedPhrases.Count;

        containsText0 = (from phrase in savedPhrases
                         where phrase.Text == text0
                         select phrase).Count() > 0;
        containsText1 = (from phrase2 in savedPhrases
                         where phrase2.Text == text1
                         select phrase2).Count() > 0;
        containsText2 = (from phrase3 in savedPhrases
                         where phrase3.Text == text2
                         select phrase3).Count() > 0;
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(allPhrases),
                        () => Assert.IsNotNull(savedPhrases),
                        () => Assert.IsTrue(containsText0),
                        () => Assert.IsTrue(containsText1),
                        () => Assert.IsTrue(containsText2),
                        () => Assert.IsTrue(phrasesCountStaysTheSame),
                        () => Assert.IsTrue(allPhrasesCount > 0),
          //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL_EDIT_CANCELEDIT()
    {
      var text0 = "This is edited Text00000._canceledit";
      var text1 = "This is edited Text11111._canceledit";
      var text2 = "This is edited Text22222222222222222222222._canceledit";
      var containsText0 = false;
      var containsText1 = false;
      var containsText2 = false;

      PhraseList allPhrases = null;
      PhraseList canceledEditPhrases = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        allPhrases = await PhraseList.GetAllAsync();

        allPhrases.BeginEdit();
        allPhrases[0].Text = text0;
        allPhrases[1].Text = text1;
        allPhrases[2].Text = text2;
        allPhrases.CancelEdit();

        canceledEditPhrases = allPhrases;

        containsText0 = (from phrase in canceledEditPhrases
                         where phrase.Text == text0
                         select phrase).Count() > 0;
        containsText1 = (from phrase in canceledEditPhrases
                         where phrase.Text == text1
                         select phrase).Count() > 0;
        containsText2 = (from phrase in canceledEditPhrases
                         where phrase.Text == text2
                         select phrase).Count() > 0;
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(allPhrases),
                        () => Assert.IsNotNull(canceledEditPhrases),
                        () => Assert.IsFalse(containsText0),
                        () => Assert.IsFalse(containsText1),
                        () => Assert.IsFalse(containsText2),
                        () => Assert.IsTrue(allPhrases.Count > 0),
          //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL_SET_NEW_LANGUAGE_SAVE()
    {

      var allPhrasesCount = -1;
      bool phrasesCountStaysTheSame = false;
      LanguageEdit initLang0 = null;
      LanguageEdit initLang1 = null;
      LanguageEdit initLang2 = null;

      PhraseList allPhrases = null;
      PhraseList savedPhrases = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        allPhrases = await PhraseList.GetAllAsync();

        allPhrasesCount = allPhrases.Count;
        initLang0 = allPhrases[0].Language;
        initLang1 = allPhrases[1].Language;
        initLang2 = allPhrases[2].Language;

        //this is fine
        allPhrases.BeginEdit();
        allPhrases.ApplyEdit();

        allPhrases[0].BeginEdit();
        allPhrases[0].Language = _ServerSpanishLang;
        allPhrases[0].ApplyEdit();

        allPhrases[1].BeginEdit();
        allPhrases[1].Language = _ServerSpanishLang;
        allPhrases[1].ApplyEdit();

        allPhrases[2].BeginEdit();
        allPhrases[2].Language = _ServerSpanishLang;
        allPhrases[2].ApplyEdit();

        savedPhrases = await allPhrases.SaveAsync();
        phrasesCountStaysTheSame = allPhrasesCount == savedPhrases.Count;
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.AreEqual(_ServerSpanishLang.Id, savedPhrases[0].LanguageId),
                        () => Assert.AreEqual(_ServerSpanishLang.Id, savedPhrases[1].LanguageId),
                        () => Assert.AreEqual(_ServerSpanishLang.Id, savedPhrases[2].LanguageId),
                        () => Assert.IsNotNull(allPhrases),
                        () => Assert.IsNotNull(savedPhrases),
                        () => Assert.IsTrue(phrasesCountStaysTheSame),
                        () => Assert.IsTrue(allPhrasesCount > 0),

                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL_SET_NEW_LANGUAGE_CANCELEDIT()
    {
      var allPhrasesCount = -1;
      bool phrasesCountStaysTheSame = false;
      LanguageEdit initLang0 = null;
      LanguageEdit initLang1 = null;
      LanguageEdit initLang2 = null;

      PhraseList allPhrases = null;
      PhraseList canceledEditPhrases = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        allPhrases = await PhraseList.GetAllAsync();

        allPhrasesCount = allPhrases.Count;
        initLang0 = allPhrases[0].Language;
        initLang1 = allPhrases[1].Language;
        initLang2 = allPhrases[2].Language;

        allPhrases[0].BeginEdit();
        allPhrases[0].Language = _ServerSpanishLang;
        allPhrases[0].CancelEdit();

        canceledEditPhrases = allPhrases;
        phrasesCountStaysTheSame = allPhrasesCount == canceledEditPhrases.Count;
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.AreEqual(initLang0.Id, canceledEditPhrases[0].LanguageId),
                        () => Assert.AreEqual(initLang1.Id, canceledEditPhrases[1].LanguageId),
                        () => Assert.AreEqual(initLang2.Id, canceledEditPhrases[2].LanguageId),
                        () => Assert.IsNotNull(allPhrases),
                        () => Assert.IsNotNull(canceledEditPhrases),
                        () => Assert.IsTrue(phrasesCountStaysTheSame),
                        () => Assert.IsTrue(allPhrasesCount > 0),

                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL_SET_NEW_LANGUAGE_SAVE_GET_CONFIRMNEWLANGUAGE()
    {
      var allPhrasesCount = -1;
      PhraseEdit testPhrase = null;
      PhraseEdit savedPhrase = null;
      PhraseEdit gottenPhrase = null;

      PhraseList allPhrases = null;
      PhraseList savedAllPhrases = null;
      PhraseList confirmAllPhrases = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //LOAD--------------
        allPhrases = await PhraseList.GetAllAsync();

        allPhrasesCount = allPhrases.Count;

        testPhrase = (from phrase in allPhrases
                      where phrase.Language.Text == _ServerEnglishLang.Text
                      select phrase).First();

        //testPhrase is english

        //EDIT---------
        testPhrase.BeginEdit();
        testPhrase.Language = _ServerSpanishLang;
        testPhrase.ApplyEdit();

        //SAVE-------------
        savedAllPhrases = await allPhrases.SaveAsync();

        savedPhrase = (from phrase in savedAllPhrases
                       where phrase.Text == testPhrase.Text
                       select phrase).First();

        Assert.AreEqual(_ServerSpanishLang.Text, savedPhrase.Language.Text);
        Assert.AreEqual(_ServerSpanishLang.Id, savedPhrase.Language.Id);
        Assert.AreEqual(_ServerSpanishLang.Id, savedPhrase.LanguageId);

        //GET-----------
        gottenPhrase = await PhraseEdit.GetPhraseEditAsync(savedPhrase.Id);

        //CONFIRM----------
        //confirm language swap worked.
        Assert.AreEqual(_ServerSpanishLang.Text, gottenPhrase.Language.Text);
        Assert.AreEqual(_ServerSpanishLang.Id, gottenPhrase.Language.Id);
        Assert.AreEqual(_ServerSpanishLang.Id, gottenPhrase.LanguageId);
        //confirm we haven't glitched our data and magically doubled the size of our allPhrases.
        confirmAllPhrases = await PhraseList.GetAllAsync();
        Assert.AreEqual(allPhrasesCount, confirmAllPhrases.Count);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(allPhrases),
                        () => Assert.IsNotNull(confirmAllPhrases),

                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL_CHECK_GLITCH()
    {
      PhraseList allPhrases = null;
      int allPhrasesCount = -1;
      PhraseList savedAllPhrases = null;
      int savedAllPhrasesCount = -2;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        allPhrases = await PhraseList.GetAllAsync();
        allPhrasesCount = allPhrases.Count;

        savedAllPhrases = await allPhrases.SaveAsync();
        savedAllPhrasesCount = savedAllPhrases.Count;
        Assert.AreEqual(allPhrasesCount, savedAllPhrasesCount);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(allPhrases),
                        () => Assert.IsTrue(allPhrasesCount > 0),
                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }
  }
}