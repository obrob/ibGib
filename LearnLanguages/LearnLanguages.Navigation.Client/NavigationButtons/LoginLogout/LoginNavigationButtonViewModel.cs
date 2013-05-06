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
  [Export(typeof(LoginNavigationButtonViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class LoginNavigationButtonViewModel : NavigationButtonViewModelBase
  {
    
    public override bool CanNavigateImpl()
    {
      return !Csla.ApplicationContext.User.Identity.IsAuthenticated;
    }

    public new string LabelText { get { return "Login"; } }

    //public string Text { get { return "Login"; } }
  }
}
