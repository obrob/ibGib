using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LearnLanguages.Common
{
  public static class LearnLanguagesExtensions
  {
    public static List<string> ParseIntoLines(this string str, bool removeEmptyEntries = true)
    {
      if (string.IsNullOrEmpty(str))
        return new List<string>();

      var lineDelimiter = CommonResources.LineDelimiter;
      lineDelimiter = lineDelimiter.Replace("\\r", "\r");
      lineDelimiter = lineDelimiter.Replace("\\n", "\n");
      var lines = new List<string>(str.Split(new string[] { lineDelimiter }, StringSplitOptions.RemoveEmptyEntries));
      return lines;
    }

    public static List<string> ParseIntoWords(this string str)
    {
      var delimiterString = CommonResources.SplitWordsDelimiterString;
      delimiterString = delimiterString.Replace("\\r", "\r");
      delimiterString = delimiterString.Replace("\\n", "\n");

      var delimiters = delimiterString.ToCharArray();
      var words = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
      return words;
    }

    public static int CountWords(this string str, bool countDuplicates = true)
    {
      var words = str.ParseIntoWords();
      if (countDuplicates)
        return words.Count;
      else
      {
        return words.Distinct().Count();
      }
    }
  }
}
