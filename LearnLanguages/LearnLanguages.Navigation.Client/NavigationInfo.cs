using System;
using System.Collections.Generic;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Navigation
{
  /// <summary>
  /// NOT SERIALIZABLE!!!  No public default constructor.  Not marked as serializable.  
  /// I note this because I foresee the possibility of having to change this if I 
  /// ever need to Csla-ize this class.
  /// </summary>
  public class NavigationInfo : IHaveUri 
  {
    public NavigationInfo(Guid navigationId, IPage targetPage)
    {
      NavigationId = navigationId;
      TargetPage = targetPage;
    }

    public Guid NavigationId { get; private set; }
    public IPage TargetPage { get; private set; }
    public Uri Uri { get; private set; }

    ///// <summary>
    ///// Navigation requested event message, for navigating to view models using convention over configuration.
    ///// EXPECTED FORMAT: [CoreText]ViewModel
    ///// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    ///// </summary>
    ///// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    ///// <param name="queryString"></param>
    //public NavigationInfo(Guid navigationId, string targetPageIdString, string baseAddress)
    //{
    //  if (navigationId == Guid.Empty)
    //    throw new ArgumentException("navigationId is Guid.Empty");

    //  NavigationId = navigationId;
    //  ViewModelCoreNoSpaces = viewModelCoreNoSpaces;
    //  Uri = new Uri(baseAddress + "/" + ViewModelCoreNoSpaces, UriKind.Absolute);
    //}
    ///// <summary>
    ///// Navigation requested event message, for navigating to view models using convention over configuration.
    ///// EXPECTED FORMAT: [CoreText]ViewModel
    ///// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    ///// </summary>
    ///// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    ///// <param name="queryString"></param>
    //public NavigationInfo(Guid navigationId, string targetPageIdString, string query, string baseAddress)
    //{
    //  if (navigationId == Guid.Empty)
    //    throw new ArgumentException("navigationId is Guid.Empty");

    //  NavigationId = navigationId;
    //  ViewModelCoreNoSpaces = viewModelCoreNoSpaces;
    //  var address = baseAddress + "/" + ViewModelCoreNoSpaces;
    //  if (!string.IsNullOrEmpty(query))
    //    address += "?" + query;
    //  Uri = new Uri(address, UriKind.Absolute);
    //}
    ///// <summary>
    ///// Navigation requested event message, for navigating to view models using convention over configuration.
    ///// EXPECTED FORMAT: [CoreText]ViewModel
    ///// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    ///// </summary>
    ///// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    ///// <param name="queryString"></param>
    //public NavigationInfo(Guid navigationId,
    //                      string targetPageIdString, 
    //                      string baseAddress,
    //                      IDictionary<string, string> queryEntries)
    //{
    //  if (queryEntries == null || queryEntries.Count == 0)
    //    throw new ArgumentNullException(
    //      "queryEntries == null || count == 0.  There does exist another overload without queryEntries.");

    //  NavigationId = navigationId;
    //  ViewModelCoreNoSpaces = viewModelCoreNoSpaces;

    //  var address = baseAddress + "/" + ViewModelCoreNoSpaces;
    //  string queryStr = "";
    //  bool firstEntry = true;
    //  foreach (var entry in queryEntries)
    //  {
    //    if (firstEntry)
    //      queryStr += entry.Key + "=" + entry.Value;
    //    else
    //      queryStr += "&" + entry.Key + "=" + entry.Value;
    //  }
    //  Uri = new Uri(address + "?" + queryStr, UriKind.Absolute);
    //}

    
  }
}
