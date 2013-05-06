using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer
{
  [Serializable]
  public class Opportunity<TTarget, TProduct> : IOpportunity<TTarget, TProduct>
  {
    /// <summary>
    /// For serialization, do not use.
    /// </summary>
    public Opportunity()
    {
    }
    /// <summary>
    /// Use this ctor.  Do not use default ctor.
    /// </summary>
    public Opportunity(Guid publisherId, object publisher, IJobInfo<TTarget, TProduct> jobInfo, 
      string category)
    {
      Id = Guid.NewGuid();
      //OpportunityId = Guid.NewGuid();
      PublisherId = publisherId;
      Publisher = publisher;
      JobInfo = jobInfo;
      Category = category;
      //Information = information;
    }

    /// <summary>
    /// Id of this offer.
    /// </summary>
    public Guid Id { get; private set; }
    ///// <summary>
    ///// Opportunity that this offer pertains to.
    ///// </summary>
    //public Guid OpportunityId { get; private set; }
    /// <summary>
    /// Id of the publisher of this offer.
    /// </summary>
    public Guid PublisherId { get; private set; }
    /// <summary>
    /// Reference to the publisher of this offer.  NOT the exchange.
    /// </summary>
    public object Publisher { get; private set;}
    /// <summary>
    /// Target for this opportunity.
    /// </summary>
    public IJobInfo<TTarget, TProduct> JobInfo { get; private set; }
    /// <summary>
    /// Information about this opportunity.  This can include anything from configuration of the Target, advice,
    /// or specifications about the Target, or who knows what.  It is wide open for implementation.
    /// </summary>
    public object Information { get { return JobInfo; } }
    /// <summary>
    /// Category of the opportunity.  E.g. Study, Test, Review, etc.
    /// </summary>
    public string Category { get; private set; }
  }
}
