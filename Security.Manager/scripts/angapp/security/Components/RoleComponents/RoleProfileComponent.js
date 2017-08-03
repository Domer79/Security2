function roleProfileComponent() {
    this.bindings = {
        role: "<",
        roleInfoCollection: "<"
    };
    this.templateUrl = "/Templates/RoleProfile";
    this.controller = "RoleProfileController";
}

itisExports.components.roleProfileComponent = roleProfileComponent;