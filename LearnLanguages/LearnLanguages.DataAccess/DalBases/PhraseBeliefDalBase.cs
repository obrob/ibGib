using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  /// <summary>
  /// This class wraps every IPhraseBeliefDal with a try..catch wrapper
  /// block that does the wrapping for each call.  The descending classes only need
  /// to implement the 
  /// </summary>
  public abstract class PhraseBeliefDalBase : IPhraseBeliefDal
  {
    public Result<PhraseBeliefDto> New(object criteria)
    {
      Result<PhraseBeliefDto> retResult = Result<PhraseBeliefDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = NewImpl(criteria);
        retResult = Result<PhraseBeliefDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.CreateFailedException(ex);
        retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    /// <summary>
    /// Fetches a belief dto based on the belief.Id.  This is NOT phraseId, ie it does not fetch all beliefs
    /// related to a phrase.
    /// </summary>
    public Result<PhraseBeliefDto> Fetch(Guid id)
    {
      Result<PhraseBeliefDto> retResult =
        Result<PhraseBeliefDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var dto = FetchImpl(id);
        retResult = Result<PhraseBeliefDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    /// <summary>
    /// Fetches all beliefs related to the phrase identified by the given phraseId.
    /// </summary>
    /// <param name="phraseId"></param>
    /// <param name="signatureDummy"></param>
    /// <returns></returns>
    public Result<ICollection<PhraseBeliefDto>> FetchAllRelatedToPhrase(Guid phraseId)
    {
      Result<ICollection<PhraseBeliefDto>> retResult = 
        Result<ICollection<PhraseBeliefDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var beliefDtosRelatedToPhrase = FetchAllRelatedToPhraseImpl(phraseId);
        retResult = Result<ICollection<PhraseBeliefDto>>.Success(beliefDtosRelatedToPhrase);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<ICollection<PhraseBeliefDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<PhraseBeliefDto> Update(PhraseBeliefDto dtoToUpdate)
    {
      Result<PhraseBeliefDto> retResult = Result<PhraseBeliefDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var updatedDto = UpdateImpl(dtoToUpdate);
        retResult = Result<PhraseBeliefDto>.Success(updatedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.UpdateFailedException(ex);
        retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<PhraseBeliefDto> Insert(PhraseBeliefDto dtoToInsert)
    {
      Result<PhraseBeliefDto> retResult = Result<PhraseBeliefDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var insertedDto = InsertImpl(dtoToInsert);
        retResult = Result<PhraseBeliefDto>.Success(insertedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.InsertFailedException(ex);
        retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<PhraseBeliefDto> Delete(Guid id)
    {
      Result<PhraseBeliefDto> retResult = Result<PhraseBeliefDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var dto = DeleteImpl(id);
        retResult = Result<PhraseBeliefDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.DeleteFailedException(ex);
        retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<PhraseBeliefDto>> GetAll()
    {
      Result<ICollection<PhraseBeliefDto>> retResult = Result<ICollection<PhraseBeliefDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        
        var allDtos = GetAllImpl();
        retResult = Result<ICollection<PhraseBeliefDto>>.Success(allDtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetAllFailedException(ex);
        retResult = Result<ICollection<PhraseBeliefDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }

    protected abstract PhraseBeliefDto NewImpl(object criteria);
    protected abstract PhraseBeliefDto FetchImpl(Guid id);
    protected abstract ICollection<PhraseBeliefDto> FetchAllRelatedToPhraseImpl(Guid phraseId);
    //protected abstract ICollection<PhraseBeliefDto> FetchImpl(ICollection<Guid> ids);
    protected abstract PhraseBeliefDto UpdateImpl(PhraseBeliefDto dto);
    protected abstract PhraseBeliefDto InsertImpl(PhraseBeliefDto dto);
    protected abstract PhraseBeliefDto DeleteImpl(Guid id);
    protected abstract ICollection<PhraseBeliefDto> GetAllImpl();

    protected void CheckAuthentication()
    {
      if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
        throw new Common.Exceptions.UserNotAuthenticatedException();
    }
  }
}
