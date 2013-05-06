using System.Collections.Generic;

namespace LearnLanguages
{
  /// <summary>
  /// Slightly fancier candy wrapper for Dictionary(_string, object_)
  /// When you want to pass just an args object and use magic strings
  /// to store individual arguments, use this.  Panacea argument object basically.  Uses an 
  /// indexer for quicker access to the Members property.
  /// </summary>
  public class MagicStringsList
  {
    public MagicStringsList()
    {
      this.Members = new Dictionary<string, object>();
    }

    public MagicStringsList(params Tuple<string, object>[] initialMembers)
      : this()
    {
      foreach (Tuple<string, object> arg in initialMembers)
      {
        string argMagicStringIdentifer = arg.Item1;
        object argObject = arg.Item2;
        this.Members.Add(argMagicStringIdentifer, argObject);
      }
    }

    /// <summary>
    /// Use this constructor if you want to use the MainObject property of this class.
    /// </summary>
    /// <param name="mainObject">The main object which this class pertains to...e.g. the object to be saved in a database, as opposed to the args on _how_ to save that object</param>
    /// <param name="otherMembers">The other members that describe what to do with the MainObject.  E.g. result, table, other specifiers and what have you.</param>
    public MagicStringsList(object mainObject, params Tuple<string, object>[] otherMembers)
      : this(otherMembers)
    {
      this.MainObject = mainObject;
    }

    //Indexer
    public object this[string magicStringIdentifier]
    {
      get
      {
        return this.Members[magicStringIdentifier];
      }
      set
      {
        this.Members[magicStringIdentifier] = value;
      }
    }

    public Dictionary<string, object> Members { get; protected set; }
    
    public bool Contains(string magicStringIdentifier)
    {
      return Members.ContainsKey(magicStringIdentifier);
    }

    public object MainObject
    {
      get
      {
        return this.Members[CommonResources.MainObjectKey];
      }
      set
      {
        this.Members[CommonResources.MainObjectKey] = value;
      }
    }


  }
}
