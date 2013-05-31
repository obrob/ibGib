using System;
using System.Linq;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using LearnLanguages.Common;
using LearnLanguages.Common.ViewModelBases;
using System.Windows;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LearnLanguages.Silverlight.Common.ViewModels;
using LearnLanguages.Silverlight.Common;
using LearnLanguages.Common.EventMessages;
using Csla.Rules;


namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddSongMultiLineTextEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddSongMultiLineTextEditViewModel : ViewModelBase<MultiLineTextEdit, MultiLineTextDto>
  {
    #region Ctors and Init

    public AddSongMultiLineTextEditViewModel()
    {
      Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
      Languages.SelectedItemChanged += HandleLanguageChanged;
      if (!bool.Parse(ViewViewModelResources.ShowInstructions))
        InstructionsVisibility = Visibility.Collapsed;
      else
        InstructionsVisibility = Visibility.Visible;
    }

    #endregion

    #region Properties

    private LanguageEdit _SongLanguage;
    public LanguageEdit SongLanguage
    {
      get { return _SongLanguage; }
      set
      {
        if (value != _SongLanguage)
        {
          _SongLanguage = value;
          NotifyOfPropertyChange(() => SongLanguage);
          NotifyOfPropertyChange(() => CanSaveAsync);
        }
      }
    }

    private string _SongText;
    public string SongText
    {
      get { return _SongText; }
      set
      {
        if (value != _SongText)
        {
          _SongText = value;
          NotifyOfPropertyChange(() => SongText);
          NotifyOfPropertyChange(() => CanSaveAsync);
        }
      }
    }

    public string LabelLanguageText { get { return ViewViewModelResources.LabelAddSongLanguage; } }
    public string LabelSongText { get { return ViewViewModelResources.LabelAddSongSongText; } }
    public string LabelSongTitle { get { return ViewViewModelResources.LabelAddSongSongTitle; } }
    public string LabelSongAdditionalMetadata { get { return ViewViewModelResources.LabelSongAdditionalMetadata; } }

    public string Instructions { get { return ViewViewModelResources.InstructionsAddSongPage; } }

    public string InstructionsSelectLanguage { get { return ViewViewModelResources.InstructionsAddSongSelectLanguage; } }
    public string InstructionsEnterSongText { get { return ViewViewModelResources.InstructionsAddSongEnterSongText; } }
    public string InstructionsEnterSongTitle { get { return ViewViewModelResources.InstructionsAddSongEnterSongTitle; } }
    public string InstructionsEnterSongAdditionalMetadata
    {
      get { return ViewViewModelResources.InstructionsAddSongEnterSongAdditionalMetadata; }
    }

    private LanguageSelectorViewModel _Languages;
    public LanguageSelectorViewModel Languages
    {
      get { return _Languages; }
      set
      {
        if (value != _Languages)
        {
          _Languages = value;
          NotifyOfPropertyChange(() => Languages);
        }
      }
    }

    private Visibility _InstructionsVisibility;
    public Visibility InstructionsVisibility
    {
      get { return _InstructionsVisibility; }
      set
      {
        if (value != _InstructionsVisibility)
        {
          _InstructionsVisibility = value;
          NotifyOfPropertyChange(() => InstructionsVisibility);
        }
      }
    }

    private bool _SongHasBeenSaved;
    public bool SongHasBeenSaved
    {
      get { return _SongHasBeenSaved; }
      set
      {
        if (value != _SongHasBeenSaved)
        {
          _SongHasBeenSaved = value;
          NotifyOfPropertyChange(() => SongHasBeenSaved);
          NotifyOfPropertyChange(() => CanSaveAsync);
        }
      }
    }

    private bool _IsSaving;
    public bool IsSaving
    {
      get { return _IsSaving; }
      set
      {
        if (value != _IsSaving)
        {
          _IsSaving = value;
          NotifyOfPropertyChange(() => IsSaving);
          NotifyOfPropertyChange(() => CanSaveAsync);
        }
      }
    }

    #endregion

    #region Methods

    private void HandleLanguageChanged(object sender, EventArgs e)
    {
      if (sender != null)
        SongLanguage = ((LanguageEditViewModel)sender).Model;
    }

    #endregion

    #region Commands

    public override bool CanSaveAsync
    {
      get
      {
        int lineCount = SongText.ParseIntoLines().Count;
        if (string.IsNullOrEmpty(SongText) ||
            lineCount < int.Parse(BusinessResources.SongMinimumLineCount) ||
            SongHasBeenSaved || //Song can only be saved once.  After this, we should be on a new screen.
            SongText.Length < 2 ||
            SongLanguage == null ||
            IsSaving)
          return false;
        else
          return true;
      }

    }

    public override async Task SaveAsync()
    {
      IsSaving = true;
      IncrementApplicationBusyEventMessage.Publish("AddSongMultiLineTextEditViewModel.SaveAsync");
      //DisableNavigationRequestedEventMessage.Publish();
      var thinkId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkId);
      try
      {
        //TODO: PUT ALL SAVE SONG STUFF INTO ONE SAVER COMMAND OBJECT.  THIS OBJECT SHOULD TAKE THE ENTERED SONG INFO, PARSE THE SONG, AND SAVE ALL SUBPHRASES AS WE DO HERE.  THIS SHOULD *NOT* GO INTO THE SONGEDIT MODEL SAVE, AS THE SONG MODEL SHOULD NOT PARSE EVERY TIME IT IS SAVED.

        ///when we save the song lyrics, we want to...
        ///  1) Parse each text line into a subphrase, parse each subphrase into a word-subphrase
        ///     if subphrase is unique and not already in the database, save that subphrase
        ///  2) Create LineEdits from text lines and add these to the song model.
        ///  3) Save the song model itself.
        ///e.g. 
        ///  this is line one
        ///  this is line two
        ///  this is line three
        ///  subphrases: "this is line one" "this is line two" "this is line three"
        ///  word-subphrases: "this" "is" "line" "one" "two" "three"

        #region PARSE THE SONGTEXT INTO LINES AND ADD TO MODEL.LINES
        {
          var songText = SongText;
          var languageText = SongLanguage.Text;
          var lines = songText.ParseIntoLines(); //removes empty entries
          var words = songText.ParseIntoWords().Where((w) => !string.IsNullOrWhiteSpace(w)).ToList();
          //var wordCount = words.Count;
          //var escapeHelper = 0;
          //words = words
          //while (words.Contains(""))
          //{
          //  History.Events.ThinkedAboutTargetEvent.Publish();

          //  //ESCAPE HELPER TO ESCAPE IN CASE OF INFINITE LOOP WITH WHILE STATEMENT.
          //  //I WANT THE WHILE LOOP TO USE THE QUICKNESS OF CONTAINS(), 
          //  //AS OPPOSED TO FOREACH-ING OR FOR-ING.  BUT I HATE POSSIBILITY OF
          //  //ENDLESS LOOP, EVEN THOUGH I DON'T SEE HOW IT COULD HAPPEN.
          //  words.Remove("");

          //  escapeHelper++;
          //  if (escapeHelper > wordCount)
          //    break;
          //}

          //I'M CHANGING THIS TO UTILIZE MY LINELIST.NEWLINELIST(INFOS) ALREADY IN 
          //PLACE.
          //SO AT THIS TIME, WE ONLY NEED TO SAVE THE WORDS THEMSELVES.  THE LINES 
          //WILL BE SAVED IN FUTURE STEP IN THIS BLOCK.
          var allWords = new List<string>(words);

          #region REMOVE ALL WORDS THAT ALREADY EXIST IN DB, SO THAT WE ARE LEFT WITH ONLY NEW WORDS

          //WE DON'T WANT TO ADD DUPLICATE WORDS TO THE DB, SO CHECK TO SEE IF ANY 
          //OF THESE WORDS ALREADY EXIST IN DB.
          var removeCmd = await RemoveAlreadyExistingPhraseTextsCommand.ExecuteAsync(languageText, allWords);
          var allWordsNotAlreadyInDatabase = removeCmd.PrunedPhraseTexts;

          #region CREATE PHRASE FOR EACH NEW WORD

          var criteria = new Business.Criteria.PhraseTextsCriteria(languageText,
            allWordsNotAlreadyInDatabase);
          History.Events.ThinkedAboutTargetEvent.Publish();
          var phraseListContainingAllWordsNotAlreadyInDatabase =
            await PhraseList.NewPhraseListAsync(criteria);
          History.Events.ThinkedAboutTargetEvent.Publish();

          #region SAVE ALL WORDS THAT ARE NEW (NOT ALREADY IN DATABASE)

          //phraseListContainingAllWordsNotAlreadyInDatabase =
          //  await phraseListContainingAllWordsNotAlreadyInDatabase.SaveAsync();
          //SO NOW, ALL OF OUR SONG'S WORDS THAT WERE NOT ALREADY IN THE DATABASE 
          //ARE INDEED NOW STORED IN THE DATABASE.

          #region CREATE LINEEDIT OBJECTS FOR EACH TEXT LINE AND ADD TO MODEL

          var lineInfoDictionary = new Dictionary<int, string>();
          for (int i = 0; i < lines.Count; i++)
          {
            History.Events.ThinkedAboutTargetEvent.Publish();
            lineInfoDictionary.Add(i, lines[i]);
          }

          //WE NOW HAVE INFO DICTIONARY WHICH HAS LINE NUMBER AND CORRESPONDING LINE TEXT
          //CREATE A NEW LINELIST WITH THIS CRITERIA WILL CREATE PHRASES WITH EACH LINE TEXT,
          //AND LINEEDITS OBJECT FOR EACH LINE NUMBER + PHRASE.
          var criteria2 = new Business.Criteria.LineInfosCriteria(languageText, lineInfoDictionary);
          Model.Lines = await LineList.NewLineListAsync(criteria2);

          //WE NOW HAVE A LIST OF LINEEDITS, EACH WITH A PHRASE WITH A SINGLE LINE OF TEXT AND LINE NUMBER.
          //SET OUR MODEL.LINES TO THIS LINELIST
          History.Events.ThinkedAboutTargetEvent.Publish();

          #region SET SONG TITLE (IF NECESSARY)
          //IF THE SONGTITLE IS EMPTY, THEN USE THE FIRST LINE OF THE SONG AS THE SONG TITLE
          if (string.IsNullOrEmpty(Model.Title))
            Model.Title = ViewViewModelResources.AutoTitlePrefix + Model.Lines.GetLine(0).Phrase.Text;
          #endregion
          History.Events.ThinkedAboutTargetEvent.Publish();

          #region ADD METADATA TYPEISSONG

          Model.AddMetadata(BusinessResources.MetadataKeyType,
                            BusinessResources.MetadataValueTypeSong);

          #endregion

          #region SAVE THE ACTUAL SONG MODEL

          //NOW WE CAN USE THE BASE SAVE
          History.Events.ThinkedAboutTargetEvent.Publish();
          var invalidLine = Model.Lines.Where((l) => !l.IsValid).FirstOrDefault();
          await base.SaveAsync();
          SongHasBeenSaved = true;
          IsSaving = false;
          History.Events.ThinkedAboutTargetEvent.Publish();
          #endregion

          #endregion

          #endregion

          #endregion

          #endregion
        }
        #endregion
      }
      catch (Exception ex)
      {
        //Don't really care about handling the exception, I just want to log that 
        //the save failed.
        Services.PublishMessageEvent(string.Format(AppResources.ErrorMsgSaveSong, ex.Message),
          MessagePriority.Medium, MessageType.Error);
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(thinkId);
        //EnableNavigationRequestedEventMessage.Publish();
        DecrementApplicationBusyEventMessage.Publish("AddSongMultiLineTextEditViewModel.SaveAsync");

      }
    }
    #endregion
  }
}
