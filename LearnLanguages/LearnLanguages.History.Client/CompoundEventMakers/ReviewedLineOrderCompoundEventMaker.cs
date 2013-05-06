using System;
using System.Linq;
using System.Collections.Generic;
using Caliburn.Micro;
using Csla.Core;
using LearnLanguages.Business;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.History.CompoundEventMakers
{
  /// <summary>
  /// This class listens to HistoryPublisher and once the LineReviewing events are published, in proper
  /// order if EventsAreSynchronous == true (false isn't implemented yet), this publishes a compound
  /// event called "ReviewedLineEvent".  This event contains the phraseId, languageId of that phrase, 
  /// timespan duration of review, and feedback (as type double) given.  
  /// </summary>
  [Export(typeof(ICompoundEventMaker))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ReviewedLineOrderCompoundEventMaker : CompoundEventMakerBase, 
                                                     IHandle<Events.ReviewingLineOrderEvent>,
                                                     IHandle<Events.FeedbackAsDoubleGivenEvent>
  {
    public ReviewedLineOrderCompoundEventMaker()
    {
      var isEnabled = bool.Parse(HistoryResources.IsEnabledReviewedLineOrderCompoundEventMaker);
      if (isEnabled)
        Enable();
    }

    #region State

    private Guid _ReviewMethodId { get; set; }
    private Guid _LineId { get; set; }
    private Guid _MultiLineTextId { get; set; }
    private string _LineText { get; set; }
    private int _LineNumber { get; set; }
    private TimeSpan _Duration { get; set; }
    private double _FeedbackAsDouble { get; set; }

    private bool _ReviewingLineOrderEventHandled { get; set; }
    private bool _FeedbackAsDoubleGivenEventHandled { get; set; }

    #endregion

    private bool _IsReset { get; set; }
    protected override void Reset()
    {
      _ReviewMethodId = Guid.Empty;
      _LineId = Guid.Empty;
      _MultiLineTextId = Guid.Empty;
      _LineText = "";
      _Duration = TimeSpan.MinValue;
      _FeedbackAsDouble = double.NaN;

      _ReviewingLineOrderEventHandled = false;
      _FeedbackAsDoubleGivenEventHandled = false;
      _IsReset = true;
    }

    #region Saga Events

    #region #1 Reviewing Line Order
    public void Handle(Events.ReviewingLineOrderEvent message)
    {
      var msgLineText = message.GetDetail<string>(HistoryResources.Key_LineText);
      var msgLineId = message.GetDetail<Guid>(HistoryResources.Key_LineId);
      _ReviewMethodId = message.GetDetail<Guid>(HistoryResources.Key_ReviewMethodId);

      _LineId = msgLineId;
      _LineText = msgLineText;
      _LineNumber = message.GetDetail<int>(HistoryResources.Key_LineNumber);
      _MultiLineTextId = message.GetDetail<Guid>(HistoryResources.Key_MultiLineTextId);

      _ReviewingLineOrderEventHandled = true;
    }
    #endregion

    #region #2 Feedback As Double Given
    /// <summary>
    /// THIS OCCURS AFTER A PHRASE HAS BEEN REVIEWED.  THIS MEANS THE PHRASE WAS VIEWED AND
    /// FEEDBACK WAS GIVEN.  TO START WITH, THIS WILL ONLY OCCUR WHEN THE ENTIRE LINE'S PHRASE
    /// IS REVIEWED.  OTHERWISE, THIS SHOULD NOT BE TRIGGERING.
    /// </summary>
    public void Handle(Events.FeedbackAsDoubleGivenEvent message)
    {
      if (!_ReviewingLineOrderEventHandled)
      {
        return;
      }

      //var msgLineId = message.Ids[HistoryResources.Key_LineId];
      //var msgLineText = message.Strings[HistoryResources.Key_LineText];
      //var msgReviewedPhraseDuration = message.Duration;
      //var msgMultiLineTextId = message.GetDetail<Guid>(HistoryResources.Key_MultiLineTextId);

      ////MAKE SURE PHRASE IDS, LINE TEXT AND PHRASE TEXT, LANGUAGE ID, AND LANGUAGE TEXT MATCH 
      ////BETWEEN REVIEWINGLINE EVENT AND REVIEWED PHRASE EVENT
      //if (_LineId != msgLineId ||
      //    _LineText != msgLineText ||
      //    _MultiLineTextId != msgMultiLineTextId)
      //{
      //  //if (!_IsReset)
      //  //  Reset();
      //  return;
      //}

      //GET THE FEEDBACK FROM THE REVIEWED PHRASE EVENT
      _FeedbackAsDouble = message.Doubles[HistoryResources.Key_FeedbackAsDouble];
      _FeedbackAsDoubleGivenEventHandled = true;

      DispatchCompoundEvent();
      Reset();
    }
    #endregion

    #endregion

    private void DispatchCompoundEvent()
    {
      if (!_ReviewingLineOrderEventHandled ||
          !_FeedbackAsDoubleGivenEventHandled)
        throw new HistoryException();

        //DISPATCH THE REVIEWED LINE COMPOUND EVENT
        var reviewedLineEvent = new Events.ReviewedLineOrderEvent(_LineId, _ReviewMethodId, _LineText, _LineNumber, 
          _FeedbackAsDouble, _Duration);
        HistoryPublisher.Ton.PublishEvent(reviewedLineEvent);   
    }

    //public void Handle(Events.ActivatedLineEvent message)
    //{
    //  var languageText = message.GetDetail<string>(HistoryResources.Key_LanguageText);
    //  var lineText = message.GetDetail<string>(HistoryResources.Key_LineText);
    //  var lineId = message.GetDetail<Guid>(HistoryResources.Key_LineId);
    //  var languageId = message.GetDetail<Guid>(HistoryResources.Key_LanguageId);
    //  var lineNumber = message.GetDetail<int>(HistoryResources.Key_LineNumber);

    //  var lineInfo = new HistoryLineInfo()
    //  {
    //    LanguageId = languageId,
    //    LanguageText = languageText,
    //    LineId = lineId,
    //    LineText = lineText,
    //    LineNumber = lineNumber
    //  };

    //  _ActiveLines.Add(lineInfo);
    //}

    protected override Guid GetId()
    {
      return Guid.Parse(HistoryResources.CompoundEventMakerIdReviewedLine);
    }
  }
}
