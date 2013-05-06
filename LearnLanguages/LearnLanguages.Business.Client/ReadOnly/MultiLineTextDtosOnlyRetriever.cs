using System;
using System.Linq;
using System.Net;
using Csla;
using Csla.Serialization;
using Csla.Core;
using LearnLanguages.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearnLanguages.DataAccess.Exceptions;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class retrieves phrases by searching using their text and language.text.
  /// </summary>
  [Serializable]
  public class MultiLineTextDtosOnlyRetriever : Common.CslaBases.ReadOnlyBase<MultiLineTextDtosOnlyRetriever>
  {
    public MultiLineTextDtosOnlyRetriever()
    {
      //_RetrievedDtos = new MobileList<MultiLineTextDto>();
    }

    #region Factory Methods

    public static async Task<MultiLineTextDtosOnlyRetriever> CreateNewAsync() 
    {
      var result = await DataPortal.CreateAsync<MultiLineTextDtosOnlyRetriever>();
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


    public static readonly PropertyInfo<string> IdsAndTitlesAsStringProperty = 
      RegisterProperty<string>(c => c.IdsAndTitlesAsString);
    /// <summary>
    /// This is a string with Id=Title|Id=Title..etc . It's a fucking lame kluge, 
    /// because their shit is broken. I've been working on this waaaay too fucking long.
    /// For now, I'm not going to differentiate song from non-song.
    /// </summary>
    public string IdsAndTitlesAsString
    {
      get { return GetProperty(IdsAndTitlesAsStringProperty); }
      set { LoadProperty(IdsAndTitlesAsStringProperty, value); }
    }

    //public static readonly PropertyInfo<MobileList<MultiLineTextDto>> RetrievedDtosProperty = 
    //  RegisterProperty<MobileList<MultiLineTextDto>>(c => c.RetrievedDtos, RelationshipTypes.PrivateField);
    //private MobileList<MultiLineTextDto> _RetrievedDtos = RetrievedDtosProperty.DefaultValue;
    //public MobileList<MultiLineTextDto> RetrievedDtos
    //{
    //  get { return GetProperty(RetrievedDtosProperty, _RetrievedDtos); }
    //  private set { _RetrievedDtos = value; }
    //}

    //public static readonly PropertyInfo<MobileList<MultiLineTextDto>> RetrievedDtosProperty =
    //  RegisterProperty<MobileList<MultiLineTextDto>>(c => c.RetrievedDtos);
    //public MobileList<MultiLineTextDto> RetrievedDtos
    //{
    //  get { return ReadProperty(RetrievedDtosProperty); }
    //  private set { LoadProperty(RetrievedDtosProperty, value); }
    //}

    #endregion

    public Dictionary<Guid, string> GetIdsAndTitles()
    {
      var retDictionary = new Dictionary<Guid, string>();
      if (string.IsNullOrEmpty(IdsAndTitlesAsString))
        return retDictionary;

      //PARSE THE IDS AND TITLES STRING
      //FORMAT IS AS FOLLOWS: ID=TITLE|ID=TITLE
      //ANY ='S AND |'S IN THE ACTUAL TITLE IS REPLACED DURING ENCODING WITH THE CORRESPONDING 
      //REPLACEMENT STRING
      
      var keyValuePairs = IdsAndTitlesAsString.Split('|');
      foreach (var idTitlePair in keyValuePairs)
      {
        var splitPair = idTitlePair.Split('=');
        var idAsString = splitPair[0];
        var title = splitPair[1];

        var id = Guid.Parse(idAsString);
        title = title.Replace(_EqualsSignEncodingReplacementString, "=");
        title = title.Replace(_PipeEncodingReplacementString, "|");

        retDictionary.Add(id, title);
      }

      return retDictionary;
    }


    private const string _PipeEncodingReplacementString = @"(*&#@$*";
    private const string _EqualsSignEncodingReplacementString= @"*#*$&$*";
    #region DP_XYZ

    
#if !SILVERLIGHT

    public void DataPortal_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var MultiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();

        Result<ICollection<MultiLineTextDto>> result = MultiLineTextDal.GetAll();
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

        foreach (var dto in result.Obj)
        {
          var id = dto.Id;
          var title = dto.Title;

          //PARSE THE IDS AND TITLES STRING
          //FORMAT IS AS FOLLOWS: ID=TITLE|ID=TITLE
          //ANY ='S AND |'S IN THE ACTUAL TITLE IS REPLACED DURING ENCODING WITH THE CORRESPONDING 
          //REPLACEMENT STRING
          if (!string.IsNullOrEmpty(IdsAndTitlesAsString))
            IdsAndTitlesAsString += "|";

          title = title.Replace("=", _EqualsSignEncodingReplacementString);
          title = title.Replace("|", _PipeEncodingReplacementString);

          IdsAndTitlesAsString += id.ToString() + "=" + title;
        }
      }
    }
  
#endif

    #endregion
  }
}
