namespace LearnLanguages.Study
{
  /// <summary>
  /// Includes methods for encoding/parsing BeliefText query strings.
  /// </summary>
  public static class BeliefHelper
  {
    public static string GetInitialEmptyBeliefText()
    {
      return StudyResources.BeliefTextPrefix;
    }
    public static string AppendKeyValueEntryToBeliefText(string beliefText, 
      string key, string value)
    {
      //IF THE BELIEF TEXT IS GREATER THAN 1, THEN IT ALREADY HAS AT LEAST
      //ONE VALUE IN IT, SO WE FIRST APPEND THE VALUE SEPARATOR
      if (beliefText.Length > StudyResources.BeliefTextPrefix.Length)
        beliefText += StudyResources.BeliefTextKeyValueEntrySeparator;
      //"key=value"
      var keyValueText = key +
                         StudyResources.BeliefTextKeyValueEqualityOperator +
                         value;
      beliefText += keyValueText;
      return beliefText;
    }
    public static string AppendBoolValueToBeliefText(string beliefText, string key, bool value)
    {
      return AppendKeyValueEntryToBeliefText(beliefText, key, value.ToString());
    }
    public static string AppendIntValueToBeliefText(string beliefText, string key, int value)
    {
      return AppendKeyValueEntryToBeliefText(beliefText, key, value.ToString());
    }
    public static string GetValueString(string beliefText, string key)
    {
      var indexOfStartOfKey =
        beliefText.IndexOf(key);// + StudyResources.BeliefTextKeyValueEqualityOperator);
      var indexOfEqualityOperator = indexOfStartOfKey + key.Length;
      var indexOfStartOfValue = indexOfEqualityOperator +
                                StudyResources.BeliefTextKeyValueEqualityOperator.Length;
      var lastIndexOfEqualityOperator =
        beliefText.LastIndexOf(StudyResources.BeliefTextKeyValueEqualityOperator);
      int indexOfLastCharacterInValue = -1;

      //IF THE INDEXOFEQUALITYOPERATOR == LASTINDEXOFEQUALITYOPERATOR, THEN WE
      //ARE ACCESSING THE LAST VALUE IN THE STRING.
      if (indexOfEqualityOperator == lastIndexOfEqualityOperator)
      {
        //VALUE IS LAST IN STRING
        indexOfLastCharacterInValue = beliefText.Length - 1;
      }
      else
      {
        //THERE IS ANOTHER VALUE AFTER THE STRING
        var indexOfNextKeyValueSeparator =
          beliefText.IndexOf(StudyResources.BeliefTextKeyValueEntrySeparator,
                             indexOfStartOfKey);
        indexOfLastCharacterInValue = indexOfNextKeyValueSeparator - 1;
      }

      var valueLength = indexOfLastCharacterInValue - indexOfStartOfValue + 1;
      var valueString = beliefText.Substring(indexOfStartOfValue, valueLength);
      return valueString;
    }
    
    public static string AppendDurationInMs(string beliefText, int durationInMs)
    {
      var key = History.HistoryResources.Key_DurationInMilliseconds;
      var value = durationInMs;
      var newBeliefText = AppendIntValueToBeliefText(beliefText, key, value);
      return newBeliefText;
    }
    public static string AppendPhraseText(string beliefText, string phraseText)
    {
      var key = History.HistoryResources.Key_PhraseText;
      return AppendKeyValueEntryToBeliefText(beliefText, key, phraseText);
    }
    public static string AppendLanguageText(string beliefText, string languageText)
    {
      var key = History.HistoryResources.Key_LanguageText;
      return AppendKeyValueEntryToBeliefText(beliefText, key, languageText);
    }
    public static string AppendPhraseWasModifiedDuringReview(string beliefText, bool wasModified = true)
    {
      var key = History.HistoryResources.Key_PhraseWasModifiedDuringReview;
      return AppendBoolValueToBeliefText(beliefText, key, wasModified);
    }
    public static int GetDurationInMs(string beliefText)
    {
      var key = History.HistoryResources.Key_DurationInMilliseconds;
      var valueString = GetValueString(beliefText, key);
      var durationInMs = int.Parse(valueString);
      return durationInMs;
    }
    public static string GetPhraseText(string beliefText)
    {
      var key = History.HistoryResources.Key_PhraseText;
      var phraseText = GetValueString(beliefText, key);
      return phraseText;
    }
    public static string GetLanguageText(string beliefText)
    {
      var key = History.HistoryResources.Key_LanguageText;
      return GetValueString(beliefText, key);
    }
  }
}
