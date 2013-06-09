#region LightLearnMethodLoggingAspect

[assembly: LightLearn.Aspects.LightLearnMethodLoggingAspect(AttributeTargetTypes = 
  "regex:LightSwitchApplication.*(DataService|DataService+DetailsClass)"
  , AttributeTargetMembers = "*")]

#endregion