using System;
using System.Net;

namespace LearnLanguages.Common.Delegates
{
  /// <summary>
  /// Callback that just sends exception data.
  /// </summary>
  /// <param name="exception"></param>
  public delegate void ExceptionCheckCallback(Exception exception);
}
