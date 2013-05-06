using System;

using LearnLanguages.Business;

using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Linq;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("linelist")]
  public class LineListTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    private LanguageEdit _ServerEnglishLang;
    private LanguageEdit _ServerSpanishLang;
    
    //[TestInitialize]
    //[Asynchronous]
    //public void InitializeLineTests()
    //{

    //  //WE NEED TO UPDATE THE CLIENT SeedData.Ton IDS.  
    //  var isLoaded = false;
    //  var linesCorrected = false;
    //  Exception error = new Exception();
    //  Exception errorLineList = new Exception();
    //  LanguageList allLanguages = null;
    //  LineList allLines = null;

    //  LanguageList.GetAll((s, r) =>
    //  {
    //    #region Initialize Language Data
    //    error = r.Error;
    //    if (error != null)
    //      throw error;

    //    allLanguages = r.Object;
    //    _ServerEnglishLang = (from language in allLanguages
    //                          where language.Text == SeedData.Ton.EnglishText
    //                          select language).First();

    //    SeedData.Ton.EnglishLanguageDto.Id = _ServerEnglishLang.Id;

    //    _ServerSpanishLang = (from language in allLanguages
    //                          where language.Text == SeedData.Ton.SpanishText
    //                          select language).First();

    //    SeedData.Ton.SpanishLanguageDto.Id = _ServerSpanishLang.Id;

    //    #endregion

    //    isLoaded = true;

    //    LineList.GetAll((s2, r2) =>
    //    {
    //      errorLineList = r2.Error;
    //      if (errorLineList != null)
    //        throw errorLineList;

    //      allLines = r2.Object;

    //      var serverHelloLineQuery = (from line in allLines
    //                                    where line.Text == SeedData.Ton.HelloText &&
    //                                          line.Language.Text == SeedData.Ton.EnglishText
    //                                    select line);
    //      LineEdit serverHelloLine = null;
    //      if (serverHelloLineQuery.Count() == 0) //we don't have the hello line in the db, so put it there
    //      {
    //        var line = allLines[0];
    //        line.BeginEdit();
    //        line.Text = SeedData.Ton.HelloText;
    //        line.Language = _ServerEnglishLang;
    //        line.ApplyEdit();
    //        serverHelloLine = line;
    //      }
    //      else
    //        serverHelloLine = serverHelloLineQuery.First();


    //      var serverHolaLineQuery = (from line in allLines
    //                                   where line.Text == SeedData.Ton.HolaText &&
    //                                         line.Language.Text == SeedData.Ton.EnglishText
    //                                   select line);
    //      LineEdit serverHolaLine = null;
    //      if (serverHolaLineQuery.Count() == 0) //we don't have the Hola line in the db, so put it there
    //      {
    //        var line = allLines[1];
    //        line.BeginEdit();
    //        line.Text = SeedData.Ton.HolaText;
    //        line.Language = _ServerSpanishLang;
    //        line.ApplyEdit();
    //        serverHolaLine = line;
    //      }
    //      else
    //        serverHolaLine = serverHolaLineQuery.First();

    //      var serverLongLineQuery = (from line in allLines
    //                                   where line.Text == SeedData.Ton.LongText &&
    //                                         line.Language.Text == SeedData.Ton.EnglishText
    //                                   select line);
    //      LineEdit serverLongLine = null;
    //      if (serverLongLineQuery.Count() == 0) //we don't have the Long line in the db, so put it there
    //      {
    //        var line = allLines[2];
    //        line.BeginEdit();
    //        line.Text = SeedData.Ton.LongText;
    //        line.Language = _ServerEnglishLang;
    //        line.ApplyEdit();
    //        serverLongLine = line;
    //      }
    //      else
    //        serverLongLine = serverLongLineQuery.First();


    //      var serverDogLineQuery = (from line in allLines
    //                                  where line.Text == SeedData.Ton.DogText &&
    //                                        line.Language.Text == SeedData.Ton.EnglishText
    //                                  select line);
    //      LineEdit serverDogLine = null;
    //      if (serverDogLineQuery.Count() == 0) //we don't have the Dog line in the db, so put it there
    //      {
    //        var line = allLines[3];
    //        line.BeginEdit();
    //        line.Text = SeedData.Ton.DogText;
    //        line.Language = _ServerSpanishLang;
    //        line.ApplyEdit();
    //        serverDogLine = line;
    //      }
    //      else
    //        serverDogLine = serverDogLineQuery.First();

    //      var validUserId = serverHelloLine.UserId;
    //      SeedData.Ton.GetTestValidUserDto().Id = validUserId;

    //      SeedData.Ton.HelloLineDto.Id = serverHelloLine.Id;
    //      SeedData.Ton.HolaLineDto.Id = serverHolaLine.Id;
    //      SeedData.Ton.LongLineDto.Id = serverLongLine.Id;
    //      SeedData.Ton.DogLineDto.Id = serverDogLine.Id;

    //      SeedData.Ton.HelloLineDto.UserId = serverHelloLine.UserId;
    //      SeedData.Ton.HolaLineDto.UserId = serverHolaLine.UserId;
    //      SeedData.Ton.LongLineDto.UserId = serverLongLine.UserId;
    //      SeedData.Ton.DogLineDto.UserId = serverDogLine.UserId;

    //      linesCorrected = true;
    //    });
    //  });

    //  EnqueueConditional(() => isLoaded);
    //  EnqueueConditional(() => linesCorrected);
    //  EnqueueCallback(() => { Assert.IsNull(error); },
    //                  () => { Assert.IsNotNull(allLanguages); },
    //                  () => { Assert.AreNotEqual(Guid.Empty, SeedData.Ton.EnglishId); },
    //                  () => { Assert.AreNotEqual(Guid.Empty, SeedData.Ton.SpanishId); },
    //                  () => { Assert.IsTrue(allLanguages.Count > 0); });
    //  EnqueueTestComplete();
    //}
    
    [TestMethod]
    [Asynchronous]
    public void GET_ALL()
    {
      var isLoaded = false;
      Exception error = null;
      LineList allLines = null;
      LineList.GetAll((s, r) =>
      {
        error = r.Error;
        if (error != null)
          throw error;

        allLines = r.Object;
        isLoaded = true;
      });

      EnqueueConditional(() => isLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNotNull(allLines); },
                      () => { Assert.IsTrue(allLines.Count > 0); });
      EnqueueTestComplete();
    }
    
    [TestMethod]
    [Asynchronous]
    public void GET_ALL_EDIT_SAVE()
    {
      var isLoaded = false;
      var isEdited = false;
      var isSaved = false;

      var loadError = new Exception();
      var saveError = new Exception();

      var allLinesCount = -1;
      bool linesCountStaysTheSame = false;
      var text0 = "This is edited Text00000._save";
      var text1 = "This is edited Text11111._save";
      var containsText0 = false;
      var containsText1 = false;

      LineList allLines = null;
      LineList savedLines = null;
      LineList.GetAll((s, r) =>
      {
        loadError = r.Error;
        if (loadError != null)
          throw loadError;

        allLines = r.Object;
        isLoaded = true;
        allLines[0].Phrase.Text = text0;
        allLines[1].Phrase.Text = text1;

        allLinesCount = allLines.Count;

        isEdited = true;
        allLines.BeginSave((s2, r2) =>
          {
            saveError = r2.Error;
            if (saveError != null)
              throw saveError;

            savedLines = (LineList)r2.NewObject;
            linesCountStaysTheSame = allLinesCount == savedLines.Count;

            containsText0 = (from line in savedLines
                             where line.Phrase.Text == text0
                             select line).Count() > 0;
            containsText1 = (from line in savedLines
                             where line.Phrase.Text == text1
                             select line).Count() > 0;
            
            isSaved = true;
          });
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueCallback(
                      () => { Assert.IsNull(loadError); },
                      () => { Assert.IsNull(saveError); },
                      () => { Assert.IsNotNull(allLines); },
                      () => { Assert.IsNotNull(savedLines); },
                      () => { Assert.IsTrue(containsText0); },
                      () => { Assert.IsTrue(containsText1); },
                      () => { Assert.IsTrue(linesCountStaysTheSame); },
                      () => { Assert.IsTrue(allLines.Count > 0); });
      EnqueueTestComplete();

    }

    [TestMethod]
    [Asynchronous]
    public void GET_ALL_EDIT_CANCELEDIT()
    {
      var isLoaded = false;
      var isEdited = false;
      var isCanceled = false;

      var loadError = new Exception();

      var text0 = "This is edited Text00000._canceledit";
      var text1 = "This is edited Text11111._canceledit";
      var containsText0 = false;
      var containsText1 = false;

      LineList allLines = null;
      LineList canceledEditLines = null;
      LineList.GetAll((s, r) =>
      {
        loadError = r.Error;
        if (loadError != null)
          throw loadError;

        allLines = r.Object;
        isLoaded = true;
        //allLines.BeginEdit();
        //allLines.CancelEdit();
        allLines.BeginEdit();
        allLines[0].Phrase.Text = text0;
        allLines[1].Phrase.Text = text1;
        allLines.CancelEdit();

        isEdited = true;
        
        canceledEditLines = allLines;

        containsText0 = (from line in canceledEditLines
                         where line.Phrase.Text == text0
                         select line).Count() > 0;
        containsText1 = (from line in canceledEditLines
                         where line.Phrase.Text == text1
                         select line).Count() > 0;
        isCanceled = true;
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isCanceled);
      EnqueueCallback(
                      () => { Assert.IsNull(loadError); },
                      () => { Assert.IsNotNull(allLines); },
                      () => { Assert.IsNotNull(canceledEditLines); },
                      () => { Assert.IsFalse(containsText0); },
                      () => { Assert.IsFalse(containsText1); },
                      () => { Assert.IsTrue(allLines.Count > 0); });
      EnqueueTestComplete();

    }
  }
}