using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class StudyDataDto
  {
    public Guid Id { get; set; }
    public string NativeLanguageText { get; set; }
    public string Username { get; set; }
  }
}
