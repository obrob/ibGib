using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LearnLanguages.Common
{
  public static class CommonHelper
  {
    /// <summary>
    /// I've added these "ValidationResult" methods to be used with CustomValidationAttribute.
    /// E.g. [CustomValidation(typeof(Common.CommonHelper), "UsernameIsValidValidationResult")]
    /// </summary>
    public static ValidationResult UsernameIsValidValidationResult(string username)
    {
      string errorDescription = "";
      var isValid = UsernameIsValid(username, out errorDescription);
      if (isValid)
        return ValidationResult.Success;
      else
        return new ValidationResult(errorDescription);
    }

    /// <summary>
    /// I've added these "ValidationResult" methods to be used with CustomValidationAttribute.
    /// E.g. [CustomValidation(typeof(Common.CommonHelper), "UsernameIsValidValidationResult")]
    /// </summary>
    public static ValidationResult PasswordIsValidValidationResult(string password)
    {
      string errorDescription = "";
      var isValid = PasswordIsValid(password, out errorDescription);
      if (isValid)
        return ValidationResult.Success;
      else
        return new ValidationResult(errorDescription);
    }

    public static bool PasswordIsValid(string password)
    {
      string dummy;
      var isValid = PasswordIsValid(password, out dummy);
      return isValid;
    }

    public static bool PasswordIsValid(string password, out string errorDescription)
    {

      if (string.IsNullOrEmpty(password))
      {
        errorDescription = CommonResources.ErrorMsgInvalidPasswordEmpty;
        return false;
      }

      if (password.Length < int.Parse(CommonResources.MinPasswordLength) ||
          password.Length > int.Parse(CommonResources.MaxPasswordLength))
      {
        errorDescription = CommonResources.ErrorMsgInvalidPasswordLength;
        return false;
      }

      if (!Regex.IsMatch(password, CommonResources.PasswordValidationRegex))
      {
        errorDescription = CommonResources.ErrorMsgInvalidPasswordComposition;
        return false;
      }

      //should fix the regex, but this will do for now.
      var containsSpecialCharacter = false;
      var specialChars = @"!@#$%^&*()-_=+";
      for (int i = 0; i < password.Length; i++)
      {
        var passwordCharAsString = password[i].ToString();
        if (specialChars.Contains(passwordCharAsString))
        {
          containsSpecialCharacter = true;
          break;
        }
      }

      if (!containsSpecialCharacter)
      {
        errorDescription = CommonResources.ErrorMsgInvalidPasswordComposition;
        return false;
      }

      errorDescription = null;
      return true; //password is valid
    }

    public static bool UsernameIsValid(string username)
    {
      string dummy = "";
      var isValid = UsernameIsValid(username, out dummy);
      return isValid;
    }

    public static bool UsernameIsValid(string username, out string errorDescription)
    {
      if (string.IsNullOrEmpty(username))
      {
        errorDescription = CommonResources.ErrorMsgInvalidUsernameEmpty;
        return false;
      }

      if (username.Length < int.Parse(CommonResources.MinUsernameLength) ||
          username.Length > int.Parse(CommonResources.MaxUsernameLength))
      {
        errorDescription = CommonResources.ErrorMsgInvalidUsernameLength;
        return false;
      }

      if (!Regex.IsMatch(username, CommonResources.UsernameValidationRegex))
      {
        errorDescription = CommonResources.ErrorMsgInvalidUsernameComposition;
        return false;
      }

      errorDescription = null;
      return true;
    }


    /// <summary>
    ///ANY CODE RUNNING ON THE SERVER SHOULD ONLY
    ///BE RUN WHEN THE CURRENT USER IS AUTHENTICATED
    ///(EXCEPT CODE INVOLVING THE AUTHENTICATION PROCESS ;)
    /// </summary>
    public static void CheckAuthentication()
    {
      if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
        throw new Common.Exceptions.UserNotAuthenticatedException();
    }

    public static bool CurrentUserIsAuthenticated()
    {
      return Csla.ApplicationContext.User.Identity.IsAuthenticated;
    }


    /// <summary>
    /// Waits for the given time in a background thread (executed with a Task)
    /// </summary>
    /// <returns>Task (async void)</returns>
    public static async Task WaitAsync(int timeToWaitInMs)
    {
      var waitTask = new Task(() => System.Threading.Thread.Sleep(timeToWaitInMs));
      waitTask.Start();
      await waitTask;
    }
  }
}
