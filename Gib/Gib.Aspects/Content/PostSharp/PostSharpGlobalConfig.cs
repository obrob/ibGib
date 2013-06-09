#region GibMethodLoggingAspect

// Configure where to apply the Gib aspects here. This is a CSLA sample:
[assembly: Gib.Aspects.GibMethodLoggingAspect(AttributeTargetTypes = 
  "*"
  , AttributeTargetMembers = "regex:(DataPortal_|Child_)")]

#endregion