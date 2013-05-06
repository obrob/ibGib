using LearnLanguages.Common.Interfaces.Translation;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace LearnLanguages.Common.Translation
{
  /// <summary>
  /// Singleton class that sets up a facade for translation service calls.
  /// For now, I'm going to set up an IBasicTranslator interface that this class
  /// will talk against. But in the future, this class could add IAdvancedTranslator
  /// capabilities, e.g.
  /// </summary>
  public class Translator
  {
    #region Singleton

    private static volatile Translator _Instance;
    private static object syncRoot = new Object();

    private Translator() { }

    public static Translator Ton
    {
      get
      {
        if (_Instance == null)
        {
          lock (syncRoot)
          {
            if (_Instance == null)
              _Instance = new Translator();
          }
        }

        return _Instance;
      }
    }

    #endregion

    [Import(typeof(IBasicTranslator))]
    public IBasicTranslator Basic { get; private set; }
  }
}
