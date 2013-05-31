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
      lines = lines.Where((l) => !string.IsNullOrWhiteSpace(l)).ToList();
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

    public static LogCategory ToLogCategory(this MessageType msgType)
    {
      LogCategory retCategory = default(LogCategory);
      switch (msgType)
      {
        case MessageType.Information:
          retCategory = LogCategory.Information;
          break;
        case MessageType.Warning:
          retCategory = LogCategory.Warning;
          break;
        case MessageType.Error:
          retCategory = LogCategory.Exception;
          break;
        case MessageType.Other:
          retCategory = LogCategory.Other;
          break;
        default:
          throw new NotImplementedException("msgType enum doesn't convert to LogCategory");
          break;
      }
      return retCategory;
    }

    public static LogPriority ToLogPriority(this MessagePriority msgPriority)
    {
      LogPriority retPriority = default(LogPriority);
      switch (msgPriority)
      {
        case MessagePriority.VeryLow:
          retPriority = LogPriority.VeryLow;
          break;
        case MessagePriority.Low:
          retPriority = LogPriority.Low;
          break;
        case MessagePriority.Medium:
          retPriority = LogPriority.Medium;
          break;
        case MessagePriority.High:
          retPriority = LogPriority.High;
          break;
        case MessagePriority.VeryHigh:
          retPriority = LogPriority.VeryHigh;
          break;
        default:
          throw new NotImplementedException("msgPriority enum doesn't convert to LogPriority");
      }
      return retPriority;
    }
  }
}
