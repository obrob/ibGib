using System;

using LearnLanguages.Business;

using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Linq;
using Csla;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using System.Threading.Tasks;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("multilinetext")]
  [Tag("mlt")]
  public class MultiLineTextTests : TestsBase
  {

    [TestMethod]
    [Asynchronous]
    public async Task CREATE_NEW()
    {
      //var isCreated = false;
      MultiLineTextEdit newMultiLineTextEdit = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        newMultiLineTextEdit = await MultiLineTextEdit.NewMultiLineTextEditAsync();
        Assert.IsTrue(newMultiLineTextEdit.User.IsAuthenticated);
        Assert.AreEqual(Csla.ApplicationContext.User.Identity.Name,
          newMultiLineTextEdit.Username);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newMultiLineTextEdit)
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
      MultiLineTextEdit multiLineTextEdit = null;
      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        var allMultiLineTexts = await MultiLineTextList.GetAllAsync();

        testId = allMultiLineTexts.First().Id;
        multiLineTextEdit = await MultiLineTextEdit.GetMultiLineTextEditAsync(testId);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(multiLineTextEdit),
                        () => Assert.IsTrue(multiLineTextEdit.Lines.Count >= 2),
                        () => Assert.AreEqual(testId, multiLineTextEdit.Id)
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
      MultiLineTextEdit newMultiLineTextEdit = null;
      MultiLineTextEdit savedMultiLineTextEdit = null;
      MultiLineTextEdit gottenMultiLineTextEdit = null;

      string mltTitle = "My Test MLT Title Here";
      string lineAText = "MultiLineTextTests.neweditbeginsaveget.Test Line A Text.  This is line A in English.";
      string lineBText = "MultiLineTextTests.neweditbeginsaveget.Test Line BBBB Text.  This is line B in English.";
      //LineEdit lineA = null;
      //LineEdit lineB = null;

      bool gottenMultiLineTextLinesCountIsTwo = false;
      bool gottenMultiLineTextContainsLineA = false;
      bool gottenMultiLineTextContainsLineB = false;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //NEW UP THE MULTILINETEXT
        newMultiLineTextEdit = await MultiLineTextEdit.NewMultiLineTextEditAsync();

        //EDIT

        //TITLE MLT
        newMultiLineTextEdit.Title = mltTitle;

        //CREATE LINES IN A LINELIST

        //1) CREATE LINE INFO DICTIONARY
        var lineInfoDictionary = new Dictionary<int, string>();
        lineInfoDictionary.Add(0, lineAText);
        lineInfoDictionary.Add(1, lineBText);

        //2) LANGUAGE TEXT
        var linesLanguageText = SeedData.Ton.EnglishText;

        //3) CRITERIA
        var criteria = new Business.Criteria.LineInfosCriteria(linesLanguageText, lineInfoDictionary);

        //4) CREATE LINES
        var lineList = await LineList.NewLineListAsync(criteria);

        //5) ASSIGN LINES
        newMultiLineTextEdit.Lines = lineList;

        savedMultiLineTextEdit = await newMultiLineTextEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenMultiLineTextEdit = 
          await MultiLineTextEdit.GetMultiLineTextEditAsync(savedMultiLineTextEdit.Id);
        gottenMultiLineTextLinesCountIsTwo = (gottenMultiLineTextEdit.Lines.Count == 2);

        gottenMultiLineTextContainsLineA = (from line in gottenMultiLineTextEdit.Lines
                                            where line.LineNumber == 0
                                            select line).First().Phrase.Text == lineAText;
        gottenMultiLineTextContainsLineB = (from line in gottenMultiLineTextEdit.Lines
                                            where line.LineNumber == 1
                                            select line).First().Phrase.Text == lineBText;
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsTrue(gottenMultiLineTextLinesCountIsTwo),
                        () => Assert.IsTrue(gottenMultiLineTextContainsLineA),
                        () => Assert.IsTrue(gottenMultiLineTextContainsLineB),
                        () => Assert.IsNotNull(newMultiLineTextEdit),
                        () => Assert.IsNotNull(savedMultiLineTextEdit),
                        () => Assert.IsNotNull(gottenMultiLineTextEdit),
                        () => Assert.AreEqual(savedMultiLineTextEdit.Id, gottenMultiLineTextEdit.Id)
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }

    //[TestMethod]
    //[Asynchronous]
    //[ExpectedException(typeof(ExpectedException))]
    //public async TaskNEW_EDIT_BEGINSAVE_GET_DELETE_GET()
    //{
    //  MultiLineTextEdit newMultiLineTextEdit = null;
    //  MultiLineTextEdit savedMultiLineTextEdit = null;
    //  MultiLineTextEdit gottenMultiLineTextEdit = null;
    //  MultiLineTextEdit deletedMultiLineTextEdit = null;

    //  LineEdit lineA = null;
    //  LineEdit lineB = null;

    //  //INITIALIZE TO EMPTY TRANSLATIONEDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
    //  MultiLineTextEdit deleteConfirmedMultiLineTextEdit = new MultiLineTextEdit();

    //  var isNewed = false;
    //  var isEdited = false;
    //  var isSaved = false;
    //  var isGotten = false;
    //  var isDeleted = false;
    //  var isDeleteConfirmed = false;

    //  //NEW
    //  MultiLineTextEdit.NewMultiLineTextEdit((sNew, rNew) =>
    //  {
    //    if (rNew.Error != null)
    //      throw rNew.Error;
    //    newMultiLineTextEdit = rNew.Object;
    //    isNewed = true;

    //    //EDIT
    //    newMultiLineTextEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
    //    newMultiLineTextEdit.Username = SeedData.Ton.TestValidUsername;
    //    LineEdit.NewLineEdit((sNewLineA, rNewLineA) =>
    //    {
    //      if (rNewLineA.Error != null)
    //        throw rNewLineA.Error;

    //      lineA = rNewLineA.Object;
    //      lineA.Text = "Test Line A Text.  This is line A in English.";
    //      lineA.LanguageId = SeedData.Ton.EnglishId;
    //      newMultiLineTextEdit.AddLine(lineA);

    //      LineEdit.NewLineEdit((sNewLineB, rNewLineB) =>
    //      {
    //        if (rNewLineB.Error != null)
    //          throw rNewLineB.Error;

    //        lineB = rNewLineB.Object;
    //        lineB.Text = "Test Line B Text.  This is line B in Spanish.";
    //        lineB.LanguageId = SeedData.Ton.SpanishId;
    //        newMultiLineTextEdit.AddLine(lineB);

    //        isEdited = true;

    //        //SAVE
    //        newMultiLineTextEdit.BeginSave((sSave, rSave) =>
    //        {
    //          if (rSave.Error!= null)
    //            throw rSave.Error;
    //          savedMultiLineTextEdit = (MultiLineTextEdit)rSave.NewObject;

    //          isSaved = true;

    //          //GET (CONFIRM SAVE)
    //          MultiLineTextEdit.GetMultiLineTextEdit(savedMultiLineTextEdit.Id, (sGet, rGet) =>
    //          {
    //            if (rGet.Error!= null)
    //              throw rGet.Error;

    //            gottenMultiLineTextEdit = rGet.Object;
    //            isGotten = true;

    //            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
    //            gottenMultiLineTextEdit.Delete();
    //            gottenMultiLineTextEdit.BeginSave((sSaveGotten, rSaveGotten) =>
    //            {
    //              if (rSaveGotten.Error != null)
    //                throw rSaveGotten.Error;

    //              deletedMultiLineTextEdit = (MultiLineTextEdit)rSaveGotten.NewObject;
    //              //TODO: Figure out why MultiLineTextEditTests final callback gets thrown twice.  When server throws an exception (expected because we are trying to fetch a deleted multiLineText that shouldn't exist), the callback is executed twice.

    //              MultiLineTextEdit.GetMultiLineTextEdit(deletedMultiLineTextEdit.Id, (sGetDeleted, rGetDeleted) =>
    //              {
    //                var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
    //                var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
    //                if (rGetDeleted.Error != null)
    //                {
    //                  isDeleteConfirmed = true;
    //                  throw new ExpectedException(rGetDeleted.Error);
    //                }
    //                deleteConfirmedMultiLineTextEdit = rGetDeleted.Object;
    //              });
    //            });
    //          });
    //        });
    //      });
    //    });
    //  });

    //  EnqueueConditional(() => isNewed);
    //  EnqueueConditional(() => isEdited);
    //  EnqueueConditional(() => isSaved);
    //  EnqueueConditional(() => isGotten);
    //  EnqueueConditional(() => isDeleted);
    //  EnqueueConditional(() => isDeleteConfirmed);
    //  EnqueueCallback(
    //                  () => { Assert.IsNotNull(newMultiLineTextEdit); },
    //                  () => { Assert.IsNotNull(savedMultiLineTextEdit); },
    //                  () => { Assert.IsNotNull(gottenMultiLineTextEdit); },
    //                  () => { Assert.IsNotNull(deletedMultiLineTextEdit); },
    //                  () => { Assert.IsNull(deleteConfirmedMultiLineTextEdit); });

    //  EnqueueTestComplete();
    //}

  }
}