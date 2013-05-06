using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class IdNotFoundException : Exception
  {
    public IdNotFoundException(Guid id)
      : base(string.Format(DalResources.ErrorMsgIdNotFoundException, id))
    {
      Id = id; 
    }

    public IdNotFoundException(string errorMsg, Guid id)
      : base(errorMsg)
    {
      Id = id;
    }

    public IdNotFoundException(Exception innerException, Guid id)
      : base(string.Format(DalResources.ErrorMsgIdNotFoundException, id), innerException)
    {
      Id = id;
    }

    public Guid Id { get; private set; }
  }
}
