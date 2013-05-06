using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  /// <summary>
  /// This class wraps every ILineDal with a try..catch wrapper
  /// block that does the wrapping for each call.  The descending classes only need
  /// to implement the 
  /// </summary>
  public abstract class LineDalBase : ILineDal
  {
    public Result<LineDto> New(object criteria)
    {
      Result<LineDto> retResult = Result<LineDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = NewImpl(criteria);
        retResult = Result<LineDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.CreateFailedException(ex);
        retResult = Result<LineDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<LineDto> Fetch(Guid id)
    {
      Result<LineDto> retResult = Result<LineDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = FetchImpl(id);
        retResult = Result<LineDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<LineDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<LineDto>> Fetch(ICollection<Guid> ids)
    {
      Result<ICollection<LineDto>> retResult = Result<ICollection<LineDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dtos = FetchImpl(ids);
        retResult = Result<ICollection<LineDto>>.Success(dtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<ICollection<LineDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<LineDto> Update(LineDto dtoToUpdate)
    {
      Result<LineDto> retResult = Result<LineDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var updatedDto = UpdateImpl(dtoToUpdate);
        retResult = Result<LineDto>.Success(updatedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.UpdateFailedException(ex);
        retResult = Result<LineDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<LineDto> Insert(LineDto dtoToInsert)
    {
      Result<LineDto> retResult = Result<LineDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var insertedDto = InsertImpl(dtoToInsert);
        retResult = Result<LineDto>.Success(insertedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.InsertFailedException(ex);
        retResult = Result<LineDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<LineDto> Delete(Guid id)
    {
      Result<LineDto> retResult = Result<LineDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var dto = DeleteImpl(id);
        retResult = Result<LineDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.DeleteFailedException(ex);
        retResult = Result<LineDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<LineDto>> GetAll()
    {
      Result<ICollection<LineDto>> retResult = Result<ICollection<LineDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var allDtos = GetAllImpl();
        retResult = Result<ICollection<LineDto>>.Success(allDtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetAllFailedException(ex);
        retResult = Result<ICollection<LineDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }

    protected abstract LineDto NewImpl(object criteria);
    protected abstract LineDto FetchImpl(Guid id);
    protected abstract ICollection<LineDto> FetchImpl(ICollection<Guid> ids);
    protected abstract LineDto UpdateImpl(LineDto dto);
    protected abstract LineDto InsertImpl(LineDto dto);
    protected abstract LineDto DeleteImpl(Guid id);
    protected abstract ICollection<LineDto> GetAllImpl();

    protected void CheckAuthentication()
    {
      if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
        throw new Common.Exceptions.UserNotAuthenticatedException();
    }


  }
}
