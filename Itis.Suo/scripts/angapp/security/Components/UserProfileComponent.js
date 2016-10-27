function userProfileComponent() {
    this.bindings = { user: "<" };
    this.template = "<h1>{{$ctrl.user.Login}}</h1>";
//    this.controller = 'UsersDetailController';
}

itisExports.components.userProfileComponent = userProfileComponent;