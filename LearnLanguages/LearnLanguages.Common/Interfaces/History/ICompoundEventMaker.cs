using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface ICompoundEventMaker
  {
    /// <summary>
    /// Id of this HistoryEvent being published
    /// </summary>
    Guid Id { get; }
    void Enable();
    void Disable();
  }
}
