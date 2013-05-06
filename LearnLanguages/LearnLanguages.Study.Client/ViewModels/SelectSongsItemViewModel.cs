 using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using Caliburn.Micro;
using System;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(SelectSongsItemViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class SelectSongsItemViewModel : ViewModelBase
  {
    public SelectSongsItemViewModel()
    {

    }

    public SelectSongsItemViewModel(string songTitle)
    {
      SongTitle = songTitle;
    }

    //public SelectSongsItemViewModel(MultiLineTextDto songDto)
    //{

    //}

    private string _SongTitle;
    public string SongTitle
    {
      get { return _SongTitle; }
      set
      {
        if (value != _SongTitle)
        {
          _SongTitle = value;
          NotifyOfPropertyChange(() => SongTitle);
        }
      }
    }

    //private MultiLineTextDto _SongDto;
    //public MultiLineTextDto SongDto
    //{
    //  get { return _SongDto; }
    //  set
    //  {
    //    if (value != _SongDto)
    //    {
    //      _SongDto = value;
    //      SongTitle = (value == null) ? "" : value.Title;
    //      NotifyOfPropertyChange(() => SongDto);
    //    }
    //  }
    //}

    private bool _IsChecked;
    public bool IsChecked
    {
      get { return _IsChecked; }
      set
      {
        if (value != _IsChecked)
        {
          _IsChecked = value;
          NotifyOfPropertyChange(() => IsChecked);
        }
      }
    }
  }
}
