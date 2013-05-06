using System;

namespace LearnLanguages.DataAccess
{
  public interface IDalManager : IDisposable
  {
    T GetProvider<T>() where T : class;
  }
}
