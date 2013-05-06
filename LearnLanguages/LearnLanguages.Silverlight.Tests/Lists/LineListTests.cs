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
  [Tag("linelist")]
  public class LineListTests : TestsBase
  {
    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL()
    {
      LineList allLines = null;
      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        allLines = await LineList.GetAllAsync();
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(allLines),
                        () => Assert.IsTrue(allLines.Count > 0),
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
      var allLinesCount = -1;
      bool linesCountStaysTheSame = false;
      var text0 = "This is edited Text00000._save";
      var text1 = "This is edited Text11111._save";
      var containsText0 = false;
      var containsText1 = false;

      LineList allLines = null;
      LineList savedLines = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        allLines = await LineList.GetAllAsync();
        allLines[0].Phrase.Text = text0;
        allLines[1].Phrase.Text = text1;

        allLinesCount = allLines.Count;

        savedLines = await allLines.SaveAsync();
        linesCountStaysTheSame = allLinesCount == savedLines.Count;

        containsText0 = (from line in savedLines
                         where line.Phrase.Text == text0
                         select line).Count() > 0;
        containsText1 = (from line in savedLines
                         where line.Phrase.Text == text1
                         select line).Count() > 0;
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(allLines),
                        () => Assert.IsNotNull(savedLines),
                        () => Assert.IsTrue(containsText0),
                        () => Assert.IsTrue(containsText1),
                        () => Assert.IsTrue(linesCountStaysTheSame),
                        () => Assert.IsTrue(allLinesCount > 0),

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
      var containsEditedText0 = false;
      var containsEditedText1 = false;

      LineList allLines = null;
      LineList canceledEditLines = null;

var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
              allLines = await LineList.GetAllAsync();
      
      allLines.BeginEdit();
         allLines[0].Phrase.Text = text0;
         allLines[1].Phrase.Text = text1;
         allLines.CancelEdit();

         canceledEditLines = allLines;

         containsEditedText0 = (from line in canceledEditLines
                          where line.Phrase.Text == text0
                          select line).Count() > 0;
         containsEditedText1 = (from line in canceledEditLines
                          where line.Phrase.Text == text1
                          select line).Count() > 0;
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(allLines),
                        () => Assert.IsNotNull(canceledEditLines),
                        () => Assert.IsFalse(containsEditedText0),
                        () => Assert.IsFalse(containsEditedText1),
                        () => Assert.IsTrue(allLines.Count > 0),
                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }
  }
}