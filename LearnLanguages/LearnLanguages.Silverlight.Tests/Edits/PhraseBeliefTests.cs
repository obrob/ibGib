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
  [Tag("phrasebelief")]
  [Tag("pb")]
  public class PhraseBeliefTests : TestsBase
  {
    private PhraseEdit _TestPhraseEdit;

    public override async System.Threading.Tasks.Task Setup()
    {
      //MUST AUTHENTICATE FIRST, SO CALL BASE.SETUP()
      await base.Setup();

      var allPhrases = await PhraseList.GetAllAsync();
      _TestPhraseEdit = allPhrases.First();
    }

    [TestMethod]
    [Asynchronous]
    public async Task CREATE_NEW()
    {
      PhraseBeliefEdit newPhraseBeliefEdit = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        newPhraseBeliefEdit = await PhraseBeliefEdit.NewPhraseBeliefEditAsync();
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsNotNull(newPhraseBeliefEdit),
                        () => Assert.IsFalse(hasError)
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task NEW_EDIT_BEGINSAVE_GET()
    {
      
      PhraseBeliefEdit newPhraseBeliefEdit = null;
      PhraseBeliefEdit savedPhraseBeliefEdit = null;
      PhraseBeliefEdit gottenPhraseBeliefEdit = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //NEW
        newPhraseBeliefEdit = await PhraseBeliefEdit.NewPhraseBeliefEditAsync();

        //EDIT
        newPhraseBeliefEdit.TimeStamp = DateTime.Now - TimeSpan.FromDays(1);
        newPhraseBeliefEdit.TimeStamp = DateTime.Now - TimeSpan.FromDays(1);
        newPhraseBeliefEdit.Text = "TestPhraseBelief.Text edited in NEW_EDIT_BEGINSAVE_GET test";
        newPhraseBeliefEdit.Strength = 2.0d;
        newPhraseBeliefEdit.Phrase = _TestPhraseEdit;
        //newPhraseBeliefEdit.Phrase.Language = _ServerEnglishLang;
        //newPhraseBeliefEdit.PhraseBeliefNumber = 0;
        //Assert.AreEqual(SeedData.Ton.TestValidUsername, newPhraseBeliefEdit.Username);

        //newPhraseBeliefEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        //newPhraseBeliefEdit.Username = SeedData.Ton.TestValidUsername;

        //SAVE
        savedPhraseBeliefEdit = await newPhraseBeliefEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenPhraseBeliefEdit =
          await PhraseBeliefEdit.GetPhraseBeliefEditAsync(savedPhraseBeliefEdit.Id);

      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newPhraseBeliefEdit),
                        () => Assert.AreNotEqual(Guid.Empty, newPhraseBeliefEdit.PhraseId),
                        () => Assert.IsNotNull(savedPhraseBeliefEdit),
                        () => Assert.IsNotNull(gottenPhraseBeliefEdit),
                        () => Assert.AreEqual(savedPhraseBeliefEdit.Id, gottenPhraseBeliefEdit.Id)
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task NEW_EDIT_BEGINSAVE_GET_DELETE_GET()
    {
      PhraseBeliefEdit newPhraseBeliefEdit = null;
      PhraseBeliefEdit savedPhraseBeliefEdit = null;
      PhraseBeliefEdit gottenPhraseBeliefEdit = null;
      PhraseBeliefEdit deletedPhraseBeliefEdit = null;

      //INITIALIZE TO EMPTY PhraseBelief EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      PhraseBeliefEdit deleteConfirmedPhraseBeliefEdit = new PhraseBeliefEdit();

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //NEW
        newPhraseBeliefEdit = await PhraseBeliefEdit.NewPhraseBeliefEditAsync();

        //EDIT
        newPhraseBeliefEdit.TimeStamp = DateTime.Now - TimeSpan.FromDays(7);
        newPhraseBeliefEdit.Text = "TestPhraseBelief.Text edited in NEW_EDIT_BEGINSAVE_GET_DELETE_GET test";
        newPhraseBeliefEdit.Strength = 3.0d;
        newPhraseBeliefEdit.Phrase = _TestPhraseEdit;
        Assert.AreEqual(SeedData.Ton.TestValidUsername, newPhraseBeliefEdit.Username);

        //SAVE
        savedPhraseBeliefEdit = await newPhraseBeliefEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenPhraseBeliefEdit =
          await PhraseBeliefEdit.GetPhraseBeliefEditAsync(savedPhraseBeliefEdit.Id);

        //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
        gottenPhraseBeliefEdit.Delete();
        deletedPhraseBeliefEdit = await gottenPhraseBeliefEdit.SaveAsync();

        try
        {
          deleteConfirmedPhraseBeliefEdit
            = await PhraseBeliefEdit.GetPhraseBeliefEditAsync(deletedPhraseBeliefEdit.Id);
        }
        catch (Csla.DataPortalException dpex)
        {
          var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
          var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
          //WE EXPECT THE ID TO NOT BE FOUND, OTHERWISE RETHROW THE EXCEPTION
          if (!TestsHelper.IsIdNotFoundException(dpex))
            throw dpex;
        }
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newPhraseBeliefEdit),
                        () => Assert.IsNotNull(savedPhraseBeliefEdit),
                        () => Assert.IsNotNull(gottenPhraseBeliefEdit),
                        () => Assert.IsNotNull(deletedPhraseBeliefEdit),
                        () => Assert.AreEqual(Guid.Empty, deleteConfirmedPhraseBeliefEdit.Id),
                        () => Assert.AreEqual(Guid.Empty, deleteConfirmedPhraseBeliefEdit.PhraseId)
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }
  }
}