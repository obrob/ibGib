namespace LearnLanguages.Common.Interfaces
{
  public interface IShellViewModel
  {
    string Title { get; set; }
    INavigationPanelViewModel NavigationPanelViewModel { get; set; }
    IViewModelBase Main { get; set; }
  }
}
