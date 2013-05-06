using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer
{
  [Serializable]
  public class StatusUpdate<TTarget, TProduct> : IStatusUpdate<TTarget, TProduct>
  {
    /// <summary>
    /// For serialization, do not use.
    /// </summary>
    public StatusUpdate()
    {
    }

    /// <summary>
    /// Use this to create an update message instance.  The opportunity, offer, offerResponse are all 
    /// relatively optional.  It just depends on who you want to hear the message.  Whoever published
    /// the opportunity is probably checking for opportunity == null and/or opportunity.id.
    /// Same goes for offer, offer response, and job.  So, set these to null depending on who you do
    /// and don't want to hear the message.
    /// </summary>
    public StatusUpdate(string status,
                        IOpportunity<TTarget, TProduct> opportunity,
                        IOffer<TTarget, TProduct> offer,
                        IOfferResponse<TTarget, TProduct> acceptanceOfferResponse,
                        IJobInfo<TTarget, TProduct> jobInfo,
                        Guid publisherId, 
                        object publisher, 
                        string category, 
                        object information)
    {
      Id = Guid.NewGuid();
      Status = status;
      Opportunity = opportunity;
      Offer = offer;
      AcceptanceOfferResponse = acceptanceOfferResponse;
      JobInfo = jobInfo;
      PublisherId = publisherId;
      Publisher = publisher;
      Category = category;
      Information = information;
      //var id = System.Threading.Thread.CurrentThread.ManagedThreadId;
    }

    /// <summary>
    /// Id of this offer.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Status of the job.  This is the meat of the update message.
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// Opportunity that this update pertains to.
    /// </summary>
    public IOpportunity<TTarget, TProduct> Opportunity { get; private set; }
    /// <summary>
    /// Offer that this update pertains to.
    /// </summary>
    public IOffer<TTarget, TProduct> Offer { get; private set; }
    /// <summary>
    /// The OfferResponse that accepted the job that this update pertains to.  I don't know if this is necessary.
    /// I'm keeping this around because of the thought of a "work permit" or some such.  A complete record...
    /// still a vague notion.
    /// </summary>
    public IOfferResponse<TTarget, TProduct> AcceptanceOfferResponse { get; private set; }
    /// <summary>
    /// JobInfo that this update pertains to.
    /// </summary>
    public IJobInfo<TTarget, TProduct> JobInfo { get; private set; }

    /// <summary>
    /// Id of the publisher of this update.
    /// </summary>
    public Guid PublisherId { get; private set; }
    /// <summary>
    /// Reference to the publisher of this update.  NOT the exchange.
    /// </summary>
    public object Publisher { get; private set;}
    /// <summary>
    /// Any additional information about the status.
    /// </summary>
    public object Information { get; private set; }
    /// <summary>
    /// Category of the job/opportunity/offer.  E.g. Study
    /// </summary>
    public string Category { get; private set; }
  }
}
