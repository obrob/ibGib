using System;
using System.Net;
using System.Linq;

using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This readonly deletes a user and all his associated data from database.
  /// </summary>
  [Serializable]
  public class DeleteMultiLineTextsReadOnly : Common.CslaBases.ReadOnlyBase<DeleteMultiLineTextsReadOnly>
  {
    #region Factory Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idsToDelete"></param>
    /// <returns></returns>
    public static async Task<DeleteMultiLineTextsReadOnly> CreateNewAsync(ICollection<Guid> idsToDelete)
    {
      var result = await DataPortal.CreateAsync<DeleteMultiLineTextsReadOnly>(idsToDelete);
      return result;
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return ReadProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<bool> WasSuccessfulProperty = RegisterProperty<bool>(c => c.WasSuccessful);
    /// <summary>
    /// True if the delete operation was successful. Otherwise, false.
    /// </summary>
    public bool WasSuccessful
    {
      get { return ReadProperty(WasSuccessfulProperty); }
      private set { LoadProperty(WasSuccessfulProperty, value); }
    }

    public static readonly PropertyInfo<string> UnsuccessfulIdsProperty = RegisterProperty<string>(c => c.UnsuccessfulIds);
    /// <summary>
    /// Pipe-delimited (|) string containing guids of songs that weren't able to be deleted.
    /// Collections don't work yet in CSLA 4.5 (not even MobileList or other mobile collections).
    /// </summary>
    public string UnsuccessfulIds
    {
      get { return ReadProperty(UnsuccessfulIdsProperty); }
      private set { LoadProperty(UnsuccessfulIdsProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create(ICollection<Guid> idsToDelete)
    {
      RetrieverId = Guid.NewGuid();
      WasSuccessful = false;

      using (var dalManager = DalFactory.GetDalManager())
      {
        var mltDal = dalManager.GetProvider<IMultiLineTextDal>();

        var distinctIdsToDelete = idsToDelete.Distinct().ToList();
        for (int i = 0; i < distinctIdsToDelete.Count; i++)
        {
          var id = distinctIdsToDelete[i];
          var singleDeleteResult = mltDal.Delete(id);
          if (!singleDeleteResult.IsSuccess)
          {
            AddUnsuccessfulId(id);
            WasSuccessful = false;
          }
        }

        WasSuccessful = string.IsNullOrEmpty(UnsuccessfulIds);
        //if (!result.IsSuccess)
        //{
        //  Exception error = result.GetExceptionFromInfo();
        //  if (error != null)
        //    throw error;
        //  else
        //    throw new DataAccess.Exceptions.AddUserFailedException(result.Msg);
        //}
      }
    }

    private void AddUnsuccessfulId(Guid id)
    {
      if (string.IsNullOrEmpty(UnsuccessfulIds))
        UnsuccessfulIds = id.ToString();
      else
        UnsuccessfulIds += "|" + id.ToString();
    }
#endif

    #endregion
  }
}
