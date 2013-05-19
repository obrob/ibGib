using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearnLanguages.DataAccess.Ef
{

  public static class EfExtensions
  {
    private static void ClearEntityCollection<TEntity>(EntityCollection<TEntity> collection)
      where TEntity : class
    {
      collection.Clear();
    }

    #region public static void DeleteAll<TData>(this ObjectSet<TData> collection, bool saveChanges = true) where TData: EntityObject

    /// <summary>
    /// Deletes all entity objects in a collection. 
    /// 
    /// WARNING: This will also delete any related objects in other collections, as well, as 
    /// this is originally made to use when deleting the entire database. This can be changed 
    /// to delete only references to objects in this collection in those other objects, but it 
    /// ain't worth the effort...and also..THIS IS NOT THREAD SAFE!!!!
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="collection"></param>
    /// <param name="saveChanges"></param>
    public static void DeleteAll<TData>(this ObjectSet<TData> collection, bool saveChanges = true)
      where TData: EntityObject
    {
      _QueuedEntityObjectsToDelete = new List<EntityObject>();
      try
      {
        var datasToDelete = new List<TData>();

        foreach (var data in collection)
          datasToDelete.Add(data);
        for (int i = 0; i < datasToDelete.Count; i++)
        {
          var toDelete = datasToDelete[i];
          DeleteEntityObject(toDelete, collection.Context);
        }

        if (saveChanges && datasToDelete.Count > 0)
          collection.Context.SaveChanges();
      }
      finally
      {
        _QueuedEntityObjectsToDelete = null;
      }
    }

    private static List<EntityObject> _QueuedEntityObjectsToDelete { get; set; }

    private static void DeleteEntityObject<TData>(TData dataToDelete, ObjectContext context)
      where TData : EntityObject
    {
      if (_QueuedEntityObjectsToDelete.Contains(dataToDelete))
        return;
      else
        _QueuedEntityObjectsToDelete.Add(dataToDelete);

      //delete nav properties (EntityCollection)
      dataToDelete
        .GetType()
        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
        .Where(pi => IsEntityCollection(pi.PropertyType))
        .ToList()
        .ForEach((pi) =>
        {
          var navEntities = (dynamic)pi.GetValue(dataToDelete);
          List<object> navEntitiesAsList = new List<object>();
          foreach (var navEntity in navEntities)
            navEntitiesAsList.Add(navEntity);
          
          for (int i = 0; i < navEntitiesAsList.Count; i++)
          {
            var relatedToDelete = (EntityObject)navEntitiesAsList[i];
            DeleteEntityObject(relatedToDelete, context);
          }
          navEntities.Clear();
        });

      //delete nav properties (other data objects, e.g. UserData)
      dataToDelete
        .GetType()
        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
        .Where(pi => IsEntityObject(pi.PropertyType))
        .ToList()
        .ForEach((pi) =>
        {
          DeleteEntityObject(dataToDelete, context);
          pi.SetValue(dataToDelete, null);
        });

      context.DeleteObject(dataToDelete);
    }
    private static bool TypeEqualsOrDescendsFrom(Type type, Type targetAncestorType)
    {
      //Work our way up the type heirarchy, looking for equality of a testType == targetAncestorType
      bool isGeneric = targetAncestorType.IsGenericType;
      var testType = type;
      while (testType != null)
      {
        if (isGeneric)
        {
          if (testType.IsGenericType && testType.GetGenericTypeDefinition() == targetAncestorType)
            return true;
        }
        else
        {
          if (!testType.IsGenericType && testType == targetAncestorType)
            return true;
        }

        //Work our way up
        testType = testType.BaseType;
      }
      return false;
    }
    private static bool IsEntityObject(Type type)
    {
      return TypeEqualsOrDescendsFrom(type, typeof(EntityObject));
    }
    private static bool IsEntityCollection(Type type)
    {
      return TypeEqualsOrDescendsFrom(type, typeof(EntityCollection<>));
    }

    #endregion

  }
}
