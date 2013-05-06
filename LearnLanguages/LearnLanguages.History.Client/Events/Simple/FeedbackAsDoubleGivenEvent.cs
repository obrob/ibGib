using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class FeedbackAsDoubleGivenEvent : Bases.FeedbackEventBase
  {
       public FeedbackAsDoubleGivenEvent()
      : base()
    {

    }

    public FeedbackAsDoubleGivenEvent(double feedback)
      : base(feedback, TimeSpan.MinValue)
    {

    }

    public FeedbackAsDoubleGivenEvent(double feedback, TimeSpan duration)
      : base(feedback, duration)
    {

    }
  }
}
