using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
using System.Linq;
using LearnLanguages.Business.Security;

namespace LearnLanguages.Silverlight.Tests
{
  //THE SEEDDATA INSTANCE IS NOT UPDATED ON THE CLIENT.  WE CANNOT TEST AGAINST SeedData.Ton IDS
  //BECAUSE THESE IDS WERE NOT UPDATED WHEN THE DB WAS SEEDED, THE SeedData.Ton ON THE SERVER 
  //WAS UPDATED.  THE RELATIONSHIPS SHOULD BE VALID HOWEVER.
  [TestClass]
  [Tag("language")]
  public class LanguageTests : TestsBase
  {
    
    //[ClassInitialize]
    //[Asynchronous]
    //public static async Task InitializeLanguageTests(TestContext context)
    //{
      //var task = LanguageList.GetAllAsync();
      //var wasSuccessful = task.Wait(5000);
      //if (!wasSuccessful)
      //  throw new Exception("GetAllLanguages timed out.");

      //var allLanguages = task.Result;
      
      //await UserPrincipal.LoginAsync("user", "password");

      //var allLanguages = await LanguageList.GetAllAsync();
      //_EnglishId = (from language in allLanguages
      //              where language.Text == SeedData.Ton.EnglishText
      //              select language.Id).First();

      //_SpanishId = (from language in allLanguages
      //              where language.Text == SeedData.Ton.SpanishText
      //              select language.Id).First();
    //}

    [TestMethod]
    [Asynchronous]
    public async Task CREATE_NEW()
    {
      var isSetup = false;
      await Setup();
      try
      {
        isSetup = true;
        var isCreated = false;
        var languageEdit = await LanguageEdit.NewLanguageEditAsync();
        isCreated = true;

        EnqueueConditional(() => isSetup);
        EnqueueConditional(() => isCreated);
        EnqueueCallback(() => Assert.IsNotNull(languageEdit));
        EnqueueCallback(() => Assert.AreNotEqual(Guid.Empty, languageEdit.UserId));

        EnqueueTestComplete();
      }
      finally
      {
        Teardown();
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task GET()
    {
      #region Setup (try..)
      var isSetup = false;
      await Setup();
      try
      {
        isSetup = true;

        EnqueueConditional(() => isSetup);
      #endregion

        Guid testId = _EnglishId;
        var isGotten = false;
        var isError = false;
        LanguageEdit languageEdit = null;
        try
        {
          languageEdit = await LanguageEdit.GetLanguageEditAsync(testId);
          isGotten = true;
        }
        catch (Exception)
        {
          isGotten = true;
          isError = true;
        }

        EnqueueConditional(() => isGotten);
        EnqueueConditional(() => isSetup);

        EnqueueCallback(() => Assert.IsNotNull(languageEdit));
        EnqueueCallback(() => Assert.IsTrue(isGotten));
        EnqueueCallback(() => Assert.IsFalse(isError));
        EnqueueCallback(() => Assert.AreEqual(testId, languageEdit.Id));

        EnqueueTestComplete();

      #region (...finally) Teardown
      }
      finally
      {
        Teardown();
      }

        #endregion
    }

    [TestMethod]
    [Asynchronous]
    public async Task NEW_EDIT_BEGINSAVE_GET()
    {
      #region Setup (try..)
      var isAsyncComplete = false;
      var isSetup = false;
      await Setup();
      try
      {
        isSetup = true;
        EnqueueConditional(() => isSetup);
      #endregion
        //NEW
        var languageEdit = await LanguageEdit.NewLanguageEditAsync();

        //EDIT
        languageEdit.Text = "TestLanguage_NEW_EDIT_BEGINSAVE_GET";

        //SAVE
        var savedLanguageEdit = await languageEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        var gottenLanguageEdit = await LanguageEdit.GetLanguageEditAsync(savedLanguageEdit.Id);

        isAsyncComplete = true;

        EnqueueConditional(() => isAsyncComplete);

        EnqueueCallback(() => Assert.IsNotNull(languageEdit));
        EnqueueCallback(() => Assert.IsNotNull(savedLanguageEdit));
        EnqueueCallback(() => Assert.IsNotNull(gottenLanguageEdit));
        EnqueueCallback(() => Assert.AreNotEqual(languageEdit.Id, savedLanguageEdit.Id));
        EnqueueCallback(() => Assert.AreEqual(savedLanguageEdit.Id, gottenLanguageEdit.Id));
        EnqueueCallback(() => Assert.AreEqual(savedLanguageEdit.Text, gottenLanguageEdit.Text));

        EnqueueTestComplete();
        
      #region (...finally) Teardown
      }
      finally
      {
        Teardown();
      }
      #endregion
    }

    
    [TestMethod]
    [Asynchronous]
    public async Task NEW_EDIT_BEGINSAVE_GET_DELETE_GET()
    {
      #region Setup (try..)
      var isAsyncComplete = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
      #endregion
        LanguageEdit languageEdit = null;
        LanguageEdit savedLanguageEdit = null;
        LanguageEdit gottenLanguageEdit = null;
        LanguageEdit deletedLanguageEdit = null;

        //INITIALIZE TO EMPTY LANGUAGE EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
        LanguageEdit deleteConfirmedLanguageEdit = new LanguageEdit();

        //NEW
        languageEdit = await LanguageEdit.NewLanguageEditAsync();

        //EDIT
        languageEdit.Text = "TestLanguage";

        //SAVE
        savedLanguageEdit = await languageEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenLanguageEdit = await LanguageEdit.GetLanguageEditAsync(savedLanguageEdit.Id);

        //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
        gottenLanguageEdit.Delete();
        deletedLanguageEdit = await gottenLanguageEdit.SaveAsync();

        EnqueueTestComplete();

        try
        {
          deleteConfirmedLanguageEdit = await LanguageEdit.GetLanguageEditAsync(deletedLanguageEdit.Id);
          throw new Exception();
        }
        catch (Csla.DataPortalException dpex)
        {
          //WE ARE EXPECTING ONLY ID NOT FOUND EXCEPTIONS
          if (!TestsHelper.IsIdNotFoundException(dpex))
            throw dpex;

          Assert.IsNotNull(languageEdit);
          Assert.IsNotNull(savedLanguageEdit);
          Assert.IsNotNull(gottenLanguageEdit);
          Assert.IsNotNull(deletedLanguageEdit);
          Assert.AreEqual(Guid.Empty, deleteConfirmedLanguageEdit.Id);
          Assert.AreEqual(string.Empty, deleteConfirmedLanguageEdit.Text);
          Assert.AreEqual(Guid.Empty, deleteConfirmedLanguageEdit.UserId);
          Assert.AreEqual(string.Empty, deleteConfirmedLanguageEdit.Username);
        }
      #region (...finally) Teardown
      }
      finally
      {
        Teardown();
        isAsyncComplete = true;
      }
      #endregion
    }

    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL()
    {
      var isAsyncComplete = false;
      #region Setup (try..)
      var isSetup = false;
      await Setup();
      try
      {
        isSetup = true;
        EnqueueConditional(() => isSetup);
      #endregion
        int countEnglish = 0;
        int countSpanish = 0;
        Guid englishId = Guid.Empty;
        Guid spanishId = Guid.Empty;
        var allLanguages = await LanguageList.GetAllAsync();

        var englishResults = from lang in allLanguages
                             where lang.Text == SeedData.Ton.EnglishText
                             select lang;
        countEnglish = englishResults.Count();
        var englishLang = englishResults.First();
        englishId = englishLang.Id;

        var spanishResults = from lang in allLanguages
                             where lang.Text == SeedData.Ton.SpanishText
                             select lang;

        countSpanish = spanishResults.Count();
        var spanishLang = spanishResults.First();
        spanishId = spanishLang.Id;
        
        isAsyncComplete = true;

        EnqueueConditional(() => isAsyncComplete);

        EnqueueCallback(() => Assert.IsNotNull(allLanguages));
        EnqueueCallback(() => Assert.AreEqual(1, countEnglish));
        EnqueueCallback(() => Assert.AreEqual(1, countSpanish));
        EnqueueCallback(() => Assert.AreEqual(_EnglishId, englishId));
        EnqueueCallback(() => Assert.AreEqual(_SpanishId, spanishId));
        EnqueueCallback(() => Assert.IsTrue(allLanguages.Count > 0));

        EnqueueTestComplete();

      #region (...finally) Teardown
      }
      finally
      {
        Teardown();
      }
      #endregion
    }
  }
}