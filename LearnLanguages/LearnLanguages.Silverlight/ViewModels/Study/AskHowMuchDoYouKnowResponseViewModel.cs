using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AskHowMuchDoYouKnowResponseViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class AskHowMuchDoYouKnowResponseViewModel : AddPhrasePhraseEditViewModel
  {
    public AskHowMuchDoYouKnowResponseViewModel()
    {
    }

    private int _PercentKnown = int.Parse(ViewViewModelResources.DefaultPercentKnown);
    public int PercentKnown
    {
      get { return _PercentKnown; }
      set
      {
        if (value != _PercentKnown)
        {
          _PercentKnown = value;
          NotifyOfPropertyChange(() => PercentKnown);
        }
      }
    }

    private string _InstructionsText = ViewViewModelResources.InstructionsAskHowMuchDoYouKnowResponse;
    public string InstructionsText
    {
      get { return _InstructionsText; }
      set
      {
        if (value != _InstructionsText)
        {
          _InstructionsText = value;
          NotifyOfPropertyChange(() => InstructionsText);
        }
      }
    }

    public string LabelAcceptPercentKnown { get { return ViewViewModelResources.LabelAcceptPercentKnown; } }
  }
}
