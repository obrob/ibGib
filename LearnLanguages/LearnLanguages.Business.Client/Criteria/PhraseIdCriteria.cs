
using System;
using System.Net;
using Csla.Serialization;
using Csla;

namespace LearnLanguages.Business.Criteria
{
  [Serializable]
  public class PhraseIdCriteria : CriteriaBase<PhraseIdCriteria>
  {
    /// <summary>
    /// DON'T USE THIS CTOR.  THIS IS A READ ONLY CRITERIA CLASS.  THIS CTOR IS ONLY HERE
    /// BECAUSE SERIALIZATION REQUIRES IT.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public PhraseIdCriteria()
    {
      //required for serialization
    }
    public PhraseIdCriteria(Guid phraseId)
    {
      PhraseId = phraseId;
    }

    public static readonly PropertyInfo<Guid> PhraseIdProperty = RegisterProperty<Guid>(c => c.PhraseId);
    public Guid PhraseId
    {
      get { return ReadProperty(PhraseIdProperty); }
      private set { LoadProperty(PhraseIdProperty, value); }
    }
  }
}
