using LearnLanguages.Common.Enums.Autonomous;
using LearnLanguages.Common.Exceptions;
using LearnLanguages.Common.Interfaces.Autonomous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnLanguages.Autonomous.Bases
{
  public abstract class AutonomousServiceBase : IAutonomousService
  {
    public Guid Id { get; protected set; }
    public string Name { get; protected set; }

    public abstract bool Enable();
    public abstract bool Disable();

    public bool CanLoad
    {
      get { return CanLoadImpl(); }
    }
    /// <summary>
    /// Base implementation checks for state is killed, executed,
    /// canceled, unloaded, or loaded. NB this means that 
    /// double-loading is possible.
    /// </summary>
    /// <returns></returns>
    protected virtual bool CanLoadImpl()
    {
      var canLoad = State == AutonomousServiceStates.Killed ||
                    State == AutonomousServiceStates.Executed ||
                    State == AutonomousServiceStates.Canceled ||
                    State == AutonomousServiceStates.Unloaded ||
                    State == AutonomousServiceStates.Loaded;

      return canLoad;
    }
    public bool CanExecute
    {
      get { return CanExecuteImpl(); }
    }
    /// <summary>
    /// Base implementation checks for state is Loaded only.
    /// </summary>
    /// <returns></returns>
    protected virtual bool CanExecuteImpl()
    {
      return State == AutonomousServiceStates.Loaded;
    }

    public void Execute(int timeAllowedInMs)
    {
      if (!CanExecute)
        throw new AutonomousServiceExecuteException(Name, null);
      State = AutonomousServiceStates.Executing;
      try
      {
        ExecuteImpl(timeAllowedInMs);
      }
      catch (Exception ex)
      {
        State = AutonomousServiceStates.ExecuteError;
        throw new AutonomousServiceExecuteException(Name, ex);
      }

      State = AutonomousServiceStates.Executed;
    }
    protected abstract void ExecuteImpl(int timeAllowedInMs);
    public void Cancel(int timeAllowedInMs)
    {
      State = AutonomousServiceStates.Executing;
      try
      {
        CancelImpl(timeAllowedInMs);
      }
      catch (Exception ex)
      {
        State = AutonomousServiceStates.CancelError;
        throw new AutonomousServiceCancelException(Name, ex);
      }

      State = AutonomousServiceStates.Canceled;
    }
    protected abstract void CancelImpl(int timeAllowedInMs);
    public void Load(int timeAllowedInMs)
    {
      if (!CanLoad)
        throw new AutonomousServiceLoadException(Name, null);
      State = AutonomousServiceStates.Executing;
      try
      {
        LoadImpl(timeAllowedInMs);
      }
      catch (Exception ex)
      {
        State = AutonomousServiceStates.LoadError;
        throw new AutonomousServiceLoadException(Name, ex);
      }

      State = AutonomousServiceStates.Loaded;
    }
    protected abstract void LoadImpl(int timeAllowedInMs);

    #region public bool IsEnabled

    private object __IsEnabledLock = new object();
    private bool _IsEnabled;
    /// <summary>
    /// ThreadSafe locked property. Override Get/SetIsEnabled to avoid
    /// using the lock.
    /// </summary>
    public bool IsEnabled
    {
      get { return GetIsEnabled(); }
      set { SetIsEnabled(value); }
    }

    protected virtual bool GetIsEnabled()
    {
      lock (__IsEnabledLock)
      {
        return _IsEnabled;
      }

    }
    protected virtual void SetIsEnabled(bool value)
    {
      lock (__IsEnabledLock)
      {
        _IsEnabled = value;
      }
    }

    #endregion
    
    #region public AutonomousServiceStates State

    private object __StateLock = new object();
    private AutonomousServiceStates _State;
    /// <summary>
    /// ThreadSafe property by default. Can override GetState and SetState
    /// to bypass using the __StateLock object.
    /// </summary>
    public AutonomousServiceStates State
    {
      get { return GetState(); }
      protected set { SetState(value); }
    }

    protected virtual void SetState(AutonomousServiceStates value)
    {
      lock (__StateLock)
      {
        _State = value;
      }
    }
    protected virtual AutonomousServiceStates GetState()
    {
      lock (__StateLock)
      {
        return _State;
      }
    }

    #endregion

  }
}
