using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  /// <summary>
  /// This class wraps every ILanguageDal with a try..catch wrapper
  /// block that does the wrapping for each call.  The descending classes only need
  /// to implement the 
  /// </summary>
  public abstract class LanguageDalBase : ILanguageDal
  {
    public Result<LanguageDto> New(object criteria)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = NewImpl(criteria);
        retResult = Result<LanguageDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.CreateFailedException(ex);
        retResult = Result<LanguageDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<LanguageDto> Fetch(Guid id)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = FetchImpl(id);
        retResult = Result<LanguageDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<LanguageDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<LanguageDto> Fetch(string languageText)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = FetchImpl(languageText);
        retResult = Result<LanguageDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<LanguageDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<LanguageDto>> Fetch(ICollection<Guid> ids)
    {
      Result<ICollection<LanguageDto>> retResult = Result<ICollection<LanguageDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dtos = FetchImpl(ids);
        retResult = Result<ICollection<LanguageDto>>.Success(dtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<ICollection<LanguageDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<LanguageDto> Update(LanguageDto dtoToUpdate)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var updatedDto = UpdateImpl(dtoToUpdate);
        retResult = Result<LanguageDto>.Success(updatedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.UpdateFailedException(ex);
        retResult = Result<LanguageDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<LanguageDto> Insert(LanguageDto dtoToInsert)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var insertedDto = InsertImpl(dtoToInsert);
        retResult = Result<LanguageDto>.Success(insertedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.InsertFailedException(ex);
        retResult = Result<LanguageDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<LanguageDto> Delete(Guid id)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var dto = DeleteImpl(id);
        retResult = Result<LanguageDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.DeleteFailedException(ex);
        retResult = Result<LanguageDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<LanguageDto>> GetAll()
    {
      Result<ICollection<LanguageDto>> retResult = Result<ICollection<LanguageDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var allDtos = GetAllImpl();
        retResult = Result<ICollection<LanguageDto>>.Success(allDtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetAllFailedException(ex);
        retResult = Result<ICollection<LanguageDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }

    protected abstract LanguageDto NewImpl(object criteria);
    protected abstract LanguageDto FetchImpl(Guid id);
    protected abstract LanguageDto FetchImpl(string languageText);
    protected abstract ICollection<LanguageDto> FetchImpl(ICollection<Guid> ids);
    protected abstract LanguageDto UpdateImpl(LanguageDto dto);
    protected abstract LanguageDto InsertImpl(LanguageDto dto);
    protected abstract LanguageDto DeleteImpl(Guid id);
    protected abstract ICollection<LanguageDto> GetAllImpl();

    protected void CheckAuthentication()
    {
      if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
        throw new Common.Exceptions.UserNotAuthenticatedException();
    }
  }
}
