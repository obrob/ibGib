using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer
{
  [Serializable]
  public class ConglomerateMessage : IConglomerateMessage
  {
    /// <summary>
    /// For serialization, do not use.
    /// </summary>
    public ConglomerateMessage()
    {
    }

    public ConglomerateMessage(Guid conglomerateId, Guid publisherId, object publisher, 
      string category, object information)
    {
      Id = Guid.NewGuid();
      ConglomerateId = conglomerateId;
      PublisherId = publisherId;
      Publisher = publisher;
      Category = category;
      Information = information;
    }

    /// <summary>
    /// Id of this offer.
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Id of conglomerate that this message pertains to.  This is like an internal message in a company, 
    /// or a family.
    /// </summary>
    public Guid ConglomerateId { get; private set; }
    /// <summary>
    /// Id of the publisher of this offer.
    /// </summary>
    public Guid PublisherId { get; private set; }
    /// <summary>
    /// Reference to the publisher of this offer.  NOT the exchange.
    /// </summary>
    public object Publisher { get; private set;}
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
