using System;
using Csla.Serialization;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Navigation.Exceptions
{
  [Serializable]
  public class PageAlreadyRegisteredException : Exception
  {
    public PageAlreadyRegisteredException(IPage page)
      : base(
             string.Format(NavigationResources.ErrorMsgPageAlreadyRegistered, 
                           page.NavText)
            )
    {

    }
  }
}
