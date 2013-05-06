using System;
using System.ComponentModel.Composition;

namespace LearnLanguages.Common.Interfaces
{
  public interface IHaveModel<T>
    where T : class
  {
    T Model { get; set; }
  }
}
