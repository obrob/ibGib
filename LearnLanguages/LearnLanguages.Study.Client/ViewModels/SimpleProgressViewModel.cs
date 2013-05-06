using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModels;
using LearnLanguages.DataAccess;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.ViewModelBases;
using Caliburn.Micro;
using System.Windows;
using System.Collections.ObjectModel;
using LearnLanguages.Study.Defaults.Simple;
using LearnLanguages.Study;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(SimpleProgressViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class SimpleProgressViewModel : PageViewModelBase// Screen, Common.Interfaces.IPageViewModel
  {
    
    #region Ctors and Init

    public SimpleProgressViewModel()
      : base()
    {
#if DEBUG
      //var sampleData = new ObservableCollection<KeyValuePair<string, int>>()
      //  {
      //    new KeyValuePair<string, int>("Key1", 300),
      //    new KeyValuePair<string, int>("Key2", 400)
      //  };

      //_Data = sampleData;
#endif
      Analyzer = new Analysis.SimplePhraseBeliefAnalyzer();
      //InitializeData();
    }

    public async Task InitializeData()
    {
      #region Thinking (try..)
      var targetId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      BusyContent = StudyResources.BusyContentSimpleProgressPageInitializingData;
      IsBusy = true;
      try
      {
      #endregion
        var analysis = 
          await Analyzer.GetAnalysis<Analysis.EstimatedKnownPhraseCountsAnalysis>(
            Analysis.AnalysisResources.AnalysisIdEstimatedKnownPhraseCounts);
        Data = analysis.Data;
      #region (...finally) Thinked
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(targetId);
        IsBusy = false;
        BusyContent = "";
      }
      #endregion
    }

    public async Task RefineAnalysis(int allottedTimeInMs)
    {
      await Analyzer.RefineAnalysis(allottedTimeInMs);
    }

    #endregion

    #region Properties

    private ObservableCollection<KeyValuePair<string, int>> _Data;
    public ObservableCollection<KeyValuePair<string, int>> Data
    {
      get { return _Data; }
      set
      {
        if (value != _Data)
        {
          _Data = value;
          NotifyOfPropertyChange(() => Data);
        }
      }
    }

    public IAnalyzer Analyzer { get; set; }

    private string _ChartTitle = StudyResources.ChartTitleSimpleProgress;
    public string ChartTitle
    {
      get { return _ChartTitle; }
      set
      {
        if (value != _ChartTitle)
        {
          _ChartTitle = value;
          NotifyOfPropertyChange(() => ChartTitle);
        }
      }
    }

    //private ObservableCollection<SimpleLanguageTextPercentKnownPair> _Data;
    //public ObservableCollection<SimpleLanguageTextPercentKnownPair> Data
    //{
    //  get { return _Data; }
    //  set
    //  {
    //    if (value != _Data)
    //    {
    //      _Data = value;
    //      NotifyOfPropertyChange(() => Data);
    //    }
    //  }
    //}

    //private string _Instructions = StudyResources.InstructionsDefaultStudyPartner;
    //public string Instructions
    //{
    //  get { return _Instructions; }
    //  set
    //  {
    //    if (value != _Instructions)
    //    {
    //      _Instructions = value;
    //      NotifyOfPropertyChange(() => Instructions);
    //    }
    //  }
    //}

    //private string _Title = StudyResources.DefaultProgressTitle;
    //public string Title
    //{
    //  get { return _Title; }
    //  set
    //  {
    //    if (value != _Title)
    //    {
    //      _Title = value;
    //      NotifyOfPropertyChange(() => Title);
    //    }
    //  }
    //}

    //private string _Description = StudyResources.DefaultProgressDescription;
    //public string Description
    //{
    //  get { return _Description; }
    //  set
    //  {
    //    if (value != _Description)
    //    {
    //      _Description = value;
    //      NotifyOfPropertyChange(() => Description);
    //    }
    //  }
    //}

    //private string _ToolTip = StudyResources.DefaultProgressToolTip;
    //public string ToolTip
    //{
    //  get { return _ToolTip; }
    //  set
    //  {
    //    if (value != _ToolTip)
    //    {
    //      _ToolTip = value;
    //      NotifyOfPropertyChange(() => ToolTip);
    //    }
    //  }
    //}
    
    #endregion

    #region Methods

    #region Base Class

    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = StudyResources.InstructionsDefaultStudyPartner;
      Title = StudyResources.TitleSimpleProgressPage;
      Description = StudyResources.DescriptionSimpleProgressPage;
      ToolTip = StudyResources.ToolTipSimpleProgressPage;
    }

    #endregion

    #endregion

    #region Commands

    #endregion

  }
}
