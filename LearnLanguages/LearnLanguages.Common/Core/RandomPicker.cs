using System;
using System.Collections;
using System.Collections.Generic;

namespace LearnLanguages.Common
{
  public class RandomPicker
  {
    public RandomPicker()
    {
      NewRandomizer();
    }

    private Random Randomizer { get; set; }

    #region Singleton Pattern Members
    private static volatile RandomPicker _Ton;
    private static object _Lock = new object();
    public static RandomPicker Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new RandomPicker();
          }
        }

        return _Ton;
      }
    }
    #endregion

    public void NewRandomizer()
    {
      Randomizer = new Random(DateTime.Now.Second + DateTime.Now.Millisecond + DateTime.Now.Hour);
    }

    public T PickOne<T>(T aObject, T bObject, out T loser, double aWeight = 0.5d, double bWeight = 0.5d)
    {
      if (aObject == null)
        throw new ArgumentNullException("aObject");
      if (bObject == null)
        throw new ArgumentNullException("bObject");
      if (aWeight < 0)
        throw new ArgumentException("aWeight");
      if (bWeight < 0)
        throw new ArgumentException("bWeight");

      double probabilityA = aWeight / (aWeight + bWeight);
      var randomDouble = Randomizer.NextDouble();
      if (randomDouble < probabilityA)
      {
        loser = bObject;
        return aObject;
      }
      else
      {
        loser = aObject;
        return bObject;
      }
    }
    public object PickOne(object aObject, object bObject, out object loser, double aWeight = 0.5d, 
      double bWeight = 0.5d)
    {
      return PickOne<object>(aObject, bObject, out loser, aWeight, bWeight);
    }

    public int NextInt(int minInclusive, int maxExclusive)
    {
      return Randomizer.Next(minInclusive, maxExclusive);
    }

    public T PickOne<T>(IList<T> list) 
    {
      if (list.Count == 0)
        return default(T);

      var i = Randomizer.Next(0, list.Count);
      var picked = list[i];
      return picked;
    }
  }
}
