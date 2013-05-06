using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer
{
  [Serializable]
  public class Offer<TTarget, TProduct> : IOffer<TTarget, TProduct>
  {
    /// <summary>
    /// For serialization, do not use.
    /// </summary>
    public Offer()
    {
    }

    public Offer(IOpportunity<TTarget, TProduct> opportunity, Guid publisherId, object publisher, double amount,
      string category, object information)
    {
      Id = Guid.NewGuid();
      Opportunity = opportunity;
      PublisherId = publisherId;
      Publisher = publisher;
      Amount = amount;
      Category = category;
      Information = information;
    }

    /// <summary>
    /// Id of this offer.
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Opportunity that this offer pertains to.
    /// </summary>
    public IOpportunity<TTarget, TProduct> Opportunity { get; private set; }
    /// <summary>
    /// Id of the publisher of this offer.
    /// </summary>
    public Guid PublisherId { get; private set; }
    /// <summary>
    /// Reference to the publisher of this offer.  NOT the exchange.
    /// </summary>
    public object Publisher { get; private set;}
    /// <summary>
    /// Amount offered to do this work.
    /// </summary>
    public double Amount { get; private set; }
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
