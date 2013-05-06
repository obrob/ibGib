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
using LearnLanguages.History;
using System.Threading.Tasks;
using LearnLanguages.Common;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The questions for order include what is the following line to any given line.
  /// So, for any list of lines of count n, the number of questions is n-1.  Add to 
  /// this two specialized questions: 1st and last line.  This gives us n+1 distinct
  /// questions to know.
  /// 
  /// If we ask in order of the lines, then the beginning of the song will get precedence
  /// over the end of the song...if we go back to beginning each time we get one wrong.
  /// We could subdivide the text into groups of lines of size L.  We can keep an in memory
  /// object to keep track of each of these groups.  We study each in order.  If we get it all 
  /// correct, then we just go through the lines of the MLT in order, as if we are just reading
  /// it actively.  If we miss one, then we skip to the next group.  When we return to the previous 
  /// group, we start at the beginning.  Once we have reviewed each of the groups correctly, all
  /// groups are considered unlearned, and we restart the process.
  /// 
  /// A group as such can be considered an MLT itself.  So, our dictionary of studiers will be 
  /// indexed by its order in the global concatenation of MLTs.  We keep two current position 
  /// indexes: CurrentGroupIndex, and CurrentLineInGroupIndex.  We also keep a KnownGroupIndexes 
  /// list.
  /// </summary>
  public class DefaultMultiLineTextsOrderStudier : StudierBase<MultiLineTextList>,
                                                   IHandle<NavigationRequestedEventMessage>, 
                                                   IHandle<History.Events.ReviewedLineOrderEvent>
  {
    #region Ctors and Init

    public DefaultMultiLineTextsOrderStudier()
    {
      Services.EventAggregator.Subscribe(this);//navigation
      HistoryPublisher.Ton.SubscribeToEvents(this);
      LineGroupSize = int.Parse(StudyResources.DefaultOrderStudierLineGroupSize);
      _KnownGroupNumbers = new List<int>();
    }

    #endregion

    #region Methods

    public async override Task<ResultArgs<StudyItemViewModelArgs>> GetNextStudyItemViewModelAsync()
    {
      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();

      LineEdit lineToStudy = null;

      //IF WE KNOW ALL OF OUR GROUPS, RESET AND START OVER
      if (_KnownGroupNumbers.Count == _LineGroups.Count)
      {
        _PrevGroupNumberStudied = -1;
        _PrevStudyWasHit = false;
        _KnownGroupNumbers.Clear();
        foreach (var completedGroup in _LineGroups)
          completedGroup.Reset();
      }

      //GET THE INITIAL GROUP NUMBER TO TRY TO STUDY
      int groupNumberToStudy = _PrevGroupNumberStudied;
      LineGroup group = null;
      if (_PrevStudyWasHit)
      {
        //PREV STUDY WAS HIT, SO TRY TO STAY WITH THE PREV GROUP.
        group = GetGroupFromGroupNumber(_PrevGroupNumberStudied);

        if (group == null)
          throw new Exception();//SINCE PREV STUDY WAS HIT, THIS SHOULD NEVER HAPPEN.

        //IF THE GROUP IS NOT YET COMPLETED, GET ITS NEXT LINE, OTHERWISE WE NEED TO TRY THE NEXT GROUP.
        if (!group.Completed)
        {
          lineToStudy = group.GetLine();
          if (lineToStudy == null)
            throw new Exception();
          _PrevGroupNumberStudied = group.GroupNumber;
        }
        else
        {
          //PREVIOUS GROUP WAS A HIT AND THAT COMPLETED THAT GROUP, SO GET NEXT GROUP
          int nextGroupNumber = GetNextUnknownGroupNumber();
          group = GetGroupFromGroupNumber(nextGroupNumber);
          if (group.Completed)
            throw new Exception();
          lineToStudy = group.GetLine();
          _PrevGroupNumberStudied = group.GroupNumber;
        }
      }
      else
      {
        //PREVIOUS WAS A MISS, SO GET LINE FROM NEXT UNKNOWN GROUP
        int nextGroupNumber = GetNextUnknownGroupNumber();
        group = GetGroupFromGroupNumber(nextGroupNumber);
        if (group.Completed)
          throw new Exception();
        lineToStudy = group.GetLine();
        _PrevGroupNumberStudied = group.GroupNumber;
      }

      //groupNumberToStudy = _PrevGroupNumberStudied;
      //if (groupNumberToStudy == _LineGroups.Count)
      //  groupNumberToStudy = 0;

      ////FIND THE FIRST GROUP NUMBER THAT IS UNKNOWN
      //for (int i = 0; i < _LineGroups.Count; i++)
      //{
      //  if (!_KnownGroupNumbers.Contains(groupNumberToStudy))
      //    break;

      //  //THAT GROUP NUMBER IS ALREADY KNOWN.  UPDATE FOR THE NEXT TRY.
      //  groupNumberToStudy++;
      //  if (groupNumberToStudy == _LineGroups.Count)
      //    groupNumberToStudy = 0;
      //}

      ////WE NOW HAVE THE GROUP NUMBER TO STUDY.  
      //var group = _LineGroups[groupNumberToStudy];
      //var lineToStudy = group.GetLine();

      //WE NOW HAVE THE LINE TO STUDY.  PROCURE A LINE ORDER 
      var result = await StudyItemViewModelFactory.Ton.Procure(lineToStudy, group.MultiLineText);
      var viewModel = result.Object;
      StudyItemViewModelArgs args = new StudyItemViewModelArgs(viewModel);
      var resultArgs = new ResultArgs<StudyItemViewModelArgs>(args);

      var retArgs = await StudyHelper.WrapInTask<ResultArgs<StudyItemViewModelArgs>>(resultArgs);
      return retArgs;
    }

    private int GetNextUnknownGroupNumber()
    {
      //THE NEXT CANDIDATE IS EITHER THE PREV GROUP NUMBER STUDIED + 1, OR THE FIRST GROUP NUMBER.
      //IF PREV GROUP NUMBER STUDIED IS THE LAST IN LINEGROUPS (COUNT-1), THEN NEXT CANDIDATE IS 0.
      int nextCandidateNumber = -1;
      int retNextGroupNumber = -1;
      if (_PrevGroupNumberStudied == (_LineGroups.Count - 1))
        nextCandidateNumber = 0;
      else
        nextCandidateNumber = _PrevGroupNumberStudied + 1;

      for (int i = 0; i < _LineGroups.Count; i++)
      {
        if (_KnownGroupNumbers.Contains(nextCandidateNumber))
        {
          //THIS CANDIDATE IS KNOWN, SO INC UNLESS WE ARE AT LAST GROUP, THEN START BACK AT BEGINNING
          if (nextCandidateNumber == (_LineGroups.Count - 1))
            nextCandidateNumber = 0;
          else
            nextCandidateNumber++;
        }
        else
        {
          //THIS CANDIDATE IS UNKNOWN, SO WE NEED LOOK NO FURTHER
          retNextGroupNumber = nextCandidateNumber;
          break;
        }
      }

      return retNextGroupNumber;
    }

    public async override Task InitializeForNewStudySessionAsync(MultiLineTextList target)
    {
      _AbortIsFlagged = false;
      _Target = target;
      if (_AbortIsFlagged)
        return;
      _PrevGroupNumberStudied = -1;
      _PrevStudyWasHit = false;
      PopulateGroups();
      _KnownGroupNumbers.Clear();
    }

    private void PopulateGroups()
    {
      _LineGroups = new List<LineGroup>();
      var currentGroupIndex = -1;

      foreach (var multiLineText in _Target)
      {
        //get ordered list of lines in MLT
        var orderedList = multiLineText.Lines.OrderBy<LineEdit, int>((l) =>
          {
            return l.LineNumber;
          }).ToList();

        var count = orderedList.Count();
        
        //WE ARE GOING TO CREATE GROUPS WITH GROUPSIZE NUMBER OF LINES PER GROUP.
        LineGroup tmpGroup = null;

        for (int i = 0; i < count; i++)
        {
          var line = orderedList[i];

          if (tmpGroup == null)
          {
            currentGroupIndex++;
            tmpGroup = new LineGroup(currentGroupIndex, multiLineText);
          }
          tmpGroup.Lines.Add(line);

          //IF OUR GROUP HAS REACHED THE GROUP SIZE OR IF WE ARE ON THE LAST ELEMENT IN THE MLT
          if (tmpGroup.Lines.Count == LineGroupSize ||
              i == count - 1)
          {
            //THIS GROUP IS FINISHED.  ADD IT TO OUR _LINEGROUPS, AND RESET OUR TMP VAR
            _LineGroups.Add(tmpGroup);
            tmpGroup = null;
          }
        }
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

    private LineGroup GetGroupFromLineNumber(int lineNumber)
    {
      var results = (from g in _LineGroups
                     where (from line in g.Lines
                            where line.LineNumber == lineNumber
                            select line).Count() == 1
                     select g);
      
      return results.FirstOrDefault();
    }

    private LineGroup GetGroupFromGroupNumber(int groupNumber)
    {
      var results = (from g in _LineGroups
                     where g.GroupNumber == groupNumber
                     select g);

      return results.FirstOrDefault();
    }

    #endregion

    #region Properties

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

    public int LineGroupSize { get; set; }

    private List<LineGroup> _LineGroups { get; set; }
    private List<int> _KnownGroupNumbers { get; set; }

    private int _PrevGroupNumberStudied { get; set; }
    private bool _PrevStudyWasHit { get; set; }

    #endregion

    #region Nested Classes

    private class LineGroup //: IHandle<History.Events.ReviewedLineEvent>
    {
      public LineGroup(int groupNumber, MultiLineTextEdit multiLineText)
      {
        Completed = false;
        MultiLineText = multiLineText;
        Lines = new List<LineEdit>();
        GroupNumber = groupNumber;
        ResetLastStudied();
        History.HistoryPublisher.Ton.SubscribeToEvents(this);
      }

      public int HitCount { get; set; }
      public int MissCount { get; set; }
      public int TotalCount { get { return HitCount + MissCount; } }

      public MultiLineTextEdit MultiLineText { get; set; }
      public List<LineEdit> Lines { get; set; }
      public int GroupNumber { get; set; }
      /// <summary>
      /// Private variable used to keep track of which line this line group should be studying.
      /// </summary>
      private int _LastStudied { get; set; }
      /// <summary>
      /// Tells this LineGroup that whatever line it is on, it was reviewed correctly (known).
      /// </summary>
      public void MarkHit()
      {
        HitCount++;
        _LastStudied++;
        if (_LastStudied >= Lines.Count-1)
        {
          Completed = true;
          //ResetLastStudied();
        }
        CurrentLine = null;
      }
      /// <summary>
      /// Tells this LineGroup that whatever line it is on, it was reviewed incorrectly (unknown).
      /// </summary>
      public void MarkMiss()
      {
        MissCount++;
        ResetLastStudied();
        CurrentLine = null;
      }

      private void ResetLastStudied()
      {
        _LastStudied = -1;
      }

      public LineEdit GetLine()
      {
        if (Completed)
        {
          ResetLastStudied();
          Completed = false;
        }
        var toStudy = _LastStudied + 1;
        if (toStudy >= Lines.Count)
          toStudy = 0;
        var nextLine = Lines[toStudy];
        CurrentLine = nextLine;
        //_LastStudied = toStudy;
        return nextLine;
      }

      private LineEdit CurrentLine { get; set; }

      public bool Completed { get; set; }

      public void Reset()
      {
        CurrentLine = null;
        Completed = false;
        ResetLastStudied();
        HitCount = 0;
        MissCount = 0;
      }

      //public void Handle(History.Events.ReviewedLineOrderEvent message)
      //{
      //  var msgLineText = message.GetDetail<string>(History.HistoryResources.Key_LineText);
      //  if (CurrentLine == null ||
      //      CurrentLine.Phrase == null ||
      //      string.IsNullOrEmpty(CurrentLine.Phrase.Text) ||
      //      msgLineText != CurrentLine.Phrase.Text)
      //    return;

      //  var feedbackAsDouble = message.GetDetail<double>(History.HistoryResources.Key_FeedbackAsDouble);
      //  var feedbackIsCorrect = feedbackAsDouble >= double.Parse(StudyResources.DefaultKnowledgeThreshold);
      //  if (feedbackIsCorrect)
      //    MarkHit();
      //  else
      //    MarkMiss();
      //}
    }

    #endregion

    public void Handle(History.Events.ReviewedLineOrderEvent message)
    {
      //GET GROUP FROM REVIEWED LINE NUMBER
      var msgLineNumber = message.GetDetail<int>(HistoryResources.Key_LineNumber);
      var group = GetGroupFromLineNumber(msgLineNumber);

      //DETERMINE IF FEEDBACK IS A HIT OR MISS
      var msgFeedbackAsDouble = message.GetDetail<double>(HistoryResources.Key_FeedbackAsDouble);
      var isHit = msgFeedbackAsDouble >= double.Parse(StudyResources.DefaultKnowledgeThreshold);

      //MARK GROUP AS HIT OR MISS.
      if (isHit)
      {
        group.MarkHit();
        if (group.Completed)
          _KnownGroupNumbers.Add(group.GroupNumber);
        _PrevStudyWasHit = true;
        //_GroupNumberToStudy = GetGroupNumberToStudy(group, prevStudyWasHit, true);
        //_GroupNumberToStudy = group.GroupNumber;
      }
      else
      {
        group.MarkMiss();
        _PrevStudyWasHit = false;
      }
    }



  }
}
