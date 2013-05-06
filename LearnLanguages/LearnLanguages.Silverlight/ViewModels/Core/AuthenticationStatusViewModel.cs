using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using System;
using Caliburn.Micro;
using LearnLanguages.Common.EventMessages;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AuthenticationStatusViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class AuthenticationStatusViewModel : ViewModelBase, 
                                               IHandle<AuthenticationChangedEventMessage>, 
                                               IDisposable
  {
    public AuthenticationStatusViewModel()
    {
      Services.EventAggregator.Subscribe(this);
      //THIS INITIALIZES PROPERTIES INTERNALLY - IT DOES NOT _*PUBLISH*_ THE EVENT
      var message = new AuthenticationChangedEventMessage();
      Handle(message);
    }

    private string _CurrentPrincipalName;
    public string CurrentPrincipalName
    {
      get { return _CurrentPrincipalName; }
      set
      {
        if (value != _CurrentPrincipalName)
        {
          _CurrentPrincipalName = value;
          NotifyOfPropertyChange(() => CurrentPrincipalName);
        }
      }
    }

    private bool _IsAuthenticated;
    public bool IsAuthenticated
    {
      get { return _IsAuthenticated; }
      set
      {
        if (value != _IsAuthenticated)
        {
          _IsAuthenticated = value;
          NotifyOfPropertyChange(() => IsAuthenticated);
        }
      }
    }

    private bool _IsInAdminRole;
    public bool IsInAdminRole
    {
      get { return _IsInAdminRole; }
      set
      {
        if (value != _IsInAdminRole)
        {
          _IsInAdminRole = value;
          NotifyOfPropertyChange(() => IsInAdminRole);
        }
      }
    }

    private bool _IsInUserRole;
    public bool IsInUserRole
    {
      get { return _IsInUserRole; }
      set
      {
        if (value != _IsInUserRole)
        {
          _IsInUserRole = value;
          NotifyOfPropertyChange(() => IsInUserRole);
        }
      }
    }

    public void Handle(AuthenticationChangedEventMessage message)
    {
      CurrentPrincipalName = message.CurrentPrincipalName;
      IsAuthenticated = message.IsAuthenticated;
      IsInAdminRole = message.IsInRole(DataAccess.DalResources.RoleAdmin);
      IsInUserRole = message.IsInRole(DataAccess.DalResources.RoleUser);
    }

    public void Dispose()
    {
      Services.EventAggregator.Unsubscribe(this);
    }
  }
}
