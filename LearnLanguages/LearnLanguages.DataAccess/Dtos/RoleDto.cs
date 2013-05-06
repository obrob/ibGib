using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class RoleDto
  {
    public Guid Id { get; set; }
    public string Text { get; set; }
  }
}
