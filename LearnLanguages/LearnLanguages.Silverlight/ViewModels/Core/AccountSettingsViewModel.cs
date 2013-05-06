using LearnLanguages.Business.Security;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Common;
using LearnLanguages.Common;
using System.Threading.Tasks;
using System;
using System.Windows;
using System.ComponentModel;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AccountSettingsViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class AccountSettingsViewModel : PageViewModelBase,
                                          IDataErrorInfo
  {
    private string _OldPassword;
    public string OldPassword
    {
      get { return _OldPassword; }
      set
      {
        if (value != _OldPassword)
        {
          _OldPassword = value;
          NotifyOfPropertyChange(() => OldPassword);
          NotifyOfPropertyChange(() => CanChangePassword);
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
          NotifyOfPropertyChange(() => CanChangePassword);
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
          NotifyOfPropertyChange(() => CanChangePassword);
        }
      }
    }

    public bool CanChangePassword
    {
      get
      {
        return !string.IsNullOrEmpty(OldPassword) &&
               CommonHelper.PasswordIsValid(NewPassword) &&
               ConfirmNewPassword == NewPassword;
      }
    }
    public async Task ChangePassword()
    {
      if (!CanChangePassword)
        return;

      var criteria = new Business.Criteria.ChangePasswordCriteria(OldPassword, NewPassword);
      var changePassword = await Business.ChangePasswordReadOnly.CreateNewAsync(criteria);
      if (changePassword.WasSuccessful)
      {
        //hack: hard-coding messagebox. need to implement some kind of dialog system.
        MessageBox.Show(ViewViewModelResources.MsgPasswordWasSuccessfullyChanged);
      }
      else
      {
        MessageBox.Show(ViewViewModelResources.ErrorMsgPasswordChangeFailed);
      }

      ClearPasswords();
    }

    private void ClearPasswords()
    {
      OldPassword = string.Empty;
      NewPassword = string.Empty;
      ConfirmNewPassword = string.Empty;
    }


    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = ViewViewModelResources.InstructionsAccountSettingsPage;
      Title = ViewViewModelResources.TitleAccountSettingsPage;
      Description = ViewViewModelResources.DescriptionAccountSettingsPage;
      ToolTip = ViewViewModelResources.ToolTipAccountSettingsPage;
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
          case "OldPassword":
            if (string.IsNullOrEmpty(OldPassword))
              return null;
            if (!CommonHelper.PasswordIsValid(OldPassword, out validResults))
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


          default:
            break;

        }

        //WE HAD NO ERRORS, SO RETURN NULL TO INDICATE NO ERRORS
        return null;
      }
    }
  }
}
