using Csla.Rules;
using System.Collections.Generic;
using Csla.Core;
using System.Collections;

namespace LearnLanguages.Business.Rules
{
  public class CollectionMinimumCountBusinessRule : Csla.Rules.BusinessRule
  {
    public CollectionMinimumCountBusinessRule(IPropertyInfo collectionProperty, int minCount)
      : base(collectionProperty)
    {
      PrimaryProperty = collectionProperty;
      InputProperties = new List<IPropertyInfo>() { collectionProperty };
      _MinCount = minCount;
    }

    /// <summary>
    /// it is absolutely imperative not to change this property in the execute function.  will introduce bugs.
    /// see http://www.lhotka.net/weblog/CSLA4BusinessRulesSubsystem.aspx
    /// </summary>
    private int _MinCount { get; set; }

    protected override void Execute(RuleContext context)
    {
      var whatWeAreCounting = context.InputPropertyValues[PrimaryProperty];
      var collection = (ICollection)(context.InputPropertyValues[PrimaryProperty]);
      if (collection.Count < _MinCount)
      {
        var errorMsg = string.Format(BusinessResources.RuleInfoMinimumCount, 
                                     PrimaryProperty.FriendlyName, 
                                     _MinCount);
        context.AddErrorResult(errorMsg);
      }
      //var target = (TranslationEdit)context.Target;
      //if (target.Phrases.Count < _MinCount)
      //  context.AddInformationResult(BusinessResources.RuleInfoMinimumCount);
    }
  }
}
