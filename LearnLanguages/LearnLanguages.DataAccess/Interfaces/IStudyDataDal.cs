using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface IStudyDataDal
  {
    Result<StudyDataDto> New(object criteria);
    //Result<StudyDataDto> Fetch(Guid id);
    Result<bool> StudyDataExistsForCurrentUser();
    Result<StudyDataDto> FetchForCurrentUser();
    Result<StudyDataDto> Update(StudyDataDto dto);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<StudyDataDto> Insert(StudyDataDto dto);
    /// <summary>
    /// Apparently cannot overload when using WCF services.
    /// </summary>
    Result<StudyDataDto> Delete(Guid id);
    Result<ICollection<StudyDataDto>> GetAll();
  }
}
