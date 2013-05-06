
using System;
using System.Net;
using Csla.Serialization;
using Csla;

namespace LearnLanguages.Business.Criteria
{
  [Serializable]
  public class PhraseCriteria : CriteriaBase<PhraseCriteria>
  {
    /// <summary>
    /// DON'T USE THIS CTOR.  THIS IS A READ ONLY CRITERIA CLASS.  THIS CTOR IS ONLY HERE
    /// BECAUSE SERIALIZATION REQUIRES IT.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public PhraseCriteria()
    {
      //required for serialization
    }
    public PhraseCriteria(PhraseEdit phrase)
    {
      Phrase = phrase;
    }

    public static readonly PropertyInfo<PhraseEdit> PhraseProperty = RegisterProperty<PhraseEdit>(c => c.Phrase);
    public PhraseEdit Phrase
    {
      get { return ReadProperty(PhraseProperty); }
      private set { LoadProperty(PhraseProperty, value); }
    }
  }
}
