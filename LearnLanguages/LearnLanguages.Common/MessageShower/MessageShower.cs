using System;

namespace LearnLanguages.Common.MessageShower
{
  public class MessageShower
  {
    #region Singleton
    private static volatile MessageShower _Ton;
    private static object syncRoot = new Object();

    private MessageShower() { }

    public static MessageShower Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (syncRoot)
          {
            if (_Ton == null)
              _Ton = new MessageShower();
          }
        }

        return _Ton;
      }
    }
    #endregion

    public void ShowModal(MessageType type, string msg)
    {
      switch (type)
      {
        case MessageType.Information:
          //Services.WindowManager.ShowDialog(
          break;
        case MessageType.Warning:
          break;
        case MessageType.Error:
          break;
        default:
          break;
      }
    }
  }
}
