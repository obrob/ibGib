using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Threading.Tasks;
using LearnLanguages.Common.EventMessages;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddLanguageLanguageEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddLanguageLanguageEditViewModel : LanguageEditViewModel
  {
    public string LabelLanguageText { get { return ViewViewModelResources.LabelAddLanguageLanguageText; } }

    public override async Task SaveAsync()
    {
      try
      {
        var model = await Model.SaveAsync();
        Model = model;
        NotifyOfPropertyChange(() => CanSave);
        Services.EventAggregator.Publish(new LanguageAddedEventMessage(Model.Text));
      }
      catch (Csla.DataPortalException dpex)
      {
        var errorMsgLanguageTextAlreadyExists =
          DataAccess.Exceptions.LanguageTextAlreadyExistsException.GetDefaultErrorMessage(Model.Text);
        if (dpex.Message.Contains(errorMsgLanguageTextAlreadyExists))
          System.Windows.MessageBox.Show(errorMsgLanguageTextAlreadyExists);
      }
    }
  }
}
