/// <reference path="../GeneratedArtifacts/viewModel.js" />

myapp.AddEditUserData.SelectRole_execute = function (screen) {  
  var user = screen.UserData, //Get the user
      selectedRole = screen.RoleDatas.selectedItem, //Get the selected role
      //Create a new UserDataRoleData to link the user to the role.
      newUserRole = screen.UserDataRoleDatas.addNew();

  newUserRole.UserDatas_Id = user.Id;
  newUserRole.RoleDatas_Id = selectedRole.Id;
  newUserRole.UserData = user;
  newUserRole.RoleData = selectedRole;

  myapp.applyChanges();
};