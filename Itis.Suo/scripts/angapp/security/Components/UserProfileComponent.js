function userProfileComponent() {
    this.bindings = {
        user: "<",
        userInfoCollection: "<"
    };
    this.templateUrl = "/Templates/UserProfile";
    this.controller = 'UsersDetailController';
}

itisExports.components.userProfileComponent = userProfileComponent;