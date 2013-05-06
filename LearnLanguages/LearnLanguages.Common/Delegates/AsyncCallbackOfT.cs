using System;
using System.Net;

namespace LearnLanguages.Common.Delegates
{
  /// <summary>
  /// Use this callback if you are dealing with just one class that you need.
  /// E.g. AsyncCallback{PhraseEdit} if you want a result populated with a PhraseEdit or
  /// an Error (exception).
  /// </summary>
  public delegate void AsyncCallback<T>(object sender, ResultArgs<T> result);// where T : class, new();
}
