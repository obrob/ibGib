using LearnLanguages.Common.Enums.Autonomous;
using LearnLanguages.Common.Exceptions;
using LearnLanguages.Common.Interfaces.Autonomous;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace LearnLanguages.Autonomous
{
  [Serializable]
  public class AppDomainContext : MarshalByRefObject, IAutonomousServiceContext
  {

    public void Abort()
    {
      throw new NotImplementedException();
    }

    public int AllowedAbortTime
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int AllowedExecuteTime
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int AllowedLoadTime
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public Task ExecuteAsync()
    {
      throw new NotImplementedException();
    }

    public IAutonomousService Service
    {
      get { throw new NotImplementedException(); }
    }

    public AutonomousServiceContextStates State { get; private set;}

    public bool TryLoadService(IAutonomousService service)
    {
      State = AutonomousServiceContextStates.Loading;
      try
      {
        if (!service.IsEnabled)
          throw new ServiceNotEnabledException(service.Name, null);
        service.Load();
      }
      catch (Exception ex)
      {
        //Don't care what the exception is as far as the flow of code is concerned.
        //Log the error, though not sure how to do that with the app domain.
        
        State = AutonomousServiceContextStates.LoadError;
      }
      State = AutonomousServiceContextStates.Loaded;
      return true;
    }
  }
}
