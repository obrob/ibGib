using System;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using Caliburn.Micro;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.ViewModels
{
  /// <summary>
  /// To Use: 
  /// 1) Create/Get from container instance to use.
  /// 2) SetQuestionAnswer() method with overload of your choosing.
  /// 3) Request to navigate to this 
  /// </summary>
  [Export(typeof(StudyAPhraseViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class StudyAPhraseViewModel : ViewModelBase,
                                       IHandle<Navigation.EventMessages.NavigatedEventMessage>
  {
    public StudyAPhraseViewModel()
    {
      Services.EventAggregator.Subscribe(this);
      QuestionAnswerViewModel = Services.Container.GetExportedValue<QuestionAnswerViewModel>();
    }

    private QuestionAnswerViewModel _QuestionAnswerViewModel;
    public QuestionAnswerViewModel QuestionAnswerViewModel
    {
      get { return _QuestionAnswerViewModel; }
      set
      {
        if (value != _QuestionAnswerViewModel)
        {
          _QuestionAnswerViewModel = value;
          NotifyOfPropertyChange(() => QuestionAnswerViewModel);
        }
      }
    }

    public void SetQuestionAnswer(PhraseEdit questionPhrase, PhraseEdit answerPhrase)
    {
    }

    public void SetQuestionAnswer(TranslationEdit translation, string questionLanguageText)
    {
    }

    public void Handle(Navigation.EventMessages.NavigatedEventMessage message)
    {
      //WE ARE LISTENING FOR A MESSAGE THAT SAYS WE WERE SUCCESSFULLY NAVIGATED TO (SHELLVIEW.MAIN == STUDYVIEWMODEL)
      //SO WE ONLY CARE ABOUT NAVIGATED EVENT MESSAGES ABOUT OUR CORE AS DESTINATION.
      if (message.NavigationInfo.ViewModelCoreNoSpaces != ViewModelBase.GetCoreViewModelName(typeof(StudyViewModel)))
        return;

      //WE HAVE BEEN SUCCESSFULLY NAVIGATED TO.

      //THIS IS A SHARED COMPOSABLE PART, SO WE DO NOT WANT TO UNSUBSCRIBE TO NAVIGATED MESSAGE
      Services.EventAggregator.Unsubscribe(this);
      
    }
  }
}
