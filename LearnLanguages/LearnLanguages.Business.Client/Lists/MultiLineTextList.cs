using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  [Serializable]
  public class MultiLineTextList : Common.CslaBases.BusinessListBase<MultiLineTextList, MultiLineTextEdit, MultiLineTextDto>
  {
    /// <summary>
    /// Retrieve all MultiLineTexts in the database, for the current user.
    /// </summary>
    /// <returns></returns>
    public static async Task<MultiLineTextList> GetAllAsync()
    {
      var result = await DataPortal.FetchAsync<MultiLineTextList>();
      return result;
    }

    /// <summary>
    /// Ids of MLTs to populate the list. This will retrieve the MLTs from the database 
    /// via these ids, and load them as children to newly created list.
    /// 
    /// These ids must belong to the current user.
    /// </summary>
    /// <param name="multiLineTextIds"></param>
    /// <returns></returns>
    public static async Task<MultiLineTextList> NewMultiLineTextListAsync(ICollection<Guid> multiLineTextIds)
    {
      var result = await DataPortal.FetchAsync<MultiLineTextList>(multiLineTextIds);
      return result;
    }

#if !SILVERLIGHT
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //protected void DataPortal_Fetch(ICollection<Guid> multiLineTextIds)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var MultiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();

    //    Result<ICollection<MultiLineTextDto>> result = MultiLineTextDal.Fetch(multiLineTextIds);
    //    if (!result.IsSuccess || result.IsError)
    //    {
    //      if (result.Info != null)
    //      {
    //        var ex = result.GetExceptionFromInfo();
    //        if (ex != null)
    //          throw new FetchFailedException(ex.Message);
    //        else
    //          throw new FetchFailedException();
    //      }
    //      else
    //        throw new FetchFailedException();
    //    }

    //    //RESULT WAS SUCCESSFUL
    //    var fetchedMultiLineTextDtos = result.Obj;
    //    foreach (var multiLineTextDto in fetchedMultiLineTextDtos)
    //    {
    //      //var MultiLineTextEdit = DataPortal.CreateChild<MultiLineTextEdit>(MultiLineTextDto);
    //      var multiLineTextEdit = DataPortal.FetchChild<MultiLineTextEdit>(multiLineTextDto);
    //      this.Add(multiLineTextEdit);
    //    }
    //  }
    //}

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var MultiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();

        Result<ICollection<MultiLineTextDto>> result = MultiLineTextDal.GetAll();
        if (!result.IsSuccess || result.IsError)
        {
          if (result.Info != null)
          {
            var ex = result.GetExceptionFromInfo();
            if (ex != null)
              throw new FetchFailedException(ex.Message);
            else
              throw new FetchFailedException();
          }
          else
            throw new FetchFailedException();
        }

        //RESULT WAS SUCCESSFUL
        var allMultiLineTextDtos = result.Obj;
        foreach (var multiLineTextDto in allMultiLineTextDtos)
        {
          //var MultiLineTextEdit = DataPortal.CreateChild<MultiLineTextEdit>(MultiLineTextDto);
          var multiLineTextEdit = DataPortal.FetchChild<MultiLineTextEdit>(multiLineTextDto);
          this.Add(multiLineTextEdit);
        }
      }
    }

    //[Transactional(TransactionalTypes.TransactionScope)]
    //protected void DataPortal_Fetch(Criteria.PhraseCriteria phraseCriteria)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var MultiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
    //    Result<ICollection<MultiLineTextDto>> result = MultiLineTextDal.FetchByPhraseId(phraseCriteria.Phrase.Id);
    //    if (!result.IsSuccess)
    //    {
    //      Exception error = result.GetExceptionFromInfo();
    //      if (error != null)
    //        throw error;
    //      else
    //        throw new FetchFailedException(result.Msg);
    //    }
        
    //    //RESULT WAS SUCCESSFUL
    //    var multiLineTextDtos = result.Obj;
    //    foreach (var multiLineTextDto in multiLineTextDtos)
    //    {
    //      //var MultiLineTextEdit = DataPortal.CreateChild<MultiLineTextEdit>(MultiLineTextDto);
    //      var multiLineTextEdit = DataPortal.FetchChild<MultiLineTextEdit>(multiLineTextDto);
    //      this.Add(multiLineTextEdit);
    //    }
    //  }
    //}

    [Transactional(TransactionalTypes.TransactionScope)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        base.Child_Update();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void DataPortal_Fetch(ICollection<Guid> multiLineTextIds)
    {
      Items.Clear();
      foreach (var id in multiLineTextIds)
      {
        var multiLineTextEdit = DataPortal.FetchChild<MultiLineTextEdit>(id);
        Items.Add(multiLineTextEdit);
      }
    }
#endif
  }
}
