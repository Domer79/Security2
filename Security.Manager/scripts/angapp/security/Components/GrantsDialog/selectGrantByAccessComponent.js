function selectGrantByAccessComponent() {
    this.bindings = {
        accessType: "<",
        accessTypeGrants: "<"
    };
    this.templateUrl = "selectgrantbyaccess.tmpl.html";
    this.controller = "SelectGrantByAccessController";
}

itisExports.components.selectGrantByAccessComponent = selectGrantByAccessComponent;