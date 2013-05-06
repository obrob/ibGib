using System;
using System.Net;
//using Caliburn.Micro;
using LearnLanguages.Business;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;
using System.Threading.Tasks;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudyItemViewModelBase : IViewModelBase, IStudyReviewMethod
  {
    string StudyItemTitle { get; }
    Task ShowAsync();
    void Abort();
    event EventHandler Shown;
  }
}
