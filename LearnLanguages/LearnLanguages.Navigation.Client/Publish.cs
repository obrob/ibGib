using System;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Navigation
{
  public static class Publish
  {

    //public static void NavigationRequest<T>(string baseAddress) where T : IViewModelBase
    //{
    //  var navInfo = CreateNavigationInfo<T>(Guid.NewGuid(), baseAddress);
    //  Services.EventAggregator.Publish(new EventMessages.NavigationRequestedEventMessage(navInfo));
    //}
    ///// <summary>
    ///// If you want to keep track of this navigation request, provide navigationId and check for this
    ///// in Handle(message) { message.NavInfo.NavigationId }
    ///// </summary>
    ///// <typeparam name="T">Type of IViewModelBase implementation</typeparam>
    ///// <param name="navigationId">tracking id that lasts the course of the navigation process</param>
    ///// <param name="baseAddress">base uri address</param>
    //public static void NavigationRequest<T>(Guid navigationId, string baseAddress) where T : IViewModelBase
    //{
    //  var navInfo = CreateNavigationInfo<T>(navigationId, baseAddress);
    //  Services.EventAggregator.Publish(new EventMessages.NavigationRequestedEventMessage(navInfo));
    //}
    //public static void Navigating(NavigationInfo navigationInfo)
    //{
    //  Services.EventAggregator.Publish(new Navigation.EventMessages.NavigatingEventMessage(navigationInfo));
    //}
    //public static void Navigated(NavigationInfo navigationInfo, IViewModelBase viewModel)
    //{
    //  Services.EventAggregator.Publish(new Navigation.EventMessages.NavigatedEventMessage(navigationInfo, viewModel));
    //}
    //public static void NavigationFailed(NavigationInfo navigationInfo)
    //{
    //  Services.EventAggregator.Publish(new Navigation.EventMessages.NavigationFailedEventMessage(navigationInfo));
    //}

    //private static NavigationInfo CreateNavigationInfo<T>(Guid navigationId, string baseAddress) where T : IViewModelBase
    //{
    //  if (typeof(T).Name == typeof(IViewModelBase).Name)
    //    throw new ArgumentException(
    //      "Generic type must be an instance of a ViewModel that implements IViewModelBase, not be IViewModelBase itself.");

    //  var viewModelCoreNoSpaces = ViewModelBase.GetCoreViewModelName(typeof(T));
    //  var navInfo = new NavigationInfo(navigationId, viewModelCoreNoSpaces, baseAddress);
    //  return navInfo;
    //}

    //public static void Navigating<T>(Guid navigationId, string baseAddress) where T : IViewModelBase
    //{
    //  var navInfo = CreateNavigationInfo<T>(navigationId, baseAddress);
    //  Services.EventAggregator.Publish(new EventMessages.NavigatingEventMessage(navInfo));
    //}
    //public static void Navigated<T>(Guid navigationId, string baseAddress) where T : IViewModelBase
    //{
    //  var navInfo = CreateNavigationInfo<T>(navigationId, baseAddress);
    //  Services.EventAggregator.Publish(new EventMessages.NavigatedEventMessage(navInfo));
    //}
    //public static void NavigationFailed<T>(Guid navigationId, string baseAddress) where T : IViewModelBase
    //{
    //  var navInfo = CreateNavigationInfo<T>(navigationId, baseAddress);
    //  Services.EventAggregator.Publish(new EventMessages.NavigationFailedEventMessage(navInfo));
    //}
    

    //public static void AuthenticationChanged()
    //{
    //  Services.EventAggregator.Publish(new AuthenticationChangedEventMessage());
    //}

    //public static void PartSatisfied(string part)
    //{
    //  Services.EventAggregator.Publish(new EventMessages.PartSatisfiedEventMessage(part));
    //}
  }
}
