using System;
using System.Linq;
using LearnLanguages.Business;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using Caliburn.Micro;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Navigation.EventMessages;
using System.Threading.Tasks;
using LearnLanguages.Common;

namespace LearnLanguages.Study
{
  /// <summary>
  /// This MeaningStudier manages a collection of SingleMultiLineTextMeaningStudiers.  Currently, this
  /// cycles through each of the MLTs and calls its Study method.  In the future, this is where we will
  /// adjust priorities to different MLTs, depending on priority of user, and how strongly we want to study 
  /// a certain MLT, due to its projected 'memory trace health' or whatever the paradigm we are using.
  /// </summary>
  public class DefaultMultiLineTextsMeaningStudier : StudierBase<MultiLineTextList>,
                                                     IHandle<NavigationRequestedEventMessage>
  {
    public DefaultMultiLineTextsMeaningStudier()
    {
      _Studiers = new Dictionary<Guid, DefaultSingleMultiLineTextMeaningStudier>();
      Services.EventAggregator.Subscribe(this);//navigation
    }

    private int _CurrentMultiLineTextIndex = -1;

    /// <summary>
    /// Collection of Studiers, indexed by the MLT that they study.
    /// </summary>
    private Dictionary<Guid, DefaultSingleMultiLineTextMeaningStudier> _Studiers { get; set; }

    #region Methods

    public override async Task<ResultArgs<StudyItemViewModelArgs>> GetNextStudyItemViewModelAsync()
    {
      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();

      //WE ARE CYCLING THROUGH ALL OF THE MLTS, SO GET NEXT MLT TO STUDY INDEX
      var multiLineTextIndex = GetNextMultiLineTextIndex();
      var currentMultiLineText = _Target[multiLineTextIndex];
      var studier = _Studiers[currentMultiLineText.Id];

      //WE HAVE ALREADY INITIALIZED EACH STUDIER, SO NOW JUST DELEGATE THE WORK TO THE STUDIER
      return await studier.GetNextStudyItemViewModelAsync();
    }

    private int GetNextMultiLineTextIndex()
    {
      //cycle through the , and get the corresponding studier for this MLT.
      _CurrentMultiLineTextIndex++;
      if (_CurrentMultiLineTextIndex > (_Target.Count - 1))
        _CurrentMultiLineTextIndex = 0;

      return _CurrentMultiLineTextIndex;
    }

    public double GetPercentKnown()
    {
      if (_Studiers.Count == 0)
        return 0.0d;

      var totalLineCount = 0;
      double totalPercentKnownNonNormalized = 0.0d;
      foreach (var studier in _Studiers)
      {
        var multiLineTextId = studier.Key;
        var results = from mlt in _Target
                      where mlt.Id == multiLineTextId
                      select mlt;
        var studierLineCount = results.First().Lines.Count;
        //var studierLineCount = studier.Key.Lines.Count;
        totalLineCount += studierLineCount;

        var studierPercentKnown = studier.Value.GetPercentKnown();
        totalPercentKnownNonNormalized += studierPercentKnown * studierLineCount;
        //eg 50% known 10 lines
        //25% known 1000 lines.  Percent known should be just over 25% known (255/1010 to be exact);
        //totalLineCount = 1010
        //totalPercentKnownNonNormalized = (.5 * 10) + (.25 * 1000) = 255
        //totalPercentKnownNonNormalized = 255 / 1010 = .252475
      }

      var totalPercentKnownNormalized = totalPercentKnownNonNormalized / totalLineCount;

      return totalPercentKnownNormalized;
    }
    
    #endregion

    public override async Task InitializeForNewStudySessionAsync(MultiLineTextList target)
    {
      _AbortIsFlagged = false;

      _Target = target;
      _Studiers.Clear();
      int initializedCount = 0;

      //CREATE STUDIER FOR EACH MULTILINETEXT IN MULTILINETEXTS TARGET
      var newJobMultiLineTextList = _Target;
      //foreach (var multiLineTextEdit in newJobMultiLineTextList)
      for (int i = 0; i < newJobMultiLineTextList.Count; i++)
      {
        if (_AbortIsFlagged)
          return;
        var multiLineTextEdit = newJobMultiLineTextList[i];
        var studier = new DefaultSingleMultiLineTextMeaningStudier();

        #region Thinking (try..)
        var targetId = Guid.NewGuid();
        History.Events.ThinkingAboutTargetEvent.Publish(targetId);
        try
        {
        #endregion
          await studier.InitializeForNewStudySessionAsync(multiLineTextEdit);
        #region (...finally) Thinked
        }
        finally
        {
          History.Events.ThinkedAboutTargetEvent.Publish(targetId);
        }
          #endregion
        if (_AbortIsFlagged)
          return;

        _Studiers.Add(multiLineTextEdit.Id, studier);
        initializedCount++;
        if (initializedCount == newJobMultiLineTextList.Count)
          return;
      }
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
