﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using Csla;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  [Serializable]
  public class UserEdit : LearnLanguages.Common.CslaBases.BusinessBase<UserEdit, UserDto>, IHaveId
  {
    #region Factory Methods
    #region Wpf Factory Methods
#if !SILVERLIGHT

    /// <summary>
    /// Creates a new UserEdit sync
    /// </summary>
    public static UserEdit NewUserEdit()
    {
      return DataPortal.Create<UserEdit>();
    }

    /// <summary>
    /// Creates a new UserEdit sync with the given userInfoCriteria
    /// </summary>
    public static UserEdit NewUserEdit(Criteria.UserInfoCriteria criteria)
    {
      return DataPortal.Create<UserEdit>(criteria);
    }

    /// <summary>
    /// Fetches a UserEdit sync with given id
    /// </summary>
    public static UserEdit GetUserEdit(Guid id)
    {
      return DataPortal.Fetch<UserEdit>(id);
    }

#endif
    #endregion

    #region Silverlight Factory Methods
#if SILVERLIGHT

    public static async Task<UserEdit> NewUserEditAsync()
    {
      var result = await DataPortal.CreateAsync<UserEdit>();
      return result;
    }

    public static async Task<UserEdit> NewUserEditAsync(Criteria.UserInfoCriteria criteria)
    {
      var result = await DataPortal.CreateAsync<UserEdit>(criteria);
      return result;
    }

    public static async Task<UserEdit> GetUserEditAsync(Guid id)
    {
      var result = await DataPortal.FetchAsync<UserEdit>(id);
      return result;
    }

#endif
    #endregion
    #endregion

    #region Business Properties & Methods

    //USERNAME
    #region public string Username
    public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(c => c.Username);
    public string Username
    {
      get { return GetProperty(UsernameProperty); }
      set { SetProperty(UsernameProperty, value); }
    }
    #endregion
    
    //SALT
    //SALTED HASHED PASSWORD

    #region Not sure if I will need these

    //PHRASE IDS
    //TRANSLATION IDS
    //LINE IDS
    //PHRASE BELIEF IDS
    //ROLE IDS
    //MLT IDS
    //LANGUAGE IDS

    #endregion

    public override void LoadFromDtoBypassPropertyChecksImpl(UserDto dto)
    {
      using (BypassPropertyChecks)
      {
        LoadProperty<Guid>(IdProperty, dto.Id);
        LoadProperty<string>(UsernameProperty, dto.Username);
      }
    }

    public override UserDto CreateDto()
    {
      UserDto retDto = new UserDto(){
                                      Id = this.Id,
                                      Username = this.Username
                                    };
      return retDto;
    }

    /// <summary>
    /// Persist object async.
    /// </summary>
    protected override async Task<UserEdit> SaveAsync(bool forceUpdate, object userState, bool isSync)
    {
      var result = await base.SaveAsync(forceUpdate, userState, isSync);
      return result;
    }

    /// <summary>
    /// Loads the default properties, including generating a new Id, inside of a using (BypassPropertyChecks) block.
    /// </summary>
    private void LoadDefaults()
    {
      using (BypassPropertyChecks)
      {
        Id = Guid.NewGuid();
        Username = DalResources.DefaultUsername;
      }
      BusinessRules.CheckRules();
    }

    public int GetEditLevel()
    {
      return EditLevel;
    }

    ///// <summary>
    ///// Does CheckAuthentication().  Then Loads the user with the current user, userid, username.
    ///// </summary>
    //internal void LoadCurrentUser()
    //{
    //  Common.CommonHelper.CheckAuthentication();
    //  var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
    //  UserId = identity.UserId;
    //  Username = identity.Name;
    //}

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();

      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UsernameProperty));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // USER MUST BE AUTHENTICATED TO BE IN THESE ROLES
      Csla.Rules.BusinessRules.AddRule(typeof(UserEdit),
        new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject,
          DalResources.RoleAdmin, DalResources.RoleUser));
      Csla.Rules.BusinessRules.AddRule(typeof(UserEdit),
        new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.GetObject,
          DalResources.RoleAdmin, DalResources.RoleUser));
      Csla.Rules.BusinessRules.AddRule(typeof(UserEdit),
        new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.EditObject,
          DalResources.RoleAdmin, DalResources.RoleUser));
      Csla.Rules.BusinessRules.AddRule(typeof(UserEdit),
        new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.DeleteObject,
          DalResources.RoleAdmin, DalResources.RoleUser));

    }

    #endregion

    #region Data Access (This is run on the server, unless run local set)

    #region Wpf DP_XYZ
#if !SILVERLIGHT

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        UserDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Create(Criteria.UserInfoCriteria criteria)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.New(criteria);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        UserDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        Result<UserDto> result = userDal.Fetch(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        UserDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var dto = CreateDto();
        var result = userDal.Insert(dto);
        
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new InsertFailedException(result.Msg);
        }
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var dto = CreateDto();
        Result<UserDto> result = userDal.Update(dto);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new UpdateFailedException(result.Msg);
        }
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.Delete(ReadProperty<Guid>(IdProperty));
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException(result.Msg);
        }
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Delete(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.Delete(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException(result.Msg);
        }
      }
    }

#endif
    #endregion //Wpf DP_XYZ
    
    #region Child DP_XYZ
    
#if !SILVERLIGHT
    
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Child_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        UserDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(UserDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Child_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.Fetch(id);
        if (result.IsError)
          throw new FetchFailedException(result.Msg);
        UserDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Insert()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        FieldManager.UpdateChildren(this);

        var userDal = dalManager.GetProvider<IUserDal>();
        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = userDal.Insert(dto);
          
          if (!result.IsSuccess)
          {
            Exception error = result.GetExceptionFromInfo();
            if (error != null)
              throw error;
            else
              throw new InsertFailedException(result.Msg);
          }
          LoadFromDtoBypassPropertyChecks(result.Obj);
        }
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        FieldManager.UpdateChildren(this);

        var userDal = dalManager.GetProvider<IUserDal>();

        var dto = CreateDto();
        var result = userDal.Update(dto);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new UpdateFailedException(result.Msg);
        }
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        
        var result = userDal.Delete(ReadProperty<Guid>(IdProperty));
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException(result.Msg);
        }
      }
    }

#endif

    #endregion

    #endregion
  }
}
