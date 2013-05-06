using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  /// <summary>
  /// This class wraps every IStudyDataDal with a try..catch wrapper
  /// block that does the wrapping for each call.  The descending classes only need
  /// to implement the abstract Impl methods.
  /// </summary>
  public abstract class StudyDataDalBase : IStudyDataDal
  {
    public Result<StudyDataDto> New(object criteria)
    {
      Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = NewImpl(criteria);
        retResult = Result<StudyDataDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.CreateFailedException(ex);
        retResult = Result<StudyDataDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    //public Result<StudyDataDto> Fetch(Guid id)
    //{
    //  Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
    //  try
    //  {
    //    Common.CommonHelper.CheckAuthentication();

    //    var dto = FetchImpl(id);
    //    retResult = Result<StudyDataDto>.Success(dto);
    //  }
    //  catch (Exception ex)
    //  {
    //    var wrappedEx = new Exceptions.FetchFailedException(ex);
    //    retResult = Result<StudyDataDto>.FailureWithInfo(null, wrappedEx);
    //  }
    //  return retResult;
    ////}
    //public Result<ICollection<StudyDataDto>> Fetch(ICollection<Guid> ids)
    //{
    //  Result<ICollection<StudyDataDto>> retResult = Result<ICollection<StudyDataDto>>.Undefined(null);
    //  try
    //  {
    //    Common.CommonHelper.CheckAuthentication();

    //    var dtos = FetchImpl(ids);
    //    retResult = Result<ICollection<StudyDataDto>>.Success(dtos);
    //  }
    //  catch (Exception ex)
    //  {
    //    var wrappedEx = new Exceptions.FetchFailedException(ex);
    //    retResult = Result<ICollection<StudyDataDto>>.FailureWithInfo(null, wrappedEx);
    //  }
    //  return retResult;
    //}
    public Result<StudyDataDto> FetchForCurrentUser()
    {
      Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = FetchForCurrentUserImpl();
        retResult = Result<StudyDataDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<StudyDataDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<bool> StudyDataExistsForCurrentUser()
    {
      Result<bool> retResult = Result<bool>.Undefined(false);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = StudyDataExistsForCurrentUserImpl();
        retResult = Result<bool>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<bool>.FailureWithInfo(false, wrappedEx);
      }
      return retResult;
    }
    public Result<StudyDataDto> Update(StudyDataDto dtoToUpdate)
    {
      Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var updatedDto = UpdateImpl(dtoToUpdate);
        retResult = Result<StudyDataDto>.Success(updatedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.UpdateFailedException(ex);
        retResult = Result<StudyDataDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<StudyDataDto> Insert(StudyDataDto dtoToInsert)
    {
      Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var insertedDto = InsertImpl(dtoToInsert);
        retResult = Result<StudyDataDto>.Success(insertedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.InsertFailedException(ex);
        retResult = Result<StudyDataDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<StudyDataDto> Delete(Guid id)
    {
      Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var dto = DeleteImpl(id);
        retResult = Result<StudyDataDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.DeleteFailedException(ex);
        retResult = Result<StudyDataDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<StudyDataDto>> GetAll()
    {
      Result<ICollection<StudyDataDto>> retResult = Result<ICollection<StudyDataDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var allDtos = GetAllImpl();
        retResult = Result<ICollection<StudyDataDto>>.Success(allDtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetAllFailedException(ex);
        retResult = Result<ICollection<StudyDataDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }

    protected abstract StudyDataDto NewImpl(object criteria);
    protected abstract StudyDataDto FetchForCurrentUserImpl();
    protected abstract bool StudyDataExistsForCurrentUserImpl();
    //protected abstract StudyDataDto FetchImpl(Guid id);
    //protected abstract ICollection<StudyDataDto> FetchImpl(ICollection<Guid> ids);
    protected abstract StudyDataDto UpdateImpl(StudyDataDto dto);
    protected abstract StudyDataDto InsertImpl(StudyDataDto dto);
    protected abstract StudyDataDto DeleteImpl(Guid id);
    protected abstract ICollection<StudyDataDto> GetAllImpl();

    protected void CheckAuthentication()
    {
      if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
        throw new Common.Exceptions.UserNotAuthenticatedException();
    }

    
    
  }
}
