using System;
using Csla.Serialization;

namespace LearnLanguages.Common.CslaBases
{
  [Serializable]
  public abstract class BusinessListBase<L, C, CDto> :
    Csla.BusinessListBase<L, C>
    where L : CslaBases.BusinessListBase<L, C, CDto>
    where C : CslaBases.BusinessBase<C, CDto>
    where CDto : class
  {

  }
}
