﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
namespace LightSwitchApplication
{
  public partial class UserDataRoleData
  {
    partial void Summary_Compute(ref string result)
    {
      result = this.RoleData.Text;
    }
  }
}
