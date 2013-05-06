using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IHistoryEvent
  {
    /// <summary>
    /// Id of this HistoryEvent being published
    /// </summary>
    Guid Id { get; }
    /// <summary>
    /// Should be Publisher's time stamp when event is created, but this is not guaranteed.
    /// </summary>
    DateTime TimeStamp { get; }
    /// <summary>
    /// Should be the event's duration, but this is not guaranteed.
    /// </summary>
    TimeSpan Duration { get; }
    /// <summary>
    /// Text describing the event.  
    /// E.g. "PhraseCreatedFromScratch", "ChildPhraseCreatedFromParent", "PhraseViewedOnScreen"
    /// "Phrase
    /// </summary>
    string Text { get; }
    /// <summary>
    /// Event's participants' ids.
    /// E.g. "ChildId=C8455D66-80DC-4F3F-AB93-3C00B036F3F0", "ParentId=1167A64B-F8CE-4C53-BCA8-8B68CDE7D593"
    /// </summary>
    MobileDictionary<string, Guid> Ids { get; }
    /// <summary>
    /// Event's strings.
    /// E.g. "Username=WorldsGreatestUser", "FavoriteColor=Blue"
    /// </summary>
    MobileDictionary<string, string> Strings { get; }
    /// <summary>
    /// Event's int values.
    /// E.g. "MaxHeightReached=32", "DucksHit=2"
    /// </summary>
    MobileDictionary<string, int> Ints { get; }
    /// <summary>
    /// Event's double values.
    /// E.g. "ScoreInPercent=35.7283849", "PredictedValue=0.00000239839294877"
    /// </summary>
    MobileDictionary<string, double> Doubles { get; }
  }
}
