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
  [Export(typeof(StudySongsViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class StudySongsViewModel : Conductor<StudySongsItemViewModel>.Collection.AllActive,
                                     IHandle<Navigation.EventMessages.NavigatedEventMessage>,
                                     IPageViewModel,
                                     IHaveId,
                                     IHandle<IOffer<MultiLineTextList, IViewModelBase>>, 
                                     IHandle<IStatusUpdate<MultiLineTextList, IViewModelBase>>
  {
    #region Ctors and Init

    public StudySongsViewModel()
    {
      Id = Guid.NewGuid();
      //_AskViewModel = Services.Container.GetExportedValue<AskDoYouKnowThisViewModel>();
      if (_StudyPartner == null)
        Services.Container.SatisfyImportsOnce(this);

      if (_StudyPartner == null)
        throw new Common.Exceptions.PartNotSatisfiedException("StudyPartner");

      PastOpportunities = new List<IOpportunity<MultiLineTextList, IViewModelBase>>();
      CurrentOpportunities = new List<IOpportunity<MultiLineTextList, IViewModelBase>>();
      FutureOpportunities = new List<IOpportunity<MultiLineTextList, IViewModelBase>>();

      Services.EventAggregator.Subscribe(this);
      Exchange.Ton.SubscribeToOffers(this);
      Exchange.Ton.SubscribeToStatusUpdates(this);
      //THE NEXT INIT STUFF HAPPEN AS HANDLE(NAVIGATED MESSAGE)
    }

    private async Task InitializeViewModelAsync()
    {
      //1) GET NATIVE LANGUAGE
      //2) POPULATE SONGS

      ClearSongs();

      //1) GET NATIVE LANGUAGE
      StudyDataRetriever retriever = null;
      #region Thinking (try..)
      var targetId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      try
      {
      #endregion
        retriever = await StudyDataRetriever.CreateNewAsync();
      #region (...finally) Thinked
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(targetId);
      }
      #endregion

      StudyData = retriever.StudyData;

      if (!retriever.StudyDataAlreadyExisted)
      {
        #region Thinking
        var targetId2 = Guid.NewGuid();
        History.Events.ThinkingAboutTargetEvent.Publish(targetId2);
        #endregion
        AskUserForExtraDataAsync(async (s2, r2) =>
        {
          #region Thinked
          History.Events.ThinkedAboutTargetEvent.Publish(targetId2);
          #endregion
          if (r2.Error != null)
            throw r2.Error;

          StudyData = r2.Object;
          //2) POPULATE SONGS
          History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
          {
            await PopulateAllSongsAsync();
          }
          History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
        });
      }
      else
      {
        //2) POPULATE SONGS
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
        {
          await PopulateAllSongsAsync();
        }
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
      }
    }

    private void ClearSongs()
    {
      Items.Clear();
    }

    #endregion

    #region Properties
    public Guid Id { get; private set; }

    private IStudyPartner _StudyPartner;
    [Import]
    public IStudyPartner StudyPartner
    {
      get { return _StudyPartner; }
      set
      {
        if (value != _StudyPartner)
        {
          _StudyPartner = value;
          NotifyOfPropertyChange(() => StudyPartner);
        }
      }
    }

    private StudyDataEdit _StudyData;
    public StudyDataEdit StudyData
    {
      get { return _StudyData; }
      set
      {
        if (value != _StudyData)
        {
          _StudyData = value;
          NotifyOfPropertyChange(() => StudyData);
        }
      }
    }

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

    //private MultiLineTextList _ModelList;
    //public MultiLineTextList ModelList
    //{
    //  get { return _ModelList; }
    //  set
    //  {
    //    if (value != _ModelList)
    //    {
    //      _ModelList = value;
    //      NotifyOfPropertyChange(() => ModelList);
    //    }
    //  }
    //}

    //private MobileList<MultiLineTextDto> _AllMultiLineTextDtos;
    //public MobileList<MultiLineTextDto> AllMultiLineTextDtos
    //{
    //  get { return _AllMultiLineTextDtos; }
    //  set
    //  {
    //    if (value != _AllMultiLineTextDtos)
    //    {
    //      _AllMultiLineTextDtos = value;
    //      NotifyOfPropertyChange(() => AllMultiLineTextDtos);
    //    }
    //  }
    //}

    //private List<MultiLineTextEdit> _SortedModelListCache;
    //public List<MultiLineTextEdit> SortedModelListCache
    //{
    //  get { return _SortedModelListCache; }
    //  set
    //  {
    //    if (value != _SortedModelListCache)
    //    {
    //      _SortedModelListCache = value;
    //      NotifyOfPropertyChange(() => SortedModelListCache);
    //    }
    //  }
    //}

    //private List<MultiLineTextDto> _SortedSongDtosCache;
    //public List<MultiLineTextDto> SortedSongDtosCache
    //{
    //  get { return _SortedSongDtosCache; }
    //  set
    //  {
    //    if (value != _SortedSongDtosCache)
    //    {
    //      _SortedSongDtosCache = value;
    //      NotifyOfPropertyChange(() => SortedSongDtosCache);
    //    }
    //  }
    //}

    private string _InstructionsSelectSong = StudyResources.InstructionsStudySongsSelectSong;
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

    private string _ButtonLabelGo = StudyResources.ButtonLabelStudySongsGo;
    public string ButtonLabelGo
    {
      get { return _ButtonLabelGo; }
      set
      {
        if (value != _ButtonLabelGo)
        {
          _ButtonLabelGo = value;
          NotifyOfPropertyChange(() => ButtonLabelGo);
        }
      }
    }

    public IPage Page { get; set; }

    private string _Title = StudyResources.TitleStudySongsViewModel;
    public string Title
    {
      get { return _Title; }
      set
      {
        if (value != _Title)
        {
          _Title = value;
          NotifyOfPropertyChange(() => Title);
        }
      }
    }

    private string _Description = StudyResources.DescriptionStudyASongViewModel;
    public string Description
    {
      get { return _Description; }
      set
      {
        if (value != _Description)
        {
          _Description = value;
          NotifyOfPropertyChange(() => Description);
        }
      }
    }

    private string _Instructions = StudyResources.InstructionsStudySongsSelectSong;
    public string Instructions
    {
      get { return _Instructions; }
      set
      {
        if (value != _Instructions)
        {
          _Instructions = value;
          NotifyOfPropertyChange(() => Instructions);
        }
      }
    }

    private string _ToolTip = StudyResources.ToolTipStudySongs;
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

    #region Flags

    private bool _GoInProgress;
    public bool GoInProgress
    {
      get { return _GoInProgress; }
      set
      {
        if (value != _GoInProgress)
        {
          _GoInProgress = value;
          NotifyOfPropertyChange(() => GoInProgress);
        }
      }
    }

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
          var songItemViewModel = Services.Container.GetExportedValue<StudySongsItemViewModel>();

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

    private void HookInto(StudySongsItemViewModel viewModel)
    {
      if (viewModel != null)
        viewModel.PropertyChanged += songItemViewModel_PropertyChanged;
    }

    private void songItemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => CanGo);
    }

    private void UnhookFrom(StudySongsItemViewModel viewModel)
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

    public void AskUserForExtraDataAsync(AsyncCallback<StudyDataEdit> callback)
    {
      var askUserViewModel = Services.Container.GetExportedValue<AskUserExtraDataViewModel>();
      askUserViewModel.ShowModal(callback);
    }

    public string GetNativeLanguageText()
    {
      if (StudyData != null)
        return StudyData.NativeLanguageText;
      else
        return "";
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
        Page = message.NavigationInfo.TargetPage;
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

    public bool CanGo
    {
      get
      {
        if (Items == null || Items.Count == 0)
          return false;

        bool anItemIsChecked = (from viewModel in Items
                                where viewModel.IsChecked
                                select viewModel).Count() > 0;

        //WE CAN GO IF AN ITEM IS CHECKED AND WE ARE NOT ALREADY GOING
        return anItemIsChecked &&
               !GoInProgress;
      }
    }

    /// <summary>
    /// This method is called when the user presses the Go button.
    /// </summary>
    public async Task Go()
    {
      if (!CanGo)
        return;

      DisableNavigationRequestedEventMessage.Publish();
      GoInProgress = true;
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
        MultiLineTextList songs = null;
        #region Try ...(thinking)
        //var targetId = new Guid(@"5D4355FE-C46E-4AA1-9E4A-45288C341C44");
        var targetId = Guid.NewGuid();
        History.Events.ThinkingAboutTargetEvent.Publish(targetId);
        try
        {
        #endregion
          songs = await MultiLineTextList.NewMultiLineTextListAsync(ids);
        #region ...Finally (thinked)
        }

        finally
        {
          History.Events.ThinkedAboutTargetEvent.Publish(targetId);
        }
        #endregion

        var nativeLanguage = await LanguageEdit.GetLanguageEditAsync(GetNativeLanguageText());
        var noExpirationDate = StudyJobInfo<MultiLineTextList, IViewModelBase>.NoExpirationDate;
        var precision = double.Parse(StudyResources.DefaultExpectedPrecision);

        //CREATE JOB INFO 
        var studyJobInfo = new StudyJobInfo<MultiLineTextList, IViewModelBase>(songs,
                                                               nativeLanguage,
                                                               noExpirationDate,
                                                               precision);
        //CREATE OPPORTUNITY
        var opportunity = new Opportunity<MultiLineTextList, IViewModelBase>(Id,
                                                             this,
                                                             studyJobInfo,
                                                             StudyResources.CategoryStudy);

        //ADD OPPORTUNITY TO OUR FUTURE OPPORTUNITIES
        FutureOpportunities.Add(opportunity);

        //LET THE HISTORY SHOW THAT YOU ARE THINKING ABOUT THIS OPPORTUNITY
        var opportunityId = opportunity.Id;
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);

        //PUBLISH THE OPPORTUNITY
        Exchange.Ton.Publish(opportunity);

        //NOW, WE WAIT UNTIL WE HEAR A HANDLE(OFFER) MESSAGE.
        //TODO: TIMEOUT FOR OPPORTUNITY, BOTH EXPIRATION DATE AND WAITING FOR OFFER TIMEOUT

      }
      finally
      {
        EnableNavigationRequestedEventMessage.Publish();
      }
    }

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

    /// <summary>
    /// These opportunities have been either completed or canceled.
    /// </summary>
    private List<IOpportunity<MultiLineTextList, IViewModelBase>> PastOpportunities { get; set; }
    /// <summary>
    /// These opportunities have had offers offered and accepted, and as far 
    /// as we know they are still in progress.  They may be complete but no complete
    /// status update has occured.
    /// </summary>
    private List<IOpportunity<MultiLineTextList, IViewModelBase>> CurrentOpportunities { get; set; }
    /// <summary>
    /// These opportunities have been published but still need to either be implemented
    /// or canceled.  At first, I assume this will be only one or zero entries.
    /// </summary>
    private List<IOpportunity<MultiLineTextList, IViewModelBase>> FutureOpportunities { get; set; }

    public void Handle(IOffer<MultiLineTextList, IViewModelBase> message)
    {
      History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);
      DisableNavigationRequestedEventMessage.Publish();
      try
      {
        //WE ONLY CARE ABOUT OFFERS IN THE STUDY CATEGORY
        if (message.Category != StudyResources.CategoryStudy)
          return;
        //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
        if (message.PublisherId == Id)
          return;

        //WE ONLY CARE ABOUT OFFERS PERTAINING TO OUR FUTURE AND PAST OPPORTUNITIES
        var resultsFuture = (from opportunity in FutureOpportunities
                             where opportunity.Id == message.Opportunity.Id
                             select opportunity);
        if (resultsFuture.Count() != 1)
        {
          //NO OPEN FUTURE OPPORTUNITIES, NOW CHECK OUR PAST
          var resultsOther = (from opportunity in PastOpportunities
                              where opportunity.Id == message.Opportunity.Id
                              select opportunity).Concat
                                (from opportunity in CurrentOpportunities
                                 where opportunity.Id == message.Opportunity.Id
                                 select opportunity);

          if (resultsOther.Count() >= 1)
          {
            //WE HAVE THIS IN THE PAST OR CURRENT, SO THIS OPPORTUNITY HAS ALREADY BEEN/IS ALREADY BEING 
            //HANDLED, SO WE WILL DENY THIS OFFER
            var denyResponse = OfferResources.OfferResponseDeny;
            var denyOfferResponse = new OfferResponse<MultiLineTextList, IViewModelBase>(message, Id, this, denyResponse,
              StudyResources.CategoryStudy, null);

            //PUBLISH DENY OFFER RESPONSE
            Exchange.Ton.Publish(denyOfferResponse);
            return;
          }
          else
            //NOT IN FUTURE OR PAST, SO DOESN'T PERTAIN TO US
            return;
        }

        //THIS PERTAINS TO A FUTURE OPPORTUNITY, SO WE WILL EXAMINE THIS FURTHER
        //FOR NOW, THIS MEANS THAT WHATEVER THE OFFER IS, WE WILL ACCEPT IT.
        //IN THE POSSIBLY NEAR FUTURE, WE WILL HAVE TO CONSIDER IF SOMEONE ELSE
        //HAS SOMETHING MORE IMPORTANT TO DO
        var pertinentOpportunity = resultsFuture.First();

        var acceptResponse = OfferResources.OfferResponseAccept;
        var acceptOfferResponse = new OfferResponse<MultiLineTextList, IViewModelBase>(message, Id, this, acceptResponse,
          StudyResources.CategoryStudy, null);

        //BEFORE PUBLISHING, MOVE OPPORTUNITY TO CURRENT OPPORTUNITIES.
        FutureOpportunities.Remove(pertinentOpportunity);
        CurrentOpportunities.Add(pertinentOpportunity);

        //PUBLISH ACCEPT OFFER RESPONSE
        Exchange.Ton.Publish(acceptOfferResponse);
        GoInProgress = false;
      }
      finally
      {
        EnableNavigationRequestedEventMessage.Publish();
      }
    }

    /// <summary>
    /// If completed event, this shows the StudyViewModel populated with the produced IViewModelBase
    /// produced by the study partner.
    /// </summary>
    /// <param name="message"></param>
    public void Handle(IStatusUpdate<MultiLineTextList, IViewModelBase> message)
    {
      //WE ONLY CARE ABOUT STUDY UPDATES
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      if (message.PublisherId == Id)
        return;

      //WE ONLY CARE ABOUT UPDATES TO OUR CURRENT OPPORTUNITIES
      if (message.Opportunity == null || !CurrentOpportunities.Contains(message.Opportunity))
        return;

      //THIS IS ONE OF THIS OBJECT'S UPDATES

      //IF THIS IS A COMPLETED STATUS UPDATE, THEN PRODUCT SHOULD BE SET.  SO, BUBBLE THIS ASPECT UP.
      if (message.Status == CommonResources.StatusCompleted)
      {
        if (message.JobInfo.Product == null)
          throw new StudyException("StatusCompleted posted but JobInfo.Product == null...Where is the product?");

        //WE HAVE A PRODUCT OF TYPE IVIEWMODELBASE THAT HAS BEEN CREATED FOR US VIA 
        //THE EXCHANGE!
        var productViewModel = message.JobInfo.Product;

        //MOVE THE OPPORTUNITY TO PAST OPPORTUNITIES
        CurrentOpportunities.Remove(message.Opportunity);
        PastOpportunities.Add(message.Opportunity);

        //LET HISTORY KNOW WE ARE DONE THINKING OF THIS OPPORTUNITY
        var targetId = message.Opportunity.Id;
        History.Events.ThinkingAboutTargetEvent.Publish(System.Guid.Empty);

        ////GET THE STUDY VIEWMODEL THAT WILL HOUSE OUR CREATED IVIEWMODELBASE
        var studyViewModel = Services.Container.GetExportedValue<ExecuteStudySongsViewModel>();
        studyViewModel.StudyScreen = productViewModel;

        //NAVIGATE TO THE EXECUTE STUDY SONGS PAGE TO BEGIN STUDYING
        var executeStudySongsPage = 
          Services.Container.GetExportedValue<ExecuteStudySongsPage>();
        Navigation.Navigator.Ton.NavigateTo(executeStudySongsPage);
      }
    }
    
  }
}
