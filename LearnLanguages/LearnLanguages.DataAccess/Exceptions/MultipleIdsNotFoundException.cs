using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class MultipleIdsNotFoundException : Exception
  {
    public MultipleIdsNotFoundException()
      : base(DalResources.ErrorMsgMultipleIdsNotFoundException)
    {
    }

    public MultipleIdsNotFoundException(string errorMsg, Guid id)
      : base(errorMsg)
    {
    }

    public MultipleIdsNotFoundException(Exception innerException)
      : base(DalResources.ErrorMsgMultipleIdsNotFoundException, innerException)
    {
    }

  }
}
