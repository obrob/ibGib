using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IExchangeMessage
  {
    Guid Id { get; }
    Guid PublisherId { get; }
    object Publisher { get; }
    object Information { get; }
    string Category { get; }
  }
}
