using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class MultipleItemsFoundDataAccessException : Exception
  {
    public MultipleItemsFoundDataAccessException()
      : base()
    {

    }

    public MultipleItemsFoundDataAccessException(string errorMsg)
      : base(errorMsg)
    {

    }


  }
}
