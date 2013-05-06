using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class LineDto
  {
    public Guid Id { get; set; }
    public Guid PhraseId { get; set; }
    public int LineNumber { get; set; }
    public Guid UserId { get; set; }      
    public string Username { get; set; }
  }
}
