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
  public class ReviewedLineEvent : Bases.SingleLineEventBase
  {
    public ReviewedLineEvent()
      : base()
    {

    }

    public ReviewedLineEvent(Guid lineId, Guid reviewMethodId, string lineText, int lineNumber, Guid phraseId,
      Guid languageId, string languageText, double feedbackAsDouble, TimeSpan duration)
      : base(lineId, lineText, lineNumber, phraseId, languageId, languageText, duration)
    {
      AddDetail(HistoryResources.Key_ReviewMethodId, reviewMethodId);
      AddDetail(HistoryResources.Key_FeedbackAsDouble, feedbackAsDouble);
    }


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
