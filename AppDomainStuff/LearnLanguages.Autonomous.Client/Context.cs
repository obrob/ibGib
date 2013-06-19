using LearnLanguages.Common.Enums.Autonomous;
using LearnLanguages.Common.Interfaces.Autonomous;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
namespace LearnLanguages.Autonomous
{
  public class Context : IAutonomousServiceContext
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

    public AutonomousServiceContextStates State
    {
      get { throw new NotImplementedException(); }
    }

    public Task<bool> TryLoadAsync(IAutonomousService service)
    {
      throw new NotImplementedException();
    }
  }
}
