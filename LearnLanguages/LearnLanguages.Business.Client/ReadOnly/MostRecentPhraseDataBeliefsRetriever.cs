using System;
using System.Net;
using Csla;
using Csla.Serialization;
using System.Threading.Tasks;
using Csla.Core;
using System.Linq;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This retriever gets the most recent phrase belief data for each
  /// phrase for the current user.
  /// </summary>
  [Serializable]
  public class MostRecentPhraseDataBeliefsRetriever : 
    Common.CslaBases.ReadOnlyBase<MostRecentPhraseDataBeliefsRetriever>
  {
    #region Factory Methods

    public static async Task<MostRecentPhraseDataBeliefsRetriever> CreateNewAsync()
    {
      var result = await DataPortal.CreateAsync<MostRecentPhraseDataBeliefsRetriever>();
      return result;
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = 
      RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return GetProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<PhraseBeliefList> MostRecentPhraseBeliefsProperty = RegisterProperty<PhraseBeliefList>(c => c.MostRecentPhraseBeliefs);
    public PhraseBeliefList MostRecentPhraseBeliefs
    {
      get { return ReadProperty(MostRecentPhraseBeliefsProperty); }
      set { LoadProperty(MostRecentPhraseBeliefsProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create()
    {
      RetrieverId = Guid.NewGuid();

      ///GET ALL THE PHRASE BELIEFS FOR THE CURRENT USER
      var allBeliefs = PhraseBeliefList.GetAll();

      ///ONLY WANT THE MOST RECENT BELIEF FOR EACH PHRASE
      ///THIS IS PROBABLY RIDICULOUSLY SLOW, BUT IF IT'S TOO
      ///SLOW, I'LL PUT IT IN A STORED PROCEDURE, AS THE FOLLOWING 
      ///COMMENTED OUT SELECT STATEMENT WORKS (AND IS PROBABLY FASTER)
      var results = from belief in allBeliefs
                    where belief.TimeStamp ==
                          (
                            from beliefInnerQuery in allBeliefs
                            where beliefInnerQuery.PhraseId == belief.PhraseId
                            select beliefInnerQuery.TimeStamp
                          ).Max()
                    select belief;

      MostRecentPhraseBeliefs = PhraseBeliefList.NewPhraseBeliefListNewedUpOnly();
      MostRecentPhraseBeliefs.AddRange(results);
/*
SELECT *
  FROM [LearnLanguagesDb].[dbo].[PhraseBeliefDatas] pb1
 WHERE TimeStamp =
                   ( SELECT MAX(TimeStamp)
                       FROM [LearnLanguagesDb].[dbo].[PhraseBeliefDatas] pb2
                      WHERE pb1.[PhraseDataId] = pb2.[PhraseDataId]
                   );
*/
    }
#endif

    #endregion
  }
}
