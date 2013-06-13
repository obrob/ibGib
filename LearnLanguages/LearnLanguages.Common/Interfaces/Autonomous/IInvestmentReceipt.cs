using System;

namespace LearnLanguages.Common.Interfaces.Autonomous
{
  public interface IInvestmentReceipt : IHaveId
  {
    bool Completed { get; }
    bool NeedAdditionalInformation { get; }
  }
}
