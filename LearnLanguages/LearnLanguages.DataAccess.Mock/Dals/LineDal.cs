using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LearnLanguages.DataAccess.Mock
{
  public class LineDal : LineDalBase
  {
    //public Result<LineDto> New(object criteria)
    //{
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
    //    var dto = new LineDto() 
    //    { 
    //      Id = Guid.NewGuid(),
    //      LanguageId = SeedData.Ton.DefaultLanguageId
    //    };
    //    retResult = Result<LineDto>.Success(dto);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<LineDto> Fetch(Guid id)
    //{
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Lines
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //      retResult = Result<LineDto>.Success(results.First());
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<LineDto>.FailureWithInfo(null,
    //          new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<LineDto> Update(LineDto dto)
    //{
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Lines
    //                  where item.Id == dto.Id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var LineToUpdate = results.First();
    //      SeedData.Ton.Lines.Remove(LineToUpdate);
    //      dto.Id = Guid.NewGuid();
    //      SeedData.Ton.Lines.Add(dto);
    //      retResult = Result<LineDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<LineDto>.FailureWithInfo(null,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<LineDto> Insert(LineDto dto)
    //{
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Lines
    //                  where item.Id == dto.Id
    //                  select item;

    //    if (results.Count() == 0)
    //    {
    //      dto.Id = Guid.NewGuid();
    //      //MIMIC LANGUAGEID REQUIRED CONSTRAINT IN DB
    //      if (dto.LanguageId == Guid.Empty || !SeedData.Ton.ContainsLanguageId(dto.LanguageId))
    //      {
    //        //I'VE RESTRUCTURED HOW TO DO EXCEPTIONHANDLING, SO THIS IS NOT QUITE HOW IT SHOULD BE DONE.
    //        //THIS SHOULD BE AN INSERTIMPL METHOD, AND IT SHOULD THROW ITS OWN EXCEPTION THAT IS WRAPPED IN THE 
    //        //PHRASEDALBASE CLASS IN AN INSERTFAILEDEXCEPTION.
    //        throw new Exceptions.InsertFailedException(string.Format(DalResources.ErrorMsgIdNotFoundException, dto.LanguageId));
    //      }
    //      SeedData.Ton.Lines.Add(dto);
    //      retResult = Result<LineDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 1) //ID ALREADY EXISTS
    //        retResult = Result<LineDto>.FailureWithInfo(dto,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else                      //MULTIPLE IDS ALREADY EXIST??
    //        retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<LineDto> Delete(Guid id)
    //{
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Lines
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var LineToRemove = results.First();
    //      SeedData.Ton.Lines.Remove(LineToRemove);
    //      retResult = Result<LineDto>.Success(LineToRemove);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<LineDto>.FailureWithInfo(null,
    //          new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public LearnLanguages.Result<ICollection<LineDto>> GetAll()
    //{
    //  Result<ICollection<LineDto>> retResult = Result<ICollection<LineDto>>.Undefined(null);
    //  try
    //  {
    //    var allDtos = new List<LineDto>(SeedData.Ton.Lines);
    //    retResult = Result<ICollection<LineDto>>.Success(allDtos);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<ICollection<LineDto>>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}

    protected override LineDto NewImpl(object criteria)
    {
      //to get to this point, we must have already been authenticated
      Debug.Assert(Csla.ApplicationContext.User.Identity.IsAuthenticated);
      //if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
      //  throw new Common.Exceptions.UserNotAuthenticatedException();

      //var languageId = SeedData.Ton.DefaultLanguageId;

      if (criteria != null)
        throw new ArgumentException("criteria expected to be null");
      //if (criteria is string)
      //{
      //  //IF WE HAVE A STRING PARAM, THEN IT IS LANGUAGETEXT, SO GET THE LANGUAGE ID FROM THAT
      //  var languageText = (string)criteria;
      //  var languageResults = (from language in SeedData.Ton.Languages
      //                         where language.Text == languageText
      //                         select language);
      //  if (languageResults.Count() == 1)
      //  {
      //    var languageDto = languageResults.First();
      //    languageId = languageDto.Id;
      //  }
      //  else if (languageResults.Count() == 0)
      //    throw new Exceptions.LanguageTextNotFoundException(languageText);
      //  else
      //    throw new Exceptions.VeryBadException();
      //}

      var username = Csla.ApplicationContext.User.Identity.Name;
      var userId = (from u in SeedData.Ton.Users
                    where u.Username == username
                    select u.Id).FirstOrDefault();
      if (userId == Guid.Empty)
        throw new Exceptions.UserNotAuthorizedException();
      

      var dto = new LineDto()
      {
        Id = Guid.NewGuid(),
        LineNumber = -1,
        UserId = userId, 
        Username = username
      };

      return dto;
    }
    protected override LineDto FetchImpl(Guid id)
    {
      var results = from item in SeedData.Ton.Lines
                    where item.Id == id
                    select item;

      if (results.Count() == 1)
        return results.First();
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override ICollection<LineDto> FetchImpl(ICollection<Guid> ids)
    {
      if (ids == null)
        throw new ArgumentNullException("ids");
      else if (ids.Count == 0)
        throw new ArgumentOutOfRangeException("ids", "ids cannot be empty.");

      var retLines = new List<LineDto>();

      foreach (var id in ids)
      {
        var dto = FetchImpl(id);
        retLines.Add(dto);
      }

      return retLines;
    }
    protected override LineDto UpdateImpl(LineDto dto)
    {
      var results = from item in SeedData.Ton.Lines
                    where item.Id == dto.Id
                    select item;

      if (results.Count() == 1)
      {
        CheckContraints(dto);

        var lineToUpdate = results.First();
        SeedData.Ton.Lines.Remove(lineToUpdate);
        dto.Id = Guid.NewGuid();
        SeedData.Ton.Lines.Add(dto);
        UpdateReferences(lineToUpdate, dto);
        return dto;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(dto.Id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override LineDto InsertImpl(LineDto dto)
    {
      var results = from item in SeedData.Ton.Lines
                    where item.Id == dto.Id
                    select item;

      if (results.Count() == 0)
      {
        CheckContraints(dto);

        dto.Id = Guid.NewGuid();
        SeedData.Ton.Lines.Add(dto);

        //ADD LINE.ID TO USER
        var resultsUser = from u in SeedData.Ton.Users
                          where u.Id == dto.UserId
                          select u;

        var user = resultsUser.First();
        user.LineIds.Add(dto.Id);
        
        return dto;
      }
      else
      {
        if (results.Count() == 1) //ID ALREADY EXISTS
          throw new Exceptions.IdAlreadyExistsException(dto.Id);
        else                      //MULTIPLE IDS ALREADY EXIST??
          throw new Exceptions.VeryBadException();
      }
    }
    protected override LineDto DeleteImpl(Guid id)
    {
      var results = from item in SeedData.Ton.Lines
                    where item.Id == id
                    select item;

      if (results.Count() == 1)
      {
        var LineToRemove = results.First();
        SeedData.Ton.Lines.Remove(LineToRemove);
        return LineToRemove;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override ICollection<LineDto> GetAllImpl()
    {
      var allDtos = new List<LineDto>(SeedData.Ton.Lines);
      return allDtos;
    }

    private void CheckContraints(LineDto dto)
    {
      //REFERENTIAL INTEGRITY
      if (dto.PhraseId == Guid.Empty || !SeedData.Ton.ContainsPhraseId(dto.PhraseId))
        throw new Exceptions.IdNotFoundException(dto.PhraseId);
      if (dto.UserId == Guid.Empty || !SeedData.Ton.ContainsUserId(dto.UserId))
        throw new Exceptions.IdNotFoundException(dto.UserId);
      if (string.IsNullOrEmpty(dto.Username) ||
         !(SeedData.Ton.GetUsername(dto.UserId) == dto.Username))
        throw new ArgumentException("dto.Username");
    }
    private void UpdateReferences(LineDto oldLine, LineDto newLine)
    {
      ////UPDATE USERS WHO REFERENCE THIS LINE
      var referencedUsers = from u in SeedData.Ton.Users
                            where u.LineIds.Contains(oldLine.Id)
                            select u;

      foreach (var user in referencedUsers)
      {
        user.LineIds.Remove(oldLine.Id);
        user.LineIds.Add(newLine.Id);
      }

      ////UPDATE TRANSLATIONS WHO REFERENCE THIS PHRASE
      //var referencedTranslations = from t in SeedData.Ton.Translations
      //                             where t.LineIds.Contains(oldLine.Id)
      //                             select t;

      //foreach (var translation in referencedTranslations)
      //{
      //  translation.LineIds.Remove(oldLine.Id);
      //  translation.LineIds.Add(newLine.Id);
      //}
    }
  }
}
