using System;
using System.Linq;

using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;
using LearnLanguages.DataAccess.Exceptions;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseBeliefList : 
    Common.CslaBases.BusinessListBase<PhraseBeliefList, 
                                      PhraseBeliefEdit, 
                                      PhraseBeliefDto>
  {
    #region Factory Methods

    /// <summary>
    /// Get all of the PhraseBeliefEdits that belong to the current user.
    /// </summary>
    public static async Task<PhraseBeliefList> GetAllAsync()
    {
      var result = await DataPortal.FetchAsync<PhraseBeliefList>();
      return result;
    }

#if !SILVERLIGHT

    /// <summary>
    /// Get all of the PhraseBeliefEdits that belong to the current user.
    /// </summary>
    public static PhraseBeliefList GetAll()
    {
      var result = DataPortal.Fetch<PhraseBeliefList>();
      return result;
    }

#endif

    /// <summary>
    /// Gets all beliefs about a given phrase, using phrase.Id
    /// </summary>
    public static async Task<PhraseBeliefList> GetBeliefsAboutPhraseAsync(Guid phraseId)
    {
      var criteria = new Criteria.PhraseIdCriteria(phraseId);
      var result = await DataPortal.FetchAsync<PhraseBeliefList>(criteria);
      return result;
    }

    public static async Task<PhraseBeliefList>
      GetBeliefsAboutPhrasesInMultiLineTextsAsync(MultiLineTextList mltList)
    {
      var result = await DataPortal.FetchAsync<PhraseBeliefList>(mltList);
      return result;
    }
    
    /// <summary>
    /// Just news up a PhraseBeliefList.
    /// </summary>
    public static PhraseBeliefList NewPhraseBeliefListNewedUpOnly()
    {
      return new PhraseBeliefList();
    }

    /// <summary>
    /// Runs locally.
    /// </summary>
    [RunLocal]
    public static async Task<PhraseBeliefList> NewPhraseBeliefListAsync()
    {
      var result = await DataPortal.CreateAsync<PhraseBeliefList>();
      return result;
    }

    #endregion

    #region Data Portal methods (including child)

#if !SILVERLIGHT
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(MultiLineTextList mltList)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        
        ///SLOW METHOD RIGHT NOW. ITERATE THROUGH ALL PHRASES 
        ///IN LINES OF MLTS, AND GET ALL BELIEFS ABOUT THOSE PHRASES.
        for (int i = 0; i < mltList.Count; i++)
        {
          var mlt = mltList[i];
          for (int j = 0; j < mlt.Lines.Count; j++)
          {
            var phrase = mlt.Lines[j].Phrase;
            Result<ICollection<PhraseBeliefDto>> result = 
              beliefDal.FetchAllRelatedToPhrase(phrase.Id);
            
            //IF ERROR
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

            //NO ERROR, SO ADD THESE BELIEFS TO OUR LIST
            AddDtos(result.Obj);
          }
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(Criteria.PhraseIdCriteria phraseIdCriteria)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();

        var phraseId = phraseIdCriteria.PhraseId;
        Result<ICollection<PhraseBeliefDto>> result = beliefDal.FetchAllRelatedToPhrase(phraseId);
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
        var fetchedPhraseBeliefDtos = result.Obj;
        LoadDtos(fetchedPhraseBeliefDtos);
      }
    }

    private void LoadDtos(ICollection<PhraseBeliefDto> dtos)
    {
      Items.Clear();
      AddDtos(dtos);
    }

    private void AddDtos(ICollection<PhraseBeliefDto> dtos)
    {
      var dtosList = dtos.ToList();
      for (int i = 0; i < dtos.Count; i++)
      {
        var beliefDto = dtosList[i];
        var beliefEdit = DataPortal.FetchChild<PhraseBeliefEdit>(beliefDto);
        this.Add(beliefEdit);
      }
      //foreach (var beliefDto in dtos)
      //{
      //  //var PhraseBeliefEdit = DataPortal.CreateChild<PhraseBeliefEdit>(PhraseBeliefDto);
      //  var beliefEdit = DataPortal.FetchChild<PhraseBeliefEdit>(beliefDto);
      //  this.Add(beliefEdit);
      //}
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();

        Result<ICollection<PhraseBeliefDto>> result = beliefDal.GetAll();
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
        var allPhraseBeliefDtos = result.Obj;
        LoadDtos(allPhraseBeliefDtos);
        var loc1 = Csla.ApplicationContext.ExecutionLocation;
        var loc2 = Csla.ApplicationContext.LogicalExecutionLocation;

        //foreach (var PhraseBeliefDto in allPhraseBeliefDtos)
        //{
        //  //var PhraseBeliefEdit = DataPortal.CreateChild<PhraseBeliefEdit>(PhraseBeliefDto);
        //  var PhraseBeliefEdit = DataPortal.FetchChild<PhraseBeliefEdit>(PhraseBeliefDto);
        //  this.Add(PhraseBeliefEdit);
        //}
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        base.Child_Update();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Child_Fetch(Criteria.PhraseIdCriteria phraseIdCriteria)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();

        var phraseId = phraseIdCriteria.PhraseId;
        Result<ICollection<PhraseBeliefDto>> result = beliefDal.FetchAllRelatedToPhrase(phraseId);
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
        var fetchedPhraseBeliefDtos = result.Obj;
        LoadDtos(fetchedPhraseBeliefDtos);
      }
    }
#endif

    #endregion

    #region AddNewCore

#if SILVERLIGHT
    protected override void AddNewCore()
    {
      AddedNew += PhraseBeliefList_AddedNew; 
      base.AddNewCore();
      AddedNew -= PhraseBeliefList_AddedNew;
    }

    private void PhraseBeliefList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<PhraseBeliefEdit> e)
    {
      //Common.CommonHelper.CheckAuthentication();
      var beliefEdit = e.NewObject;
      beliefEdit.LoadCurrentUser();
      //var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
      //beliefEdit.UserId = identity.UserId;
      //beliefEdit.Username = identity.Name;
    }
#else //SERVER
    protected override PhraseBeliefEdit AddNewCore()
    {
      var beliefEdit = base.AddNewCore();
      beliefEdit.LoadCurrentUser();
      return beliefEdit;
    }
#endif

    #endregion

    protected override void OnChildChanged(Csla.Core.ChildChangedEventArgs e)
    {
      base.OnChildChanged(e);
      //if (e.ChildObject != null)
      //  (Csla.Core.BusinessBase)e.ChildObject.BusinessRules.CheckRules();
    }

  }
}
