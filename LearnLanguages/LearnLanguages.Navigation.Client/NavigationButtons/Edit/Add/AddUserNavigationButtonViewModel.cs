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
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddUserNavigationButtonViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class AddUserNavigationButtonViewModel : NavigationButtonViewModelBase
  {
    public override bool CanNavigateImpl()
    {
      var identity = Csla.ApplicationContext.User.Identity;
      return (identity.IsAuthenticated &&
              Csla.ApplicationContext.User.IsInRole(LearnLanguages.DataAccess.DalResources.RoleAdmin));
    }
  }
}
