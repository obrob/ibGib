using System;
using System.Net;
using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess;
using System.Threading.Tasks;
using LearnLanguages.Business.Criteria;
using LearnLanguages.Common.Interfaces.Autonomous;
//For right now, have this compiler directive
#if !SILVERLIGHT
using LearnLanguages.Autonomous.Core;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
#endif

namespace LearnLanguages.Business.ReadOnly.Autonomous
{
  /// <summary>
  /// This ReadOnly Enables the service manager on the server.
  /// </summary>
  [Serializable]
  public class ChangeEnableAutonomousServiceManagerReadOnly : Common.CslaBases.ReadOnlyBase<ChangeEnableAutonomousServiceManagerReadOnly>
  {
    #region Factory Methods

    public static async Task<ChangeEnableAutonomousServiceManagerReadOnly> CreateNewAsync(bool enable)
    {
      var result = await DataPortal.CreateAsync<ChangeEnableAutonomousServiceManagerReadOnly>(enable);
      return result;
    }

    #endregion

    #region Properties

    #region public bool? WasSuccessful

    public static readonly PropertyInfo<bool?> WasSuccessfulProperty = RegisterProperty<bool?>(c => c.WasSuccessful);
    public bool? WasSuccessful
    {
      get { return ReadProperty(WasSuccessfulProperty); }
      set { LoadProperty(WasSuccessfulProperty, value); }
    }

    #endregion


    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public async void DataPortal_Create(bool enable)
    {
      Id = Guid.NewGuid();
      bool? result = null;

      if (!Services.IsInitialized)
      {
        //This is where we do the server-side container initialization. I don't know of a better
        //place to put this, and we have a use case for it right now with the autonomous services.
        CompositionContainer serverContainer = InitializeContainer();
        Services.Initialize(serverContainer);
        serverContainer.SatisfyImportsOnce(this);
      }

      //The service manager should not be null at this point.
      if (_ServiceManager == null)
        throw new Exception("ServiceManager is null. It was not resolved from the container.");

      if (enable)
        result = await _ServiceManager.EnableAsync();
      else
        result = await _ServiceManager.DisableAsync();

      WasSuccessful = result;
    }

    [Import]
    private IAutonomousServiceManager _ServiceManager { get; set; }

    private CompositionContainer InitializeContainer()
    {
      AssemblyCatalog catThis = new AssemblyCatalog(GetType().Assembly);
      AssemblyCatalog catAutonomous = new AssemblyCatalog(typeof(ServiceManager).Assembly);
      AggregateCatalog catAggregate = new AggregateCatalog(catThis, catAutonomous);
      CompositionContainer retContainer = new CompositionContainer(catAggregate);
      
      return retContainer;
    }
#endif

    #endregion
  }
}
