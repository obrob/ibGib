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

namespace LearnLanguages.Silverlight.Tests
{
  public static class TestsHelper
  {
    public static bool IsIdNotFoundException(Csla.DataPortalException dpException)
    {
      var containsIdStrings = (dpException.Message.Contains("The Id")           && 
                               dpException.Message.Contains("was not found"));
      return containsIdStrings;
    }

    public static bool IsUserNotFoundException(Csla.DataPortalException dpException)
    {
      var containsIdStrings = (dpException.Message.Contains("The username") &&
                               dpException.Message.Contains("was not found"));
      return containsIdStrings;
    }
  }
}
