using System;
using Csla.Serialization;

namespace LearnLanguages.Navigation.Exceptions
{
  [Serializable]
  public class NavigationTextAlreadyExistsException : Exception
  {
    public NavigationTextAlreadyExistsException(string navText)
      : base(
             string.Format(NavigationResources.ErrorMsgNavigationTextAlreadyExists, 
                           navText)
            )
    {

    }
  }
}
