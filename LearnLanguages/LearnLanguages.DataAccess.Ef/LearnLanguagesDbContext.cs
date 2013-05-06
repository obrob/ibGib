using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace LearnLanguages.DataAccess.Ef
{
  public class LearnLanguagesDbContext : DbContext
  {
    public LearnLanguagesDbContext()
      : base()
    {

    }

    public LearnLanguagesDbContext(string nameOrConnectionString)
      : base(nameOrConnectionString)
    {

    }

    public DbSet<PhraseDto> Phrases { get; set; }
    public DbSet<LanguageDto> Languages { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
      //base.OnModelCreating(modelBuilder);
    }
  }
}
