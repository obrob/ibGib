using System;
using LearnLanguages.Business;
using System.Collections.Generic;
using Csla.Serialization;

namespace LearnLanguages.History.Bases
{
  [Serializable]
  public abstract class ParentPhraseChildPhraseEventBase : HistoryEventBase
  {
    public ParentPhraseChildPhraseEventBase()
      : base()
    {

    }

    public ParentPhraseChildPhraseEventBase(PhraseEdit parent, PhraseEdit child)
      : base(TimeSpan.MinValue, 
             new KeyValuePair<string, object>(HistoryResources.Key_ChildId, child.Id), 
             new KeyValuePair<string, object>(HistoryResources.Key_ParentId, parent.Id))
    {
    }

    public ParentPhraseChildPhraseEventBase(PhraseEdit parent, PhraseEdit child, TimeSpan duration)
      : base(duration,
             new KeyValuePair<string, object>(HistoryResources.Key_ChildId, child.Id),
             new KeyValuePair<string, object>(HistoryResources.Key_ParentId, parent.Id))
    {
    }
  }
}
