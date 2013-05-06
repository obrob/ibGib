using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Mock
{
  public class MultiLineTextDal : MultiLineTextDalBase
  {
    //public Result<MultiLineTextDto> New(object criteria)
    //{
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
    //    var dto = new MultiLineTextDto() 
    //    { 
    //      Id = Guid.NewGuid(),
    //      LanguageId = SeedData.Ton.DefaultLanguageId
    //    };
    //    retResult = Result<MultiLineTextDto>.Success(dto);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<MultiLineTextDto> Fetch(Guid id)
    //{
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.MultiLineTexts
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //      retResult = Result<MultiLineTextDto>.Success(results.First());
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<MultiLineTextDto>.FailureWithInfo(null,
    //          new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<MultiLineTextDto> Update(MultiLineTextDto dto)
    //{
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.MultiLineTexts
    //                  where item.Id == dto.Id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var MultiLineTextToUpdate = results.First();
    //      SeedData.Ton.MultiLineTexts.Remove(MultiLineTextToUpdate);
    //      dto.Id = Guid.NewGuid();
    //      SeedData.Ton.MultiLineTexts.Add(dto);
    //      retResult = Result<MultiLineTextDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<MultiLineTextDto>.FailureWithInfo(null,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<MultiLineTextDto> Insert(MultiLineTextDto dto)
    //{
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.MultiLineTexts
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
    //        //MultiLineTextDALBASE CLASS IN AN INSERTFAILEDEXCEPTION.
    //        throw new Exceptions.InsertFailedException(string.Format(DalResources.ErrorMsgIdNotFoundException, dto.LanguageId));
    //      }
    //      SeedData.Ton.MultiLineTexts.Add(dto);
    //      retResult = Result<MultiLineTextDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 1) //ID ALREADY EXISTS
    //        retResult = Result<MultiLineTextDto>.FailureWithInfo(dto,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else                      //MULTIPLE IDS ALREADY EXIST??
    //        retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<MultiLineTextDto> Delete(Guid id)
    //{
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.MultiLineTexts
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var MultiLineTextToRemove = results.First();
    //      SeedData.Ton.MultiLineTexts.Remove(MultiLineTextToRemove);
    //      retResult = Result<MultiLineTextDto>.Success(MultiLineTextToRemove);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<MultiLineTextDto>.FailureWithInfo(null,
    //          new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public LearnLanguages.Result<ICollection<MultiLineTextDto>> GetAll()
    //{
    //  Result<ICollection<MultiLineTextDto>> retResult = Result<ICollection<MultiLineTextDto>>.Undefined(null);
    //  try
    //  {
    //    var allDtos = new List<MultiLineTextDto>(SeedData.Ton.MultiLineTexts);
    //    retResult = Result<ICollection<MultiLineTextDto>>.Success(allDtos);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<ICollection<MultiLineTextDto>>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}

    protected override MultiLineTextDto NewImpl(object criteria)
    {
      var dto = new MultiLineTextDto()
      {
        Id = Guid.NewGuid(),
        UserId = SeedData.Ton.GetTestValidUserDto().Id,
        Username = SeedData.Ton.TestValidUsername
      };
      return dto;
    }
    protected override MultiLineTextDto FetchImpl(Guid id)
    {
      var results = from item in SeedData.Ton.MultiLineTexts
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
    protected override ICollection<MultiLineTextDto> FetchImpl(ICollection<Guid> ids)
    {
      if (ids == null)
        throw new ArgumentNullException("ids");
      else if (ids.Count == 0)
        throw new ArgumentOutOfRangeException("ids", "ids cannot be empty.");

      var retMultiLineTexts = new List<MultiLineTextDto>();

      foreach (var id in ids)
      {
        var dto = FetchImpl(id);
        retMultiLineTexts.Add(dto);
      }

      return retMultiLineTexts;
    }
    protected override ICollection<MultiLineTextDto> FetchByIdImpl(Guid lineId)
    {
      var results = from multiLineText in SeedData.Ton.MultiLineTexts
                    where multiLineText.LineIds.Contains(lineId)
                    select multiLineText;

      return results.ToList();
    }
    protected override MultiLineTextDto UpdateImpl(MultiLineTextDto dto)
    {
      var results = from item in SeedData.Ton.MultiLineTexts
                    where item.Id == dto.Id
                    select item;

      if (results.Count() == 1)
      {
        CheckValidity(dto);
        CheckReferentialIntegrity(dto);
        var MultiLineTextToUpdate = results.First();
        SeedData.Ton.MultiLineTexts.Remove(MultiLineTextToUpdate);
        dto.Id = Guid.NewGuid();
        SeedData.Ton.MultiLineTexts.Add(dto);
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
    protected override MultiLineTextDto InsertImpl(MultiLineTextDto dto)
    {
      var results = from item in SeedData.Ton.MultiLineTexts
                    where item.Id == dto.Id
                    select item;

      if (results.Count() == 0)
      {
        CheckValidity(dto);
        dto.Id = Guid.NewGuid();
        SeedData.Ton.MultiLineTexts.Add(dto);
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
    protected override MultiLineTextDto DeleteImpl(Guid id)
    {
      var results = from item in SeedData.Ton.MultiLineTexts
                    where item.Id == id
                    select item;

      if (results.Count() == 1)
      {
        var MultiLineTextToRemove = results.First();
        SeedData.Ton.MultiLineTexts.Remove(MultiLineTextToRemove);
        return MultiLineTextToRemove;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override ICollection<MultiLineTextDto> GetAllImpl()
    {
      var allDtos = new List<MultiLineTextDto>(SeedData.Ton.MultiLineTexts);
      return allDtos;
    }

    private void CheckValidity(MultiLineTextDto dto)
    {
      //VALIDITY
      if (dto == null)
        throw new ArgumentNullException("dto");
      if (dto.LineIds.Count < int.Parse(DalResources.MinLinesPerMultiLineText))
        throw new ArgumentOutOfRangeException("dto.LineIds.Count");
    }
    private static void CheckReferentialIntegrity(MultiLineTextDto dto)
    {
      //LINE IDS ARE VALID
      foreach (var id in dto.LineIds)
      {
        var count = (from p in SeedData.Ton.Lines
                     where p.Id == id
                     select p).Count();

        if (count == 1)
          continue;
        else if (count == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException();
      }

      //USER IS VALID
      var userCount = (from user in SeedData.Ton.Users
                       where user.Id == dto.UserId &&
                             user.Username == dto.Username
                       select user).Count();
      if (userCount == 0)
        throw new Exceptions.IdNotFoundException(dto.UserId);
      else if (userCount != 1)
        throw new Exceptions.VeryBadException();
    }
  }
}
