using System;
using Csla.Serialization;
using System.Collections.Generic;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class MultiLineTextDto
  {
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string AdditionalMetadata { get; set; }
    public ICollection<Guid> LineIds { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; }  
  }
}
