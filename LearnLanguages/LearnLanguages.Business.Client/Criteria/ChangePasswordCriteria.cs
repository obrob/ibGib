
using System;
using System.Net;
using Csla.Serialization;
using Csla;

namespace LearnLanguages.Business.Criteria
{
  [Serializable]
  public class ChangePasswordCriteria : CriteriaBase<ChangePasswordCriteria>
  {
    /// <summary>
    /// DON'T USE THIS CTOR.  THIS IS A READ ONLY CRITERIA CLASS.  THIS CTOR IS ONLY HERE
    /// BECAUSE SERIALIZATION REQUIRES IT.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public ChangePasswordCriteria()
    {
      //required for serialization
    }
    
    public ChangePasswordCriteria(string oldClearUnsaltedPassword, string newClearUnsaltedPassword)
    {
      LoadProperty<string>(OldPasswordProperty, oldClearUnsaltedPassword);
      LoadProperty<string>(NewPasswordProperty, newClearUnsaltedPassword);
    }

    public static readonly PropertyInfo<string> OldPasswordProperty = RegisterProperty<string>(c => c.OldPassword);
    public string OldPassword
    {
      get { return ReadProperty(OldPasswordProperty); }
      private set { LoadProperty(OldPasswordProperty, value); }
    }

    public static readonly PropertyInfo<string> NewPasswordProperty = 
      RegisterProperty<string>(c => c.NewPassword);
    public string NewPassword
    {
      get { return ReadProperty(NewPasswordProperty); }
      private set { LoadProperty(NewPasswordProperty, value); }
    }

  }
}
