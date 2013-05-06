//using System;
//using System.Linq;
//using System.ComponentModel.Composition;
//using LearnLanguages.Business;
//using LearnLanguages.DataAccess;
//using LearnLanguages.Common.ViewModelBases;
//using System.Windows;
//using System.Collections.Generic;
//using System.IO;
//using System.Text.RegularExpressions;

//namespace LearnLanguages.Silverlight.ViewModels
//{
//  [Export(typeof(IWantToLearnASongPhraseEditViewModel))]
//  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
//  public class IWantToLearnASongPhraseEditViewModel : ViewModelBase<PhraseEdit, PhraseDto>
//  {
//    #region Ctors and Init

//    public IWantToLearnASongPhraseEditViewModel()
//    {
//      Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
//      Languages.SelectedItemChanged += HandleLanguageChanged;
//      if (!bool.Parse(ViewViewModelResources.ShowInstructions))
//        InstructionsVisibility = Visibility.Collapsed;
//      else
//        InstructionsVisibility = Visibility.Visible;

//      Saved += HandlePhraseSaved;
//    }

//    #endregion

//    #region Properties

//    private LanguageSelectorViewModel _Languages;
//    public LanguageSelectorViewModel Languages
//    {
//      get { return _Languages; }
//      set
//      {
//        if (value != _Languages)
//        {
//          _Languages = value;
//          NotifyOfPropertyChange(() => Languages);
//        }
//      }
//    }

//    public string LabelPhraseText { get { return ViewViewModelResources.LabelIWantToLearnASongPhraseText; } }
//    public string LabelLanguageText { get { return ViewViewModelResources.LabelLanguageTextIWantToLearnASongView; } }

//    public string InstructionsSelectLanguage { get { return ViewViewModelResources.InstructionsSelectLanguageIWantToLearnASong; } }
//    public string InstructionsEnterPhrase { get { return ViewViewModelResources.InstructionsEnterPhraseIWantToLearnASong; } }
//    public int MaxInstructionsWidth { get { return int.Parse(ViewViewModelResources.MaxInstructionsWidth); } }

//    private Visibility _InstructionsVisibility;
//    public Visibility InstructionsVisibility
//    {
//      get { return _InstructionsVisibility; }
//      set
//      {
//        if (value != _InstructionsVisibility)
//        {
//          _InstructionsVisibility = value;
//          NotifyOfPropertyChange(() => InstructionsVisibility);
//        }
//      }
//    }

//    #endregion

//    #region Methods

//    private void HandlePhraseSaved(object sender, Common.EventArgs.ModelEventArgs<PhraseEdit> e)
//    {
//      ///when we save the phrase, we want to...
//      ///parse each line into a subphrase, parse each subphrase into a word-subphrase
//      ///if subphrase is unique, save that subphrase
//      ///e.g. 
//      ///  this is line one
//      ///  this is line two
//      ///  this is line three
//      ///  subphrases: "this is line one" "this is line two" "this is line three"
//      ///  word-subphrases: "this" "is" "line" "one" "two" "three"

//      //PARSE PHRASE INTO WORDS
//      var parentPhrase = e.Model;
//      var parentPhraseText = parentPhrase.Text;

//      var lineDelimiter = ViewViewModelResources.LineDelimiter;
//      lineDelimiter = lineDelimiter.Replace("\\r", "\r");
//      lineDelimiter = lineDelimiter.Replace("\\n", "\n");
//      var lines = new List<string>(parentPhraseText.Split(new string[] { lineDelimiter }, StringSplitOptions.RemoveEmptyEntries));

//      var splitIntoWordsPattern = ViewViewModelResources.RegExSplitPatternWords;
//      splitIntoWordsPattern = splitIntoWordsPattern.Replace(@"\\", @"\");
//      var words = new List<string>(Regex.Split(parentPhraseText, splitIntoWordsPattern));

//      var allSubPhrases = new List<string>(lines);
//      allSubPhrases.AddRange(words);

//      //REMOVE DUPLICATES
//      allSubPhrases = allSubPhrases.Distinct().ToList();

//      //CREATE PHRASE FOR EACH LINE AND WORD
//      var languageText = parentPhrase.Language.Text;
//      PhraseList.NewPhraseList(new Business.Criteria.PhraseTextsCriteria(languageText, allSubPhrases), (s, r) =>
//        {
//          if (r.Error != null)
//            throw r.Error;

//          var allSubPhrasesPhraseList = r.Object;
//          allSubPhrasesPhraseList.BeginSave((s2, r2) =>
//            {
//              if (r2.Error != null)
//                throw r2.Error;

//              allSubPhrasesPhraseList = (PhraseList)r2.NewObject;
//            });
//        });

//    }
//    private void HandleLanguageChanged(object sender, EventArgs e)
//    {
//      if (sender != null)
//        Model.Language = ((LanguageEditViewModel)sender).Model;
//    }

//    #endregion
//  }
//}
