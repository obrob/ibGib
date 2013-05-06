using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class StudyDataNotFoundForUserException : Exception
  {
    public StudyDataNotFoundForUserException(string username)
      : base(string.Format(DalResources.ErrorMsgStudyDataNotFoundForUserException, username))
    {
      Username = username;
    }

    public StudyDataNotFoundForUserException(string errorMsg, string username)
      : base(errorMsg)
    {
      Username = username;
    }

    public StudyDataNotFoundForUserException(Exception innerException, string username)
      : base(string.Format(DalResources.ErrorMsgStudyDataNotFoundForUserException, username), innerException)
    {
      Username = username;
    }

    /// <summary>
    /// Username whose StudyData cannot be found.
    /// </summary>
    public string Username { get; private set; }
  }
}
