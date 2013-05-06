using System;
using System.Net;
using Csla.Serialization;

namespace LearnLanguages.Common.CslaBases
{
  [Serializable]
  public abstract class CommandBase<C> : Csla.CommandBase<C>
    where C : CslaBases.CommandBase<C>
  {

  }
}
