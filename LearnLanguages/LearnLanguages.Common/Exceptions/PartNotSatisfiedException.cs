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
using Csla.Serialization;

namespace LearnLanguages.Common.Exceptions
{
  [Serializable]
  public class PartNotSatisfiedException : Exception
  {
    public PartNotSatisfiedException(string partName)
      : base(CommonResources.ErrorMsgNotInjected(partName))
    {

    }
  }
}
