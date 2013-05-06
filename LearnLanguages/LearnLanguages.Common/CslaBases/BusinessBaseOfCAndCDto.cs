using System;
using Csla;
using Csla.Serialization;
using Csla.DataPortalClient;
using System.Threading.Tasks;

namespace LearnLanguages.Common.CslaBases
{
  [Serializable]
  public abstract class BusinessBase<C, CDto> : Csla.BusinessBase<C>
    where C : CslaBases.BusinessBase<C, CDto>
    where CDto : class
  {

    #region Business Properties and Methods

    #region public Guid Id
    public static readonly PropertyInfo<Guid> IdProperty = RegisterProperty<Guid>(c => c.Id);
    public Guid Id
    {
      get { return GetProperty(IdProperty); }
      set { SetProperty(IdProperty, value); }
    }
    #endregion

    public abstract CDto CreateDto();

    public virtual void Child_Create(CDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);
    }

    public virtual void LoadFromDtoBypassPropertyChecks(CDto dto)
    {
      LoadFromDtoBypassPropertyChecksImpl(dto);
      BusinessRules.CheckRules();
    }
    public abstract void LoadFromDtoBypassPropertyChecksImpl(CDto dto);

    protected virtual void SetIdBypassPropertyChecks(Guid id)
    {
      using (BypassPropertyChecks)
      {
        Id = id;
      }
    }

    //public override Task<C> SaveAsync(bool forceUpdate)
    //{
    //  //EXPOSES METHOD TO DESCENDANTS
    //  return base.SaveAsync(forceUpdate);
    //}

    protected override Task<C> SaveAsync(bool forceUpdate, object userState, bool isSync)
    {
      //EXPOSES METHOD TO DESCENDANTS
      return base.SaveAsync(forceUpdate, userState, isSync);
    }

    #endregion

    #region DP_XYZ (All: .net, sl, child)

    protected override void DataPortal_Create()
    {
      //THIS GETS RID OF THE DEFAULT RUNLOCAL ATTRIBUTE ON THE CSLA.BUSINESSBASE.
      //EXPOSES METHOD TO DESCENDANTS
      base.DataPortal_Create();
    }

    protected override void DataPortal_Insert()
    {
      //EXPOSES METHOD TO DESCENDANTS
      base.DataPortal_Insert();
    }

    /// <summary>
    /// For some reason, this isn't seen in the children classes, so I'm making it abstract here.
    /// </summary>
    /// <param name="id"></param>
    protected virtual void Child_Fetch(Guid id)
    {
      //wtf!!!!
      var execLocation = Csla.ApplicationContext.ExecutionLocation;
      var logExecLocation = Csla.ApplicationContext.LogicalExecutionLocation;
      var wtf = @"WTF!!!!!!!!!!!!!";
    }

    #endregion

  }
}
