using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class PhraseDto
  {
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid LanguageId  { get; set; } 
    public Guid UserId { get; set; }      
    public string Username { get; set; }
  }
}
