using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.History.Bases
{
  public class ThinkingEventBase : HistoryEventBase
  {
    public ThinkingEventBase(Guid targetId)
      : base(TimeSpan.MinValue)
    {
      AddTargetIdDetail(targetId);
    }

    private void AddTargetIdDetail(Guid targetId)
    {
      AddDetail(HistoryResources.Key_TargetId, targetId);
    }
  }
}
