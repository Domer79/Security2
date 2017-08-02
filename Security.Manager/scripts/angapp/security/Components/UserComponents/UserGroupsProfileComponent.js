function userGroupsProfileComponent() {
    this.bindings = { groups: "<" };
    this.templateUrl = "/Templates/UserGroupsList";
    this.controller = 'UserGroupsController';
}

itisExports.components.userGroupsProfileComponent = userGroupsProfileComponent;