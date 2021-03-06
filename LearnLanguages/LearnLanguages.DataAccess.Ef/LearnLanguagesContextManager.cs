﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Csla.Data;
using LearnLanguages.DataAccess.Exceptions;
using System.Data.Objects;

namespace LearnLanguages.DataAccess.Ef
{
  public sealed class LearnLanguagesContextManager
  {
    private LearnLanguagesContextManager()
    {
      Initialize();
    }

    #region Singleton Pattern Members
    private static object _InstanceLock = new object();
    private static volatile LearnLanguagesContextManager _Instance;
    public static LearnLanguagesContextManager Instance
    {
      get
      {
        if (_Instance == null)
        {
          lock (_InstanceLock)
          {
            if (_Instance == null)
            {
              _Instance = new LearnLanguagesContextManager();
            }
          }
        }

        return _Instance;
      }
    }
    #endregion

    private void Initialize()
    {
#if DEBUG
      using (LearnLanguagesContext context = new LearnLanguagesContext())
      {
        if (bool.Parse(EfResources.ShouldSeedDbWithData))
        {
          try
          {
            context.Connection.Open();
            DeleteDatabaseData(context);
            SeedContext(context);
            context.SaveChanges();
          }
          catch (Exception ex)
          {
            throw new DatabaseException(ex);
          }
        }
      }
#endif
    }

    private void DeleteDatabaseData(LearnLanguagesContext context)
    {
      //DELETE...Each call does a context.SaveChanges() at the end.
      context.StudyDataDatas.DeleteAll();
      //DeleteMultiLineTextsAndLines(context);
      context.MultiLineTextDatas.DeleteAll();
      context.LineDatas.DeleteAll();
      //DeletePhrasesAndTranslations(context);
      context.TranslationDatas.DeleteAll();
      context.PhraseDatas.DeleteAll();
      context.LanguageDatas.DeleteAll();
      //DeleteUsersAndRoles(context);
      context.UserDatas.DeleteAll();
      context.RoleDatas.DeleteAll();//true = only SaveChanges at the very end.
    }

    private void DeleteMultiLineTextsAndLines(LearnLanguagesContext context)
    {
      var mltsToDelete = context.MultiLineTextDatas.ToList();
      for (int i = 0; i < mltsToDelete.Count; i++)
      {
        var mltToDelete = mltsToDelete[i];
        mltToDelete.LineDatas.Clear();
        context.MultiLineTextDatas.DeleteObject(mltToDelete);
      }
      context.SaveChanges();
    }

    private void DeleteUsersAndRoles(LearnLanguagesContext context)
    {
      var usersToDelete = context.UserDatas.ToList();
      for (int i = 0; i < usersToDelete.Count; i++)
      {
        var userToDelete = usersToDelete[i];
        userToDelete.RoleDatas.Clear();
        //var rolesToDeleteThisIteration = userToDelete.RoleDatas.ToList();
        //foreach (var roleToDelete in rolesToDeleteThisIteration)
        //  context.RoleDatas.DeleteObject(roleToDelete);
        context.UserDatas.DeleteObject(userToDelete);
      }
      context.SaveChanges();
    }

    private void DeletePhrasesAndTranslations(LearnLanguagesContext context)
    {
      //I want to delete each phrase, and delete each translation related to
      //that phrase. Then save the changes to the context.
      var phrasesToDelete = context.PhraseDatas.ToList();
      for (int i = 0; i < phrasesToDelete.Count; i++)
      {
        var phraseToDelete = phrasesToDelete[i];
        var translationsToDeleteThisIteration = phraseToDelete.TranslationDatas.ToList();
        foreach (var translationToDelete in translationsToDeleteThisIteration)
        {
          context.TranslationDatas.DeleteObject(translationToDelete);
        }
        context.PhraseDatas.DeleteObject(phraseToDelete);
      }
      context.SaveChanges();
    }
                                             

    private void SeedContext(LearnLanguagesContext context)
    {
      try
      {
        SeedRoles(context);
        SeedUsers(context);
        SeedLanguages(context);
        SeedPhrases(context);
        SeedTranslations(context);
        SeedLines(context);
        SeedMultiLineTexts(context);
        SeedStudyDatas(context);
      }
      catch (Exception ex)
      {
        var msg = ex.Message;
        throw;
      }
    }

    private static void SeedRoles(LearnLanguagesContext context)
    {
      //ROLES 
      foreach (var roleDto in SeedData.Ton.Roles)
      {
        var roleData = EfHelper.ToData(roleDto);
        context.RoleDatas.AddObject(roleData);
        context.SaveChanges();

        //UPDATE SEED DATA THAT REFERENCES THE ID OF THIS DATA
        var affectedUsers = (from userDto in SeedData.Ton.Users
                             where userDto.RoleIds.Contains(roleDto.Id)
                             select userDto).ToList();

        foreach (var affectedUser in affectedUsers)
        {
          affectedUser.RoleIds.Remove(roleDto.Id);
          affectedUser.RoleIds.Add(roleData.Id);
        }

        roleDto.Id = roleData.Id;
      }
    }

    private static void SeedUsers(LearnLanguagesContext context)
    {
      //USERS
      foreach (var userDto in SeedData.Ton.Users)
      {
        //var userData = EfHelper.ToData(userDto, false);
        var userData = context.UserDatas.CreateObject();
        userData.Id = userDto.Id;
        userData.Username = userDto.Username;
        userData.Salt = userDto.Salt;
        userData.SaltedHashedPasswordValue = userDto.SaltedHashedPasswordValue;

        //manually add roles (cannot use dal.getroles because we are seeding 
        //the data and initializing the context that would use)
        foreach (var roleId in userDto.RoleIds)
        {
          var userRoleData = (from roleData in context.RoleDatas
                              where roleData.Id == roleId
                              select roleData).First();
          if (userRoleData == null)
            throw new Exceptions.SeedDataException();
          userData.RoleDatas.Add(userRoleData);
        }

        context.UserDatas.AddObject(userData);
        context.SaveChanges();

        #region UPDATE AFFECTED SEED DATA THAT REFERENCES THE ID OF THIS USER

        var affectedLanguages = (from languageDto in SeedData.Ton.Languages
                                 where languageDto.UserId == userDto.Id
                                 select languageDto);

        foreach (var affectedLanguage in affectedLanguages)
        {
          affectedLanguage.UserId = userData.Id;
        }

        var affectedPhrases = (from phraseDto in SeedData.Ton.Phrases
                               where phraseDto.UserId == userDto.Id
                               select phraseDto);

        foreach (var affectedPhrase in affectedPhrases)
        {
          affectedPhrase.UserId = userData.Id;
        }

        var affectedTranslations = (from translationDto in SeedData.Ton.Translations
                                    where translationDto.UserId == userDto.Id
                                    select translationDto);

        foreach (var affectedTranslation in affectedTranslations)
        {
          affectedTranslation.UserId = userData.Id;
        }

        var affectedLines = (from lineDto in SeedData.Ton.Lines
                             where lineDto.UserId == userDto.Id
                             select lineDto);

        foreach (var affectedLine in affectedLines)
        {
          affectedLine.UserId = userData.Id;
        }

        var affectedMultiLineTexts = (from multiLineTextDto in SeedData.Ton.MultiLineTexts
                                      where multiLineTextDto.UserId == userDto.Id
                                      select multiLineTextDto);

        foreach (var affectedMultiLineText in affectedMultiLineTexts)
        {
          affectedMultiLineText.UserId = userData.Id;
        }

        var affectedPhraseBeliefs = (from phraseBeliefDto in SeedData.Ton.PhraseBeliefs
                                     where phraseBeliefDto.UserId == userDto.Id
                                     select phraseBeliefDto);

        foreach (var affectedPhraseBelief in affectedPhraseBeliefs)
        {
          affectedPhraseBelief.UserId = userData.Id;
        }

        userDto.Id = userData.Id;
        #endregion
      }
    }

    private static void SeedLanguages(LearnLanguagesContext context)
    {
      //LANGUAGES
      foreach (var langDto in SeedData.Ton.Languages)
      {
        var langData = EfHelper.ToData(langDto, context);
        context.LanguageDatas.AddObject(langData);
        context.SaveChanges();

        //UPDATE SEED DATA PHRASES WITH NEW LANGUAGE ID
        var affectedPhrases = (from phraseDto in SeedData.Ton.Phrases
                               where phraseDto.LanguageId == langDto.Id
                               select phraseDto).ToList();
        foreach (var phraseDto in affectedPhrases)
        {
          phraseDto.LanguageId = langData.Id;//new Id
        }

        //UPDATE USERS
        var affectedUsers = (from userDto in SeedData.Ton.Users
                             where userDto.LanguageIds.Contains(langDto.Id)
                             select userDto).ToList();

        foreach (var affectedUser in affectedUsers)
        {
          affectedUser.LanguageIds.Remove(langDto.Id);
          affectedUser.LanguageIds.Add(langData.Id);
        }

        langDto.Id = langData.Id;
      }
    }

    private static void SeedPhrases(LearnLanguagesContext context)
    {
      //PHRASES
      foreach (var phraseDto in SeedData.Ton.Phrases)
      {
        var phraseData = EfHelper.AddToContext(phraseDto, context);
        //var phraseData = EfHelper.ToData(phraseDto);
        //context.PhraseDatas.AddObject(phraseData);
        context.SaveChanges();

        //UPDATE SEED DATA THAT REFERENCES THE ID OF THIS DATA
        var affectedUsers = (from userDto in SeedData.Ton.Users
                             where userDto.PhraseIds.Contains(phraseDto.Id)
                             select userDto);//.ToList();

        foreach (var affectedUser in affectedUsers)
        {
          affectedUser.PhraseIds.Remove(phraseDto.Id);
          affectedUser.PhraseIds.Add(phraseData.Id);
        }

        var affectedTranslations = (from translationDto in SeedData.Ton.Translations
                                    where translationDto.PhraseIds.Contains(phraseDto.Id)
                                    select translationDto);
        foreach (var affectedTranslation in affectedTranslations)
        {
          affectedTranslation.PhraseIds.Remove(phraseDto.Id);
          affectedTranslation.PhraseIds.Add(phraseData.Id);
        }

        var affectedLines = (from lineDto in SeedData.Ton.Lines
                             where lineDto.PhraseId == phraseDto.Id
                             select lineDto);
        foreach (var affectedLine in affectedLines)
        {
          affectedLine.PhraseId = phraseData.Id;
        }

        phraseDto.Id = phraseData.Id;
      }
    }

    private static void SeedTranslations(LearnLanguagesContext context)
    {
      //TRANSLATIONS
      foreach (var translationDto in SeedData.Ton.Translations)
      {
        var translationData = EfHelper.AddToContext(translationDto, context);
        context.SaveChanges();

        //UPDATE USERS
        var affectedUsers = (from userDto in SeedData.Ton.Users
                             where userDto.TranslationIds.Contains(translationDto.Id)
                             select userDto).ToList();

        foreach (var affectedUser in affectedUsers)
        {
          affectedUser.PhraseIds.Remove(translationDto.Id);
          affectedUser.PhraseIds.Add(translationData.Id);
        }

        translationDto.Id = translationData.Id;

      }
    }

    private static void SeedLines(LearnLanguagesContext context)
    {
      //LINES
      foreach (var lineDto in SeedData.Ton.Lines)
      {
        var lineData = EfHelper.AddToContext(lineDto, context);
        context.SaveChanges();

        //UPDATE MLTS
        var affectedMlts = SeedData.Ton.MultiLineTexts.Where((mlt) => mlt.LineIds.Contains(lineDto.Id));
        foreach (var affectedMlt in affectedMlts)
        {
          affectedMlt.LineIds.Remove(lineDto.Id);
          affectedMlt.LineIds.Add(lineData.Id);
        }

        //UPDATE USERS
        var affectedUsers = (from userDto in SeedData.Ton.Users
                             where userDto.LineIds.Contains(lineDto.Id)
                             select userDto).ToList();

        foreach (var affectedUser in affectedUsers)
        {
          affectedUser.LineIds.Remove(lineDto.Id);
          affectedUser.LineIds.Add(lineData.Id);
        }

        lineDto.Id = lineData.Id;
      }
    }

    private static void SeedMultiLineTexts(LearnLanguagesContext context)
    {
      //LINES
      foreach (var mltDto in SeedData.Ton.MultiLineTexts)
      {
        var mltData = EfHelper.AddToContext(mltDto, context);
        context.SaveChanges();

        //UPDATE USERS
        var affectedUsers = (from userDto in SeedData.Ton.Users
                             where userDto.MultiLineTextIds.Contains(mltDto.Id)
                             select userDto).ToList();

        foreach (var affectedUser in affectedUsers)
        {
          affectedUser.MultiLineTextIds.Remove(mltDto.Id);
          affectedUser.MultiLineTextIds.Add(mltData.Id);
        }

        mltDto.Id = mltData.Id;
      }
    }
    private static void SeedStudyDatas(LearnLanguagesContext context)
    {
      //STUDY DATAS
      foreach (var studyDataDto in SeedData.Ton.StudyDatas)
      {
        var studyDataData = EfHelper.AddToContext(studyDataDto, context);
        context.SaveChanges();

        ////UPDATE USERS
        //var affectedUsers = (from userDto in SeedData.Ton.Users
        //                     where userDto.TranslationIds.Contains(studyDataDto.Id)
        //                     select userDto).ToList();

        //foreach (var affectedUser in affectedUsers)
        //{
        //  affectedUser.PhraseIds.Remove(studyDataDto.Id);
        //  affectedUser.PhraseIds.Add(studyDataDto.Id);
        //}

        studyDataDto.Id = studyDataData.Id;
      }
    }

    public ObjectContextManager<LearnLanguagesContext> GetManager()
    {
      return ObjectContextManager<LearnLanguagesContext>.GetManager(EfResources.LearnLanguagesConnectionStringKey);
    }
  }
}
