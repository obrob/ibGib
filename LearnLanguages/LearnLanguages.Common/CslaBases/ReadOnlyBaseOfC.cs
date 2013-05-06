using System;
using System.Net;
using Csla.Serialization;

namespace LearnLanguages.Common.CslaBases
{
  [Serializable]
  public abstract class ReadOnlyBase<C> : Csla.ReadOnlyBase<C>
    where C : CslaBases.ReadOnlyBase<C>
  {

  }
}
