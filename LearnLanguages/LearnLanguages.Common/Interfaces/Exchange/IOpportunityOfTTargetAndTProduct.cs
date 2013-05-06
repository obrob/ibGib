using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOpportunity<TTarget, TProduct> : IOpportunity
  {
    IJobInfo<TTarget, TProduct> JobInfo { get; }
  }
}
