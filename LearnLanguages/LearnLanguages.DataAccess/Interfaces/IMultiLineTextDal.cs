using System;
using System.Collections.Generic;

namespace LearnLanguages.DataAccess
{
  public interface IMultiLineTextDal
  {
    Result<MultiLineTextDto> New(object criteria);
    Result<MultiLineTextDto> Fetch(Guid id);
    /// <summary>
    /// Fetches all multiLineTexts that contain the line in lineDto using that line's id
    /// </summary>
    Result<ICollection<MultiLineTextDto>> FetchByLineId(Guid lineId);
    Result<ICollection<MultiLineTextDto>> Fetch(ICollection<Guid> ids);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<MultiLineTextDto> Update(MultiLineTextDto dto);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<MultiLineTextDto> Insert(MultiLineTextDto dto);
    Result<MultiLineTextDto> Delete(Guid id);
    Result<ICollection<MultiLineTextDto>> GetAll();
  }
}
