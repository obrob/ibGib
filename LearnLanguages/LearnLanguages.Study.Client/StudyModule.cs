using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight
{
  [Export(typeof(IModule))]
  public class StudyModule : IModule
  {
    
    public void Initialize()
    {
      //INITIALIZE RECORDER
    }
  }
}
