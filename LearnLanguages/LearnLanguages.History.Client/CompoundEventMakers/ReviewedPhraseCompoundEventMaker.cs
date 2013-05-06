using System;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.History.CompoundEventMakers
{
  /// <summary>
  /// This class listens to HistoryPublisher and once the PhraseReviewing events are published, in proper
  /// order if EventsAreSynchronous == true (false isn't implemented yet), this publishes a compound
  /// event called "ReviewedPhraseEvent".  This event contains the phraseId, languageId of that phrase, 
  /// timespan duration of review, and feedback (as type double) given.  
  /// </summary>
  [Export(typeof(ICompoundEventMaker))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ReviewedPhraseCompoundEventMaker : CompoundEventMakerBase, 
                                                  IHandle<Events.ReviewingPhraseEvent>,
                                                  IHandle<Events.ViewingPhraseOnScreenEvent>,
                                                  IHandle<Events.ViewedPhraseOnScreenEvent>,
                                                  IHandle<Events.FeedbackAsDoubleGivenEvent>
  {
    public ReviewedPhraseCompoundEventMaker()
    {
      var isEnabled = bool.Parse(HistoryResources.IsEnabledReviewedPhraseCompoundEventMaker);
      if (isEnabled)
        Enable();
      Reset();
    }

    #region State

    private Guid _ReviewMethodId { get; set; }


    private Guid _PhraseAId { get; set; }
    private string _PhraseAText { get; set; }
    private Guid _LanguageAId { get; set; }
    private string _LanguageAText { get; set; }

    private Guid _PhraseBId { get; set; }
    private string _PhraseBText { get; set; }
    private Guid _LanguageBId { get; set; }
    private string _LanguageBText { get; set; }
    
    private double _FeedbackAsDouble { get; set; }


    private bool _ReviewingEventHandled { get; set; }

    private bool _ViewingAEventHandled { get; set; }
    private bool _ViewedAEventHandled { get; set; }
    private bool _ViewingBEventHandled { get; set; }
    private bool _ViewedBEventHandled { get; set; }
    
    private bool _FeedbackGivenEventHandled { get; set; }


    private DateTime _ViewingATimestamp { get; set; }
    private DateTime _ViewedATimestamp { get; set; }
    private DateTime _ViewingBTimestamp { get; set; }
    private DateTime _ViewedBTimestamp { get; set; }

    private DateTime _FeedbackTimestamp { get; set; }

    #endregion

    private bool _IsReset { get; set; }
    protected override void Reset()
    {
      _ReviewingEventHandled = false;
      
      _FeedbackGivenEventHandled = false;
      _ViewingAEventHandled = false;
      _ViewedAEventHandled = false;
      _ViewingBEventHandled = false;
      _ViewedBEventHandled = false;

      _FeedbackTimestamp = DateTime.MinValue;
      _ViewingATimestamp = DateTime.MinValue;
      _ViewedATimestamp = DateTime.MinValue;
      _ViewingBTimestamp = DateTime.MinValue;
      _ViewedBTimestamp = DateTime.MinValue;

      _PhraseAId = Guid.Empty;
      _PhraseAText = "";
      _LanguageAId = Guid.Empty;
      _LanguageAText = "";
      _PhraseBId = Guid.Empty;
      _PhraseBText = "";
      _LanguageBId = Guid.Empty;
      _LanguageBText = "";
      
      _IsReset = true;
    }

    #region Saga Events

    #region #1 Reviewing Phrase
    public void Handle(Events.ReviewingPhraseEvent message)
    {
      Reset();
      _IsReset = false;
      _ReviewingEventHandled = true;
      _ReviewMethodId = message.GetDetail<Guid>(HistoryResources.Key_ReviewMethodId);
    }
    #endregion

    #region #2 Phrase Viewing
    /// <summary>
    /// THIS OCCURS AT THE START OF SHOWING THE PHRASE.
    /// </summary>
    public void Handle(Events.ViewingPhraseOnScreenEvent message)
    {
      if (!_ReviewingEventHandled)
      {
        Reset();
        return;
      }

      var msgPhraseId = (Guid)message.GetDetail(HistoryResources.Key_PhraseId);
      var msgPhraseText = message.GetDetail<string>(HistoryResources.Key_PhraseText);
      var msgLanguageId = (Guid)message.GetDetail(HistoryResources.Key_LanguageId);
      var msgLanguageText = message.GetDetail<string>(HistoryResources.Key_LanguageText);

      if (!_ViewingAEventHandled)
      {
        _PhraseAId = msgPhraseId;
        _PhraseAText = msgPhraseText;
        _LanguageAId = msgLanguageId;
        _LanguageAText = msgLanguageText;

        _ViewingAEventHandled = true;
        _ViewingATimestamp = DateTime.Now;
      }
      else if (!_ViewingBEventHandled)
      {
        _PhraseBId = msgPhraseId;
        _PhraseBText = msgPhraseText;
        _LanguageBId = msgLanguageId;
        _LanguageBText = msgLanguageText;

        _ViewingBEventHandled = true;
        _ViewingBTimestamp = DateTime.Now;
      }
      else
      {
        //WE ALREADY HAVE BOTH PHRASE A AND PHRASE B FILLED OUT
        return;
      }

    }
    #endregion

    #region #3 Phrase Viewed
    /// <summary>
    /// THIS OCCURS AFTER WE HAVE COMPLETED SHOWING THE PHRASE AND IT IS NOW NO LONGER IN VIEW.
    /// THIS IS THE SECOND MESSAGE WE LISTEN FOR.
    /// </summary>
    public void Handle(Events.ViewedPhraseOnScreenEvent message)
    {
      var msgPhraseId = (Guid)message.GetDetail(HistoryResources.Key_PhraseId);
      var msgLanguageId = (Guid)message.GetDetail(HistoryResources.Key_LanguageId);

      if (_ViewingAEventHandled &&
          msgPhraseId == _PhraseAId &&
          msgLanguageId == _LanguageAId)
      {
        _ViewedAEventHandled = true;
        _ViewedATimestamp = DateTime.Now;
      }
      else if (_ViewingBEventHandled &&
        msgPhraseId == _PhraseBId &&
        msgLanguageId == _LanguageBId)
      {
        _ViewedBEventHandled = true;
        _ViewedBTimestamp = DateTime.Now;
      }
      else
      {
        Reset();
        return;
      }
    }
    #endregion

    #region #4 Feedback Given (dispatches compound event if successful)
    public void Handle(Events.FeedbackAsDoubleGivenEvent message)
    {
      if (!_ViewingAEventHandled || !_ViewedAEventHandled ||
          !_ViewingBEventHandled || !_ViewedBEventHandled)
      {
        Reset();
        return;
      }

      _FeedbackAsDouble = (double)message.GetDetail(HistoryResources.Key_FeedbackAsDouble);
      _FeedbackGivenEventHandled = true;
      _FeedbackTimestamp = DateTime.Now;

      DispatchCompoundEvent();
    }
    #endregion

    #endregion

    private void DispatchCompoundEvent()
    {
      if (!_ReviewingEventHandled ||
          !_ViewingAEventHandled ||
          !_ViewedAEventHandled ||
          !_ViewingBEventHandled ||
          !_ViewedBEventHandled ||
          !_FeedbackGivenEventHandled ||
          _PhraseAId == Guid.Empty ||
          _PhraseBId == Guid.Empty)
        throw new HistoryException();

      //A
      var durationA = _ViewedATimestamp - _ViewingATimestamp;
      var reviewedAEvent = new Events.ReviewedPhraseEvent(_FeedbackAsDouble, _PhraseAId, _PhraseAText, 
        _LanguageAId, _LanguageAText, _ReviewMethodId, durationA);
      HistoryPublisher.Ton.PublishEvent(reviewedAEvent);

      //B
      var durationB = _ViewedBTimestamp - _ViewingBTimestamp;
      var reviewedBEvent = new Events.ReviewedPhraseEvent(_FeedbackAsDouble, _PhraseBId, _PhraseBText,
        _LanguageBId, _LanguageBText, _ReviewMethodId, durationB);
      HistoryPublisher.Ton.PublishEvent(reviewedBEvent);

      Reset();
    }

    public override void Enable()
    {
      Reset();
      base.Enable();
    }

    protected override Guid GetId()
    {
      return Guid.Parse(HistoryResources.CompoundEventMakerIdReviewedPhrase);
    }
  }
}
