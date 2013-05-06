using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer
{
  [Serializable]
  public class Cancelation : ICancelation
  {
    /// <summary>
    /// For serialization, do not use.
    /// </summary>
    public Cancelation()
    {
    }

    public Cancelation(IOpportunity opportunity, Guid publisherId, object publisher)
    {
      Id = Guid.NewGuid();
      OpportunityId = opportunity.Id;
      PublisherId = publisherId;
      Publisher = publisher;
      Category = opportunity.Category;
      Information = opportunity.Information;
    }

    public Cancelation(Guid opportunityId, Guid publisherId, object publisher, string category, object information)
    {
      Id = Guid.NewGuid();
      OpportunityId = opportunityId;
      PublisherId = publisherId;
      Publisher = publisher;
      Category = category;
      Information = information;
    }

    /// <summary>
    /// Id of this message.
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Opportunity that this message pertains to.
    /// </summary>
    public Guid OpportunityId { get; private set; }
    /// <summary>
    /// Id of the publisher of this message.
    /// </summary>
    public Guid PublisherId { get; private set; }
    /// <summary>
    /// Reference to the publisher of this message.  NOT the exchange.
    /// </summary>
    public object Publisher { get; private set;}
    /// <summary>
    /// Any additional information about the message.
    /// </summary>
    public object Information { get; private set; }
    /// <summary>
    /// Category of the message.  E.g. Study
    /// </summary>
    public string Category { get; private set; }
  }
}
