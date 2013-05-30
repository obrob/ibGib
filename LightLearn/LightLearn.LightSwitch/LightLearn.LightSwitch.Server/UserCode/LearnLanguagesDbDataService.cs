using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Security.Server;
namespace LightSwitchApplication
{
  public partial class LearnLanguagesDbDataService
  {
    partial void UserDatas_Inserting(UserData entity)
    {
      if (entity.Id == Guid.Empty)
        entity.Id = Guid.NewGuid();
    }

    partial void RoleDatas_Inserting(RoleData entity)
    {
      if (entity.Id == Guid.Empty)
        entity.Id = Guid.NewGuid();
    }

    partial void SaveChanges_Executing()
    {

    }
  }
}
