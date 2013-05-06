using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Common;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Navigation.EventMessages;
using Caliburn.Micro;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The studier that actually "does" something.
  /// 
  /// This implementation: "Dumb" studier.  This doesn't know anything about whether 
  /// a phrase's history, it "simply" takes the phrase, produces an offer to show a Q & A about it.
  /// Listens for the ViewModel to publish a Q & A response
  /// </summary>
  public class DefaultPhraseMeaningStudier : StudierBase<PhraseEdit>, 
                                             IStudyReviewMethod,
                                             IHandle<NavigationRequestedEventMessage>

  {
    #region Ctors and Init
    public DefaultPhraseMeaningStudier()
    {
      Services.EventAggregator.Subscribe(this);//navigation
    }
    #endregion

    #region Methods

    public override async Task InitializeForNewStudySessionAsync(PhraseEdit target)
    {
#if DEBUG
      var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
      var thinkingTargetId = target.Id;
      History.Events.ThinkingAboutTargetEvent.Publish(thinkingTargetId);

      _AbortIsFlagged = false;
      _Target = target;

      History.Events.ThinkedAboutTargetEvent.Publish(thinkingTargetId);
    }

    public override async Task<ResultArgs<StudyItemViewModelArgs>> GetNextStudyItemViewModelAsync()
    {
      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();

      //USING THE TARGET PHRASE, 
      //IF IT IS IN THE NATIVE LANGUAGE, THEN IT JUST POPS UP A NATIVE LANGUAGE STUDY QUESTION.
      //IF IT IS IN A DIFFERENT LANGUAGE, THEN IT POPS UP EITHER DIRECTION Q & A, 50% CHANCE.
      #region Thinking
      var targetId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      #endregion
      var retriever = await StudyDataRetriever.CreateNewAsync();
      #region Thinked
      History.Events.ThinkedAboutTargetEvent.Publish(targetId);
      #endregion

      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();

      var nativeLanguageText = retriever.StudyData.NativeLanguageText;
      if (string.IsNullOrEmpty(nativeLanguageText))
        throw new StudyException("No native language set.");

      if (_Target == null)
        throw new StudyException("No PhraseEdit to study, _StudyJobInfo.Target == null.");

      var phraseEdit = _Target;
      var phraseText = phraseEdit.Text;
      if (string.IsNullOrEmpty(phraseText))
        throw new StudyException("Attempted to study empty phrase text, (PhraseEdit)_Target.Text is null or empty.");

      var languageText = phraseEdit.Language.Text;

      //WE HAVE A PHRASEEDIT WITH A LANGUAGE AND WE HAVE OUR NATIVE LANGUAGE, 
      //SO WE HAVE ENOUGH TO PROCURE A VIEW MODEL NOW.
      var result = await StudyItemViewModelFactory.Ton.ProcureAsync(phraseEdit, nativeLanguageText);

      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();

      var studyItemViewModel = result.Object;
      studyItemViewModel.Shown += new EventHandler(studyItemViewModel_Shown);
      var args = new StudyItemViewModelArgs(studyItemViewModel);
      var resultArgs = new ResultArgs<StudyItemViewModelArgs>(args);
      _Phrase = phraseEdit;//hack
      return await StudyHelper.WrapInTask<ResultArgs<StudyItemViewModelArgs>>(resultArgs);
    }

    private void studyItemViewModel_Shown(object sender, EventArgs e)
    {
      if (_AbortIsFlagged)
        return;

      //we are now reviewing a phrase
      var eventReviewingPhrase = new History.Events.ReviewingPhraseEvent(_Phrase, ReviewMethodId);
      History.HistoryPublisher.Ton.PublishEvent(eventReviewingPhrase);
    }

    private PhraseEdit _Phrase { get; set; }

    #endregion

    public Guid ReviewMethodId
    {
      get { return Guid.Parse(StudyResources.ReviewMethodIdDefaultPhraseMeaningStudier); }
    }

    public void Handle(NavigationRequestedEventMessage message)
    {
      AbortStudying();
    }

    private void AbortStudying()
    {
      _AbortIsFlagged = true;
    }

    private object _AbortLock = new object();
    private bool _abortIsFlagged = false;
    private bool _AbortIsFlagged
    {
      get
      {
        lock (_AbortLock)
        {
          return _abortIsFlagged;
        }
      }
      set
      {
        lock (_AbortLock)
        {
          _abortIsFlagged = value;
        }
      }
    }
  }
}
