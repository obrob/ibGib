using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Mock
{
  public class DalManager : IDalManager
  {
    private static string _TypeMask = typeof(DalManager).FullName.Replace("DalManager", @"{0}");

    public T GetProvider<T>() where T : class
    {
      var typeName = string.Format(_TypeMask, typeof(T).Name.Substring(1)); //substring to take out the I in interface
      var type = Type.GetType(typeName);
      if (type != null)
        return Activator.CreateInstance(type) as T;
      else
        throw new NotImplementedException(typeName);
    }

    public void Dispose()
    {
      //todo: implement dalmanager.dispose
    }
  }
}
