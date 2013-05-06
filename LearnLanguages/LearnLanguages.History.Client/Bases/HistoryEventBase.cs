using System;
using System.Collections.Generic;
using Csla.Serialization;
using Csla.Core;

namespace LearnLanguages.History.Bases
{
  [Serializable]
  public abstract class HistoryEventBase : Common.Interfaces.IHistoryEvent
  {
    /// <summary>
    /// Does NOT create the dictionaries Ids, Strings, Ints, Doubles.
    /// </summary>
    public HistoryEventBase()
    {
      Id = Guid.NewGuid();
      TimeStamp = DateTime.Now;
    }

    /// <summary>
    /// DOES not create the dictionaries Ids, Strings, Ints, Doubles, even if these are not utilized.
    /// Create your own ctor if you want to keep from creating these would-be empty dictionaries.
    /// </summary>
    /// <param name="duration">event duration</param>
    /// <param name="text">text describing the event</param>
    /// <param name="details">key/value pairs of details of event.  e.g. ("ChildId", 9579019B-A958-40EF-9021-D622FCD17DE7), ("ParentLanguage", "English")</param>
    public HistoryEventBase(TimeSpan duration, params KeyValuePair<string, object>[] details)
      : this()
    {
      Duration = duration;

      Ids = new MobileDictionary<string, Guid>();
      Strings = new MobileDictionary<string, string>();
      Ints = new MobileDictionary<string, int>();
      Doubles = new MobileDictionary<string, double>();

      AddDetails(details);
    }

    /// <summary>
    /// This should be most common.  Use this when you have a specific item that the event
    /// pertains to.  This can be used in multiple events to link them together to form
    /// something similar to a saga, or a long-running process.
    /// For common types, use HistoryResources.Key_TargetTypePhrase/Line/MultiLineText as the 
    /// targetType string.
    /// </summary>
    public HistoryEventBase(Guid targetId, 
                            string targetType, 
                            TimeSpan duration, 
                            params KeyValuePair<string, object>[] details)
      : this(duration, details)
    {
      AddDetail(HistoryResources.Key_TargetId, targetId);
      AddDetail(HistoryResources.Key_TargetType, targetType);
    }


    protected void AddDetails(KeyValuePair<string, object>[] details)
    {
      if (details != null && details.Length > 0)
      {
        foreach (var detail in details)
          AddDetail(detail.Key, detail.Value);
      }
    }

    public Guid Id { get; protected set; }

    public DateTime TimeStamp { get; private set; }
    public TimeSpan Duration { get; protected set; }

    /// <summary>
    /// Default virtual implementation of GetText is return this.GetType().FullName;
    /// </summary>
    public string Text { get { return GetText(); } }

    protected virtual string GetText()
    {
      return this.GetType().FullName;
    }

    public virtual void AddDetail(string key, object value)
    {
      if (value is Guid)
        Ids.Add(key, (Guid)value);
      else if (value is string)
        Strings.Add(key, (string)value);
      else if (value is int)
        Ints.Add(key, (int)value);
      else if (value is double)
        Doubles.Add(key, (double)value);
      else
        Strings.Add(key, value.ToString());
    }
    public virtual bool TryGetDetail(string key, out object detail)
    {
      if (string.IsNullOrEmpty(key))
        throw new ArgumentNullException("key");

      bool containsDetail = false;
      detail = null;
      if (Ids.ContainsKey(key))
      {
        containsDetail = true;
        detail = Ids[key];
      }
      else if (Strings.ContainsKey(key))
      {
        containsDetail = true;
        detail = Strings[key];
      }
      else if (Ints.ContainsKey(key))
      {
        containsDetail = true;
        detail = Ints[key];
      }
      else if (Doubles.ContainsKey(key))
      {
        containsDetail = true;
        detail = Doubles[key];
      }

      return containsDetail;
    }
    public virtual object GetDetail(string key)
    {
      object detail = null;
      if (!TryGetDetail(key, out detail))
        throw new ArgumentException("key");
      return detail;
    }

    public virtual T GetDetail<T>(string key)
    {
      return (T)GetDetail(key);
    }
    public virtual bool TryGetDetail<T>(string key, out T detail)
    {
      detail = default(T);
      try
      {
        object detailUncasted = null;
        var hasDetailUncasted = TryGetDetail(key, out detailUncasted);
        if (!hasDetailUncasted)
          return false;

        if (!(detailUncasted is T))
          return false;

        detail = (T)detailUncasted;
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public MobileDictionary<string, Guid> Ids { get; protected set; }
    public MobileDictionary<string, string> Strings { get; protected set; }
    public MobileDictionary<string, int> Ints { get; protected set; }
    public MobileDictionary<string, double> Doubles { get; protected set; }
  }
}
