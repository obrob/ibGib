using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  /// <summary>
  /// This class wraps every IMultiLineTextDal with a try..catch wrapper
  /// block that does the wrapping for each call.  The descending classes only need
  /// to implement the 
  /// </summary>
  public abstract class MultiLineTextDalBase : IMultiLineTextDal
  {
    public Result<MultiLineTextDto> New(object criteria)
    {
      Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = NewImpl(criteria);
        retResult = Result<MultiLineTextDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.CreateFailedException(ex);
        retResult = Result<MultiLineTextDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<MultiLineTextDto> Fetch(Guid id)
    {
      Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = FetchImpl(id);
        retResult = Result<MultiLineTextDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<MultiLineTextDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<MultiLineTextDto>> Fetch(ICollection<Guid> ids)
    {
      Result<ICollection<MultiLineTextDto>> retResult = Result<ICollection<MultiLineTextDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dtos = FetchImpl(ids);
        retResult = Result<ICollection<MultiLineTextDto>>.Success(dtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<ICollection<MultiLineTextDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    /// <summary>
    /// Returns all multiLineTexts that contains the lineId.
    /// </summary>
    /// <param name="lineDto"></param>
    /// <returns></returns>
    public Result<ICollection<MultiLineTextDto>> FetchByLineId(Guid lineId)
    {
      Result<ICollection<MultiLineTextDto>> retResult = Result<ICollection<MultiLineTextDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dtos = FetchByIdImpl(lineId);
        retResult = Result<ICollection<MultiLineTextDto>>.Success(dtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<ICollection<MultiLineTextDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<MultiLineTextDto> Update(MultiLineTextDto dtoToUpdate)
    {
      Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var updatedDto = UpdateImpl(dtoToUpdate);
        retResult = Result<MultiLineTextDto>.Success(updatedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.UpdateFailedException(ex);
        retResult = Result<MultiLineTextDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<MultiLineTextDto> Insert(MultiLineTextDto dtoToInsert)
    {
      Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var insertedDto = InsertImpl(dtoToInsert);
        retResult = Result<MultiLineTextDto>.Success(insertedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.InsertFailedException(ex);
        retResult = Result<MultiLineTextDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<MultiLineTextDto> Delete(Guid id)
    {
      Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var dto = DeleteImpl(id);
        retResult = Result<MultiLineTextDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.DeleteFailedException(ex);
        retResult = Result<MultiLineTextDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<MultiLineTextDto>> GetAll()
    {
      Result<ICollection<MultiLineTextDto>> retResult = Result<ICollection<MultiLineTextDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var allDtos = GetAllImpl();
        retResult = Result<ICollection<MultiLineTextDto>>.Success(allDtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetAllFailedException(ex);
        retResult = Result<ICollection<MultiLineTextDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }

    protected abstract MultiLineTextDto NewImpl(object criteria);
    protected abstract MultiLineTextDto FetchImpl(Guid id);
    protected abstract ICollection<MultiLineTextDto> FetchByIdImpl(Guid lineId);
    protected abstract ICollection<MultiLineTextDto> FetchImpl(ICollection<Guid> ids);
    protected abstract MultiLineTextDto UpdateImpl(MultiLineTextDto dto);
    protected abstract MultiLineTextDto InsertImpl(MultiLineTextDto dto);
    protected abstract MultiLineTextDto DeleteImpl(Guid id);
    protected abstract ICollection<MultiLineTextDto> GetAllImpl();

    
  }
}
