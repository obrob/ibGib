using System;
using System.Linq;
using Csla;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.DataAccess;
#if !SILVERLIGHT
using LearnLanguages.DataAccess.Exceptions;
#endif
using LearnLanguages.Business.Security;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  [Serializable]
  public class LanguageEdit : Common.CslaBases.BusinessBase<LanguageEdit, LanguageDto>, IHaveId
  {
    #region Factory Methods

    #region Wpf Factory Methods
#if !SILVERLIGHT

    public static LanguageEdit NewLanguageEdit()
    {
      return DataPortal.Create<LanguageEdit>();
    }

    public static LanguageEdit NewLanguageEdit(Guid id)
    {
      return DataPortal.Create<LanguageEdit>(id);
    }

    public static LanguageEdit GetLanguageEdit(Guid id)
    {
      return DataPortal.Fetch<LanguageEdit>(id);
    }

    public static LanguageEdit GetLanguageEdit(string languageText)
    {
      return DataPortal.Fetch<LanguageEdit>(languageText);
    }

    public static Guid GetDefaultLanguageId()
    {
      var allLanguages = LanguageList.GetAll();
      Guid defaultLanguageId = Guid.Empty;
      try
      {
        defaultLanguageId = (from language in allLanguages
                             where language.Text == DalResources.DefaultEnglishLanguageText
                             select language).First().Id;
      }
      catch (Exception ex)
      {
        throw new DataAccess.Exceptions.GeneralDataAccessException(ex);
      }
      return defaultLanguageId;
    }

#endif
    #endregion

    #region Async Factory Methods

    /// <summary>
    /// Created on the server.
    /// </summary>
    public static async Task<LanguageEdit> NewLanguageEditAsync()
    {
      return await DataPortal.CreateAsync<LanguageEdit>();
    }

    public static async Task<LanguageEdit> GetLanguageEditAsync(Guid id)
    {
      return await DataPortal.FetchAsync<LanguageEdit>(id);
    }

    public static async Task<LanguageEdit> GetLanguageEditAsync(string languageText)
    {
      LanguageEdit retLanguage = null;

      //HACK: GETLANGUAGEEDIT BY LANGUAGE TEXT.  RIGHT NOW, THIS GETALL'S THE LANGUAGES AND LOOKS IN THAT RESULT..  NEED TO IMPLEMENT THIS IN LANGUAGEDAL (GETLANGUAGEBYTEXT).
      var allLanguages = await LanguageList.GetAllAsync();
      try
      {
        var results = (from language in allLanguages
                       where language.Text == languageText
                       select language);

        if (results.Count() > 0)
          retLanguage = results.First();

        return retLanguage;
      }
      catch (Exception ex)
      {
        throw new DataAccess.Exceptions.GeneralDataAccessException(ex);
      }
    }

    public static async Task<Guid> GetDefaultLanguageIdAsync()
    {
      var allLanguages = await LanguageList.GetAllAsync();

      Guid defaultLanguageId = Guid.Empty;
      try
      {
        defaultLanguageId = (from language in allLanguages
                             where language.Text == DalResources.DefaultEnglishLanguageText
                             select language).First().Id;

        return defaultLanguageId;
      }
      catch (Exception ex)
      {
        throw new DataAccess.Exceptions.GeneralDataAccessException(ex);
      }
    }

    #endregion

    #region Shared Factory Methods

    /// <summary>
    /// Does NOT use the data portal.  This just news up an instance and plugs in the 
    /// properties bypassing property checks.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static LanguageEdit NewLanguageEdit(LanguageDto dto)
    {
      LanguageEdit retPhrase = new LanguageEdit();
      retPhrase.LoadFromDtoBypassPropertyChecks(dto);
      retPhrase.BusinessRules.CheckRules();
      return retPhrase;
    }
    #endregion //Shared Factory Methods

    #endregion //Factory Methods

    #region Business Properties & Methods

    //LANGUAGE
    #region public string Text
    public static readonly PropertyInfo<string> TextProperty = RegisterProperty<string>(c => c.Text);
    public string Text
    {
      get { return GetProperty(TextProperty); }
      set { SetProperty(TextProperty, value); }
    }
    #endregion

    //USER
    #region public Guid UserId
    public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(c => c.UserId);
    public Guid UserId
    {
      get { return GetProperty(UserIdProperty); }
      set { SetProperty(UserIdProperty, value); }
    }
    #endregion
    #region public string Username
    public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(c => c.Username);
    public string Username
    {
      get { return GetProperty(UsernameProperty); }
      set { SetProperty(UsernameProperty, value); }
    }
    #endregion
    #region public UserIdentity User
    public static readonly PropertyInfo<UserIdentity> UserProperty =
      RegisterProperty<UserIdentity>(c => c.User, RelationshipTypes.Child);
    public UserIdentity User
    {
      get { return GetProperty(UserProperty); }
      private set { LoadProperty(UserProperty, value); }
    }
    #endregion

    public override void LoadFromDtoBypassPropertyChecksImpl(LanguageDto dto)
    {
      using (BypassPropertyChecks)
      {
        LoadProperty<Guid>(IdProperty, dto.Id);
        LoadProperty<string>(TextProperty, dto.Text);
        LoadProperty<string>(TextProperty, dto.Text);
        LoadProperty<Guid>(UserIdProperty, dto.UserId);
        LoadProperty<string>(UsernameProperty, dto.Username);
        //if (!string.IsNullOrEmpty(dto.Username))
        //  User = DataPortal.FetchChild<UserIdentity>(dto.Username);
      }
    }

    public override LanguageDto CreateDto()
    {
      LanguageDto retDto = new LanguageDto()
      {
        Id = this.Id,
        Text = this.Text,
        UserId = this.UserId,
        Username = this.Username
      };

      return retDto;
    }

    protected override async Task<LanguageEdit> SaveAsync(bool forceUpdate, object userState, bool isSync)
    {
      var result = await base.SaveAsync(forceUpdate, userState, isSync);
      return result;
    }

    /// <summary>
    /// Loads the default properties, including generating a new Id, inside of a using (BypassPropertyChecks) block.
    /// </summary>
    protected void LoadDefaults()
    {
      using (BypassPropertyChecks)
      {
        Id = Guid.Empty;
        Text = DalResources.DefaultNewLanguageText;
      }
    }

    public override string ToString()
    {
      return Text;
    }

    #region Equals overrides

    public override int GetHashCode()
    {
      //THREE THINGS MAKE IT THE SAME: USERID, LANGUAGEID, AND LANGUAGE TEXT
      var userIdString = "userId=" + UserId.ToString();
      var languageIdString = "languageId=" + Id.ToString();
      var languageTextString = "languageText=" + Text;

      return (userIdString + "|" + languageIdString + "|" + languageTextString).GetHashCode();
    }

    public override bool Equals(System.Object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      LanguageEdit languageEdit = obj as LanguageEdit;
      if ((System.Object)languageEdit == null)
      {
        return false;
      }
      
      // Return true if the fields match:
      return Text == languageEdit.Text && 
             Id == languageEdit.Id;
    }
    public static bool operator ==(LanguageEdit a, LanguageEdit b)
    {
      // If both are null, or both are same instance, return true.
      if (System.Object.ReferenceEquals(a, b))
      {
        return true;
      }

      // If one is null, but not both, return false.
      if (((object)a == null) || ((object)b == null))
      {
        return false;
      }

      // Return true if the fields match:
      return a.Id == b.Id && 
             a.Text == b.Text;
    }
    public static bool operator !=(LanguageEdit a, LanguageEdit b)
    {
      return !(a == b);
    }

    #endregion

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();

      // TODO: add validation rules
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(TextProperty));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // TODO: add object-level authorization rules
      // USER MUST BE AUTHENTICATED TO BE IN THESE ROLES
      Csla.Rules.BusinessRules.AddRule(typeof(LanguageEdit),
        new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject, 
          DalResources.RoleAdmin, DalResources.RoleUser));
      Csla.Rules.BusinessRules.AddRule(typeof(LanguageEdit),
        new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.GetObject,
          DalResources.RoleAdmin, DalResources.RoleUser));
      Csla.Rules.BusinessRules.AddRule(typeof(LanguageEdit),
        new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.EditObject,
          DalResources.RoleAdmin, DalResources.RoleUser));
      Csla.Rules.BusinessRules.AddRule(typeof(LanguageEdit),
        new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.DeleteObject,
          DalResources.RoleAdmin, DalResources.RoleUser));

    }

    #endregion

    #region Data Access (This is run on the server, unless run local set)

    #region WPF DP_XYZ

#if !SILVERLIGHT && !NETFX_CORE

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();
        var result = languageDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        LanguageDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    protected void DataPortal_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();
        Result<LanguageDto> result = languageDal.Fetch(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException();
        }
        LanguageDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    protected void DataPortal_Fetch(string languageText)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();
        Result<LanguageDto> result = languageDal.Fetch(languageText);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException();
        }
        LanguageDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      //Dal is responsible for setting new Id
      var dto = CreateDto();

      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Insert(dto);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new InsertFailedException();
          
        }
        SetIdBypassPropertyChecks(result.Obj.Id);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();
        var dto = CreateDto();

        Result<LanguageDto> result = languageDal.Update(dto);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new UpdateFailedException();
        }
      
        SetIdBypassPropertyChecks(result.Obj.Id);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Delete(ReadProperty<Guid>(IdProperty));
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException();
        }
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Delete(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Delete(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException();
        }
      }
    }
#endif
    
    #endregion

    #region Child DP_XYZ

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Create(Guid id)
    {
      using (BypassPropertyChecks)
      {
        this.Id = id;
        this.Text = "";
      }
    }

#if !SILVERLIGHT

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Child_Create()
    {
      using (BypassPropertyChecks)
      {
        Id = Guid.Empty;
        Text = "";
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(LanguageDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Child_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Fetch(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        LanguageDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(string languageText)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Fetch(languageText);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        LanguageDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Insert()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = languageDal.Insert(dto);
          if (!result.IsSuccess)
          {
            Exception error = result.GetExceptionFromInfo();
            if (error != null)
              throw error;
            else
              throw new InsertFailedException(result.Msg);
          }
          Id = result.Obj.Id;
        }
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        using (BypassPropertyChecks)
        {
          var result = languageDal.Update(CreateDto());
          if (!result.IsSuccess)
          {
            Exception error = result.GetExceptionFromInfo();
            if (error != null)
              throw error;
            else
              throw new UpdateFailedException(result.Msg);
          }
          Id = result.Obj.Id;
        }
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Delete(Id);
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
