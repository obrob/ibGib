using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModelBases;
using System;
using System.Threading.Tasks;
using LearnLanguages.Silverlight.Common;
using LearnLanguages.Common.EventMessages;
using LearnLanguages.Navigation.EventMessages;
using Caliburn.Micro;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddSongViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddSongViewModel : PageViewModelBase,
                                  IHandle<NavigatedEventMessage>
  {
    public AddSongViewModel()
    {
      Services.EventAggregator.Subscribe(this);
    }

    private async Task InitializeModelAsync()
    {

      var thinkId = System.Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkId);
      DisableNavigationRequestedEventMessage.Publish();
      try
      {
        var newMultiLineTextEdit = await MultiLineTextEdit.NewMultiLineTextEditAsync();
        var songViewModel = Services.Container.GetExportedValue<AddSongMultiLineTextEditViewModel>();
        songViewModel.Model = newMultiLineTextEdit;
        Song = songViewModel;
      }
      finally
      {
        History.Events.ThinkedAboutTargetEvent.Publish(thinkId);
        EnableNavigationRequestedEventMessage.Publish();
      }
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

    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = ViewViewModelResources.InstructionsAddSongPage;
      Title = ViewViewModelResources.TitleAddSongPage;
      Description = ViewViewModelResources.DescriptionAddSongPage;
      ToolTip = ViewViewModelResources.ToolTipAddSongPage;
    }

    public async void Handle(NavigatedEventMessage message)
    {
      if (message.NavigationInfo.TargetPage.ContentViewModel == this)
      {
        await InitializeModelAsync();
      }
    }
  }
}
