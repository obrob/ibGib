using System;
using LearnLanguages.History;
using LearnLanguages.Business;
using LearnLanguages.History.Events;
using LearnLanguages.History.Bases;
using System.Threading.Tasks;

namespace LearnLanguages.Study
{
  //NOT NEEDED AT THE MOMENT. AM SAVING AUTOTRANSLATIONS AUTOMATICALLY WITH THE AZURE 
  //TRANSLATOR.

  ///// <summary>
  ///// Not a "Recorder" strictly speaking, but rather a listener that listens for PhraseAutoTranslatedEvent's.
  ///// If it hears this event and this ShouldRecord (enabled), then when a phrase is automatically translated
  ///// using a service such as Microsoft Translator (Bing), then it automatically saves this to DB, which almost
  ///// acts as a caching function until the user overrides the translation with a custom translation.
  ///// </summary>
  //public class DefaultPhraseAutoTranslatedRecorder : HistoryRecorderBase<History.Events.PhraseAutoTranslatedEvent>
  //{
  //  public DefaultPhraseAutoTranslatedRecorder()
  //  {
  //    Id = Guid.Parse(StudyResources.DefaultPhraseAutoTranslatedRecorderId);
  //  }

  //  /// <summary>
  //  /// Always returns true.
  //  /// Since this is not really a recorder, it doesn't do anything special like filtering events.
  //  /// Use the IsEnabled property to enable/disable this object.
  //  /// </summary>
  //  protected override bool ShouldRecord(History.Events.PhraseAutoTranslatedEvent message)
  //  {
  //    return true;
  //  }

  //  /// <summary>
  //  /// Saves the translation if one does not already exist in DB.
  //  /// </summary>
  //  /// <param name="message"></param>
  //  protected override async Task RecordAsync(History.Events.PhraseAutoTranslatedEvent message)
  //  {
  //    //FIRST, WE NEED TO MAKE SURE WE DON'T ALREADY HAVE THIS TRANSLATION PAIR (SOURCE PHRASE, TRANSLATION PHRASE)
  //    //IN OUR DATABASE.
  //    var criteria =
  //      new Business.Criteria.TranslationSearchCriteria(message.SourcePhrase,
  //                                                      message.TranslatedPhrase.Language.Text);
  //    var retriever = await Business.TranslationSearchRetriever.CreateNewAsync(criteria);
  //    if (retriever.Translation != null)
  //    {
  //      //WE ALREADY HAVE A TRANSLATION FOR THIS, SO WE DON'T NEED TO DO ANYTHING FURTHER.
  //      return;
  //    }

  //    //NO PRIOR TRANSLATION EXISTS, SO CREATE AND SAVE.

  //    //CREATE
  //    var createTranslationCriteria =
  //      new Business.Criteria.ListOfPhrasesCriteria(message.SourcePhrase, message.TranslatedPhrase);
  //    var creator = await TranslationCreator.CreateNewAsync(createTranslationCriteria);

  //    //SAVE
  //    await creator.Translation.SaveAsync();
  //  }
  //}
}
