using System;
using System.ComponentModel.Composition;

namespace LearnLanguages.Common.Interfaces
{
  public interface IHaveModelList<TList>
    where TList : class
  {
    TList ModelList { get; set; }
  }
}
