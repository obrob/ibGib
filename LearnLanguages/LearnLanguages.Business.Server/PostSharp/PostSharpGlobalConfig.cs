#region GibMethodLoggingAspect

// Configure where to apply the Gib aspects here. This is a CSLA sample:
//[assembly: Gib.Aspects.GibMethodLoggingAspect(AttributeTargetTypes = 
//  "*"
//  , AttributeTargetMembers = "regex:(DataPortal_|Child_)")]

#define Child
#define DataPortal

#if Child
// Configure where to apply the Gib aspects here. This is a CSLA sample:
[assembly: Gib.Aspects.GibMethodLoggingAspect(AttributeTargetTypes =
  "*"
  , AttributeTargetMembers = "Child_*")]
#endif

#if DataPortal
// Configure where to apply the Gib aspects here. This is a CSLA sample:
[assembly: Gib.Aspects.GibMethodLoggingAspect(AttributeTargetTypes =
  "*"
  , AttributeTargetMembers = "DataPortal_*")]
#endif

#endregion