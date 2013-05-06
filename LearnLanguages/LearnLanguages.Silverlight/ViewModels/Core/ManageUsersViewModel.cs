using System.Windows;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using System.Text.RegularExpressions;
using System;
using LearnLanguages.Silverlight.Common;
using LearnLanguages.Common;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ManageUsersViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ManageUsersViewModel : PageViewModelBase, 
                                      IDataErrorInfo
  {
    public ManageUsersViewModel()
    {
    }

    #region AddUser

    private string _NewUsername;
    public string NewUsername
    {
      get { return _NewUsername; }
      set
      {
        if (value != _NewUsername)
        {
          _NewUsername = value;
          NotifyOfPropertyChange(() => NewUsername);
          NotifyOfPropertyChange(() => CanAddUser);
        }
      }
    }

    private string _NewPassword;
    public string NewPassword
    {
      get { return _NewPassword; }
      set
      {
        if (value != _NewPassword)
        {
          _NewPassword = value;
          NotifyOfPropertyChange(() => NewPassword);
          NotifyOfPropertyChange(() => CanAddUser);
        }
      }
    }

    private string _ConfirmNewPassword;
    public string ConfirmNewPassword
    {
      get { return _ConfirmNewPassword; }
      set
      {
        if (value != _ConfirmNewPassword)
        {
          _ConfirmNewPassword = value;
          NotifyOfPropertyChange(() => ConfirmNewPassword);
          NotifyOfPropertyChange(() => CanAddUser);
        }
      }
    }

    public bool CanAddUser
    {
      get
      {
        //PASSWORDS MATCH
        bool passwordsMatch = NewPassword == ConfirmNewPassword;

        //PASSWORD IS VALID
        bool newPasswordIsValid = CommonHelper.PasswordIsValid(NewPassword);

        //USERNAME IS VALID
        bool newUsernameIsValid = CommonHelper.UsernameIsValid(NewUsername);

        bool canAddUser = DataAccess.DalHelper.IsInRoleToAddUser() &&
                          passwordsMatch &&
                          newUsernameIsValid &&
                          newPasswordIsValid;
        
        return canAddUser;
      }
    }
    public async Task AddUser()
    {
      #region Thinking
      var thinkingId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkingId);
      #endregion
      var criteria = new Csla.Security.UsernameCriteria(NewUsername, NewPassword);
      try
      {
        var creator = await Business.NewUserCreator.CreateNewAsync(criteria);
        if (creator.WasSuccessful)
          MessageBox.Show(string.Format(ViewViewModelResources.MessageNewUserAdded, NewUsername));
        else
          MessageBox.Show(string.Format(ViewViewModelResources.ErrorMsgAddUserWasUnsuccessful, NewUsername));
      }
      finally
      {
        #region Thinked
        History.Events.ThinkedAboutTargetEvent.Publish(thinkingId);
        #endregion
      }
    }

    #endregion

    #region RemoveUser

    private string _UsernameToRemove;
    public string UsernameToRemove
    {
      get { return _UsernameToRemove; }
      set
      {
        if (value != _UsernameToRemove)
        {
          _UsernameToRemove = value;
          NotifyOfPropertyChange(() => UsernameToRemove);
          NotifyOfPropertyChange(() => CanRemoveUser);
        }
      }
    }

    public bool CanRemoveUser
    {
      get
      {
        return !string.IsNullOrEmpty(UsernameToRemove);
      }
    }
    public async Task RemoveUser()
    {
      var result = MessageBox.Show(ViewViewModelResources.MsgConfirmDeleteUser, 
                                   ViewViewModelResources.MsgConfirmDeleteUserTitle, 
                                   MessageBoxButton.OKCancel);
      if (result == MessageBoxResult.Cancel)
      {
        MessageBox.Show(ViewViewModelResources.MsgDeleteUserCanceled);
        return;
      }
      #region Thinking
      var targetId = new Guid("C2F2BD3D-AD11-43B0-B34D-F53F33A845CD");
      History.Events.ThinkingAboutTargetEvent.Publish(targetId);
      #endregion
      try
      {
        var readOnly = await Business.DeleteUserReadOnly.CreateNewAsync(UsernameToRemove);
        if (readOnly.WasSuccessful)
          MessageBox.Show(string.Format(ViewViewModelResources.MessageUserWasRemoved, UsernameToRemove));
        else
          MessageBox.Show(string.Format(ViewViewModelResources.ErrorMsgRemoveUserWasUnsuccessful, NewUsername));
      }
      finally
      {
        #region Thinked
        History.Events.ThinkedAboutTargetEvent.Publish(targetId);
        #endregion
      }
    }

    #endregion

    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = ViewViewModelResources.InstructionsManageUsersPage;
      Title = ViewViewModelResources.TitleManageUsersPage;
      Description = ViewViewModelResources.DescriptionManageUsersPage;
      ToolTip = ViewViewModelResources.ToolTipManageUsersPage;
    }

    public string Error
    {
      get { return null; }
    }

    public string this[string propertyName]
    {
      get
      {
        string validResults = "";

        switch (propertyName)
        {
          //ADD USER
          case "NewUsername":
            if (string.IsNullOrEmpty(NewUsername))
              return null;
            if (!CommonHelper.UsernameIsValid(NewUsername, out validResults))
              return validResults;
            break;
          case "NewPassword":
            if (string.IsNullOrEmpty(NewPassword))
              return null;
            if (!CommonHelper.PasswordIsValid(NewPassword, out validResults))
              return validResults;
            break;
          case "ConfirmNewPassword":
            if (string.IsNullOrEmpty(ConfirmNewPassword))
              return null;
            if (NewPassword != ConfirmNewPassword)
              return CommonResources.ErrorMsgConfirmNewPasswordDoesNotMatch;
            break;


          //REMOVE USER
          case "UsernameToRemove":
            if (string.IsNullOrEmpty(UsernameToRemove))
              return null;
            if (!CommonHelper.UsernameIsValid(UsernameToRemove, out validResults))
              return validResults;
            break;


          default:
            break;

        }

        //WE HAD NO ERRORS, SO RETURN NULL TO INDICATE NO ERRORS
        return null;
      }
    }
  }
}
