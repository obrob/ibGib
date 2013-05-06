using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IJobInfo
  {
    Guid Id { get; }
    DateTime ExpirationDate { get; }
    object Criteria { get; }
  }
}
