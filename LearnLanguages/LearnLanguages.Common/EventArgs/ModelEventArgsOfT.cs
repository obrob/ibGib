using System;

namespace LearnLanguages.Common.EventArgs
{
  public class ModelEventArgs<TModel> : System.EventArgs
  {
    public ModelEventArgs(TModel model)
    {
      Model = model;
    }

    public TModel Model { get; private set; }
  }
}
