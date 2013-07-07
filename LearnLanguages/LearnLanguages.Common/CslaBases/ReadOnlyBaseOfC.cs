using System;
using System.Net;
using Csla.Serialization;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Common.CslaBases
{
  [Serializable]
  public abstract class ReadOnlyBase<C> : Csla.ReadOnlyBase<C>, IHaveId
    where C : CslaBases.ReadOnlyBase<C>
  {
    public Guid Id { get; protected set; }
  }
}
