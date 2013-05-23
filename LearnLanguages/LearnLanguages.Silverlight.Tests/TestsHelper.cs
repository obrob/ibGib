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
      var relevantErrorMsg = GetRelevantErrorMsg(dpException);
      var containsIdStrings = relevantErrorMsg.Contains("The Id") &&
                              relevantErrorMsg.Contains("was not found");
      return containsIdStrings;
    }

    public static string GetRelevantErrorMsg(Csla.DataPortalException dpException)
    {
      string relevantErrorMsg = "";
      if (dpException.Message.Contains("One or more errors occurred."))
        relevantErrorMsg = dpException.ErrorInfo.InnerError.Message;
      else
        relevantErrorMsg = dpException.Message;

      return relevantErrorMsg;
    }
    public static bool IsUserNotFoundException(Csla.DataPortalException dpException)
    {
      var relevantErrorMsg = GetRelevantErrorMsg(dpException);
      var containsIdStrings = relevantErrorMsg.Contains("The username") &&
                              relevantErrorMsg.Contains("was not found");
      return containsIdStrings;
    }
  }
}
