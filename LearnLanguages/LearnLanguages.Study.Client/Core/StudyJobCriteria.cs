using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  public class StudyJobCriteria
  {
    public StudyJobCriteria(LanguageEdit language)
    {
      Language = language;
    }
    public StudyJobCriteria(LanguageEdit language, double expectedPrecision)
    {
      Language = language;
      ExpectedPrecision = expectedPrecision;
    }
    
    public LanguageEdit Language { get; private set; }
    public double ExpectedPrecision { get; private set; }
  }
}
