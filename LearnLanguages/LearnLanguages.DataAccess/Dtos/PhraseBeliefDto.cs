using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public class PhraseBeliefDto
  {
    public Guid Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Text { get; set; }
    public double Strength { get; set; }
    public Guid BelieverId { get; set; }
    public Guid ReviewMethodId { get; set; }
    public Guid PhraseId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; }
  }
}
