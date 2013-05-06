using Csla;
using Csla.Core;
using Csla.Serialization;
using System;
using System.Collections.Generic;

namespace LearnLanguages.Business.Criteria
{
  [Serializable]
  public class PhraseTextLanguageTextPairsCriteria : 
    CriteriaBase<PhraseTextLanguageTextPairsCriteria>
  {
    
    #region Ctors

    public PhraseTextLanguageTextPairsCriteria()
    {
      PhraseTextLanguageTextPairs = new MobileList<Tuple<string, string>>();
    }

    public PhraseTextLanguageTextPairsCriteria(
      MobileList<Tuple<string, string>> phraseTextLanguageTextPairs)
    {
      if (phraseTextLanguageTextPairs == null)
        throw new ArgumentNullException("phraseTextLanguageTextPairs");

      PhraseTextLanguageTextPairs = phraseTextLanguageTextPairs;
    }

    public PhraseTextLanguageTextPairsCriteria(
      params Tuple<string, string>[] phraseTextLanguageTextPairs )
    {
      if (phraseTextLanguageTextPairs == null)
        throw new ArgumentNullException("phraseTextLanguageTextPairs");

      PhraseTextLanguageTextPairs = new MobileList<Tuple<string, string>>(phraseTextLanguageTextPairs);
    }

    public PhraseTextLanguageTextPairsCriteria(string phraseTextA, string languageTextA, 
                                               string phraseTextB, string languageTextB)
    {
      var tupleA = new Tuple<string, string>();
      tupleA.Item1 = phraseTextA;
      tupleA.Item2 = languageTextA;
      var tupleB = new Tuple<string, string>();
      tupleB.Item1 = phraseTextB;
      tupleB.Item2 = languageTextB;
      PhraseTextLanguageTextPairs = new MobileList<Tuple<string, string>>()
      {
        tupleA, tupleB
      };
    }

    #endregion

    #region PhraseTextLanguageTextPairs

    public static readonly PropertyInfo<MobileList<Tuple<string, string>>> 
      PhraseTextLanguageTextPairsProperty =
        RegisterProperty<MobileList<Tuple<string, string>>>(c => c.PhraseTextLanguageTextPairs);
    public MobileList<Tuple<string, string>> PhraseTextLanguageTextPairs
    {
      get { return ReadProperty(PhraseTextLanguageTextPairsProperty); }
      set { LoadProperty(PhraseTextLanguageTextPairsProperty, value); }
    }

    #endregion

  }
}
