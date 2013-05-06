using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class IdAlreadyExistsException : Exception
  {
    public IdAlreadyExistsException(Guid id)
      : base(string.Format(DalResources.ErrorMsgIdAlreadyExistsException, id))
    {
      Id = id; 
    }

    public IdAlreadyExistsException(string errorMsg, Guid id)
      : base(errorMsg)
    {
      Id = id;
    }

    public IdAlreadyExistsException(Exception innerException, Guid id)
      : base(string.Format(DalResources.ErrorMsgIdAlreadyExistsException, id), innerException)
    {
      Id = id;
    }

    public Guid Id { get; private set; }
  }
}
