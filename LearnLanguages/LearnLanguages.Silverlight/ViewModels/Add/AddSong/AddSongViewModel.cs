using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModelBases;
using System;
using System.Threading.Tasks;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddSongViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddSongViewModel : ViewModelBase
  {
    public AddSongViewModel()
    {
      InitializeModelAsync();
    }

    private async Task InitializeModelAsync()
    {
      #region Thinking
      var thinkId = System.Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkId);
      #endregion
      var newMultiLineTextEdit = await MultiLineTextEdit.NewMultiLineTextEditAsync();
      #region Thinked
      History.Events.ThinkedAboutTargetEvent.Publish(thinkId);
      #endregion

      var songViewModel = Services.Container.GetExportedValue<AddSongMultiLineTextEditViewModel>();
      songViewModel.Model = newMultiLineTextEdit;
      Song = songViewModel;
    }

    private AddSongMultiLineTextEditViewModel _Song;
    public AddSongMultiLineTextEditViewModel Song
    {
      get { return _Song; }
      set
      {
        if (value != _Song)
        {
          _Song = value;
          NotifyOfPropertyChange(() => Song);
        }
      }
    }
  }
}
