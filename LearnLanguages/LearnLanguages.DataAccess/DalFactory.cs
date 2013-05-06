using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace LearnLanguages.DataAccess
{
  public static class DalFactory
  {
    private static Type _DalType;

    public static IDalManager GetDalManager()
    {
      if (_DalType == null)
      {
        var dalTypeName = ConfigurationManager.AppSettings[DalResources.AppSettingsKeyDalManagerTypeName];
        if (!string.IsNullOrEmpty(dalTypeName))
          _DalType = Type.GetType(dalTypeName);
        else 
          throw new NullReferenceException(DalResources.AppSettingsKeyDalManagerTypeName);

        if (_DalType == null)
          throw new ArgumentException(string.Format("Type {0} could not be found.", dalTypeName));
      }
      return (IDalManager)Activator.CreateInstance(_DalType);
    }
  }
}
