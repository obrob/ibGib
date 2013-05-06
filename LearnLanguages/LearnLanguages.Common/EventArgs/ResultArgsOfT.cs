using System;

namespace LearnLanguages.Common
{
  public class ResultArgs<T> : System.EventArgs
  {
    public ResultArgs() { }
    public ResultArgs(T obj)
    {
      Object = obj;
      Error = null;
    }
    public ResultArgs(Exception error)
    {
      Object = default(T);
      Error = error;
    }

    public T Object { get; set; }
    public Exception Error { get; set; }
  }
}
