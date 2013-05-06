using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  /// <summary>
  /// DETAILS: LINE ID
  /// REVIEWMETHODID
  /// LINETEXT
  /// LINENUMBER
  /// PHRASEID
  /// LANGUAGEID
  /// LANGUAGETEXT
  /// FEEDBACKASDOUBLE
  /// DURATION
  /// </summary>
  [Serializable]
  public class ReviewedLineOrderEvent : Bases.SingleLineEventBase
  {
    public ReviewedLineOrderEvent()
      : base()
    {

    }

    public ReviewedLineOrderEvent(Guid lineId, Guid reviewMethodId, string lineText, 
      int lineNumber, double feedbackAsDoubleGiven, TimeSpan duration)
      : base(lineId, reviewMethodId, lineText, lineNumber, feedbackAsDoubleGiven, duration)
    {

    }

    //todo: left off here.  need to make this ctor...see how it will be used.

    //public ReviewedLineOrderEvent(Guid lineId, Guid reviewMethodId, string lineText, int lineNumber, Guid phraseId,
    //  Guid languageId, string languageText, double feedbackAsDouble, TimeSpan duration)
    //  : base(lineId, lineText, lineNumber, phraseId, languageId, languageText, duration)
    //{
    //  AddDetail(HistoryResources.Key_ReviewMethodId, reviewMethodId);
    //  AddDetail(HistoryResources.Key_FeedbackAsDouble, feedbackAsDouble);
    //}


    //public LineReviewedEvent(LineEdit line, Guid reviewMethodId)
    //  : base(line, 
    //         new KeyValuePair<string, object>(HistoryResources.Key_ReviewMethodId, reviewMethodId))
    //{

    //}

    //public LineReviewedEvent(LineEdit line, Guid reviewMethodId, TimeSpan duration)
    //  : base(line, 
    //         duration,
    //         new KeyValuePair<string, object>(HistoryResources.Key_ReviewMethodId, reviewMethodId))
    //{

    //}

    //public LineReviewedEvent(LineEdit line, Guid reviewMethodId, TimeSpan duration, double feedbackAsDouble)
    //  : base(line,
    //         duration,
    //         new KeyValuePair<string, object>(HistoryResources.Key_ReviewMethodId, reviewMethodId),
    //         new KeyValuePair<string, object>(HistoryResources.Key_FeedbackAsDouble, feedbackAsDouble))
    //{

    //}
  }
}
