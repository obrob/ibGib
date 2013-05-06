
using System;
using System.Net;
using Csla.Serialization;
using Csla;

namespace LearnLanguages.Business.Criteria
{
  [Serializable]
  public class UserInfoCriteria : CriteriaBase<UserInfoCriteria>
  {
    /// <summary>
    /// DON'T USE THIS CTOR.  THIS IS A READ ONLY CRITERIA CLASS.  THIS CTOR IS ONLY HERE
    /// BECAUSE SERIALIZATION REQUIRES IT.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public UserInfoCriteria()
    {
      //required for serialization
    }
    
    public UserInfoCriteria(string username, string clearUnsaltedPassword)
    {
      LoadProperty<string>(UsernameProperty, username);
      LoadProperty<string>(ClearUnsaltedPasswordProperty, clearUnsaltedPassword);
      //LoadProperty<string>(SaltedHashedPasswordProperty, saltedHashedPassword);
      //LoadProperty<int>(SaltProperty, salt);
    }

    public UserInfoCriteria(string username)
    {
      LoadProperty<string>(UsernameProperty, username);
      //LoadProperty<string>(ClearUnsaltedPasswordProperty, clearUnsaltedPassword);
      //LoadProperty<string>(SaltedHashedPasswordProperty, saltedHashedPassword);
      //LoadProperty<int>(SaltProperty, salt);
    }


    public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(c => c.Username);
    public string Username
    {
      get { return ReadProperty(UsernameProperty); }
      private set { LoadProperty(UsernameProperty, value); }
    }

    public static readonly PropertyInfo<string> ClearUnsaltedPasswordProperty = 
      RegisterProperty<string>(c => c.ClearUnsaltedPassword);
    public string ClearUnsaltedPassword
    {
      get { return ReadProperty(ClearUnsaltedPasswordProperty); }
      private set { LoadProperty(ClearUnsaltedPasswordProperty, value); }
    }

    //public static readonly PropertyInfo<string> SaltedHashedPasswordProperty =
    //  RegisterProperty<string>(c => c.SaltedHashedPassword);
    //public string SaltedHashedPassword
    //{
    //  get { return ReadProperty(SaltedHashedPasswordProperty); }
    //  private set { LoadProperty(SaltedHashedPasswordProperty, value); }
    //}

    //public static readonly PropertyInfo<int> SaltProperty =
    //  RegisterProperty<int>(c => c.Salt);
    //public int Salt
    //{
    //  get { return ReadProperty(SaltProperty); }
    //  private set { LoadProperty(SaltProperty, value); }
    //}
  }
}
