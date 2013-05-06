using Caliburn.Micro;

namespace LearnLanguages.Common.Interfaces
{
  /// <summary>
  /// The ILogger can operate in two ways.
  /// 
  /// 1) (Old way)
  /// You can use it directly, with the Log method.
  /// 
  /// 2) (New cool way) You can publish a MessageEvent to the relevant event
  /// aggregator. The implementation of this ILogger interface should interpret 
  /// those MessageEvents and call the Log method using the message event's 
  /// properties.
  /// 
  /// Left off here...need to put this into design documents.
  /// </summary>
  public interface ILogger : IHandle<Events.MessageEvent>
  {
    void Log(string msg, LogPriority priority, LogCategory category);
  }
}