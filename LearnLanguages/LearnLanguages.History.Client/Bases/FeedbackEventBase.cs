using System;
using System.Collections.Generic;
using LearnLanguages.Business;
using Csla.Serialization;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.History.Bases
{
  [Serializable]
  public abstract class FeedbackEventBase : HistoryEventBase
  {
    public FeedbackEventBase()
      : base()
    {

    }

    public FeedbackEventBase(double feedback)
      : base(TimeSpan.MinValue, new KeyValuePair<string, object>(HistoryResources.Key_FeedbackAsDouble, feedback)) 
    {

    }

    public FeedbackEventBase(double feedback, TimeSpan duration)
      : base(duration, new KeyValuePair<string, object>(HistoryResources.Key_FeedbackAsDouble, feedback))
    {

    }

    /// <summary>
    /// If you know the target of the feedback, then you can use this overload
    /// with the targetId, targetType.
    /// </summary>
    public FeedbackEventBase(Guid targetId, string targetType, double feedback, TimeSpan duration)
      : base(targetId, 
             targetType, 
             duration, 
             new KeyValuePair<string, object>(HistoryResources.Key_FeedbackAsDouble, feedback))
    {
      
    }

    //public FeedbackEventBase(IFeedback Feedback, TimeSpan duration)
    //  : base(duration,
    //         new KeyValuePair<string, object>(HistoryResources.Key_FeedbackId, Feedback.Id))
    //{

    //}

    //public FeedbackEventBase(IFeedback Feedback, params KeyValuePair<string, object>[] details)
    //  : base(TimeSpan.MinValue,
    //         new KeyValuePair<string, object>(HistoryResources.Key_FeedbackId, Feedback.Id))
    //{
    //  AddDetails(details);
    //}

    //public FeedbackEventBase(IFeedback Feedback, TimeSpan duration, params KeyValuePair<string, object>[] details)
    //  : base(duration,
    //         new KeyValuePair<string, object>(HistoryResources.Key_FeedbackId, Feedback.Id))
    //{
    //  AddDetails(details);
    //}
  }
}
