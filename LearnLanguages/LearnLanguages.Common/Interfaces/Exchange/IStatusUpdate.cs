using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IStatusUpdate : IExchangeMessage
  {
    string Status { get; }
  }
}
