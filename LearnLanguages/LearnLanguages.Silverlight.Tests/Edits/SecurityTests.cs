using System;

using LearnLanguages.Business.Security;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using System.Threading.Tasks;


namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("security")]
  public class SecurityTests : TestsBase
  {
    private string _TestValidUsernameAdmin = "user";
    //private string _TestRoleAdmin = "Admin";
    private string _TestRoleUser = "User";
    private string _TestValidPasswordAdmin = "password";
    //private SaltedHashedPassword _TestSaltedHashedPassword;
    //private string _TestSaltedHashedPassword = @"瞌訖ꎚ壿喐ຯ缟㕧";
    //private int _TestSalt = -54623530;

    private string _TestInvalidUsername = "ImNotAValidUser";
    private string _TestInvalidPassword = "ImNotAValidPassword";

    [TestMethod]
    [Asynchronous]
    public async Task TEST_LOGIN_ASYNC_SUCCESS_VALID_USERNAME_VALID_PASSWORD_LOGOUT()
    {
      var asyncIsCompleted = false;
      EnqueueConditional(() => asyncIsCompleted);
      try
      {
        await UserPrincipal.LoginAsync(_TestValidUsernameAdmin, _TestValidPasswordAdmin);
      }
      finally
      {
        asyncIsCompleted = true;
        EnqueueCallback(() => Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(_TestRoleUser)),
                      () => Assert.AreEqual(_TestValidUsernameAdmin, Csla.ApplicationContext.User.Identity.Name),
                      () => Assert.IsTrue(Csla.ApplicationContext.User.Identity.IsAuthenticated),
                      () => UserPrincipal.Logout()
                      );
        EnqueueTestComplete();
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task TEST_LOGIN_ASYNC_FAIL_INVALID_USERNAME_INVALID_PASSWORD()
    {
      var asyncIsCompleted = false;
      EnqueueConditional(() => asyncIsCompleted);
      try
      {
        await UserPrincipal.LoginAsync(_TestInvalidUsername, _TestInvalidPassword);
      }
      finally
      {
        asyncIsCompleted = true;
        EnqueueCallback(() => Assert.IsFalse(Csla.ApplicationContext.User.IsInRole(_TestRoleUser)),
                        () => Assert.AreNotEqual(_TestValidUsernameAdmin, 
                                                 Csla.ApplicationContext.User.Identity.Name),
                        () => Assert.IsFalse(Csla.ApplicationContext.User.Identity.IsAuthenticated)
                        );
        EnqueueTestComplete();
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task TEST_LOGIN_ASYNC_FAIL_VALID_USERNAME_INVALID_PASSWORD()
    {
      var asyncIsCompleted = false;
      EnqueueConditional(() => asyncIsCompleted);
      try
      {
        await UserPrincipal.LoginAsync(_TestValidUsernameAdmin, _TestInvalidPassword);
      }
      finally
      {
        asyncIsCompleted = true;
        EnqueueCallback(() => Assert.IsFalse(Csla.ApplicationContext.User.IsInRole(_TestRoleUser)),
                        () => Assert.AreNotEqual(_TestValidUsernameAdmin, 
                                                 Csla.ApplicationContext.User.Identity.Name),
                        () => Assert.IsFalse(Csla.ApplicationContext.User.Identity.IsAuthenticated)
                        );
        EnqueueTestComplete();
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task TEST_LOGIN_ASYNC_FAIL_INVALID_USERNAME_VALID_PASSWORD()
    {
      var asyncIsCompleted = false;
      EnqueueConditional(() => asyncIsCompleted);
      try
      {
        await UserPrincipal.LoginAsync(_TestInvalidUsername, _TestValidPasswordAdmin);
      }
      finally
      {
        asyncIsCompleted = true;
        EnqueueCallback(() => Assert.IsFalse(Csla.ApplicationContext.User.IsInRole(_TestRoleUser)),
                        () => Assert.AreNotEqual(_TestValidUsernameAdmin, 
                                                 Csla.ApplicationContext.User.Identity.Name),
                        () => Assert.IsFalse(Csla.ApplicationContext.User.Identity.IsAuthenticated)
                        );
        EnqueueTestComplete();
      }
    }

    [TestMethod]
    [Asynchronous]
    public async Task TEST_LOGIN_ADMIN_ADD_USER_DELETE_USER_LOGOUT()
    {

      var newUserLoginWasSuccessful = false;
      //var newUserIsInUserRole = false;
      //ASSUME DELETED USER LOGIN IS SUCCESSFUL
      var deletedUserLoginWasSuccessful = true;

      Business.NewUserCreator creator = null;
      Business.DeleteUserReadOnly deleter = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        //LOGIN AS THE ADMIN
        await UserPrincipal.LoginAsync(_TestValidUsernameAdmin, _TestValidPasswordAdmin);

        //CREATE THE USER
        var testNewUsername = "user929283";
        var testNewUserPassword = "password223jfkj";
        var criteria = new Csla.Security.UsernameCriteria(testNewUsername, testNewUserPassword);
        creator = await Business.NewUserCreator.CreateNewAsync(criteria);

        //LOGOUT ADMIN
        UserPrincipal.Logout();

        //LOGIN AS THE NEW USER
        await UserPrincipal.LoginAsync(testNewUsername, testNewUserPassword);

        //CONFIRM LOGIN
        newUserLoginWasSuccessful = Common.CommonHelper.CurrentUserIsAuthenticated();

        //LOGOUT NEW USER
        UserPrincipal.Logout();
        
        //LOG BACK IN AS ADMIN
        await UserPrincipal.LoginAsync(_TestValidUsernameAdmin, _TestValidPasswordAdmin);

        //DELETE THE USER
        deleter = await Business.DeleteUserReadOnly.CreateNewAsync(testNewUsername);

        //LOGOUT ADMIN AGAIN
        UserPrincipal.Logout();

        //TRY TO LOG THE DELETED USER BACK IN, BUT SHOULD FAIL (BUT NOT THROW EXCEPTION)
        await UserPrincipal.LoginAsync(testNewUsername, testNewUserPassword);

        //THIS SHOULD BE FALSE.
        deletedUserLoginWasSuccessful = Common.CommonHelper.CurrentUserIsAuthenticated();

        EnqueueTestComplete();
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        var adminRole = DataAccess.SeedData.Ton.AdminRoleText;
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(adminRole)),
                        () => Assert.IsTrue(creator.WasSuccessful),
                        () => Assert.IsTrue(deleter.WasSuccessful),
                        () => Assert.IsFalse(deletedUserLoginWasSuccessful),
                        () => Assert.IsFalse(Common.CommonHelper.CurrentUserIsAuthenticated())
                        );

        EnqueueTestComplete();
        Teardown();
        isAsyncComplete = true;
      }
    }
    [TestMethod]
    [Asynchronous]
    public async Task TEST_ADD_20_RANDOM_USERS_RANDOM_PASSWORDS_MUST_CLEAN_SOLUTION_FIRST()
    {
      int numToDo = 20;
      int maxAttempts = 50;
      var creationAttempts = 0;
      var creationSuccesses = 0;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        for (int i = 0; i < numToDo; i++)
        {
          string randomUsername = GenerateRandomUsername();
          string randomPassword = GenerateRandomPassword();
          var criteria = new Csla.Security.UsernameCriteria(randomUsername, randomPassword);

          try
          {
            creationAttempts++;
            var creator = await Business.NewUserCreator.CreateNewAsync(criteria);
            creationSuccesses++;
          }
          catch (Exception)
          {
            //Don't care about errors.
            //we just don't increment creationSuccesses
          }
        }
      }
      catch
      {
        hasError = true;
      }
      finally
      {
        EnqueueConditional(() => (creationSuccesses == numToDo) || (maxAttempts == creationAttempts));

        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(_TestRoleUser)),
                        () => Assert.IsTrue(creationAttempts < maxAttempts),
                        () => Assert.AreEqual(numToDo, creationSuccesses),
                        () => Teardown()
                       );

        EnqueueTestComplete();
        
        isAsyncComplete = true;
      }
    }
    
    [TestMethod]
    [Asynchronous]
    public async Task TEST_DELETE_USER_THAT_DOESNT_EXIST()
    {
      string randomUsername = GenerateRandomUsername();
      Business.DeleteUserReadOnly deleter = null;

      var isAsyncComplete = false;
      var hasError = false;
      EnqueueConditional(() => isAsyncComplete);
      await Setup();
      try
      {
        deleter = await Business.DeleteUserReadOnly.CreateNewAsync(randomUsername);
      }
      catch (Csla.DataPortalException dpex)
      {
        //WE EXPECT AN EXCEPTION OF TYPE USER NOT FOUND. OTHERWISE,
        //WE NEED TO RETHROW THE EXCEPTION.
        if (!TestsHelper.IsUserNotFoundException(dpex))
          throw dpex;
      }
      catch
      {
        //HAS ERROR REFERS TO THE METHOD HAS A PROBLEM.
        //IT DOES NOT MEAN THAT WE HAVE AN EXCEPTION, BECAUSE
        //WE ARE INDEED EXPECTING AN EXCEPTION.
        hasError = true;
      }
      finally
      {
        EnqueueCallback(
                        () => Assert.IsFalse(hasError),
                        () => Assert.IsNull(deleter),
                        () => Teardown()
                        );

        EnqueueTestComplete();
        
        isAsyncComplete = true;
      }
    }

    #region Helpers

    private string GenerateRandomPassword()
    {
      int minValidPasswordLength = 6;
      int maxValidPasswordLength = 15;
      string validPasswordChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+";
      string randomPassword = "";
      bool randomPasswordIsValid = false;
      do
      {
        randomPassword =
          GenerateRandomString(validPasswordChars, minValidPasswordLength, maxValidPasswordLength);
        //randomPasswordIsValid = Regex.IsMatch(randomPassword, CommonResources.PasswordValidationRegex);
        randomPasswordIsValid = Common.CommonHelper.PasswordIsValid(randomPassword);
      } while (!randomPasswordIsValid);

      return randomPassword;
    }

    #region private List<string> _usernames

    private object ___usernamesLock = new object();
    private List<string> __usernames = new List<string>();
    private List<string> _usernames
    {
      get
      {
        lock (___usernamesLock)
        {
          var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
          return __usernames;
        }
      }
      set
      {
        lock (___usernamesLock)
        {
          __usernames = value;
        }
      }
    }

    #endregion

    private string GenerateRandomUsername()
    {
      int minValidUsernameLength = 3;
      int maxValidUsernameLength = 30;
      string validUsernameChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";
      string randomUsername = "";
      bool randomUsernameIsValid = false;

      do
      {
        randomUsername =
          GenerateRandomString(validUsernameChars, minValidUsernameLength, maxValidUsernameLength);
        //randomUsernameIsValid = Regex.IsMatch(randomUsername, CommonResources.UsernameValidationRegex);
        randomUsernameIsValid = Common.CommonHelper.UsernameIsValid(randomUsername);
        if (randomUsernameIsValid)
        {
          var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
          if (_usernames.Contains(randomUsername))
            randomUsernameIsValid = false;
        }
      } while (!randomUsernameIsValid);

      _usernames.Add(randomUsername);
      return randomUsername;
    }

    private static string GenerateRandomString(string validChars, int minLength, int maxLength)
    {
      System.Threading.Thread.Sleep(20); //for randomizer
      Random r = new Random(DateTime.Now.Millisecond * DateTime.Now.Second + DateTime.Now.Minute);
      int length = r.Next(minLength, maxLength + 1);
      string generatedString = "";
      for (int i = 0; i < length; i++)
      {
        int randomCharIndex = r.Next(0, validChars.Length);
        char randomChar = validChars[randomCharIndex];
        generatedString += randomChar;
      }

      return generatedString;
    }

    #endregion
  }
}
