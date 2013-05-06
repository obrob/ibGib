using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface ITranslationDal
  {
    Result<TranslationDto> New(object criteria);
    Result<TranslationDto> Fetch(Guid id);
    /// <summary>
    /// Fetches all translations that contain the phrase in phraseDto using that phrases id
    /// </summary>
    Result<ICollection<TranslationDto>> FetchByPhraseId(Guid phraseId);
    Result<ICollection<TranslationDto>> Fetch(ICollection<Guid> ids);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<TranslationDto> Update(TranslationDto dto);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<TranslationDto> Insert(TranslationDto dto);
    Result<TranslationDto> Delete(Guid id);
    Result<ICollection<TranslationDto>> GetAll();
  }
}
