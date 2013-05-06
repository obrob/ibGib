using Csla.Rules;
using System.Collections.Generic;
using Csla.Core;
using System.Collections;

namespace LearnLanguages.Business.Rules
{
  public class CollectionCountsAreEqualBusinessRule : Csla.Rules.BusinessRule
  {
    public CollectionCountsAreEqualBusinessRule(IPropertyInfo primaryProperty, IPropertyInfo secondaryProperty)
      : base(primaryProperty)
    {
      var primaryIsCorrectType = primaryProperty.Type.IsAssignableFrom(typeof(ICollection));
      var secondaryIsCorrectType = secondaryProperty.Type.IsAssignableFrom(typeof(ICollection));
      InputProperties = new List<IPropertyInfo>() { primaryProperty, secondaryProperty };
      _SecondaryProperty = secondaryProperty;
    }

    /// <summary>
    /// it is absolutely imperative not to change this property in the execute function.  will introduce bugs.
    /// see http://www.lhotka.net/weblog/CSLA4BusinessRulesSubsystem.aspx
    /// </summary>
    private IPropertyInfo _SecondaryProperty { get; set; }

    protected override void Execute(RuleContext context)
    {
      var primaryCollection = (ICollection)context.InputPropertyValues[PrimaryProperty];
      var secondaryCollection = (ICollection)context.InputPropertyValues[_SecondaryProperty];

      if (primaryCollection.Count != secondaryCollection.Count)
        context.AddInformationResult(string.Format(BusinessResources.RuleInfoCountsMustBeEqual, 
                                                   PrimaryProperty.Name, 
                                                   _SecondaryProperty.Name));
    }
  }
}
