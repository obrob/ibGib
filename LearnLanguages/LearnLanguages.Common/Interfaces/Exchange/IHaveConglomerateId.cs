using System;

namespace LearnLanguages.Common.Interfaces
{
  /// <summary>
  /// This refers to an id of the conglomerate's containing this entity.  A conglomerate example
  /// would be DefaultStudier, DefaultMultiLineTextsStuder, DefaultLineStudier, etc.
  /// </summary>
  public interface IHaveConglomerateId
  {
    Guid ConglomerateId { get; }
  }
}
