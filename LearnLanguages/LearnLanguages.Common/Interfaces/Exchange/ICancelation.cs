using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface ICancelation : IExchangeMessage
  {
    /// <summary>
    /// Id of opportunity that was canceled.
    /// </summary>
    Guid OpportunityId { get; }
  }
}
