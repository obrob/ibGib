using System;
using System.ComponentModel.Composition;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;
using LearnLanguages.Common.Interfaces;
using Caliburn.Micro;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Navigation.EventMessages;
using System.Threading.Tasks;
using LearnLanguages.Common;

namespace LearnLanguages.Study
{

  public class DefaultMultiLineTextsStudier : StudierBase<MultiLineTextList>, //:
                                              IHandle<NavigationRequestedEventMessage>
  {
    public DefaultMultiLineTextsStudier()
    {
      Services.EventAggregator.Subscribe(this);//navigation
    }
    public override async Task InitializeForNewStudySessionAsync(MultiLineTextList target)
    {
      _AbortIsFlagged = false;
      _Target = target;
      _MeaningStudier = new DefaultMultiLineTextsMeaningStudier();
      await _MeaningStudier.InitializeForNewStudySessionAsync(target);
      if (_AbortIsFlagged)
        return;
      
      _OrderStudier = new DefaultMultiLineTextsOrderStudier();
      await _OrderStudier.InitializeForNewStudySessionAsync(target);
    }

    public override async Task<ResultArgs<StudyItemViewModelArgs>> GetNextStudyItemViewModelAsync()
    {
      if (_AbortIsFlagged)
        return await StudyHelper.GetAbortedAsync();

      //THIS LAYER OF STUDY DECIDES ON WHAT IS IMPORTANT: MEANING OR ORDER, THEN DELEGATES STUDY TO
      //THE CORRESPONDING STUDIER.

      //todo: analyze history events
      //for now, we will assume there is no history for the MLT, and thus we should focus on understanding
      //meaning of the song.
      //depending on history may ask user for info about MLT

      //update meaning and order percent knowns.  
      //These will affect the probability of choosing Meaning vs. Order practice.
      UpdatePercentKnowns();

      //if the randomDouble is below the threshold, then we go with meaning, else we go with order.
      if (ShouldStudyMeaning())
      {
        return await _MeaningStudier.GetNextStudyItemViewModelAsync();
        //_OrderStudier.GetNextStudyItemViewModel(callback);//DEBUG ONLY.  
      }
      else
      {
        //TODO: IMPLEMENT ORDER STUDIER DO AND REFERENCE THIS IN DEFAULT MLTS STUDIER
        //_MeaningStudier.GetNextStudyItemViewModel(callback);//only because this is the only impl i have going.  this DES NOT GO HERE!!!!!!!!!!!!!!!!!!!!
        return await _OrderStudier.GetNextStudyItemViewModelAsync();
      }
    }

    private void UpdatePercentKnowns()
    {
      if (_AbortIsFlagged)
        return;

      //todo: update percent knowns using history
      if (_MeaningStudier == null)
        MeaningPercentKnown = 0.0d;
      else
        MeaningPercentKnown = _MeaningStudier.GetPercentKnown();
    }

    /// <summary>
    /// This only uses the percent of meaning known to calculate the odds and then run a probability
    /// to see if we should study meaning.
    /// </summary>
    private bool ShouldStudyMeaning()
    {
      ///THIS HAS THREE AREAS: KNOW NONE OF MEANING OF SONG (WHERE STUDYING ORDER WOULD BE WORTHLESS)
      ///                      KNOW SOME OF SONG (WHERE ORDER WOULD BE OKAY SOMETIMES)
      ///                      KNOW MOST OF SONG (WHERE ORDER WOULD BE OKAY MORE OFTEN)
      ///                      KNOW ALL OF SONG (WHERE WE SHOULD BE STUDYING ALMOST STRICTLY ORDER)

      double thresholdSome = double.Parse(StudyResources.PercentKnownSome);
      double thresholdMost = double.Parse(StudyResources.PercentKnownMost);
      double thresholdAll = double.Parse(StudyResources.PercentKnownAll);

      //IF WE DON'T KNOW MUCH OF THE SONG AT ALL, THEN WE SHOULD ALWAYS STUDY MEANING
      if (MeaningPercentKnown < thresholdSome)
        return true;

      var probabilityChooseMeaning = 0.0d;

      if (MeaningPercentKnown < thresholdMost)
      {
        //WE KNOW SOME OF THE MEANING
        probabilityChooseMeaning = double.Parse(StudyResources.ProbabilityChooseMeaningIfKnowSomeOfMeaning);
      }
      else if (MeaningPercentKnown < thresholdAll)
      {
        //WE KNOW MOST OF THE MEANING
        probabilityChooseMeaning = double.Parse(StudyResources.ProbabilityChooseMeaningIfKnowMostOfMeaning);
      }
      else
      {
        //WE KNOW ALL OF THE MEANING
        probabilityChooseMeaning = double.Parse(StudyResources.ProbabilityChooseMeaningIfKnowAllOfMeaning);
      }

      #region Deprecated using probability function
      ////function: y = -((x-.1)^(6) + (x-.1)^4) + .9
      ////x = percent of meaning known
      ////y = probability should study meaning
      
      //var x = MeaningPercentKnown;
      //var A = Math.Pow(x-0.1, 6); //1st term in above function, (x-0.1)^6
      //var B = Math.Pow(x-0.1, 4); //2nd term in above function, (x-0.1)^4
      //var probabilityChooseMeaning = -(A + B) + .9;
      //if (probabilityChooseMeaning < 0.1)
      //  probabilityChooseMeaning = 0.1;
      #endregion

      //Now we have our probability.  We can think of this probability as a threshold between 0.0 and 1.0.
      //If we have a probability of 0.7, then if we choose a random double between 0.0 and 1.0 and it is 
      //below 0.7, then we can think of that as hitting the probability, otherwise we are not hitting it.
      //If our probability is 0.1, then we are very unlikely to choose a random double below 0.1.

      //So, choose our random double between 0.0 and 1.0
      var random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute);
      var randomDouble = random.NextDouble();

      //Check to see if this is below our probability of choosing meaning.
      var randomDoubleIsBelowThreshold = (randomDouble < probabilityChooseMeaning);

      //if it is below, then we "hit" choose meaning, otherwise we will choose order.
      var chooseMeaning = randomDoubleIsBelowThreshold;

      //Overall, I don't like this wording, but I think it works.
      return chooseMeaning;
    }

    private DefaultMultiLineTextsOrderStudier _OrderStudier { get; set; }
    private DefaultMultiLineTextsMeaningStudier _MeaningStudier { get; set; }

    //private double MeaningWeight { get; set; }
    //private double OrderWeight { get; set; }
    private double MeaningPercentKnown { get; set; }
    //private double OrderPercentKnown { get; set; }

    //public void Handle(IStatusUpdate<MultiLineTextEdit, IViewModelBase> message)
    //{
    //  //WE ONLY CARE ABOUT STUDY MESSAGES
    //  if (message.Category != StudyResources.CategoryStudy)
    //    return;

    //  //WE ONLY CARE ABOUT MESSAGES PUBLISHED BY MLT MEANING STUDIERS
    //  if (
    //       (message.Publisher != null) &&
    //       !(message.Publisher is DefaultMultiLineTextsMeaningStudier)
    //     )
    //    return;

    //  ////WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
    //  //if (message.PublisherId == Id)
    //  //  return;

    //  //THIS IS ONE OF THIS OBJECT'S UPDATES, SO BUBBLE IT BACK UP WITH THIS JOB'S INFO

    //  //IF THIS IS A COMPLETED STATUS UPDATE, THEN PRODUCT SHOULD BE SET.  SO, BUBBLE THIS ASPECT UP.
    //  if (message.Status == CommonResources.StatusCompleted && message.JobInfo.Product != null)
    //  {
    //    //_StudyJobInfo.Product = message.JobInfo.Product;
    //  }

    //  //CREATE THE BUBBLING UP UPDATE
    //  var statusUpdate = new StatusUpdate<MultiLineTextList, IViewModelBase>(message.Status, null, null,
    //    //null, _StudyJobInfo, Id, this, StudyResources.CategoryStudy, null);

    //  //PUBLISH TO BUBBLE UP
    //  Exchange.Ton.Publish(statusUpdate);
    //}

    public void Handle(NavigationRequestedEventMessage message)
    {
      AbortStudying();
    }

    private void AbortStudying()
    {
      _AbortIsFlagged = true;
    }

    private object _AbortLock = new object();
    private bool _abortIsFlagged = false;
    private bool _AbortIsFlagged
    {
      get
      {
        lock (_AbortLock)
        {
          return _abortIsFlagged;
        }
      }
      set
      {
        lock (_AbortLock)
        {
          _abortIsFlagged = value;
        }
      }
    }
  }
}
