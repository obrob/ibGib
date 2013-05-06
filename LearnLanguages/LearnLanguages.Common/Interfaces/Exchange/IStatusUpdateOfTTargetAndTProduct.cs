using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IStatusUpdate<TTarget, TProduct> : IStatusUpdate
  {
    IOpportunity<TTarget, TProduct> Opportunity { get; }
    IOffer<TTarget, TProduct> Offer { get; }
    IOfferResponse<TTarget, TProduct> AcceptanceOfferResponse { get; }
    IJobInfo<TTarget, TProduct> JobInfo { get; }
  }
}
