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
  [Tag("phrase")]
  public class PhraseTests : TestsBase
  {
    //private LanguageEdit _ServerEnglishLang;
    //private LanguageEdit _ServerSpanishLang;

    public override async System.Threading.Tasks.Task Setup()
    {
      await base.Setup();

      //WE NEED TO UPDATE THE CLIENT SeedData.Ton IDS.  
      LanguageList allLanguages = null;
      PhraseList allPhrases = null;

      if (_AllLanguages == null)
        _AllLanguages = await LanguageList.GetAllAsync();
      allLanguages = _AllLanguages;

      _ServerEnglishLang = (from language in _AllLanguages
                            where language.Text == SeedData.Ton.EnglishText
                            select language).First();

      SeedData.Ton.EnglishLanguageDto.Id = _ServerEnglishLang.Id;

      _ServerSpanishLang = (from language in _AllLanguages
                            where language.Text == SeedData.Ton.SpanishText
                            select language).First();

      SeedData.Ton.SpanishLanguageDto.Id = _ServerSpanishLang.Id;


      if (_InitialAllPhrases == null)
        _InitialAllPhrases = await PhraseList.GetAllAsync();
      allPhrases = _InitialAllPhrases;

      var serverHelloPhraseQuery = (from phrase in allPhrases
                                    where phrase.Text == SeedData.Ton.HelloText &&
                                          phrase.Language.Text == SeedData.Ton.EnglishText
                                    select phrase);
      PhraseEdit serverHelloPhrase = null;
      if (serverHelloPhraseQuery.Count() == 0) //we don't have the hello phrase in the db, so put it there
      {
        var phrase = allPhrases[0];
        phrase.BeginEdit();
        phrase.Text = SeedData.Ton.HelloText;
        phrase.Language = _ServerEnglishLang;
        phrase.ApplyEdit();
        serverHelloPhrase = phrase;
      }
      else
        serverHelloPhrase = serverHelloPhraseQuery.First();


      var serverHolaPhraseQuery = (from phrase in allPhrases
                                   where phrase.Text == SeedData.Ton.HolaText &&
                                         phrase.Language.Text == SeedData.Ton.EnglishText
                                   select phrase);
      PhraseEdit serverHolaPhrase = null;
      if (serverHolaPhraseQuery.Count() == 0) //we don't have the Hola phrase in the db, so put it there
      {
        var phrase = allPhrases[1];
        phrase.BeginEdit();
        phrase.Text = SeedData.Ton.HolaText;
        phrase.Language = _ServerSpanishLang;
        phrase.ApplyEdit();
        serverHolaPhrase = phrase;
      }
      else
        serverHolaPhrase = serverHolaPhraseQuery.First();

      var serverLongPhraseQuery = (from phrase in allPhrases
                                   where phrase.Text == SeedData.Ton.LongText &&
                                         phrase.Language.Text == SeedData.Ton.EnglishText
                                   select phrase);
      PhraseEdit serverLongPhrase = null;
      if (serverLongPhraseQuery.Count() == 0) //we don't have the Long phrase in the db, so put it there
      {
        var phrase = allPhrases[2];
        phrase.BeginEdit();
        phrase.Text = SeedData.Ton.LongText;
        phrase.Language = _ServerEnglishLang;
        phrase.ApplyEdit();
        serverLongPhrase = phrase;
      }
      else
        serverLongPhrase = serverLongPhraseQuery.First();

      var serverDogPhraseQuery = (from phrase in allPhrases
                                  where phrase.Text == SeedData.Ton.DogText &&
                                        phrase.Language.Text == SeedData.Ton.EnglishText
                                  select phrase);
      PhraseEdit serverDogPhrase = null;
      if (serverDogPhraseQuery.Count() == 0) //we don't have the Dog phrase in the db, so put it there
      {
        var phrase = allPhrases[3];
        phrase.BeginEdit();
        phrase.Text = SeedData.Ton.DogText;
        phrase.Language = _ServerSpanishLang;
        phrase.ApplyEdit();
        serverDogPhrase = phrase;
      }
      else
        serverDogPhrase = serverDogPhraseQuery.First();

      var validUserId = serverHelloPhrase.UserId;
      SeedData.Ton.GetTestValidUserDto().Id = validUserId;

      SeedData.Ton.HelloPhraseDto.Id = serverHelloPhrase.Id;
      SeedData.Ton.HolaPhraseDto.Id = serverHolaPhrase.Id;
      SeedData.Ton.LongPhraseDto.Id = serverLongPhrase.Id;
      SeedData.Ton.DogPhraseDto.Id = serverDogPhrase.Id;

      SeedData.Ton.HelloPhraseDto.UserId = serverHelloPhrase.UserId;
      SeedData.Ton.HolaPhraseDto.UserId = serverHolaPhrase.UserId;
      SeedData.Ton.LongPhraseDto.UserId = serverLongPhrase.UserId;
      SeedData.Ton.DogPhraseDto.UserId = serverDogPhrase.UserId;

      Assert.AreNotEqual(Guid.Empty, SeedData.Ton.EnglishId);
      Assert.AreNotEqual(Guid.Empty, SeedData.Ton.SpanishId);
      Assert.IsTrue(allLanguages.Count > 0);
    }

    [TestMethod]
    [Asynchronous]
    public async Task CREATE_NEW()
    {
      PhraseEdit newPhraseEdit = null;
      
      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        newPhraseEdit = await PhraseEdit.NewPhraseEditAsync();
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newPhraseEdit)//,
                        //() => Assert.AreEqual(SeedData.Ton.EnglishId, newPhraseEdit.LanguageId),
                        //() => Assert.IsNotNull(newPhraseEdit.Language)
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
      PhraseEdit gottenPhraseEdit = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        var allPhrases = await PhraseList.GetAllAsync();
        testId = allPhrases.First().Id;
        gottenPhraseEdit = await PhraseEdit.GetPhraseEditAsync(testId);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(gottenPhraseEdit),
                        () => Assert.AreEqual(testId, gottenPhraseEdit.Id)
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
      PhraseEdit newPhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //NEW
        newPhraseEdit = await PhraseEdit.NewPhraseEditAsync();

        //EDIT
        newPhraseEdit.Text = "TestPhrase NEW_EDIT_BEGINSAVE_GET";
        newPhraseEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Ton.TestValidUsername;
        newPhraseEdit.LanguageId = await LanguageEdit.GetDefaultLanguageIdAsync();

        //SAVE
        savedPhraseEdit = await newPhraseEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenPhraseEdit = await PhraseEdit.GetPhraseEditAsync(savedPhraseEdit.Id);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newPhraseEdit),
                        () => Assert.IsNotNull(savedPhraseEdit),
                        () => Assert.IsNotNull(gottenPhraseEdit),
                        () => Assert.AreEqual(savedPhraseEdit.Id, gottenPhraseEdit.Id),
                        () => Assert.AreEqual(savedPhraseEdit.Text, gottenPhraseEdit.Text)
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
      PhraseEdit newPhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;
      PhraseEdit deletedPhraseEdit = null;

      //INITIALIZE TO EMPTY Phrase EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      PhraseEdit deleteConfirmedPhraseEdit = new PhraseEdit();

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //NEW
        newPhraseEdit = await PhraseEdit.NewPhraseEditAsync();

        //EDIT
        newPhraseEdit.Text = "TestPhrase NEW_EDIT_BEGINSAVE_GET_DELETE_GET";
        newPhraseEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Ton.TestValidUsername;
        newPhraseEdit.LanguageId = await LanguageEdit.GetDefaultLanguageIdAsync();

        //SAVE
        savedPhraseEdit = await newPhraseEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenPhraseEdit = await PhraseEdit.GetPhraseEditAsync(savedPhraseEdit.Id);

        //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
        gottenPhraseEdit.Delete();
        deletedPhraseEdit = await gottenPhraseEdit.SaveAsync();

        try
        {
          deleteConfirmedPhraseEdit = await PhraseEdit.GetPhraseEditAsync(deletedPhraseEdit.Id);
        }
        catch (Csla.DataPortalException dpex)
        {
          var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
          var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
          //WE EXPECT AN ID NOT FOUND EXCEPTION. IF SOMETHING ELSE, RETHROW IT.
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
                        () => Assert.IsNotNull(newPhraseEdit),
                        () => Assert.IsNotNull(savedPhraseEdit),
                        () => Assert.IsNotNull(gottenPhraseEdit),
                        () => Assert.IsNotNull(deletedPhraseEdit),
                        () => Assert.AreEqual(Guid.Empty, deleteConfirmedPhraseEdit.Id),
                        () => Assert.AreEqual(Guid.Empty, deleteConfirmedPhraseEdit.LanguageId),
                        () => Assert.AreEqual(Guid.Empty, deleteConfirmedPhraseEdit.UserId),
                        () => Assert.AreEqual(string.Empty, deleteConfirmedPhraseEdit.Username)
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task MAKE_PHRASE_WITH_REALLY_LONG_VARIED_TEXT()
    {
      #region var reallyLongText
      var reallyLongText = @"asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`";
      #endregion

      PhraseEdit newPhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;


      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //NEW
        newPhraseEdit = await PhraseEdit.NewPhraseEditAsync();

        //EDIT
        newPhraseEdit.Text = reallyLongText;
        newPhraseEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Ton.TestValidUsername;
        newPhraseEdit.LanguageId = await LanguageEdit.GetDefaultLanguageIdAsync();

        //SAVE
        savedPhraseEdit = await newPhraseEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenPhraseEdit = await PhraseEdit.GetPhraseEditAsync(savedPhraseEdit.Id);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newPhraseEdit),
                        () => Assert.IsNotNull(savedPhraseEdit),
                        () => Assert.IsNotNull(gottenPhraseEdit),
                        () => Assert.AreEqual(savedPhraseEdit.Id, gottenPhraseEdit.Id),
                        () => Assert.AreEqual(savedPhraseEdit.Text, gottenPhraseEdit.Text)
                        );
        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task MAKE_PHRASE_WITH_REALLY_REALLY_LONG_NUMERICAL_TEXT()
    {
      var maxLength = 300000;//worked
      maxLength = 100000;//worked
      maxLength = 30000; //just for a big number, to lessen time to do test.
      var i = 0;
      string reallyReallyLongText = "";
      while (reallyReallyLongText.Length < maxLength)
      {
        reallyReallyLongText += i.ToString();
        i++;
        if (i == maxLength) //shouldn't happen, but no big deal if it does. It's just a reallyraeallyreallyreally long text.
          break;
      }
      if (reallyReallyLongText.Length > maxLength)
        reallyReallyLongText = reallyReallyLongText.Substring(0, maxLength);

      PhraseEdit newPhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {

        //NEW
        newPhraseEdit = await PhraseEdit.NewPhraseEditAsync();

        //EDIT
        newPhraseEdit.Text = reallyReallyLongText;
        newPhraseEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Ton.TestValidUsername;
        newPhraseEdit.LanguageId = await LanguageEdit.GetDefaultLanguageIdAsync();

        //SAVE
        savedPhraseEdit = await newPhraseEdit.SaveAsync();

        //GET (CONFIRM SAVE)
        gottenPhraseEdit = await PhraseEdit.GetPhraseEditAsync(savedPhraseEdit.Id);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNotNull(newPhraseEdit),
                        () => Assert.IsNotNull(savedPhraseEdit),
                        () => Assert.IsNotNull(gottenPhraseEdit),
                        () => Assert.AreEqual(savedPhraseEdit.Id, gottenPhraseEdit.Id),
                        () => Assert.AreEqual(reallyReallyLongText.Length, gottenPhraseEdit.Text.Length),
                        () => Assert.AreEqual(savedPhraseEdit.Text, gottenPhraseEdit.Text)
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task GET_ALL_PHRASES_THAT_CONTAIN_THE_LETTER_H_IN_ALL_LANGUAGES()
    {
      Guid testId = Guid.Empty;
      PhraseEdit testPhrase = null;
      
      //THE SEARCH TEXT WE ARE LOOKING FOR
      string textToLookFor = "h";
      
      //ASSUME TRUE
      bool allRetrievedPhrasesContainTheCorrectText = true;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        var phrases = await PhraseList.GetAllContainingTextAsyncCaseInsensitive(textToLookFor);
        testId = phrases.First().Id;

        //CHECK TO SEE IF ALL THE PHRASES CONTAIN THE TEXT TO LOOK FOR, LIKE THEY ARE SUPPOSED TO
        for (int i = 0; i < phrases.Count; i++)
        {
          var phrase = phrases[i];

          //LOWERCASE THE TEXTS FOR THE CHECK, BECAUSE THE RESULTS ARE CASE-INSENSITIVE,
          //BUT THE STRING.CONTAINS(TEXT) METHOD IS CASE-SENSITIVE
          var lowercasePhraseText = phrase.Text.ToLower();
          var lowercaseTextToLookFor = textToLookFor.ToLower();

          if (!lowercasePhraseText.Contains(lowercaseTextToLookFor))
            //THIS SHOULDN'T HAPPEN
            allRetrievedPhrasesContainTheCorrectText = false;
        }

        //JUST TO CHECK TO SEE THAT OUR RESULTS WERE AT LEAST ONE, AND THAT IT'S A VALID PHRASE
        testPhrase = await PhraseEdit.GetPhraseEditAsync(testId);
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsTrue(allRetrievedPhrasesContainTheCorrectText),
                        () => Assert.IsNotNull(testPhrase),
                        () => Assert.AreEqual(testId, testPhrase.Id)
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }

  }
}