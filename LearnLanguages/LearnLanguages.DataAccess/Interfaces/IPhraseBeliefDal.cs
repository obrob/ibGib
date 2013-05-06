using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface IPhraseBeliefDal
  {
    Result<PhraseBeliefDto> New(object criteria);
    Result<PhraseBeliefDto> Fetch(Guid id);
    Result<ICollection<PhraseBeliefDto>> FetchAllRelatedToPhrase(Guid phraseId);
    Result<PhraseBeliefDto> Update(PhraseBeliefDto dto);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<PhraseBeliefDto> Insert(PhraseBeliefDto dto);
    Result<PhraseBeliefDto> Delete(Guid id);
    Result<ICollection<PhraseBeliefDto>> GetAll();
  }
}
