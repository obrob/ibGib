using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IJobInfo<TTarget, TProduct> : IJobInfo
  {
    TTarget Target { get; }
    TProduct Product { get; set; }
  }
}
