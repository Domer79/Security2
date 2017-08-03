function roleGrantsProfileComponent() {
    this.bindings = { grants: "<" };
    this.templateUrl = "/Templates/RoleGrantList";
    this.controller = "RoleGrantsController";
}

itisExports.components.roleGrantsProfileComponent = roleGrantsProfileComponent;