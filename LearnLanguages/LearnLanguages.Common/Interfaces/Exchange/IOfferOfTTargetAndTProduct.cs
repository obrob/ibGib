using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOffer<TTarget, TProduct> : IOffer
  {
    IOpportunity<TTarget, TProduct> Opportunity { get; }
    double Amount { get; }
  }
}
