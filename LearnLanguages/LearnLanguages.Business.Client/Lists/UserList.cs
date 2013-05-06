using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;
using System.ComponentModel;
using LearnLanguages.Business.Security;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  [Serializable]
  public class UserList : Common.CslaBases.BusinessListBase<UserList, UserEdit, UserDto>
  {
    #region Factory Methods

#if !SILVERLIGHT
    public static UserList GetAll()
    {
      return DataPortal.Fetch<UserList>();
    }
#endif

    /// <summary>
    /// Gets all of the UserEdits available to the current user.
    /// </summary>
    public static async Task<UserList> GetAllAsync()
    {
      var result = await DataPortal.FetchAsync<UserList>();
      return result;
    }

    public static async Task<UserList> NewUserListAsync(ICollection<Guid> userIds)
    {
      var result = await DataPortal.FetchAsync<UserList>(userIds);
      return result;
    }
    
    /// <summary>
    /// Just news up a UserList object. Doesn't touch the DataPortal.
    /// </summary>
    /// <returns></returns>
    public static UserList NewUserListNewedUpOnly()
    {
      return new UserList();
    }

    /// <summary>
    /// Runs locally.
    /// </summary>
    [RunLocal]
    public static async Task<UserList> NewUserListAsync()
    {
      var result = await DataPortal.CreateAsync<UserList>();
      return result;
    }


    #endregion

    #region Data Portal methods (including child)

#if !SILVERLIGHT
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public void DataPortal_Fetch(ICollection<Guid> userIds)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var userDal = dalManager.GetProvider<IUserIdentityDal>();

    //    Result<ICollection<UserDto>> result = userDal.Fetch(userIds);
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
    //    var fetchedUserDtos = result.Obj;
    //    foreach (var userDto in fetchedUserDtos)
    //    {
    //      //var UserEdit = DataPortal.CreateChild<UserEdit>(UserDto);
    //      var userEdit = DataPortal.FetchChild<UserEdit>(userDto);
    //      this.Add(userEdit);
    //    }
    //  }
    //}
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var UserDal = dalManager.GetProvider<IUserDal>();

        Result<ICollection<UserDto>> result = UserDal.GetAll();
        if (!result.IsSuccess || result.IsError)
        {
          if (result.Info != null)
          {
            var ex = result.GetExceptionFromInfo();
            if (ex != null)
              throw new GetAllFailedException(ex.Message);
            else
              throw new GetAllFailedException();
          }
          else
            throw new GetAllFailedException();
        }

        //RESULT WAS SUCCESSFUL
        var allUserDtos = result.Obj;
        foreach (var userDto in allUserDtos)
        {
          //var UserEdit = DataPortal.CreateChild<UserEdit>(UserDto);
          var userEdit = DataPortal.FetchChild<UserEdit>(userDto);
          this.Add(userEdit);
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Child_Fetch(ICollection<Guid> userIds)
    {
      Items.Clear();
      foreach (var id in userIds)
      {
        var userEdit = DataPortal.FetchChild<UserEdit>(id);
        Items.Add(userEdit);
      }
    }

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public void DataPortal_Fetch(string username)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var UserDal = dalManager.GetProvider<IUserIdentityDal>();

    //    Result<ICollection<UserDto>> result = UserDal.Fetch(username);
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
    //    var userDtos = result.Obj;
    //    foreach (var UserDto in userDtos)
    //    {
    //      //var UserEdit = DataPortal.CreateChild<UserEdit>(UserDto);
    //      var UserEdit = DataPortal.FetchChild<UserEdit>(UserDto);
    //      this.Add(UserEdit);
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
        
#endif
    
    #endregion

    #region AddNewCore

#if SILVERLIGHT
    protected override void AddNewCore()
    {
      AddedNew += UserList_AddedNew; 
      base.AddNewCore();
      AddedNew -= UserList_AddedNew;
    }

    private void UserList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<UserEdit> e)
    {
      //Common.CommonHelper.CheckAuthentication();
      //var userEdit = e.NewObject;
      //userEdit.LoadCurrentUser();
      //var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
      //userEdit.UserId = identity.UserId;
      //userEdit.Username = identity.Name;
    }
#else
    protected override UserEdit AddNewCore()
    {
      //SERVER
      var userEdit = base.AddNewCore();
      //userEdit.LoadCurrentUser();
      return userEdit;
    }
#endif

    #endregion

    protected override void OnChildChanged(Csla.Core.ChildChangedEventArgs e)
    {
      base.OnChildChanged(e);
      //if (e.ChildObject != null)
      //  (Csla.Core.BusinessBase)e.ChildObject.BusinessRules.CheckRules();
    }
  }
}
