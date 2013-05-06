
using System;
using System.Net;
using Csla.Serialization;
using Csla;
using System.Collections.Generic;
using Csla.Core;

namespace LearnLanguages.Business.Criteria
{
  /// <summary>
  /// This is NOT the same as PhraseList.  This is just a List[PhraseEdit] criteria object.  Tried to make this
  /// just an ICollection, but I need indexing.
  /// </summary>
  [Serializable]
  public class ListOfPhrasesCriteria : CriteriaBase<ListOfPhrasesCriteria>
  {
    public ListOfPhrasesCriteria()
    {
      Phrases = new MobileList<PhraseEdit>();
    }
    public ListOfPhrasesCriteria(params PhraseEdit[] phrases)
    {
      Phrases = new MobileList<PhraseEdit>(phrases);
    }

    public ListOfPhrasesCriteria(IList<PhraseEdit> phrases)
    {
      Phrases = new MobileList<PhraseEdit>(phrases);
    }

    public static readonly PropertyInfo<IList<PhraseEdit>> PhrasesProperty = 
      RegisterProperty<IList<PhraseEdit>>(c => c.Phrases);
    public IList<PhraseEdit> Phrases
    {
      get { return ReadProperty(PhrasesProperty); }
      private set { LoadProperty(PhrasesProperty, value); }
    }
  }
}
