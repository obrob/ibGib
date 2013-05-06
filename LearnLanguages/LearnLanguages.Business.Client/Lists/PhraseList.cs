using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;
using System.ComponentModel;
using LearnLanguages.Business.Security;
using System.Threading.Tasks;

namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseList : Common.CslaBases.BusinessListBase<PhraseList, PhraseEdit, PhraseDto>
  {
    #region Factory Methods

    /// <summary>
    /// Gets all phrases belonging to the current user async.
    /// </summary>
    public static async Task<PhraseList> GetAllAsync()
    {
      var result = await DataPortal.FetchAsync<PhraseList>();
      return result;
    }

#if !SILVERLIGHT
    public static PhraseList GetAll()
    {
      var result = DataPortal.Fetch<PhraseList>();
      return result;
    }

    public static PhraseList GetAllContainingText(string text)
    {
      var result = DataPortal.Fetch<PhraseList>(text);
      return result;
    }
#endif

    /// <summary>
    /// Gets all phrases that include the given text somewhere as a substring of the Phrase.Text property.
    /// </summary>
    /// <param name="text">Substring of text to search for.</param>
    public static async Task<PhraseList> GetAllContainingTextAsyncCaseInsensitive(string text)
    {
      var result = await DataPortal.FetchAsync<PhraseList>(text);
      return result;
    }



    /// <summary>
    /// Creates a PhraseList, populating it with the phrases for the current user
    /// with the given ids in phraseIds.
    /// These are created as children of the list (normal behavior).
    /// </summary>
    /// <param name="phraseIds">Ids of the phrases for the current user with which to populate the newly created list.</param>
    public static async Task<PhraseList> NewPhraseListAsync(ICollection<Guid> phraseIds)
    {
      var result = await DataPortal.FetchAsync<PhraseList>(phraseIds);
      return result;
    }

    public static async Task<PhraseList> NewPhraseListAsync(Criteria.PhraseTextsCriteria phraseTextsCriteria)
    {
      var result = await DataPortal.CreateAsync<PhraseList>(phraseTextsCriteria);
      return result;
    }

    public static PhraseList NewPhraseListNewedUpOnly()
    {
      return new PhraseList();
    }

    /// <summary>
    /// Runs locally.
    /// </summary>
    [RunLocal]
    public static async Task<PhraseList> NewPhraseListAsync()
    {
      var result = await DataPortal.CreateAsync<PhraseList>();
      return result;
    }

    #endregion

    #region Data Portal methods (including child)

#if !SILVERLIGHT
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Create(Criteria.PhraseTextsCriteria phraseTextsCriteria)
    {
#if DEBUG
      var sep = " ||| ";
      var msg = DateTime.Now.ToShortTimeString() +
                   "PhraseList.DP_Create" + sep +
                   "ThreadID = " +
                   System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + sep +
                   "PhraseTexts.Count = " + phraseTextsCriteria.PhraseTexts.Count.ToString() + sep +
                   "1st Phrase = " + phraseTextsCriteria.PhraseTexts[0];
      System.Diagnostics.Trace.WriteLine(msg);
      //Services.Log(msg, LogPriority.Low, LogCategory.Information);
#endif


      if (phraseTextsCriteria.PhraseTexts.Count == 0)
        throw new ArgumentException("phraseTextsCriteria");
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageText = phraseTextsCriteria.LanguageText;
        var language = DataPortal.FetchChild<LanguageEdit>(languageText);
       
        //WE NOW HAVE OUR LANGUAGEEDIT THAT WILL BE USED FOR ALL PHRASE TEXTS.
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        //PhraseList newPhraseList = PhraseList.NewPhraseList();
        for (int i = 0; i < phraseTextsCriteria.PhraseTexts.Count; i++)
        {
        //foreach (var phraseText in phraseTextsCriteria.PhraseTexts)
          var phraseText = phraseTextsCriteria.PhraseTexts[i];
          if (string.IsNullOrEmpty(phraseText))
            continue;
          PhraseEdit phraseEdit = DataPortal.CreateChild<PhraseEdit>();
          phraseEdit.Language = language;
          phraseEdit.Text = phraseText;
          Add(phraseEdit);
        }
      }
    }

    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(ICollection<Guid> phraseIds)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        Result<ICollection<PhraseDto>> result = phraseDal.Fetch(phraseIds);
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
        var fetchedPhraseDtos = result.Obj;
        foreach (var phraseDto in fetchedPhraseDtos)
        {
          //var PhraseEdit = DataPortal.CreateChild<PhraseEdit>(PhraseDto);
          var phraseEdit = DataPortal.FetchChild<PhraseEdit>(phraseDto);
          this.Add(phraseEdit);
        }
      }
    }

    
    //[EditorBrowsable(EditorBrowsableState.Never)]
    protected void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        Result<ICollection<PhraseDto>> result = phraseDal.GetAll();
        if (!result.IsSuccess || result.IsError)
        {
          if (result.Info != null)
          {
            var ex = result.GetExceptionFromInfo();
            if (ex != null)
              throw new GetAllFailedException(ex.Message);
            else
              throw new GetAllFailedException();
          }
          else
            throw new GetAllFailedException();
        }

        //RESULT WAS SUCCESSFUL
        var allPhraseDtos = result.Obj;
        foreach (var phraseDto in allPhraseDtos)
        {
          //var PhraseEdit = DataPortal.CreateChild<PhraseEdit>(PhraseDto);
          var phraseEdit = DataPortal.FetchChild<PhraseEdit>(phraseDto);
          this.Add(phraseEdit);
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void DataPortal_Fetch(string text)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        Result<ICollection<PhraseDto>> result = phraseDal.Fetch(text);
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
        var phraseDtos = result.Obj;
        foreach (var phraseDto in phraseDtos)
        {
          //var PhraseEdit = DataPortal.CreateChild<PhraseEdit>(PhraseDto);
          var PhraseEdit = DataPortal.FetchChild<PhraseEdit>(phraseDto);
          this.Add(PhraseEdit);
        }
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
    public void Child_Fetch(ICollection<Guid> phraseIds)
    {
      Items.Clear();
      foreach (var id in phraseIds)
      {
        var phraseEdit = DataPortal.FetchChild<PhraseEdit>(id);
        Items.Add(phraseEdit);
      }
    }
#endif

    #endregion

    #region AddNewCore

#if SILVERLIGHT
    protected override void AddNewCore()
    {
      AddedNew += PhraseList_AddedNew; 
      base.AddNewCore();
      AddedNew -= PhraseList_AddedNew;
    }

    private void PhraseList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<PhraseEdit> e)
    {
      //Common.CommonHelper.CheckAuthentication();
      var phraseEdit = e.NewObject;
      phraseEdit.LoadCurrentUser();
      //var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
      //phraseEdit.UserId = identity.UserId;
      //phraseEdit.Username = identity.Name;
    }
#else
    protected override PhraseEdit AddNewCore()
    {
      //SERVER
      var phraseEdit = base.AddNewCore();
      phraseEdit.LoadCurrentUser();
      return phraseEdit;
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
