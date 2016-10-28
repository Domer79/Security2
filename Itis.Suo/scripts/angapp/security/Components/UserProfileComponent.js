function userProfileComponent() {
    this.bindings = { user: "<" };
    this.templateUrl = "/Templates/UserProfile";
    this.controller = 'UsersDetailController';
}

itisExports.components.userProfileComponent = userProfileComponent;