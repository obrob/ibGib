using System;
using System.Threading.Tasks;
using System.Linq;

using LearnLanguages.Business;
using LearnLanguages.Common.Interfaces;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using LearnLanguages.Common.Analysis;
using LearnLanguages.Common.Exceptions;
using LearnLanguages.Common;
using System.Threading;

namespace LearnLanguages.Analysis
{
  /// <summary>
  /// This analyzer gets data regarding the strengths of PhraseBeliefs.
  /// This creates 
  /// </summary>
  public class SimplePhraseBeliefAnalyzer : 
    AnalyzerBase<EstimatedKnownPhraseCountsAnalysis>
  {
    public SimplePhraseBeliefAnalyzer()
    {
      Id = Guid.Parse(AnalysisResources.IdSimplePhraseBeliefAnalyzer);
      _MinNumberOfBeliefsToInterpolateWordStrength = 
        int.Parse(AnalysisResources.MinNumberOfPhraseBeliefsToInterpolateStrength);

      _MinBeliefStrengthForKnowledge =
        double.Parse(AnalysisResources.MinimumStrengthThresholdForKnowledge);
    }

    protected override async Task<EstimatedKnownPhraseCountsAnalysis>
      GetAnalysisImpl(string analysisIdString = "", bool recordInDb = false)
    {
      if (analysisIdString == AnalysisResources.AnalysisIdEstimatedKnownPhraseCounts)
      {
        ///IF WE ALREADY HAVE AN ANALYSIS, THEN RETURN THAT. REFINE ANALYSIS
        ///IS RESPONSIBLE FOR UPDATING THIS ONCE THE ANALYSIS IS CREATED.
        if (_Analysis != null)
        {
          _Analysis = null;
          _Retriever = null;
          _UnrefinedPhraseBeliefs = null;
          AnalysisHasBeenBuilt = false;
          //return _Analysis;
        }

        ///OUR DATA WILL BE IN THE FORM OF A LIST OF KEYVALUEPAIRS
        ObservableCollection<KeyValuePair<string, int>> data =
          new ObservableCollection<KeyValuePair<string, int>>();

        ///GET THE MOST RECENT BELIEF FOR EACH AND EVERY PHRASE 
        ///FOR THE CURRENT USER. I AM STORING THIS IN A PROPERTY 
        ///FOR USE WITH THE REFINE ANALYSIS
        if (_Retriever == null)
          _Retriever = await MostRecentPhraseDataBeliefsRetriever.CreateNewAsync();

        var results = from belief in _Retriever.MostRecentPhraseBeliefs
                      group belief by belief.Phrase.Language.Text into languageGroup
                      orderby languageGroup.Key
                      select languageGroup;

        foreach (var group in results)
        {
          var languageText = group.Key;
          var totalPhrasesInLanguage = group.Count();
          var knownCount = 0;
          foreach (var belief in group)
          {
            ///IF THE STRENGTH MEETS OUR MINIMUM THRESHOLD FOR KNOWLEDGE
            ///THEN WE CONSIDER THE PHRASE TO BE KNOWN
            if (belief.Strength >= _MinBeliefStrengthForKnowledge)
            {
              knownCount++;
            }
          }

          ///WE NOW KNOW HOW MANY PHRASES WE KNOW FOR THIS LANGUAGE, SO 
          ///ADD IT TO THE DATA
          data.Add(new KeyValuePair<string, int>(languageText, knownCount));
        }

        ///AND CREATE OUR ANALYSIS
        _Analysis = new EstimatedKnownPhraseCountsAnalysis(data);
        AnalysisHasBeenBuilt = true;

        ///WE DONE MERN!
        return _Analysis;
      }

      ///IF WE'VE GOTTEN THIS FAR, THE CALLER IS ASKING FOR AN ANALYSIS
      ///THAT THIS ANALYZER DOES NOT RECOGNIZE
      throw new AnalysisIdNotRecognizedException(analysisIdString, 0);
    }

    /// <summary>
    /// Our initial analysis just gets all of our phrase beliefs and
    /// counts the beliefs above a certain threshold as "known". The 
    /// refinement process will break down the multi-word phrases
    /// into their individual words, and if those words don't already
    /// correspond to a phrasebelief text, we are going to interpolate
    /// (guess) if we know that individual word from the phrase(s) that
    /// contain it.
    /// 
    /// E.g. If we have a phrase "The dog ate the poo." and the associated
    /// phrase belief for this is 1.0, then it is safe to assume that 
    /// each individual word in this phrase has a belief of 1.0 as well.
    /// 
    /// A more complicated example would be if the phrase belief for this
    /// is 0.75. In which case, it is possible that the individual word
    /// is known or not known. So, we will look for another phrase. If 
    /// there are sufficient other phrases, then we can take an average
    /// of those phrase belief strengths, and adjust accordingly for any
    /// more concrete beliefs. Say there is another phrase "A dog is nice."
    /// and this phrase has a belief of 0.1 (unknown). Then it is safe to 
    /// assume that "dog" is not known at all, in which case this trumps
    /// the average.
    /// 
    /// Future analyzers can have even more complicated interpretations of 
    /// information...that is why this architecture exists!
    /// </summary>
    /// <param name="timeAllottedInMs">Estimated amount of time that will be allotted to this analyzer to do its refinement.</param>
    /// <returns></returns>
    protected override async Task RefineAnalysisImpl(int timeAllottedInMs)
    {
      ///RIGHT NOW, WE ONLY UNDERSTAND HOW TO HANDLE INDEFINITE TIME ALLOTTMENTS
      if (timeAllottedInMs > 0)
        throw new NotImplementedException("Only expect infinite time alotted for RefineAnalysis");
      
      ///BUILD UNREFINED PHRASES LIST, IF WE HAVEN'T ALREADY DONE SO
      if (_UnrefinedPhraseBeliefs == null)
        BuildUnrefinedPhraseBeliefs();

      ///IF OUR UNREFINED PHRASE BELIEFS IS EMPTY, THEN WE CAN
      ///REFINE NO FURTHER. SLEEP FOR A LITTLE BIT SO THIS ISN'T 
      ///CALLED IMMEDIATELY OVER AND OVER, DOING NOTHING.
      if (_UnrefinedPhraseBeliefs.Count == 0)
      {
        Thread.Sleep(int.Parse(AnalysisResources.TimeToSleepIfRefinedCompletelyInMs));
        return;
      }

      ///SET HOW MANY PHRASES ARE WE GOING TO DO IN THIS REFINEMENT
      var numPhrasesPerRefinement = 
        int.Parse(AnalysisResources.NumberOfPhrasesPerRefinement);

      ///GET THAT NUMBER OF PHRASE BELIEFS' INDIVIDUAL WORDS
      int phraseBeliefsAdded = 0;
      List<string> individualWords = new List<string>();
      for (int i = _UnrefinedPhraseBeliefs.Count - 1; i >= 0; i--)
      {
        ///GET THE PHRASE TEXT FOR THIS PHRASE BELIEF
        var phraseText = _UnrefinedPhraseBeliefs[i].Phrase.Text;

        ///GO AHEAD AND REMOVE THE PHRASE BELIEF FROM OUR LIST OF 
        ///UNREFINED PHRASE BELIEFS, BECAUSE WE ARE DEFINITELY
        ///PROCESSING IT, EVEN IF THERE IS AN EXCEPTION. WE WANT 
        ///IT OUT OF THIS LIST.
        _UnrefinedPhraseBeliefs.RemoveAt(i);

        ///PARSE THE TEXT INTO WORDS
        var words = phraseText.ParseIntoWords();

        ///IF THE PHRASETEXT IS ONLY A SINGLE WORD, THEN WE CAN'T
        ///BREAK THAT DOWN ANY FURTHER. SO KEEP LOOKING FOR 
        ///PHRASES TO REFINE.
        if (words.Count == 1)
          continue;
        
        individualWords.AddRange(words);

        ///INC THE PHRASE BELIEFS WE'VE DONE SO FAR. IF WE'VE DONE
        ///THE MAX FOR THIS REFINEMENT, THEN BREAK OUT OF THE LOOP.
        phraseBeliefsAdded++;
        if (phraseBeliefsAdded == numPhrasesPerRefinement)
          break;
      }
      
      ///STRIP DUPLICATES
      individualWords = individualWords.Distinct().ToList();

      ///CREATE INTERPOLATED PSEUDO-BELIEFS (GUESS AT THE BELIEF 
      ///STRENGTH) OF EACH INDIVIDUAL WORD, AND ADJUST OUR COUNT
      ///ACCORDINGLY.
      InterpolateWordsAndAdjustData(individualWords);
    }

    private void InterpolateWordsAndAdjustData(List<string> individualWords)
    {
      ///WE'RE GOING TO USE THESE ADJUSTMENTS AT THE END TO AUGMENT
      ///THE CURRENT ANALYSIS.DATA. STRING = LANGUAGETEXT. INT = NUMBER
      ///OF WORDS TO ADD TO THE KNOWN COUNT
      var adjustments = new Dictionary<string, int>();

      for (int i = 0; i < individualWords.Count; i++)
      {
        var word = individualWords[i];
        var allBeliefs = _Retriever.MostRecentPhraseBeliefs;

        #region CHECK IF THE WORD IS A PHRASE BELIEF ITSELF

        ///FIRST CHECK TO SEE IF WE ALREADY HAVE A PHRASEBELIEF 
        ///CORRESPONDING TO THIS WORD. IF WE DO, THEN WE CAN
        ///CONTINUE TO THE NEXT INDIVIDUAL WORD.
        var alreadyHaveBelief = (from b1 in allBeliefs
                                 where b1.Phrase.Text.ToLower() == word.ToLower()
                                 select b1).Count() > 0;
        if (alreadyHaveBelief)
          continue;

        #endregion

        #region CHECK IF THE WORD IS FULLY KNOWN

        ///IF THE WORD IS IN A PHRASE THAT IS FULLY KNOWN, THEN
        ///WE ASSUME THE WORD ITSELF IS FULLY KNOWN
        var fullKnownThreshold = double.Parse(AnalysisResources.BeliefStrengthKnownAll);
        var fullyKnownBeliefsWithWord = from b2 in allBeliefs
                                        where (b2.Phrase.Text).ToLower().Contains(word.ToLower()) &&
                                              b2.Strength >= fullKnownThreshold
                                        select b2;
        var wordIsInFullyKnownBelief = fullyKnownBeliefsWithWord.Count() > 0;

        if (wordIsInFullyKnownBelief)
        {
          var languageTextOfWord =
            fullyKnownBeliefsWithWord.First().Phrase.Language.Text;
          AddToKnownAdjustments(adjustments, languageTextOfWord);

          continue;
        }

        #endregion

        #region CHECK IF THE WORD IS NOT KNOWN AT ALL

        ///IF THE WORD IS IN A PHRASE THAT IS COMPLETELY UNKNOWN,
        ///THEN WE ASSUME THE WORD ITSELF IS UNKNOWN
        var fullUnknownThreshold = double.Parse(AnalysisResources.BeliefStrengthKnownNone);
        var fullyUnknownBeliefsWithWord = from b3 in allBeliefs
                                          where b3.Text.Contains(word) &&
                                                b3.Strength <= fullUnknownThreshold
                                          select b3;
        var wordIsInFullyUnknownBeliefs = fullyUnknownBeliefsWithWord.Count() > 0;

        if (wordIsInFullyUnknownBeliefs)
        {
          ///IF THE WORD IS FULLY UNKNOWN, THEN WE MAKE NO 
          ///ADJUSTMENT AND CONTINUE TO THE NEXT WORD
          continue;
        }

        #endregion

        #region GET INTERPOLATED SCORE FOR WORD

        var allBeliefsContainingWord = from b in allBeliefs
                                       where (b.Phrase.Text).ToLower() == word.ToLower()
                                       select b;

        ///WE NEED AT LEAST THIS NUMBER OF BELIEFS CONTAINING THE WORD 
        ///TO INTERPOLATE THE INDIVIDUAL WORD'S STRENGTH
        var minNumberBeliefs = _MinNumberOfBeliefsToInterpolateWordStrength;
        if (allBeliefsContainingWord.Count() < minNumberBeliefs)
          continue;

        ///GET THE AVERAGE BELIEF STRENGTH OF PHRASES THAT CONTAIN THIS WORD
        var totalStrength = 0.0d;
        foreach (var belief in allBeliefsContainingWord)
          totalStrength += belief.Strength;
        var avgBeliefStrength = totalStrength / ((double)allBeliefsContainingWord.Count());
        
        ///IF THE AVERAGE BELIEF STRENGTH IS AT OR ABOVE OUR
        ///MIN THRESHOLD TO CONSIDER THE WORD TO BE "KNOWN", 
        ///THEN ADD THE WORD TO THE ADJUSTMENTS
        if (avgBeliefStrength >= _MinBeliefStrengthForKnowledge)
        {
          var languageTextOfWord = allBeliefsContainingWord.First().Phrase.Language.Text;
          AddToKnownAdjustments(adjustments, languageTextOfWord);
        }

        #endregion

      } 

      #region APPLY OUR ADJUSTMENTS TO ANALYSIS.DATA
      System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
          {
            foreach (var adjustment in adjustments)
            {
              ///LIST OUR KEY/VALUES IN TERMS WE UNDERSTAND
              var languageText = adjustment.Key;
              var additionalKnownCount = adjustment.Value;

              ///GET THE RELEVANT KEY/VALUE PAIR IN THE ANALYSIS.DATA
              var languageKnownCountPair = (from pair in _Analysis.Data
                                            where pair.Key == languageText
                                            select pair).First();

              ///CALCULATE THE ADJUSTED KNOWN COUNT
              var originalKnownCount = languageKnownCountPair.Value;
              var adjustedKnownCount = originalKnownCount + additionalKnownCount;


              ///REPLACE OUR ANALYSIS.DATA
              _Analysis.Data.Remove(languageKnownCountPair);
              _Analysis.Data.Add(
                new KeyValuePair<string, int>(languageText, adjustedKnownCount));
            }
          });

      #endregion
    }

    private static void AddToKnownAdjustments(Dictionary<string, int> adjustments, 
      string languageTextOfWord)
    {
      ///ADD THE WORD TO OUR ADJUSTMENTS
      if (adjustments.ContainsKey(languageTextOfWord))
      {
        adjustments[languageTextOfWord] = adjustments[languageTextOfWord] + 1;
      }
      else
      {
        ///THIS IS OUR FIRST ADJUSTMENT, SO IT'S JUST ONE
        adjustments[languageTextOfWord] = 1;
      }
    }

    private void BuildUnrefinedPhraseBeliefs()
    {
      if (_Retriever != null && _Retriever.MostRecentPhraseBeliefs != null)
      {
        _UnrefinedPhraseBeliefs =
          new List<PhraseBeliefEdit>(_Retriever.MostRecentPhraseBeliefs);
      }
    }

    private MostRecentPhraseDataBeliefsRetriever _Retriever { get; set; }

    #region private EstimatedKnownPhraseCountsAnalysis _Analysis

    private object ___AnalysisLock = new object();
    private EstimatedKnownPhraseCountsAnalysis __Analysis;
    private EstimatedKnownPhraseCountsAnalysis _Analysis
    {
      get
      {
        lock (___AnalysisLock)
        {
          return __Analysis;
        }
      }
      set
      {
        lock (___AnalysisLock)
        {
          __Analysis = value;
        }
      }
    }

    #endregion

    //private EstimatedKnownPhraseCountsAnalysis _Analysis { get; set; }

    private List<PhraseBeliefEdit> _UnrefinedPhraseBeliefs { get; set; }

    private double _MinBeliefStrengthForKnowledge { get; set; }
    private int _MinNumberOfBeliefsToInterpolateWordStrength { get; set; }
  }
}
