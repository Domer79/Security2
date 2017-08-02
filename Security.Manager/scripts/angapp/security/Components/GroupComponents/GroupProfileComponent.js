function groupProfileComponent() {
    this.bindings = {
        group: "<",
        groupInfoCollection: "<"
    };
    this.templateUrl = "/Templates/GroupProfile";
    this.controller = "GroupProfileController";
}

itisExports.components.groupProfileComponent = groupProfileComponent;