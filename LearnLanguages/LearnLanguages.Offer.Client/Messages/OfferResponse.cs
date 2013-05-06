using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer
{
  [Serializable]
  public class OfferResponse<TTarget, TProduct> : IOfferResponse<TTarget, TProduct>
  {
    /// <summary>
    /// Do not use this.  For serialization only.
    /// </summary>
    public OfferResponse()
    {
      
    }

    public OfferResponse(IOffer<TTarget, TProduct> offer, Guid publisherId, object publisher, 
      string response, string category, object information)
    {
      Id = Guid.NewGuid();
      Offer = offer;
      PublisherId = publisherId;
      Publisher = publisher;
      Response = response;
      Category = category;
      Information = information;
    }

    public Guid Id { get; private set; }
    public IOffer<TTarget, TProduct> Offer { get; private set; }
    public Guid PublisherId { get; private set; }
    public object Publisher { get; private set; }
    public string Response { get; private set; }
    /// <summary>
    /// Any additional information about the offer.
    /// </summary>
    public object Information { get; private set; }
    /// <summary>
    /// Category of the offer.  E.g. Study
    /// </summary>
    public string Category { get; private set; }
  }
}
