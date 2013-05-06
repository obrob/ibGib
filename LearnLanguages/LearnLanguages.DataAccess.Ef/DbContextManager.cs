using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Csla;

namespace LearnLanguages.DataAccess.Ef
{
  public class DbContextManager<C> : IDisposable where C : DbContext
  {
    #region Ctors and Init

    private DbContextManager(string nameOrConnectionString, string label)
    {
      _Label = label;
      _NameOrConnectionString = nameOrConnectionString;
      Context = (C)Activator.CreateInstance(typeof(C), _NameOrConnectionString);
      var debugLoc = Csla.ApplicationContext.LogicalExecutionLocation;
      Context.Database.Connection.Open();
    }

    #endregion

    #region Static Methods

    public static DbContextManager<C> GetManager(string nameOrConnectionString)
    {
      return GetManager(nameOrConnectionString, "default");
    }

    public static DbContextManager<C> GetManager(string nameOrConnectionString, string label)
    {
      DbContextManager<C> manager = null;

      //CHECK IF WE ALREADY HAVE A MANAGER WITH THIS NAME/CONN/LABEL
      var contextManagerKey = GetContextManagerKey(nameOrConnectionString, label);
      if (ApplicationContext.LocalContext.Contains(contextManagerKey))
      {
        //REUSE OUR EXISTENT DBCONTEXT
        manager = (DbContextManager<C>)(ApplicationContext.LocalContext[contextManagerKey]);
      }
      else
      {
        //CREATE NEW DBCONTEXT
        manager = new DbContextManager<C>(nameOrConnectionString, label);
        ApplicationContext.LocalContext[contextManagerKey] = manager;
      }
      manager.AddRef();

      return manager;
    }

    /// <summary>
    /// Right now, just returns the label.  I see no reason to concatenate the name/connection string.
    /// </summary>
    private static string GetContextManagerKey(string nameOrConnectionString, string label)
    {
      if (string.IsNullOrEmpty(label) || string.IsNullOrEmpty(nameOrConnectionString))
        throw new DbContextManagerException(EfResources.ErrorMsgPropertyNotAssigned);

      return label;
    }

    #endregion

    #region Fields and Properties

    private object _Lock = new object();
    private string _Label;
    private string _NameOrConnectionString;
    private int _RefCount;

    public C Context { get; private set; }

    #endregion

    #region Methods

    public void Dispose()
    {
      DeRef();
    }

    private void AddRef()
    {
      _RefCount++;
    }

    private void DeRef()
    {
      lock (_Lock)
      {
        _RefCount--;
        if (_RefCount == 0)
        {
          Context.Database.Connection.Close();
          Context.Dispose();
          ApplicationContext.LocalContext.Remove(GetContextManagerKey(_NameOrConnectionString, _Label));
        }
      }
    }

    #endregion
  }
}