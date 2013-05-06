using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Common.Interfaces;
using System.Threading.Tasks;
using LearnLanguages.Navigation.EventMessages;
using LearnLanguages.History.Events;

namespace LearnLanguages.Study
{
  /// <summary>
  /// StudyPartner base class that targets MLTs.  
  /// You do not HAVE to descend from this, it just provides some 
  /// very basic plumbing, including basic Exchange handshaking and some
  /// useful properties.
  /// </summary>
  public abstract class MultiLineTextListStudyPartnerBase : StudyPartnerBase
  {
    #region Ctors & Init

    public MultiLineTextListStudyPartnerBase
      ()
    {
      _ViewModel = new ViewModels.DefaultStudyPartnerViewModel();
      _FeedbackViewModel = new ViewModels.PercentKnownFeedbackViewModel();
      _FeedbackViewModel.IsEnabled = false;
      _ViewModel.FeedbackViewModel = _FeedbackViewModel;
      _FeedbackTimedOut = false;
      Services.EventAggregator.Subscribe(this);//navigation
      
      Id = Guid.NewGuid();
      _OpenOffers = new List<IOffer<MultiLineTextList, IViewModelBase>>();
      _DeniedOffers = new List<IOffer<MultiLineTextList, IViewModelBase>>();
      _CurrentOffer = null;

      Exchange.Ton.SubscribeToOpportunities(this);
      Exchange.Ton.SubscribeToOfferResponses(this);
      Exchange.Ton.SubscribeToStatusUpdates(this);
      Exchange.Ton.SubscribeToCancelations(this);
    }

    #endregion

    #region Properties

    //public Guid Id { get; protected set; }
    //public Guid ConglomerateId { get; protected set; }

    private object _AbortLock = new object();
    private bool _abortIsFlagged = false;
    protected bool _AbortIsFlagged
    {
      get
      {
        lock (_AbortLock)
        {
          return _abortIsFlagged;
        }
      }
      set
      {
        lock (_AbortLock)
        {
          _abortIsFlagged = value;
        }
      }
    }

    private object _CompleteLock = new object();
    private bool _completeIsFlagged = false;
    protected bool _CompleteIsFlagged
    {
      get
      {
        lock (_CompleteLock)
        {
          return _completeIsFlagged;
        }
      }
      set
      {
        lock (_CompleteLock)
        {
          _completeIsFlagged = value;
        }
      }
    }

    protected bool _IsStudying { get; set; }

    /// <summary>
    /// Did the Feedback mechanism time out?
    /// </summary>
    protected bool _FeedbackTimedOut { get; set; }

    #region protected DefaultStudyPartnerViewModel _ViewModel (locked property)

    protected volatile object _ViewModelLock = new object();
    protected ViewModels.DefaultStudyPartnerViewModel _viewModel = null;
    /// <summary>
    /// This is the ViewModel that gets returned to the opportunity as the jobInfo.Product.
    /// This will house study item view models.
    /// </summary>
    protected ViewModels.DefaultStudyPartnerViewModel _ViewModel
    {
      get
      {
        lock (_ViewModelLock)
        {
          return _viewModel;
        }
      }
      set
      {
        lock (_ViewModelLock)
        {
          _viewModel = value;
        }
      }
    }

    #endregion

    protected IFeedbackViewModelBase _FeedbackViewModel { get; set; }

    #endregion

    #region Methods

    #region Exchange Methods

    /// <summary>
    /// Analyze opportunities published on Exchange.
    /// </summary>
    public override void Handle(IOpportunity<MultiLineTextList, IViewModelBase> message)
    {
      History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);

      //WE ONLY CARE ABOUT STUDY OPPORTUNITIES
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      if (message.PublisherId == Id)
        return;

      //CURRENTLY, WE ONLY CARE IF THE JOB INFO IS A STUDY JOB INFO<MLTLIST>
      if (!(message.JobInfo is StudyJobInfo<MultiLineTextList, IViewModelBase>))
        return;

      //WE ONLY CARE IF WE UNDERSTAND THE STUDYJOBINFO.CRITERIA
      var studyJobInfo = (StudyJobInfo<MultiLineTextList, IViewModelBase>)message.JobInfo;
      if (!(studyJobInfo.Criteria is StudyJobCriteria))
        return;

      //WE HAVE A GENUINE OPPORTUNITY WE WOULD BE INTERESTED IN
      //MAKE THE OFFER FOR THE JOB
      var offer =
        new Offer<MultiLineTextList, IViewModelBase>(message,
                                                     this.Id,
                                                     this,
                                                     GetOfferAmount(),
                                                     StudyResources.CategoryStudy,
                                                     null);

      //FIRST ADD THIS OFFER TO OUR LIST OF OFFERS
      _OpenOffers.Add(offer);

      //PUBLISH THE OFFER
      Exchange.Ton.Publish(offer);
    }
    /// <summary>
    /// Analyze OfferResponse's published on Exchange.
    /// </summary>
    public override void Handle(IOfferResponse<MultiLineTextList, IViewModelBase> message)
    {
      History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);

      //WE ONLY CARE ABOUT OFFERS IN THE STUDY CATEGORY
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      if (message.PublisherId == Id)
        return;

      //WE ONLY CARE ABOUT RESPONSES TO OFFERS THAT WE MADE
      var results = (from offer in _OpenOffers
                     where offer.Id == message.Offer.Id
                     select offer);

      if (results.Count() != 1)
        return;

      var pertinentOffer = results.First();

      if (message.Response == OfferResources.OfferResponseDeny)
      {
        //DENIED OFFER RESPONSE
        _OpenOffers.Remove(pertinentOffer);
        _DeniedOffers.Add(pertinentOffer);
        return;
      }
      else if (message.Response != OfferResources.OfferResponseAccept)
      {
        //INVALID OFFER RESPONSE THAT WE DON'T KNOW HOW TO HANDLE
        _OpenOffers.Remove(pertinentOffer);
        var msg = string.Format(StudyResources.WarningMsgUnknownOfferResponseResponse, message.Response);
        Services.Log(msg, LogPriority.High, LogCategory.Warning);
        return;
      }

      ////WE HAVE AN ACCEPT RESPONSE WITH AN OPPORTUNITY THAT WE CARE ABOUT
      ////WHATEVER OPPORTUNITY WE ARE WORKING ON, THE NEW ONE SUPERCEDES IT
      //if (_CurrentOffer != null)
      //  AbortCurrent();

      //WE HAVE ALREADY CHECKED THIS TYPE CAST BEFORE MAKING OUR OFFER, IT SHOULD NOT HAVE CHANGED. 
      var jobInfo = (StudyJobInfo<MultiLineTextList, IViewModelBase>)message.Offer.Opportunity.JobInfo;
      _CurrentOffer = message.Offer;

      //SET THE PRODUCT...WE WILL KEEP A REFERENCE TO THIS VIEWMODEL, AND MAKE CHANGES AS WE STUDY.
      //BUT AS FAR AS THE JOB OPPORTUNITY IS CONCERNED, WE ARE ALREADY COMPLETED WITH JUST THE 
      //ASSIGNMENT!
      jobInfo.Product = _ViewModel;

      //PUBLISH COMPLETED UPDATE
      var completedUpdate =
        new StatusUpdate<MultiLineTextList, IViewModelBase>(CommonResources.StatusCompleted,
                                                            _CurrentOffer.Opportunity,
                                                            _CurrentOffer,
                                                            message,
                                                            jobInfo,
                                                            Id,
                                                            this,
                                                            StudyResources.CategoryStudy,
                                                            null);
      Exchange.Ton.Publish(completedUpdate);

      var suppress = StudyAsync(jobInfo.Target);
    }
    /// <summary>
    /// Handles cancelation messages. Not implemented yet.
    /// </summary>
    /// <param name="message"></param>
    public override void Handle(ICancelation message)
    {
      throw new System.NotImplementedException();
    }
    /// <summary>
    /// "Default" conglomerate messaging system.  This studier can keep in contact with the other default
    /// studiers through this message stream.
    /// Not implemented yet, and I'm not even sure if this is needed.
    /// </summary>
    /// <param name="message"></param>
    public override void Handle(IConglomerateMessage message)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// Handles update messages.
    /// </summary>
    /// <param name="message"></param>
    public override void Handle(IStatusUpdate<MultiLineTextList, IViewModelBase> message)
    {
      //IF WE DON'T HAVE A CURRENT OFFER, THEN WE DON'T NEED TO LISTEN TO THIS MESSAGE.
      if (_CurrentOffer == null)
        return;

      //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      if (message.PublisherId == Id)
        return;

      //WE ONLY CARE ABOUT STUDY MESSAGES
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //NOT SURE IF THIS IS RIGHT
      if (_CurrentOffer.Opportunity.JobInfo.Id != message.JobInfo.Id)
        return;

      //THIS IS ONE OF THIS OBJECT'S UPDATES, SO BUBBLE IT BACK UP WITH THIS OFFER, OPPORTUNITY, ETC.

      //CREATE THE BUBBLING UP UPDATE
      var opportunity = _CurrentOffer.Opportunity;
      var offer = _CurrentOffer;
      var jobInfo = opportunity.JobInfo;
      var statusUpdate =
        new StatusUpdate<MultiLineTextList, IViewModelBase>(message.Status,
                                                            opportunity,
                                                            offer,
                                                            null,
                                                            jobInfo,
                                                            Id,
                                                            this,
                                                            StudyResources.CategoryStudy,
                                                            null);


      //BEFORE PUBLISHING UPDATE, CHECK FOR PRODUCT
      //IF THIS IS A COMPLETED STATUS UPDATE, THEN PRODUCT SHOULD BE SET.  SO, BUBBLE THIS ASPECT UP.
      if (message.Status == CommonResources.StatusCompleted)
      {
        if (message.JobInfo.Product != null && _CurrentOffer.Opportunity.JobInfo.Product != null)
        {
          jobInfo.Product = message.JobInfo.Product;
        }

        //WE'RE DONE WITH THE CURRENT OFFER
        _CurrentOffer = null;
      }

      //PUBLISH TO BUBBLE UP
      Exchange.Ton.Publish(statusUpdate);
    }
    /// <summary>
    /// Gets the offer amount to post for job offerings. Default implementation
    /// always returns the StudyResources.DefaultAmountDefaultMultiLineTextsStudier.
    /// </summary>
    /// <returns></returns>
    protected virtual double GetOfferAmount()
    {
      return double.Parse(StudyResources.DefaultAmountDefaultMultiLineTextsStudier);
    }

    #endregion

    public override void Handle(NavigationRequestedEventMessage message)
    {
      AbortStudying();
    }

    #region Study Methods

    protected virtual async Task StudyAsync(MultiLineTextList multiLineTexts)
    {
      _AbortIsFlagged = false;
      StudyItemViewModelBase contentVM = null;
      IFeedbackViewModelBase feedbackVM = _ViewModel.FeedbackViewModel;

      ///INITIALIZE HISTORY PUBLISHER
      History.HistoryPublisher.Ton.PublishEvent(new StartingStudySessionEvent());


      ///OKAY, SO AT THIS POINT, WE HAVE DONE OUR WORK THROUGH THE EXCHANGE.  
      ///THE CALLER HAS A REFERENCE TO OUR _VIEWMODEL PROPERTY.  WE HAVE CONTROL
      ///OF THE STUDY PROCESS THROUGH THIS _VIEWMODEL PROPERTY.  WE USE TWO SUB VIEWMODELS
      ///AT THIS POINT: STUDYITEM VIEWMODEL, AND FEEDBACK VIEWMODEL.  WE WILL IGNORE TIMEOUTS
      ///TO SIMPLIFY THINGS.  WE JUST NEED TO DO A FEW THINGS:
      ///1) INITIALIZE FOR OUR STUDY SESSION
      ///2) GET NEXT/ ASSIGN _VIEWMODEL.STUDYITEMVIEWMODEL AND _VIEWMODEL.FEEDBACKVIEWMODEL.
      ///3) SHOW STUDYITEMVIEWMODEL.
      ///4) WHEN SHOW IS DONE, ENABLE FEEDBACK VIEWMODEL AND GET FEEDBACK


      #region Thinking (try..)
      var targetId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      try
      {
      #endregion
        ///1) INITIALIZE FOR OUR STUDY SESSION
        await InitializeForNewStudySessionAsync(multiLineTexts);
      #region (...finally) Thinked
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(targetId);
      }
        #endregion

      //STUDY SESSION LOOP
      while (!_CompleteIsFlagged && !_AbortIsFlagged)
      {
        //BEGINNING OF A SINGLE STUDY ITEM
        _IsStudying = true;
        _ViewModel.FeedbackViewModel.IsEnabled = false;

        ///2) GET NEXT/ ASSIGN _VIEWMODEL.STUDYITEMVIEWMODEL AND _VIEWMODEL.FEEDBACKVIEWMODEL.
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
        //STUDY ITEM VIEWMODEL CONTENT
        _ViewModel.StudyItemViewModel = await GetNextStudyItemViewModelAsync();
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);

        contentVM = _ViewModel.StudyItemViewModel;
        if (contentVM == null)
        {
          //IF WE CAN'T GET A NEW STUDY ITEM VIEWMODEL, THEN THE STUDY SESSION IS COMPLETED.
          _CompleteIsFlagged = true;
          break;
        }
        
        #region Abort Check
        if (_AbortIsFlagged)
        {
          _IsStudying = false;
          break;
        }
        #endregion

        ///3) SHOW STUDY ITEM VIEWMODEL.
        ///   DO *NOT* THINK BEFORE AFTER, CAUSE THIS IS WAITING FOR USER INPUT
          await contentVM.ShowAsync();
        
        #region Abort Check
        if (_AbortIsFlagged)
        {
          _IsStudying = false;
          break;
        }
        #endregion

        ///4) WHEN SHOW IS DONE, GET FEEDBACK
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
        var feedbackTimeout = int.Parse(StudyResources.DefaultFeedbackTimeoutMilliseconds);
        var feedback = await feedbackVM.GetFeedbackAsync(feedbackTimeout);
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
        
        //RIGHT NOW I'M NOT DOING ANYTHING WITH THIS FEEDBACK, AS IT IS PUBLISHED WHEN
        //IT IS CREATED BY THE FEEDBACKVM. I'M JUST KEEPING IT HERE TO SHOW THAT I CAN
        //GET THE FEEDBACK HERE. THERE MIGHT BE A BETTER WAY TO DO THIS ANYWAY.
        #region Abort Check
        if (_AbortIsFlagged)
        {
          _IsStudying = false;
          break;
        }
        #endregion
      }
    }

    protected abstract Task<StudyItemViewModelBase> GetNextStudyItemViewModelAsync();
    protected abstract Task InitializeForNewStudySessionAsync(MultiLineTextList multiLineTexts);
    protected virtual void AbortStudying()
    {
      _AbortIsFlagged = true;
    }

    #endregion

    #endregion
  }
}
