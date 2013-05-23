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
  [Tag("line")]
  public class LineTests : TestsBase
  {
    [TestMethod]
    [Asynchronous]
    public async Task CREATE_NEW()
    {
      LineEdit newLineEdit = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        newLineEdit = await LineEdit.NewLineEditAsync();
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newLineEdit)
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }
    
    [TestMethod]
    [Asynchronous]
    public async Task GET()
    {
      Guid testId = Guid.Empty;
      LineEdit lineEdit = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        var results = await LineList.GetAllAsync();
        testId = results.First().Id;
        lineEdit = await LineEdit.GetLineEditAsync(testId);  
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(lineEdit),
                        () => Assert.AreEqual(testId, lineEdit.Id)
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
      //INITIALIZE ERRORS TO EXCEPTION, BECAUSE WE TEST IF THEY ARE NULL LATER
      LineEdit newLineEdit = null;
      LineEdit savedLineEdit = null;
      LineEdit gottenLineEdit = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //NEW
        newLineEdit = await LineEdit.NewLineEditAsync();

        //EDIT
        newLineEdit.Phrase.Text = "TestLine.PhraseText NEW_EDIT_BEGINSAVE_GET";
        newLineEdit.Phrase.Language = _ServerEnglishLang;
        newLineEdit.LineNumber = 0;
        //Assert.AreEqual(SeedData.Ton.TestValidUsername, newLineEdit.Username);
        //newLineEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        //newLineEdit.Username = SeedData.Ton.TestValidUsername;

        //SAVE
        savedLineEdit = await newLineEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenLineEdit = await LineEdit.GetLineEditAsync(savedLineEdit.Id);

      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newLineEdit),
                        () => Assert.IsNotNull(savedLineEdit),
                        () => Assert.IsNotNull(gottenLineEdit),
                        () => Assert.AreEqual(savedLineEdit.Id, gottenLineEdit.Id)
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
      LineEdit newLineEdit = null;
      LineEdit savedLineEdit = null;
      LineEdit gottenLineEdit = null;
      LineEdit deletedLineEdit = null;

      //INITIALIZE TO EMPTY Line EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      LineEdit confirmDeleteLineEdit = new LineEdit();

      var deleteIsConfirmed = false;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //NEW
        newLineEdit = await LineEdit.NewLineEditAsync();

        //EDIT
        newLineEdit.Phrase.Text = "TestLine.PhraseText NEW_EDIT_BEGINSAVE_GET";
        newLineEdit.Phrase.Language = _ServerEnglishLang;
        newLineEdit.LineNumber = 0;
        Assert.AreEqual(SeedData.Ton.TestValidUsername, newLineEdit.Username);

        //SAVE
        savedLineEdit = await newLineEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenLineEdit = await LineEdit.GetLineEditAsync(savedLineEdit.Id);

        //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
        gottenLineEdit.Delete();
        deletedLineEdit = await gottenLineEdit.SaveAsync();

        try
        {
          confirmDeleteLineEdit = await LineEdit.GetLineEditAsync(deletedLineEdit.Id);
          var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
          var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;

        }
        catch (Csla.DataPortalException dpex)
        {
          if (TestsHelper.IsIdNotFoundException(dpex))
            deleteIsConfirmed = true;
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
                        () => Assert.IsNotNull(newLineEdit),
                        () => Assert.IsNotNull(savedLineEdit),
                        () => Assert.IsNotNull(gottenLineEdit),
                        () => Assert.IsNotNull(deletedLineEdit),
                        () => Assert.AreEqual(Guid.Empty, confirmDeleteLineEdit.Id),
                        () => Assert.AreEqual(Guid.Empty, confirmDeleteLineEdit.UserId),
                        () => Assert.IsNull(confirmDeleteLineEdit.Phrase),
                        () => Assert.IsNull(confirmDeleteLineEdit.User),
                        () => Assert.IsTrue(deleteIsConfirmed)
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }
    
  }
}