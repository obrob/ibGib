using System;
using Caliburn.Micro;
using System.Diagnostics;

namespace LearnLanguages.History
{
  public class ConsoleHistoryListener : IHandle<Bases.HistoryEventBase>
  {
    #region Singleton Pattern Members
    private static volatile ConsoleHistoryListener _Ton;
    private static object _Lock = new object();
    /// <summary>
    /// Singleton instance.  Named "The" for esthetic reasons.
    /// </summary>
    public static ConsoleHistoryListener Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
            {
              _Ton = new ConsoleHistoryListener();
              HistoryPublisher.Ton.SubscribeToEvents(_Ton);
            }
          }
        }

        return _Ton;
      }
    }
    #endregion

    public void Handle(Bases.HistoryEventBase message)
    {
      var name = message.GetType().Name;
      Debug.WriteLine(" ");
      Debug.WriteLine(name);
      Debug.WriteLine(DateTime.Now.ToLongTimeString());
      foreach (var d in message.Doubles)
      {
        Debug.WriteLine(d.Key + " = " + d.Value);
      }
      foreach (var s in message.Strings)
      {
        Debug.WriteLine(s.Key + " = " + s.Value);
      }
      foreach (var i in message.Ints)
      {
        Debug.WriteLine(i.Key + " = " + i.Value);
      }
      foreach (var id in message.Ids)
      {
        Debug.WriteLine(id.Key + " = " + id.Value);
      }
      Debug.WriteLine(" ");
    }
  }
}
