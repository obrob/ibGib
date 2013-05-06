using System;
using System.Linq;
using System.ComponentModel.Composition;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Business;
using LearnLanguages.Offer;
using Caliburn.Micro;
using System.ComponentModel;
using LearnLanguages.Common.Delegates;
using System.Threading;
using System.Collections.Generic;
using LearnLanguages.History.Events;
using LearnLanguages.Navigation.EventMessages;
using System.Threading.Tasks;
using LearnLanguages.Common;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The central piece of the DefaultStudy stuff. This is the piece that ties together
  /// all the different aspects of study. 
  /// </summary>
  //[Export(typeof(IStudyPartner))]
  //public class DefaultStudyPartner : MultiLineTextListStudyPartnerBase
  //{
  //  #region Ctors & Init

  //  public DefaultStudyPartner()
  //  {
  //    _Studier = new DefaultMultiLineTextsStudier();
  //    _ViewModel = new ViewModels.DefaultStudyPartnerViewModel();
  //    _FeedbackViewModel = new ViewModels.PercentKnownFeedbackViewModel();
  //    _FeedbackViewModel.IsEnabled = false;
  //    _ViewModel.FeedbackViewModel = _FeedbackViewModel;
  //    _FeedbackTimedOut = false;
  //    Services.EventAggregator.Subscribe(this);//navigation
  //    _ReviewedLineRecorder = new DefaultReviewedLineRecorder();
  //    _ReviewedLineRecorder.IsEnabled = true;
  //    _ReviewedWordInPhraseRecorder = new DefaultReviewedWordInPhraseRecorder();
  //    _ReviewedWordInPhraseRecorder.IsEnabled = false;
  //    //_PhraseAutoTranslatedRecorder = new DefaultPhraseAutoTranslatedRecorder();
  //    //_PhraseAutoTranslatedRecorder.IsEnabled = true;
  //  }

  //  #endregion
    
  //  #region Recorders

  //  private DefaultReviewedLineRecorder _ReviewedLineRecorder { get; set; }
  //  private DefaultReviewedWordInPhraseRecorder _ReviewedWordInPhraseRecorder { get; set; }
  //  //private DefaultPhraseAutoTranslatedRecorder _PhraseAutoTranslatedRecorder { get; set; }

  //  #endregion

  //  #region Studiers

  //  /// <summary>
  //  /// This is the only studier that the DefaultStudyPartner knows how to talk to
  //  /// directly at this time. This is because this class only is set up to be 
  //  /// able to "Study Song(s)". When we do "Study Phrases" or something similar,
  //  /// we will need to have another studier here.
  //  /// </summary>
  //  protected DefaultMultiLineTextsStudier _Studier { get; set; }

  //  #endregion

  //  #region Properties

  //  /// <summary>
  //  /// Did the Feedback mechanism time out?
  //  /// </summary>
  //  protected bool _FeedbackTimedOut { get; set; }

  //  #region protected DefaultStudyMultiLineTextsViewModel _ViewModel (locked property)

  //  protected volatile object _ViewModelLock = new object();
  //  protected ViewModels.DefaultStudyPartnerViewModel _viewModel = null;
  //  /// <summary>
  //  /// This is the ViewModel that gets returned to the opportunity as the jobInfo.Product.
  //  /// This will house study item view models.
  //  /// </summary>
  //  protected ViewModels.DefaultStudyPartnerViewModel _ViewModel
  //  {
  //    get
  //    {
  //      lock (_ViewModelLock)
  //      {
  //        return _viewModel;
  //      }
  //    }
  //    set
  //    {
  //      lock (_ViewModelLock)
  //      {
  //        _viewModel = value;
  //      }
  //    }
  //  }
    
  //  #endregion

  //  protected IFeedbackViewModelBase _FeedbackViewModel { get; set; }

  //  private bool _IsStudying { get; set; }

  //  private object _AbortLock = new object();
  //  private bool _abortIsFlagged = false;
  //  private bool _AbortIsFlagged
  //  {
  //    get
  //    {
  //      lock (_AbortLock)
  //      {
  //        return _abortIsFlagged;
  //      }
  //    }
  //    set
  //    {
  //      lock (_AbortLock)
  //      {
  //        _abortIsFlagged = value;
  //      }
  //    }
  //  }

  //  #endregion

  //  #region Handle Methods

  //  #region Exchange Methods

  //  /// <summary>
  //  /// Analyze opportunities published on Exchange.
  //  /// </summary>
  //  public override void Handle(IOpportunity<MultiLineTextList, IViewModelBase> message)
  //  {
  //    //WE ONLY CARE ABOUT STUDY OPPORTUNITIES
  //    if (message.Category != StudyResources.CategoryStudy)
  //      return;

  //    //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
  //    if (message.PublisherId == Id)
  //      return;

  //    //CURRENTLY, WE ONLY CARE IF THE JOB INFO IS A STUDY JOB INFO<MLTLIST>
  //    if (!(message.JobInfo is StudyJobInfo<MultiLineTextList, IViewModelBase>))
  //      return;

  //    //WE ONLY CARE IF WE UNDERSTAND THE STUDYJOBINFO.CRITERIA
  //    var studyJobInfo = (StudyJobInfo<MultiLineTextList, IViewModelBase>)message.JobInfo;
  //    if (!(studyJobInfo.Criteria is StudyJobCriteria))
  //      return;

  //    //WE HAVE A GENUINE OPPORTUNITY WE WOULD BE INTERESTED IN
  //    //MAKE THE OFFER FOR THE JOB
  //    var offer = 
  //      new Offer<MultiLineTextList, IViewModelBase>(message,
  //                                                   this.Id,
  //                                                   this,
  //                                                   double.Parse(StudyResources.DefaultAmountDefaultMultiLineTextsStudier),
  //                                                   StudyResources.CategoryStudy,
  //                                                   null);

  //    //FIRST ADD THIS OFFER TO OUR LIST OF OFFERS
  //    _OpenOffers.Add(offer);

  //    //PUBLISH THE OFFER
  //    Exchange.Ton.Publish(offer);
  //  }
  //  /// <summary>
  //  /// Analyze OfferResponse's published on Exchange.
  //  /// </summary>
  //  public override void Handle(IOfferResponse<MultiLineTextList, IViewModelBase> message)
  //  {
  //    //WE ONLY CARE ABOUT OFFERS IN THE STUDY CATEGORY
  //    if (message.Category != StudyResources.CategoryStudy)
  //      return;

  //    //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
  //    if (message.PublisherId == Id)
  //      return;

  //    //WE ONLY CARE ABOUT RESPONSES TO OFFERS THAT WE MADE
  //    var results = (from offer in _OpenOffers
  //                   where offer.Id == message.Offer.Id
  //                   select offer);

  //    if (results.Count() != 1)
  //      return;

  //    var pertinentOffer = results.First();

  //    if (message.Response == OfferResources.OfferResponseDeny)
  //    {
  //      //DENIED OFFER RESPONSE
  //      _OpenOffers.Remove(pertinentOffer);
  //      _DeniedOffers.Add(pertinentOffer);
  //      return;
  //    }
  //    else if (message.Response != OfferResources.OfferResponseAccept)
  //    {
  //      //INVALID OFFER RESPONSE THAT WE DON'T KNOW HOW TO HANDLE
  //      _OpenOffers.Remove(pertinentOffer);
  //      var msg = string.Format(StudyResources.WarningMsgUnknownOfferResponseResponse, message.Response);
  //      Services.Log(msg, LogPriority.High, LogCategory.Warning);
  //      return;
  //    }

  //    ////WE HAVE AN ACCEPT RESPONSE WITH AN OPPORTUNITY THAT WE CARE ABOUT
  //    ////WHATEVER OPPORTUNITY WE ARE WORKING ON, THE NEW ONE SUPERCEDES IT
  //    //if (_CurrentOffer != null)
  //    //  AbortCurrent();

  //    //WE HAVE ALREADY CHECKED THIS TYPE CAST BEFORE MAKING OUR OFFER, IT SHOULD NOT HAVE CHANGED. 
  //    var jobInfo = (StudyJobInfo<MultiLineTextList, IViewModelBase>)message.Offer.Opportunity.JobInfo;
  //    _CurrentOffer = message.Offer;

  //    //SET THE PRODUCT...WE WILL KEEP A REFERENCE TO THIS VIEWMODEL, AND MAKE CHANGES AS WE STUDY.
  //    //BUT AS FAR AS THE JOB OPPORTUNITY IS CONCERNED, WE ARE ALREADY COMPLETED WITH JUST THE 
  //    //ASSIGNMENT!
  //    jobInfo.Product = _ViewModel;

  //    //PUBLISH COMPLETED UPDATE
  //    var completedUpdate =
  //      new StatusUpdate<MultiLineTextList, IViewModelBase>(CommonResources.StatusCompleted,
  //                                                          _CurrentOffer.Opportunity,
  //                                                          _CurrentOffer,
  //                                                          message,
  //                                                          jobInfo,
  //                                                          Id,
  //                                                          this,
  //                                                          StudyResources.CategoryStudy,
  //                                                          null);
  //    Exchange.Ton.Publish(completedUpdate);

  //    StartNewStudySession(jobInfo.Target);
  //  }
  //  /// <summary>
  //  /// Handles cancelation messages.
  //  /// </summary>
  //  /// <param name="message"></param>
  //  public override void Handle(ICancelation message)
  //  {
  //    throw new System.NotImplementedException();
  //  }
  //  /// <summary>
  //  /// "Default" conglomerate messaging system.  This studier can keep in contact with the other default
  //  /// studiers through this message stream.
  //  /// </summary>
  //  /// <param name="message"></param>
  //  public override void Handle(IConglomerateMessage message)
  //  {
  //    throw new NotImplementedException();
  //  }
  //  /// <summary>
  //  /// Handles update messages.
  //  /// </summary>
  //  /// <param name="message"></param>
  //  public override void Handle(IStatusUpdate<MultiLineTextList, IViewModelBase> message)
  //  {
  //    //IF WE DON'T HAVE A CURRENT OFFER, THEN WE DON'T NEED TO LISTEN TO THIS MESSAGE.
  //    if (_CurrentOffer == null)
  //      return;

  //    //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
  //    if (message.PublisherId == Id)
  //      return;

  //    //WE ONLY CARE ABOUT STUDY MESSAGES
  //    if (message.Category != StudyResources.CategoryStudy)
  //      return;

  //    //NOT SURE IF THIS IS RIGHT
  //    if (_CurrentOffer.Opportunity.JobInfo.Id != message.JobInfo.Id)
  //      return;

  //    //THIS IS ONE OF THIS OBJECT'S UPDATES, SO BUBBLE IT BACK UP WITH THIS OFFER, OPPORTUNITY, ETC.

  //    //CREATE THE BUBBLING UP UPDATE
  //    var opportunity = _CurrentOffer.Opportunity;
  //    var offer = _CurrentOffer;
  //    var jobInfo = opportunity.JobInfo;
  //    var statusUpdate =
  //      new StatusUpdate<MultiLineTextList, IViewModelBase>(message.Status,
  //                                                          opportunity,
  //                                                          offer,
  //                                                          null,
  //                                                          jobInfo,
  //                                                          Id,
  //                                                          this,
  //                                                          StudyResources.CategoryStudy,
  //                                                          null);


  //    //BEFORE PUBLISHING UPDATE, CHECK FOR PRODUCT
  //    //IF THIS IS A COMPLETED STATUS UPDATE, THEN PRODUCT SHOULD BE SET.  SO, BUBBLE THIS ASPECT UP.
  //    if (message.Status == CommonResources.StatusCompleted)
  //    {
  //      if (message.JobInfo.Product != null && _CurrentOffer.Opportunity.JobInfo.Product != null)
  //      {
  //        jobInfo.Product = message.JobInfo.Product;
  //      }

  //      //WE'RE DONE WITH THE CURRENT OFFER
  //      _CurrentOffer = null;
  //    }

  //    //PUBLISH TO BUBBLE UP
  //    Exchange.Ton.Publish(statusUpdate);
  //  }

  //  #endregion

  //  #region Navigation

  //  public void Handle(NavigationRequestedEventMessage message)
  //  {
  //    AbortStudying();
  //  }

  //  #endregion

  //  #endregion

  //  #region Methods

  //  /// <summary>
  //  /// Initializes _Studier, _ViewModel, and whatever else needs to be initialized when starting 
  //  /// to study a new MultiLineTextList (new opportunity published).
  //  /// </summary>
  //  private async Task InitializeForNewStudySession(MultiLineTextList multiLineTexts)
  //  {
  //    //INITIALIZE THIS
  //    _AbortIsFlagged = false;

  //    //INITIALIZE STUDIER
  //    await _Studier.InitializeForNewStudySessionAsync(multiLineTexts);

  //    //INITIALIZE STUDY HEARTBEAT
  //    //_StudyHeartbeat.DoWork += (s, r) =>
  //    //  {
  //    //    while (_ViewModel != null && !_AbortIsFlagged && !r.Cancel)
  //    //    {
  //    //      if (!_IsStudying && _ViewModel.StudyItemViewModel == null)
  //    //      {
  //    //        StudyNextItem();
  //    //      }
  //    //      System.Threading.Thread.Sleep(int.Parse(StudyResources.DefaultStudyHeartbeatTimeMilliseconds));
  //    //    }
  //    //  };
  //  }

  //  //CONVERT DEFAULT STUDY PARTNER START NEW STUDY SESSION BEHAVIOR TO USE AGGREGATED EVENTS
  //  private async Task StartNewStudySession(MultiLineTextList multiLineTexts)
  //  {
  //    _AbortIsFlagged = false;
  //    ///INITIALIZE HISTORY PUBLISHER
  //    History.HistoryPublisher.Ton.PublishEvent(new StartingStudySessionEvent());

  //    //LET HISTORY KNOW WE ARE NOW THINKING ABOUT SOMETHING (INITIALIZEFORNEWSTUDYSESSION)
  //    var targetId = Guid.NewGuid();
  //    History.Events.ThinkingAboutTargetEvent.Publish(targetId);

  //    ///OKAY, SO AT THIS POINT, WE HAVE DONE OUR WORK THROUGH THE EXCHANGE.  
  //    ///THE CALLER HAS A REFERENCE TO OUR _VIEWMODEL PROPERTY.  WE HAVE CONTROL
  //    ///OF THE STUDY PROCESS THROUGH THIS _VIEWMODEL PROPERTY.  WE USE TWO SUB VIEWMODELS
  //    ///AT THIS POINT: STUDYITEM VIEWMODEL, AND FEEDBACK VIEWMODEL.  WE WILL IGNORE TIMEOUTS
  //    ///TO SIMPLIFY THINGS.  WE JUST NEED TO DO A FEW THINGS:
  //    ///1) INITIALIZE STUDY SESSION (INITIALIZE STUDIERS FOR OUR MLT LIST)
  //    ///2) GET NEXT/ ASSIGN _VIEWMODEL.STUDYITEMVIEWMODEL AND _VIEWMODEL.FEEDBACKVIEWMODEL.
  //    ///3) SHOW STUDYITEMVIEWMODEL.
  //    ///4) WHEN SHOW IS DONE, ENABLE FEEDBACK VIEWMODEL.
  //    ///5) WHEN FEEDBACK IS PROVIDED, GO TO #2.
  //    await _Studier.InitializeForNewStudySessionAsync(multiLineTexts);

  //    //LET HISTORY KNOW WE HAVE COMPLETED OUR THINKING ABOUT (INITIALIZEFORNEWSTUDYSESSION)
  //    History.Events.ThinkedAboutTargetEvent.Publish(targetId);

  //    //THIS STUDY THREAD EXECUTES ONE ITERATION OF STUDY.  ASSIGNS VIEWMODEL, SHOWS, GETS FEEDBACK.
  //    BackgroundWorker studyThread = new BackgroundWorker();

  //    #region Study Thread DoWork
  //    studyThread.DoWork += async (s3, r3) =>
  //    {
  //      if (_AbortIsFlagged)
  //        return;

  //      _IsStudying = true;

  //      ResultArgs<StudyItemViewModelArgs> result = null;
  //      #region Thinking (try..)
  //      var targetId2 = Guid.NewGuid();
  //      History.Events.ThinkingAboutTargetEvent.Publish(targetId2);
  //      try
  //      {
  //      #endregion
  //        result = await _Studier.GetNextStudyItemViewModelAsync();
  //        #region (...finally) Thinked
  //      }
  //      finally
  //      {
  //        History.Events.ThinkedAboutTargetEvent.Publish(targetId2);
  //      }
  //        #endregion

  //      #region Abort Check
  //      if (_AbortIsFlagged)
  //      {
  //        _IsStudying = false;
  //        return;
  //      }
  //      #endregion

  //      var studyItemViewModel = result.Object.ViewModel;
  //      _ViewModel.StudyItemViewModel = studyItemViewModel;
  //      _ViewModel.FeedbackViewModel = new ViewModels.PercentKnownFeedbackViewModel();
  //      _ViewModel.FeedbackViewModel.IsEnabled = false;

  //      if (studyItemViewModel == null)
  //        return;

  //      studyItemViewModel.Show((e2) =>
  //      {
  //        if (e2 != null)
  //          throw e2;

  //        #region Abort Check
  //        if (_AbortIsFlagged)
  //        {
  //          _IsStudying = false;
  //          return;
  //        }
  //        #endregion
  //        //IS DONE SHOWING, SO GET FEEDBACK
  //        int timeout = int.Parse(StudyResources.DefaultFeedbackTimeoutMilliseconds);
  //        _ViewModel.FeedbackViewModel.GetFeedbackAsync(timeout, (s2, r2) =>
  //        {
  //          if (r2.Error != null)
  //            throw r2.Error;

  //          #region Abort Check
  //          if (_AbortIsFlagged)
  //          {
  //            _IsStudying = false;
  //            return;
  //          }
  //          #endregion

  //          _IsStudying = false;
  //        });
  //      });

  //      while (_IsStudying)
  //      {
  //        #region Abort Check
  //        if (_AbortIsFlagged)
  //        {
  //          _IsStudying = false;
  //          return;
  //        }
  //        #endregion
  //        Thread.Sleep(int.Parse(StudyResources.DefaultFeedbackCheckIntervalMilliseconds));
  //      }
  //    };
  //    #endregion
  //    #region Study Thread Completed
  //    studyThread.RunWorkerCompleted += (s4, r4) =>
  //    {
  //      if (r4.Error != null)
  //        throw r4.Error;

  //      if (!_AbortIsFlagged)
  //        studyThread.RunWorkerAsync();
  //    };
  //    #endregion

  //    studyThread.RunWorkerAsync();
  //  }


  //  private void AbortStudying()
  //  {
  //    _AbortIsFlagged = true;
  //  }

  //  #endregion
    
  //}
}
