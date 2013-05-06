using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModelBases;
using System;
using System.Threading.Tasks;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddPhraseViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddPhraseViewModel : ViewModelBase
  {
    public AddPhraseViewModel()
    {
      InitializeModelAsync();
    }

    private async Task InitializeModelAsync()
    {
      var thinkId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkId);
      var blankPhraseCreator = await BlankPhraseCreator.CreateNewAsync();
      History.Events.ThinkedAboutTargetEvent.Publish(thinkId);
      //var newPhraseEdit = await PhraseEdit.NewPhraseEditAsync();
      var phraseViewModel = Services.Container.GetExportedValue<AddPhrasePhraseEditViewModel>();
      phraseViewModel.Model = blankPhraseCreator.Phrase;
      Phrase = phraseViewModel;

      //PhraseEdit.NewPhraseEdit((s, r) =>
      //{
      //  if (r.Error != null)
      //    throw r.Error;

      //  var phraseViewModel = Services.Container.GetExportedValue<AddPhrasePhraseEditViewModel>();
      //  phraseViewModel.Model = r.Object;
      //  Phrase = phraseViewModel;
      //});
    }

    private AddPhrasePhraseEditViewModel _Phrase;
    public AddPhrasePhraseEditViewModel Phrase
    {
      get { return _Phrase; }
      set
      {
        if (value != _Phrase)
        {
          _Phrase = value;
          NotifyOfPropertyChange(() => Phrase);
        }
      }
    }
  }
}
