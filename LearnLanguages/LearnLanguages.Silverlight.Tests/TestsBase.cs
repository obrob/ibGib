using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using LearnLanguages.Business.Security;
using System.Threading.Tasks;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Linq;

namespace LearnLanguages.Silverlight.Tests
{
  /// <summary>
  /// Base class that has async setup & sync teardown methods that log in 
  /// and log out respectively.
  /// </summary>
  public class TestsBase : SilverlightTest
  {
    public TestsBase()
    {
      ClientSeedDataIsInitialized = false;
    }

    protected LanguageEdit _ServerEnglishLang;
    protected LanguageEdit _ServerSpanishLang;
    
    protected static Guid _EnglishId { get; set; }
    protected static Guid _SpanishId { get; set; }

    protected static LanguageList _AllLanguages { get; set; }
    protected static PhraseList _InitialAllPhrases { get; set; }
    
    public virtual async Task Setup()
    {
      //LOGIN
      await UserPrincipal.LoginAsync("user", "password");

      //SET OUR LANGUAGES
      _ServerEnglishLang = await LanguageEdit.GetLanguageEditAsync("English");
      _ServerSpanishLang = await LanguageEdit.GetLanguageEditAsync("Spanish");

      if (_AllLanguages == null)
        _AllLanguages = await LanguageList.GetAllAsync();

      _EnglishId = (from language in _AllLanguages
                    where language.Text == SeedData.Ton.EnglishText
                    select language.Id).First();

      _SpanishId = (from language in _AllLanguages
                    where language.Text == SeedData.Ton.SpanishText
                    select language.Id).First();


      await InitializeClientSeedDataSingleton();

    }
    public virtual void Teardown()
    {
      UserPrincipal.Logout();
    }

    //TODO: SEE IF I CAN MAKE A BASECLASS TEST EXECUTOR METHOD THAT TAKES
    //A POINTER AND A PARAMS ()=> ACTION ARGUMENT. THIS WAY, I CAN JUST WRITE THE 
    //TRY-FINALLY SETUP TEARDOWN STUFF IN ONE PLACE.

    protected bool ClientSeedDataIsInitialized { get; set; }

    public virtual async Task InitializeClientSeedDataSingleton()
    {
      if (ClientSeedDataIsInitialized)
        return;

      //UPDATE THE CLIENT SEEDDATA LANGUAGE IDS
      SeedData.Ton.EnglishLanguageDto.Id = _ServerEnglishLang.Id;
      SeedData.Ton.SpanishLanguageDto.Id = _ServerSpanishLang.Id;

      //UPDATE THE CLIENT SEEDDATA PHRASES

      if (_InitialAllPhrases == null)
        _InitialAllPhrases = await PhraseList.GetAllAsync();
      PhraseList allPhrases = null;
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

      ClientSeedDataIsInitialized = true;
    }
    
  }
}
