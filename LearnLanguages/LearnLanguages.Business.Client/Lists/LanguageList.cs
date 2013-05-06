using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  [Serializable]
  public class LanguageList : Common.CslaBases.BusinessListBase<LanguageList, LanguageEdit, LanguageDto>
  {
    #region Factory Methods

    public static async Task<LanguageList> GetAllAsync()
    {
      var result = await DataPortal.FetchAsync<LanguageList>();
      return result;
    }
    
#if !SILVERLIGHT
    public static LanguageList GetAll()
    {
      return DataPortal.Fetch<LanguageList>();
    }
#endif

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        Result<ICollection<LanguageDto>> result = languageDal.GetAll();
        if (!result.IsSuccess || result.IsError)
        {
          if (result.Info != null)
          {
            var ex = result.GetExceptionFromInfo();
            if (ex != null)
              throw new FetchFailedException(ex.Message);
            else
              throw new FetchFailedException();
          }
          else
            throw new FetchFailedException();
        }

        //RESULT WAS SUCCESSFUL
        var allLanguageDtos = result.Obj;
        foreach (var languageDto in allLanguageDtos)
        {
          var languageEdit = DataPortal.FetchChild<LanguageEdit>(languageDto);
          Add(languageEdit);
        }
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        base.Child_Update();
      }
    }
#endif

    #endregion
  }
}
