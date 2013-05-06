using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface ILineDal
  {
    Result<LineDto> New(object criteria);
    Result<LineDto> Fetch(Guid id);
    Result<ICollection<LineDto>> Fetch(ICollection<Guid> ids);
    Result<LineDto> Update(LineDto dto);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<LineDto> Insert(LineDto dto);
    Result<LineDto> Delete(Guid id);
    Result<ICollection<LineDto>> GetAll();
  }
}
