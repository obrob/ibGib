using System;
using Csla.Serialization;

namespace LearnLanguages.Silverlight.Tests
{
  /// <summary>
  /// Use this for expected exceptions in unit testing.  Throw the actual exception as the inner exception.
  /// This way, we know that it is the right exception and the correct/expected context for the exception.
  /// IOW, say a DataPortalException was thrown, but it was an insert exception instead of the expected fetch one.
  /// So, we don't say ExpectedException(DataPortalException), we say ExpectedException(ExpectedException) and 
  /// set its inner exception to the DataPortalException.
  /// </summary>
  [Serializable]
  public class ExpectedException : Exception
  {
    public ExpectedException(Exception innerException)
      : base(TestsResources.ErrorMsgExpectedException, innerException)
    {

    }
  }
}
