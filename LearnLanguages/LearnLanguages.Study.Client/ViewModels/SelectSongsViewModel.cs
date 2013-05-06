using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;

using Csla.Core;
using Caliburn.Micro;

using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common.EventMessages;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Study;
using LearnLanguages.Study.Pages;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Study.ViewModels
{
  /// <summary>
  /// When studying a song, we need 3 things:
  /// 1) User's Native Language
  /// 2) A song to study.
  /// 3) A StudyPartner to do the studying.
  /// 
  /// This view model checks for #1, and shows a modal dialog if it can't find the native language.
  /// This view model's main function is #2, select the song to study.  Once it has that, it will pass 
  /// control over to the StudyPartner who is then free to navigate however it likes.
  /// </summary>
  [Export(typeof(SelectSongsViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class SelectSongsViewModel : Conductor<SelectSongsItemViewModel>.Collection.AllActive,
                                     IHandle<Navigation.EventMessages.NavigatedEventMessage>,
                                     IViewModelBase,
                                     IHaveId
  {
    #region Ctors and Init

    public SelectSongsViewModel()
    {
      Id = Guid.NewGuid();

      //Services.EventAggregator.Subscribe(this);
    }

    private async Task InitializeViewModelAsync()
    {
      ClearSongs();
      History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
      {
        await PopulateAllSongsAsync();
      }
      History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
    }

    private void ClearSongs()
    {
      Items.Clear();
    }

    #endregion

    #region Properties
    public Guid Id { get; private set; }

    private Dictionary<Guid, string> _MultiLineTextIdsAndTitles { get; set; }
    private List<string> _SortedSongTitlesCache;
    public List<string> SortedSongTitlesCache
    {
      get { return _SortedSongTitlesCache; }
      set
      {
        if (value != _SortedSongTitlesCache)
        {
          _SortedSongTitlesCache = value;
          NotifyOfPropertyChange(() => SortedSongTitlesCache);
        }
      }
    }

    private string _InstructionsSelectSong = StudyResources.InstructionsSelectSongsSelectSong;
    public string InstructionsSelectSong
    {
      get { return _InstructionsSelectSong; }
      set
      {
        if (value != _InstructionsSelectSong)
        {
          _InstructionsSelectSong = value;
          NotifyOfPropertyChange(() => InstructionsSelectSong);
        }
      }
    }

    
    #region Flags

    private bool _CheckAllToggleIsChecked;
    public bool CheckAllToggleIsChecked
    {
      get { return _CheckAllToggleIsChecked; }
      set
      {
        if (value != _CheckAllToggleIsChecked)
        {
          _CheckAllToggleIsChecked = value;
          NotifyOfPropertyChange(() => CheckAllToggleIsChecked);
          ToggleAllChecks();
        }
      }
    }

    private bool _AbortIsFlagged;
    public bool AbortIsFlagged
    {
      get { return _AbortIsFlagged; }
      set
      {
        if (value != _AbortIsFlagged)
        {
          _AbortIsFlagged = value;
          NotifyOfPropertyChange(() => AbortIsFlagged);
          NotifyOfPropertyChange(() => CanStopPopulating);
        }
      }
    }

    private bool _IsPopulating;
    public bool IsPopulating
    {
      get { return _IsPopulating; }
      set
      {
        if (value != _IsPopulating)
        {
          _IsPopulating = value;
          NotifyOfPropertyChange(() => IsPopulating);
          NotifyOfPropertyChange(() => CanStopPopulating);
          NotifyOfPropertyChange(() => CanApplyFilter);
          if (_IsPopulating)
            ProgressVisibility = Visibility.Visible;
          else
            ProgressVisibility = Visibility.Collapsed;
        }
      }
    }

    #endregion

    #region Progress

    private Visibility _ProgressVisibility;
    public Visibility ProgressVisibility
    {
      get { return _ProgressVisibility; }
      set
      {
        if (value != _ProgressVisibility)
        {
          _ProgressVisibility = value;
          NotifyOfPropertyChange(() => ProgressVisibility);
        }
      }
    }

    private double _ProgressValue;
    public double ProgressValue
    {
      get { return _ProgressValue; }
      set
      {
        //if (value != _ProgressValue)
        //{
        _ProgressValue = value;
        NotifyOfPropertyChange(() => ProgressValue);
        //}
      }
    }

    private double _ProgressMaximum;
    public double ProgressMaximum
    {
      get { return _ProgressMaximum; }
      set
      {
        //if (value != _ProgressMaximum)
        //{
        _ProgressMaximum = value;
        NotifyOfPropertyChange(() => ProgressMaximum);
        //}
      }
    }

    #endregion

    #region Filter
    private string _FilterLabel = StudyResources.LabelStudySongsFilter;
    public string FilterLabel
    {
      get { return _FilterLabel; }
      set
      {
        if (value != _FilterLabel)
        {
          _FilterLabel = value;
          NotifyOfPropertyChange(() => FilterLabel);
        }
      }
    }

    private string _FilterText = "";
    public string FilterText
    {
      get { return _FilterText; }
      set
      {
        if (value != _FilterText)
        {
          _FilterText = value;
          //PopulateViewModels(ModelList);
          NotifyOfPropertyChange(() => FilterText);
        }
      }
    }

    private string _LabelTextApplyFilterButton = StudyResources.ButtonLabelApplyFilter;
    public string LabelTextApplyFilterButton
    {
      get { return _LabelTextApplyFilterButton; }
      set
      {
        if (value != _LabelTextApplyFilterButton)
        {
          _LabelTextApplyFilterButton = value;
          NotifyOfPropertyChange(() => LabelTextApplyFilterButton);
        }
      }
    }
    #endregion

    #region Base
    public bool ShowGridLines
    {
      get { return bool.Parse(StudyResources.ShowGridLines); }
    }

    private Visibility _ViewModelVisibility;
    public Visibility ViewModelVisibility
    {
      get { return _ViewModelVisibility; }
      set
      {
        if (value != _ViewModelVisibility)
        {
          _ViewModelVisibility = value;
          NotifyOfPropertyChange(() => ViewModelVisibility);
        }
      }
    }

    private string _ToolTip = StudyResources.ToolTipSelectSongs;
    public string ToolTip
    {
      get { return _ToolTip; }
      set
      {
        if (value != _ToolTip)
        {
          _ToolTip = value;
          NotifyOfPropertyChange(() => ToolTip);
        }
      }
    }

    private bool _IsEnabled = true;
    public bool IsEnabled
    {
      get { return _IsEnabled; }
      set
      {
        if (value != _IsEnabled)
        {
          _IsEnabled = value;
          NotifyOfPropertyChange(() => IsEnabled);
        }
      }
    }

    private bool _IsBusy;
    public bool IsBusy
    {
      get { return _IsBusy; }
      set
      {
        if (value != _IsBusy)
        {
          _IsBusy = value;
          NotifyOfPropertyChange(() => IsBusy);
        }
      }
    }

    private string _BusyContent;
    public string BusyContent
    {
      get { return _BusyContent; }
      set
      {
        if (value != _BusyContent)
        {
          _BusyContent = value;
          NotifyOfPropertyChange(() => BusyContent);
        }
      }
    }


    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// Gets all songs in DB for this user, and calls PopulateItems.
    /// </summary>
    private async Task PopulateAllSongsAsync()
    {
      MultiLineTextDtosOnlyRetriever retriever = null;
      #region Thinking (try...)
      var targetId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      try
      {
      #endregion
        //var allMultiLineTexts = await MultiLineTextList.GetAllAsync();
        retriever = await MultiLineTextDtosOnlyRetriever.CreateNewAsync();
      //ModelList = allMultiLineTexts;
        //AllMultiLineTextDtos.AddRange(retriever.MultiLineTextDtos);
      }
      #region (...finally) Thinked
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(targetId);
      }
      #endregion

      _MultiLineTextIdsAndTitles = retriever.GetIdsAndTitles();
      /// For now, I'm not going to differentiate song from non-song.
      List<string> songTitles = new List<string>();
      foreach (var entry in _MultiLineTextIdsAndTitles)
      {
        var title = entry.Value;
        songTitles.Add(title);
      }

      songTitles.Sort();

      #region Sort MLT by Title Comparison (Comparison<MultiLineTextEdit> comparison = (a, b) =>)

      //      Comparison<MultiLineTextEdit> comparison = (a, b) =>
      Comparison<MultiLineTextDto> comparison = (a, b) =>
      {
        //WE'RE GOING TO TEST CHAR ORDER IN ALPHABET
        string aTitle = a.Title.ToLower();
        string bTitle = b.Title.ToLower();

        //IF THEY'RE THE SAME TITLES IN LOWER CASE, THEN THEY ARE EQUAL
        if (aTitle == bTitle)
          return 0;

        //ONLY NEED TO TEST CHARACTERS UP TO LENGTH
        int shorterTitleLength = aTitle.Length;
        bool aIsShorter = true;
        if (bTitle.Length < shorterTitleLength)
        {
          shorterTitleLength = bTitle.Length;
          aIsShorter = false;
        }

        int result = 0; //assume a and b are equal (though we know they aren't if we've reached this point)

        for (int i = 0; i < shorterTitleLength; i++)
        {
          if (aTitle[i] < bTitle[i])
          {
            result = -1;
            break;
          }
          else if (aTitle[i] > bTitle[i])
          {
            result = 1;
            break;
          }
        }

        //IF THEY ARE STILL EQUAL, THEN THE SHORTER PRECEDES THE LONGER
        if (result == 0)
        {
          if (aIsShorter)
            result = -1;
          else
            result = 1;
        }

        return result;
      };

      #endregion
      
      //List<MultiLineTextEdit> songs = null;

      //CACHE ALL SONGS SO WE WON'T HAVE TO DOWNLOAD THEM AGAIN.  THIS IS ASSUMING MY CURRENT NAVIGATION
      //STRUCTURE WHICH MEANS THAT YOU CAN'T ADD SONGS WITHOUT RECREATING THIS ENTIRE VIEWMODEL.
      //THIS CACHE WILL BE USED FOR FILTERING.
      _SortedSongTitlesCache = songTitles;

      PopulateItems();
    }

    //todo: StudyASongViewModel uses background worker. Can convert this to using async keyword async method instead.
    private void PopulateItems()
    {
      if (IsPopulating && AbortIsFlagged)
        return;

      BackgroundWorker worker = new BackgroundWorker();
      #region worker.DoWork += ((s, r) =>
      worker.DoWork += ((s, r) =>
      {
        //IsBusy = true;
        IsPopulating = true;

        
        if (Items.Count > 0)
          foreach (var viewModel in Items)
          {
            UnhookFrom(viewModel);
          }
        Items.Clear();
        #region Sort MLT by Title Comparison (Comparison<MultiLineTextEdit> comparison = (a, b) =>)

        //Comparison<MultiLineTextEdit> comparison = (a, b) =>
        Comparison<MultiLineTextDto> comparison = (a, b) =>
        {
          //WE'RE GOING TO TEST CHAR ORDER IN ALPHABET
          string aTitle = a.Title.ToLower();
          string bTitle = b.Title.ToLower();

          //IF THEY'RE THE SAME TITLES IN LOWER CASE, THEN THEY ARE EQUAL
          if (aTitle == bTitle)
            return 0;

          //ONLY NEED TO TEST CHARACTERS UP TO LENGTH
          int shorterTitleLength = aTitle.Length;
          bool aIsShorter = true;
          if (bTitle.Length < shorterTitleLength)
          {
            shorterTitleLength = bTitle.Length;
            aIsShorter = false;
          }

          int result = 0; //assume a and b are equal (though we know they aren't if we've reached this point)

          for (int i = 0; i < shorterTitleLength; i++)
          {
            if (aTitle[i] < bTitle[i])
            {
              result = -1;
              break;
            }
            else if (aTitle[i] > bTitle[i])
            {
              result = 1;
              break;
            }
          }

          //IF THEY ARE STILL EQUAL, THEN THE SHORTER PRECEDES THE LONGER
          if (result == 0)
          {
            if (aIsShorter)
              result = -1;
            else
              result = 1;
          }

          return result;
        };

        #endregion

        List<string> filteredSongs = FilterSongsByTitle(SortedSongTitlesCache);
        filteredSongs.Sort();

        int counter = 0;
        int iterationsBetweenComeUpForAir = 20;
        int totalCount = filteredSongs.Count();
        ProgressMaximum = totalCount;

        for (int i = 0; i < filteredSongs.Count; i++)
        {
          History.Events.ThinkedAboutTargetEvent.Publish();
          //CREATE THE VIEWMODEL
          var songItemViewModel = Services.Container.GetExportedValue<SelectSongsItemViewModel>();

          //ASSIGN THE VIEWMODEL'S MODEL AS THE SONG
          var songTitle = SortedSongTitlesCache[i];
          songItemViewModel.SongTitle = songTitle;
          HookInto(songItemViewModel);

          //INSERT THE VIEWMODEL IN THE PROPER ORDER INTO OUR ITEMS COLLECTION
          Items.Insert(i, songItemViewModel);

          //UPDATE/DO COME UP FOR AIR
          counter++;
          ProgressValue = counter;
          if (counter % iterationsBetweenComeUpForAir == 0)
          {
            if (AbortIsFlagged)
            {
              AbortIsFlagged = false;
              ProgressValue = 0;

              break;
            }
          }

        }

        IsPopulating = false;
      });
      #endregion

      worker.RunWorkerAsync();
    }

    private void HookInto(SelectSongsItemViewModel viewModel)
    {
      if (viewModel != null)
        viewModel.PropertyChanged += songItemViewModel_PropertyChanged;
    }

    private void songItemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => CanDeleteSelected);
    }

    private void UnhookFrom(SelectSongsItemViewModel viewModel)
    {
      if (viewModel != null)
        viewModel.PropertyChanged -= songItemViewModel_PropertyChanged;
    }

    /// <summary>
    /// Filters songs using FilterText property.  Does NOT sort results.
    /// </summary>
    /// <param name="songTitles">song list to apply filter to</param>
    /// <returns>filtered song list (of type MultiLineTextEdit)</returns>
    private List<string> FilterSongsByTitle(List<string> songTitles)
    {
      if (string.IsNullOrEmpty(FilterText))
        return songTitles;
          
      var _filterText = FilterText;

      var filteredSongTitleList = (from songTitle in songTitles
                                   where songTitle.ToLower().Contains(FilterText.ToLower())
                                   select songTitle).ToList();

      return filteredSongTitleList;
    }

    private void ToggleAllChecks()
    {
      foreach (var viewModel in Items)
      {
        viewModel.IsChecked = CheckAllToggleIsChecked;
      }
    }

    /// <summary>
    /// Initializes ViewModel after navigated.
    /// </summary>
    /// <param name="message"></param>
    public void Handle(Navigation.EventMessages.NavigatedEventMessage message)
    {
      //WE ARE LISTENING FOR A MESSAGE THAT SAYS WE WERE SUCCESSFULLY NAVIGATED TO.
      //SO WE ONLY CARE ABOUT IT IF WE ARE THE TARGETPAGE CONTENT VIEWMODEL
      if (message.NavigationInfo.TargetPage.ContentViewModel != this)
        return;

      //WE HAVE BEEN SUCCESSFULLY NAVIGATED TO.

      var targetId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      DisableNavigationRequestedEventMessage.Publish();
      try
      {
        var suppress = InitializeViewModelAsync();
      }
      finally
      {
        EnableNavigationRequestedEventMessage.Publish();
        History.Events.ThinkedAboutTargetEvent.Publish(targetId);
      }
    }

    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public void OnImportsSatisfied()
    {
      //
    }

    #endregion

    #region Commands

    public bool CanDeleteSelected
    {
      get
      {
        if (Items == null || Items.Count == 0)
          return false;

        bool anItemIsChecked = (from viewModel in Items
                                where viewModel.IsChecked
                                select viewModel).Count() > 0;

        //WE CAN GO IF AN ITEM IS CHECKED AND WE ARE NOT ALREADY GOING
        return anItemIsChecked;
      }
    }

    /// <summary>
    /// This method is called when the user presses the Go button.
    /// </summary>
    public async Task DeleteSelected()
    {
      if (!CanDeleteSelected)
        return;

      Business.DeleteMultiLineTextsReadOnly deleter = null;

      var description = "SelectSongsViewModel.DeleteSelected";
      IncrementApplicationBusyEventMessage.Publish(description);
      try
      {
        var ids = new MobileList<Guid>();
        foreach (var songViewModel in Items)
        {
          if (songViewModel.IsChecked)
          {
            Guid id = default(Guid);
            var results = from entry in _MultiLineTextIdsAndTitles
                          where entry.Value == songViewModel.SongTitle
                          select entry;
            id = results.First().Key;
            ids.Add(id);
          }
        }

        var thinkId = new Guid(@"70CDC58A-40CD-46C6-A496-BCF23D77E8C2");
        try
        {
          deleter = await Business.DeleteMultiLineTextsReadOnly.CreateNewAsync(ids);
          
        }
        finally
        {
          History.Events.ThinkedAboutTargetEvent.Publish(thinkId);
        }       
      }
      finally
      {
        DecrementApplicationBusyEventMessage.Publish(description);
      }

      History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
      if (!deleter.WasSuccessful)
      {
        MessageBox.Show(StudyResources.ErrorMsgSomeSongsWereNotDeletedSimple);
        Services.PublishMessageEvent(string.Format(StudyResources.ErrorMsgSomeSongsWereNotDeletedDetailed, deleter.UnsuccessfulIds),
                                     Common.MessagePriority.High,
                                     Common.MessageType.Error);
      }
      else
        MessageBox.Show(StudyResources.MsgSongsSuccessfullyDeleted);
    }//left off here: front end done. need to create stored procedure and import it into the edmx. then fill it in in the dal.

    public bool CanStopPopulating
    {
      get
      {
        //if abort is already flagged, then we're already trying to stop populating.
        return (IsPopulating && !AbortIsFlagged);
      }
    }
    public void StopPopulating()
    {
      AbortIsFlagged = true;
    }

    public bool CanApplyFilter
    {
      get
      {
        return (!IsPopulating &&
                !AbortIsFlagged &&
                 SortedSongTitlesCache != null &&
                 SortedSongTitlesCache.Count > 0);
      }
    }
    public void ApplyFilter()
    {
      PopulateItems();
      //PopulateItems(AllSongsCache);
    }

    #endregion
    
  }
}
