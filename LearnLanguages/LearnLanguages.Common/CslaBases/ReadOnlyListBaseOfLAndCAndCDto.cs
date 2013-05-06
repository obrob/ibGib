using Csla.Serialization;
using System;

namespace LearnLanguages.Common.CslaBases
{
  [Serializable]
  public abstract class ReadOnlyListBase<L, C, CDto> :
    Csla.ReadOnlyListBase<L, C>
    where L : CslaBases.ReadOnlyListBase<L, C, CDto>
    where C : CslaBases.BusinessBase<C, CDto>
    where CDto : class
  {

  }
}
