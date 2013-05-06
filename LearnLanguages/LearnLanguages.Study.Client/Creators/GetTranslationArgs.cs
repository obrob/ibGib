//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using LearnLanguages.Business;

//namespace LearnLanguages.Study
//{
//  /// <summary>
//  /// sourceOrServiceName == "" means that the translation was gotten from our database.
//  /// Otherwise, this is the name of the source/service that did the translation.  E.g. "Bing", "Google",
//  /// "Bob_Bobbingtonsmithsonton".
//  /// </summary>
//  public class GetTranslationArgs : EventArgs
//  {
//    /// <summary>
//    /// sourceOrServiceName == "" means that the translation was gotten from our database.
//    /// Otherwise, this is the name of the source/service that did the translation.  E.g. "Bing", "Google",
//    /// "Bob_Bobbingtonsmithsonton".
//    /// </summary>
//    /// <param name="translation">the gotten translation</param>
//    /// <param name="sourceOrServiceName">the string name of the source/service who provided the translation.  "" (empty string) means that this translation was already in our database.</param>
//    public GetTranslationArgs(TranslationEdit translation, string sourceOrServiceName = "")
//    {
//      Translation = translation;
//      SourceOrServiceName = sourceOrServiceName;
//    }

//    public TranslationEdit Translation { get; private set; }
//    public string SourceOrServiceName { get; private set; }

//    /// <summary>
//    /// returns true if source/service name is empty, false if it is not.
//    /// </summary>
//    public bool TranslationGottenFromDatabase() { return SourceOrServiceName == ""; }
//  }
//}
