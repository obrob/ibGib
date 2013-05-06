using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface ILanguageDal
  {
    Result<LanguageDto> New(object criteria);

    Result<LanguageDto> Fetch(Guid id);
    Result<LanguageDto> Fetch(string languageText);
    
    Result<LanguageDto> Update(LanguageDto dto);
    
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<LanguageDto> Insert(LanguageDto dto);

    /// <summary>
    /// Apparently cannot overload when using WCF services.
    /// </summary>
    Result<LanguageDto> Delete(Guid id);

    Result<ICollection<LanguageDto>> GetAll();
  }
}
