using System;
using LearnLanguages.Business;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  public class StudyJobInfo<TTarget, TProduct> : IJobInfo<TTarget, TProduct>
  {
    /// <summary>
    /// Creates a new StudyJobInfo object with the given "immutable" parameters.
    /// Product however can be set once externally (e.g. when the job is done).
    /// ExpectedPrecision defaults to 0, ie anything will do.
    /// </summary>
    public StudyJobInfo(TTarget target, LanguageEdit language, DateTime expirationDate, double expectedPrecision)
    {
      Id = Guid.NewGuid();
      Target = target;
      ExpirationDate = expirationDate;
      Criteria = new StudyJobCriteria(language, expectedPrecision);
    }

    public StudyJobInfo(TTarget target, LanguageEdit language, DateTime expirationDate, object criteria)
    {
      Id = Guid.NewGuid();
      Target = target;
      ExpirationDate = expirationDate;
      Criteria = new StudyJobCriteria(language);
    }

    public Guid Id { get; private set; }
    
    public TTarget Target { get; private set; }
    private TProduct _Product;
    public TProduct Product 
    {
      get { return _Product;}
      set
      {
        if (_Product == null)
        {
          _Product = value;
        }
      }
    }

    public DateTime ExpirationDate { get; private set; }
    public object Criteria { get; private set; }

    public static DateTime NoExpirationDate
    {
      get
      {
        return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;
      }
    }

  }
}
