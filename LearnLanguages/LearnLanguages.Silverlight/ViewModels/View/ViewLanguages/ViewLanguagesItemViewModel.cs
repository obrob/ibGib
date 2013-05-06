using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using LearnLanguages.Common.ViewModelBases;
using System.Threading.Tasks;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewLanguagesItemViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewLanguagesItemViewModel : ViewModelBase<LanguageEdit, LanguageDto>
  {
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

    public override async Task SaveAsync()
    {
      try
      {
        var model = await Model.SaveAsync();
        Model = model;
        NotifyOfPropertyChange(() => CanSave);
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
