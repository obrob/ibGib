using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddTranslationTranslationEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  //public class AddTranslationTranslationEditViewModel : ViewModelBase<TranslationEdit, TranslationDto>
  public class AddTranslationTranslationEditViewModel : TranslationEditViewModelBase<AddTranslationPhrasesViewModel>
  {
    //public AddTranslationTranslationEditViewModel()
    //{
    //  PhrasesViewModel = new TranslationPhrasesViewModel();
    //}

    //public override void SetModel(TranslationEdit model)
    //{
    //  base.SetModel(model);
    //  PhrasesViewModel.ModelList = model.Phrases;
    //}

    //private TranslationPhrasesViewModel _PhrasesViewModel;
    //public TranslationPhrasesViewModel PhrasesViewModel
    //{
    //  get { return _PhrasesViewModel; }
    //  set
    //  {
    //    if (value != _PhrasesViewModel)
    //    {
    //      _PhrasesViewModel = value;
    //      NotifyOfPropertyChange(() => PhrasesViewModel);
    //    }
    //  }
    //}



  }
}
