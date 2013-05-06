using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  /// <summary>
  /// Simple IStudyPartner base class.  You do not HAVE to descend from this, it just provides some 
  /// very basic plumbing.
  /// </summary>
  public abstract class StudyPartnerBase : IStudyPartner
  {
    public StudyPartnerBase()
    {
      Id = Guid.NewGuid();
      _OpenOffers = new List<IOffer<MultiLineTextList, IViewModelBase>>();
      _DeniedOffers = new List<IOffer<MultiLineTextList, IViewModelBase>>();
      _CurrentOffer = null;

      Exchange.Ton.SubscribeToOpportunities(this);
      Exchange.Ton.SubscribeToOfferResponses(this);
      Exchange.Ton.SubscribeToStatusUpdates(this);
      Exchange.Ton.SubscribeToCancelations(this);
    }

    public Guid Id { get; protected set; }
    public Guid ConglomerateId { get; protected set; }

    protected List<IOffer<MultiLineTextList, IViewModelBase>> _OpenOffers { get; set; }
    protected List<IOffer<MultiLineTextList, IViewModelBase>> _DeniedOffers { get; set; }
    protected IOffer<MultiLineTextList, IViewModelBase> _CurrentOffer { get; set; }

    protected MultiLineTextList GetCurrentTarget() 
    {
      return _CurrentOffer.Opportunity.JobInfo.Target;
    }

    /// <summary>
    /// Analyze opportunities published on Exchange.
    /// </summary>
    public abstract void Handle(IOpportunity<MultiLineTextList, IViewModelBase> message);
    /// <summary>
    /// Analyze OfferResponse's published on Exchange.
    /// </summary>
    public abstract void Handle(IOfferResponse<MultiLineTextList, IViewModelBase> message);
    public abstract void Handle(ICancelation message);
    /// <summary>
    /// "Default" conglomerate messaging system.  This studier can keep in contact with the other default
    /// studiers through this message stream.
    /// </summary>
    /// <param name="message"></param>
    public abstract void Handle(IConglomerateMessage message);
    public abstract void Handle(IStatusUpdate<MultiLineTextList, IViewModelBase> message);

    public abstract void Handle(Navigation.EventMessages.NavigationRequestedEventMessage message);
  }
}
