﻿using System;
using System.Net;
using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This readonly deletes a user and all his associated data from database.
  /// </summary>
  [Serializable]
  public class DeleteUserReadOnly : Common.CslaBases.ReadOnlyBase<DeleteUserReadOnly>
  {
    #region Factory Methods

    public static async Task<DeleteUserReadOnly> CreateNewAsync(string username)
    {
      var result = await DataPortal.CreateAsync<DeleteUserReadOnly>(username);
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

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create(string username)
    {
      RetrieverId = Guid.NewGuid();
      WasSuccessful = false;

      using (var dalManager = DalFactory.GetDalManager())
      {
        var customIdentityDal = dalManager.GetProvider<IUserDal>();
        var result = customIdentityDal.Delete(username);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DataAccess.Exceptions.AddUserFailedException(result.Msg);
        }
        WasSuccessful = true;
      }
    }
#endif

    #endregion
  }
}
