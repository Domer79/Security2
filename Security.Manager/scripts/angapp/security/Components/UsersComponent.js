function usersComponent() {
    this.bindings = { users: "<" };
    this.templateUrl = "/Templates/AdminPanelUsers";
    this.controller = 'UsersController';
}

itisExports.components.usersComponent = usersComponent;