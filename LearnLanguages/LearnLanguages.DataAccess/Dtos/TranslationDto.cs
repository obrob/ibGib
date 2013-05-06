using System;
using Csla.Serialization;
using System.Collections.Generic;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class TranslationDto
  {
    public Guid Id { get; set; }
    public ICollection<Guid> PhraseIds { get; set; }
    public Guid ContextPhraseId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; }  
  }
}
