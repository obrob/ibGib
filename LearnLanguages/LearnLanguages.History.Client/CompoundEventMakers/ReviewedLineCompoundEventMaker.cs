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
  public class ReviewedLineCompoundEventMaker : CompoundEventMakerBase, 
                                                IHandle<Events.ReviewingPhraseEvent>,
                                                IHandle<Events.ReviewedPhraseEvent>, 
                                                IHandle<Events.ActivatedLineEvent>
  {
    public ReviewedLineCompoundEventMaker()
    {
      var isEnabled = bool.Parse(HistoryResources.IsEnabledReviewedLineCompoundEventMaker);
      if (isEnabled)
        Enable();
      _ActiveLines = new MobileList<HistoryLineInfo>();
    }

    #region State

    private Guid _ReviewMethodId { get; set; }
    private Guid _LineId { get; set; }
    private string _LineText { get; set; }
    private int _LineNumber { get; set; }
    private Guid _PhraseId { get; set; }
    private Guid _LanguageId { get; set; }
    private string _LanguageText { get; set; }
    private TimeSpan _ReviewedPhraseDuration { get; set; }
    private double _FeedbackAsDouble { get; set; }

    private bool _ReviewingLineEventHandled { get; set; }
    private bool _ReviewedPhraseEventHandled { get; set; }
    private MobileList<HistoryLineInfo> _ActiveLines { get; set; }

    #endregion

    private bool _IsReset { get; set; }
    protected override void Reset()
    {
      _ReviewMethodId = Guid.Empty;
      _LineId = Guid.Empty;
      _PhraseId = Guid.Empty;
      _LineText = "";
      _LanguageId = Guid.Empty;
      _LanguageText = "";
      _ReviewedPhraseDuration = TimeSpan.MinValue;
      _FeedbackAsDouble = double.NaN;

      _ReviewingLineEventHandled = false;
      _ReviewedPhraseEventHandled = false;
      _IsReset = true;
    }

    #region Saga Events

    #region #1 Reviewing Phrase
    public void Handle(Events.ReviewingPhraseEvent message)
    {
      //WE ONLY CARE ABOUT PHRASES WHOSE TEXTS ARE EQUAL TO OUR ACTIVE LINES' TEXTS IN SAME LANGUAGE
      var msgPhraseText = message.GetDetail<string>(HistoryResources.Key_PhraseText);
      var msgLanguageText = message.GetDetail<string>(HistoryResources.Key_LanguageText);
      var msgLanguageId = message.GetDetail<Guid>(HistoryResources.Key_LanguageId);
      var results = from activeLineInfo in _ActiveLines
                    where activeLineInfo.LineText == msgPhraseText &&
                          activeLineInfo.LanguageText == msgLanguageText &&
                          activeLineInfo.LanguageId == msgLanguageId
                    select activeLineInfo;

      if (results.Count() == 0)
        return;

      var lineInfo = results.First();

      //THIS REVIEWING PHRASE EVENT PERTAINS TO ONE OF OUR ACTIVE LINES, AND WE 
      //ONLY LISTEN TO ONE REVIEWING PHRASE EVENT AT A TIME, SO RESET.
      Reset();
      _IsReset = false;
        
      //INTERNALIZE STATE FOR THIS NEW REVIEWING EVENT.
      _ReviewMethodId = message.Ids[HistoryResources.Key_ReviewMethodId];
      _LineId = lineInfo.LineId;
      _PhraseId = message.Ids[HistoryResources.Key_PhraseId];
      _LineText = lineInfo.LineText;
      _LanguageId = lineInfo.LanguageId;
      _LanguageText = lineInfo.LanguageText;
      _LineNumber = lineInfo.LineNumber;

      _ReviewingLineEventHandled = true;
    }
    #endregion

    #region #2 Reviewed Phrase 
    /// <summary>
    /// THIS OCCURS AFTER A PHRASE HAS BEEN REVIEWED.  THIS MEANS THE PHRASE WAS VIEWED AND
    /// FEEDBACK WAS GIVEN.  TO START WITH, THIS WILL ONLY OCCUR WHEN THE ENTIRE LINE'S PHRASE
    /// IS REVIEWED.  OTHERWISE, THIS SHOULD NOT BE TRIGGERING.
    /// </summary>
    public void Handle(Events.ReviewedPhraseEvent message)
    {
      if (!_ReviewingLineEventHandled)
      {
        return;
      }

      var msgPhraseId = message.Ids[HistoryResources.Key_PhraseId];
      var msgPhraseText = message.Strings[HistoryResources.Key_PhraseText];
      var msgLanguageId = message.Ids[HistoryResources.Key_LanguageId];
      var msgLanguageText = message.Strings[HistoryResources.Key_LanguageText];
      var msgReviewedPhraseDuration = message.Duration;

      //MAKE SURE PHRASE IDS, LINE TEXT AND PHRASE TEXT, LANGUAGE ID, AND LANGUAGE TEXT MATCH 
      //BETWEEN REVIEWINGLINE EVENT AND REVIEWED PHRASE EVENT
      if (_PhraseId != msgPhraseId ||
          _LineText != msgPhraseText ||
          _LanguageId != msgLanguageId ||
          _LanguageText != msgLanguageText)
      {
        //if (!_IsReset)
        //  Reset();
        return;
      }

      //GET THE FEEDBACK FROM THE REVIEWED PHRASE EVENT
      _FeedbackAsDouble = message.Doubles[HistoryResources.Key_FeedbackAsDouble];
      _ReviewedPhraseEventHandled = true;

      DispatchCompoundEvent();
    }
    #endregion

    #endregion

    private void DispatchCompoundEvent()
    {
      if (!_ReviewingLineEventHandled ||
          !_ReviewedPhraseEventHandled)
        throw new HistoryException();

        //DISPATCH THE REVIEWED LINE COMPOUND EVENT
        var reviewedLineEvent = new Events.ReviewedLineEvent(_LineId, _ReviewMethodId, _LineText, _LineNumber, 
          _PhraseId, _LanguageId, _LanguageText, _FeedbackAsDouble, _ReviewedPhraseDuration);
        HistoryPublisher.Ton.PublishEvent(reviewedLineEvent);   
    }

    public void Handle(Events.ActivatedLineEvent message)
    {
      var languageText = message.GetDetail<string>(HistoryResources.Key_LanguageText);
      var lineText = message.GetDetail<string>(HistoryResources.Key_LineText);
      var lineId = message.GetDetail<Guid>(HistoryResources.Key_LineId);
      var languageId = message.GetDetail<Guid>(HistoryResources.Key_LanguageId);
      var lineNumber = message.GetDetail<int>(HistoryResources.Key_LineNumber);

      var lineInfo = new HistoryLineInfo()
      {
        LanguageId = languageId,
        LanguageText = languageText,
        LineId = lineId,
        LineText = lineText,
        LineNumber = lineNumber
      };

      _ActiveLines.Add(lineInfo);
    }

    protected override Guid GetId()
    {
      return Guid.Parse(HistoryResources.CompoundEventMakerIdReviewedLine);
    }
  }
}
