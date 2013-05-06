//using System.ComponentModel.Composition;
//using LearnLanguages.Common.ViewModelBases;
//using LearnLanguages.Silverlight.Interfaces;
//using Caliburn.Micro;
//using System.Windows;
//using LearnLanguages.Business;
//using System.ComponentModel;
//using System;
//using LearnLanguages.Delegates;

//namespace LearnLanguages.Silverlight.ViewModels
//{
//  [Export(typeof(IWantToLearnASongViewModel))]
//  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
//  public class IWantToLearnASongViewModel : ViewModelBase
//  {
//    public IWantToLearnASongViewModel()
//    {
//      PhraseEdit.NewPhraseEdit((s, r) =>
//        {
//          if (r.Error != null)
//            throw r.Error;

//          var phraseViewModel = Services.Container.GetExportedValue<IWantToLearnASongPhraseEditViewModel>();
//          phraseViewModel.Model = r.Object;
//          Phrase = phraseViewModel;
//        });
//    }

//    private IWantToLearnASongPhraseEditViewModel _Phrase;
//    public IWantToLearnASongPhraseEditViewModel Phrase
//    {
//      get { return _Phrase; }
//      set
//      {
//        if (value != _Phrase)
//        {
//          _Phrase = value;
//          NotifyOfPropertyChange(() => Phrase);
//        }
//      }
//    }

//  }
//}
