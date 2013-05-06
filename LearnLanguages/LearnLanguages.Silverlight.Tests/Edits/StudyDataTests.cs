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
  [Tag("studydata")]
  public class StudyDataTests : TestsBase
  {

    [TestMethod]
    [Asynchronous]
    public async Task GET_VIA_RETRIEVER()
    {
      StudyDataEdit studyDataEdit = null;
      StudyDataRetriever retriever = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        retriever = await StudyDataRetriever.CreateNewAsync();
        studyDataEdit = retriever.StudyData;
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(retriever),
                        () => Assert.IsNotNull(studyDataEdit),

                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task NEW_EDIT_BEGINSAVE_GET()
    {

      StudyDataEdit newStudyDataEdit = null;
      StudyDataEdit savedStudyDataEdit = null;
      StudyDataEdit gottenStudyDataEdit = null;

      var studyDataAlreadyExisted = false;
      var editedStudyDataText = "Spanish";
      StudyDataRetriever retriever = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //NEW
        retriever = await StudyDataRetriever.CreateNewAsync();
        newStudyDataEdit = retriever.StudyData;

        //EDIT
        newStudyDataEdit.NativeLanguageText = editedStudyDataText;
        //newStudyDataEdit.Username = SeedData.Ton.TestValidUsername;

        //SAVE
        savedStudyDataEdit = await newStudyDataEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        retriever = await StudyDataRetriever.CreateNewAsync();
        gottenStudyDataEdit = retriever.StudyData;

        //STUDY DATA SHOULD HAVE ALREADY EXISTED AS WE JUST SAVED IT.
        studyDataAlreadyExisted = retriever.StudyDataAlreadyExisted;

        EnqueueTestComplete();
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newStudyDataEdit),
                        () => Assert.IsNotNull(savedStudyDataEdit),
                        () => Assert.IsNotNull(gottenStudyDataEdit),
                        () => Assert.AreEqual(savedStudyDataEdit.Id, gottenStudyDataEdit.Id),
                        () => Assert.AreEqual(editedStudyDataText, gottenStudyDataEdit.NativeLanguageText),
                        () => Assert.IsTrue(studyDataAlreadyExisted),
                        () => Assert.AreEqual(savedStudyDataEdit.NativeLanguageText,
                                              gottenStudyDataEdit.NativeLanguageText),
                        //KEEP THIS LAST IN THE CALLBACKS
                        () => Teardown()
                        );

        EnqueueTestComplete();
        isAsyncComplete = true;
      }
    }


  }
}