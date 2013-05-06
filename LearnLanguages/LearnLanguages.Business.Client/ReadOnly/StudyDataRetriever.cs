using System;
using System.Net;
using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class gets a study data for the current user.  If it does not exist, it creates a new 
  /// study data, filled with defaults, and returns that.  It flags this with the bool 
  /// StudyDataAlreadyExisted property.
  /// </summary>
  [Serializable]
  public class StudyDataRetriever : Common.CslaBases.ReadOnlyBase<StudyDataRetriever>
  {
    #region Factory Methods

    /// <summary>
    /// Retrieves the StudyDataEdit for the current user.  If the current user does not have one,
    /// a new one is created.  If the current user does have one, it is fetched.
    /// </summary>
    public static async Task<StudyDataRetriever> CreateNewAsync()
    {
      var result = await DataPortal.CreateAsync<StudyDataRetriever>();
      return result;
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return GetProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<StudyDataEdit> StudyDataProperty = 
      RegisterProperty<StudyDataEdit>(c => c.StudyData);
    public StudyDataEdit StudyData
    {
      get { return ReadProperty(StudyDataProperty); }
      private set { LoadProperty(StudyDataProperty, value); }
    }

    public static readonly PropertyInfo<bool> StudyDataAlreadyExistedProperty = 
      RegisterProperty<bool>(c => c.StudyDataAlreadyExisted);
    public bool StudyDataAlreadyExisted
    {
      get { return ReadProperty(StudyDataAlreadyExistedProperty); }
      private set { LoadProperty(StudyDataAlreadyExistedProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create()
    {
      RetrieverId = Guid.NewGuid();

      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
        Result<bool> resultExists = studyDataDal.StudyDataExistsForCurrentUser();
        if (!resultExists.IsSuccess)
        {
          Exception error = resultExists.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(resultExists.Msg);
        }
        var userHasStudyData = resultExists.Obj;

        //POPULATE OUR STUDY DATA DTO
        //StudyDataDto dto = null;

        StudyDataAlreadyExisted = userHasStudyData;
        StudyData = DataPortal.Fetch<StudyDataEdit>();
        #region old
          //  Result<StudyDataDto> resultFetch = studyDataDal.FetchForCurrentUser();
          //  if (!resultFetch.IsSuccess)
          //  {
          //    Exception error = resultFetch.GetExceptionFromInfo();
          //    if (error != null)
          //      throw error;
          //    else
          //      throw new FetchFailedException(resultFetch.Msg);
          //  }

          //  dto = resultFetch.Obj;
          //  StudyDataAlreadyExisted = true;
          //}
          //else
          //{
          //  //THE USER DOESN'T HAVE STUDY DATA
          //  //SET PROPS ACCORDINGLY
          //  dto = new StudyDataDto()
          //  {
          //    Id = Guid.Empty,
          //    NativeLanguageText = "",
          //    Username = Csla.ApplicationContext.User.Identity.Name
          //  };
          //  StudyDataAlreadyExisted = false;
          #endregion

        ////OUR DTO IS NOW POPULATED
        //StudyData = StudyDataEdit.NewStudyDataEdit();
        //StudyData.LoadFromDtoBypassPropertyChecks(dto);
      }
    }

#endif

    #endregion
  }
}
