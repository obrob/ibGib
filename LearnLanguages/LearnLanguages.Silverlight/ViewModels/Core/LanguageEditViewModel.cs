using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LanguageEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class LanguageEditViewModel : ViewModelBase<LanguageEdit, LanguageDto>
  {
    public override string ToString()
    {
      if (Model == null)
        return "";
      else
        return Model.Text;
    }
  }
}
