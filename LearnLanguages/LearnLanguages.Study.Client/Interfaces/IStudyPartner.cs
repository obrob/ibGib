using System;
using System.Net;
//using Caliburn.Micro;
using LearnLanguages.Business;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Navigation.EventMessages;

namespace LearnLanguages.Study.Interfaces
{
  /// <summary>
  /// This has the behavior necessary for a study partner who communicates using the exchange.
  /// </summary>
  public interface IStudyPartner : IHaveId,
                                   IHandle<IOpportunity<MultiLineTextList, IViewModelBase>>,
                                   IHandle<IOfferResponse<MultiLineTextList, IViewModelBase>>,
                                   IHandle<IStatusUpdate<MultiLineTextList, IViewModelBase>>,
                                   IHandle<ICancelation>,
                                   IHandle<IConglomerateMessage>, 
                                   IHandle<NavigationRequestedEventMessage>
  {
    //void Study(MultiLineTextList multiLineTexts, LanguageEdit language, IEventAggregator eventAggregator);
    //IDoAJob<IStudyJobInfo<MultiLineTextList>, MultiLineTextList> MultiLineTextsStudier { get; }
    //void StudyPhrases(PhraseList phrases, IEventAggregator eventAggregator);
    //void StudyTranslations(TranslationList translations, IEventAggregator eventAggregator);
    //void Chug(IEventAggregator eventAggregator);
    //void ShowProgress(IEventAggregator eventAggregator);
    //double GetProgressAsOneNumber();
    //void GetProgressAsOneNumberAsync(Common.Delegates.AsyncCallback<double> callback);
    //double GetProgressAsOneNumber(TranslationList translations);
    //void GetProgressAsOneNumberAsync(TranslationList translations, Common.Delegates.AsyncCallback<double> callback);
    //double GetProgressAsOneNumber(MultiLineTextList multiLineTexts);
    //void GetProgressAsOneNumberAsync(MultiLineTextList multiLineTexts, Common.Delegates.AsyncCallback<double> callback);
  }
}
