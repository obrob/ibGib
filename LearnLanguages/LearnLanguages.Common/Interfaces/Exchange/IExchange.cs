using System;
using Caliburn.Micro;

namespace LearnLanguages.Common.Interfaces
{
  public interface IExchange
  {
    Guid Id { get; }

    void Publish(IOpportunity opportunity);
    void Publish(IOffer offer);
    void Publish(IOfferResponse offerResponse);
    void Publish(IStatusUpdate workStatusUpdate);

    void SubscribeToOpportunities(object subscriber);
    void UnsubscribeFromOpportunities(object subscriber);

    void SubscribeToOffers(object subscriber);
    void UnsubscribeFromOffers(object subscriber);

    void SubscribeToOfferResponses(object subscriber);
    void UnsubscribeFromOfferResponses(object subscriber);

    void SubscribeToStatusUpdates(object subscriber);
    void UnsubscribeFromStatusUpdates(object subscriber);
  }
}
