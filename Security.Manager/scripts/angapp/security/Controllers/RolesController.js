function RolesController($scope, $mdDialog, rolesService, $state, dialogs) {
    BaseListController.call(this, $scope, $state, $mdDialog);
    var self = this;
    this.selectedItems;

    var roleTabs = [
        { name: "Профиль", active: true },
        { name: "Участники" },
        { name: "Разрешения" }
    ];

    var tabs = $state.current.data.tabs2;
    if (typeof tabs.roles === "undefined") {
        tabs.roles = roleTabs;
        tabs.current = roleTabs;
    }

    this.getItems = function() {
        return $scope.$ctrl.roles;
    };

    this.dialogOk = function(role) {
        rolesService.addRole(role).then(function() {
            reloadState();
        });
    };

    $scope.isSelectedItem = function (name) {
        let selectedItem = $state.is('roles.detail', { name: `${name}` });
        return selectedItem;
    };

    this.afterItemClick = function (selectedItems) {
        self.selectedItems = selectedItems;
        var role = selectedItems[selectedItems.length - 1];
        if (role !== null && typeof role !== "undefined")
            $state.go('roles.detail', { name: role.Name });
    };

    function reloadState() {
        $state.reload("roles");
    }

    $scope.deleteRoles = function ($event) {
        var textContent = "";

        self.selectedItems.forEach(function (item) {
            textContent += item.Name + "; ";
        });

        dialogs.confirm($event, "Подтвердите удаление следующих элементов: ", textContent).then(function () {
            rolesService.removeRoles(self.selectedItems).then(function () {
                $state.reload("roles");
            });
        });
    };

    $scope.$emit("leftpanelload", "roles");

    var leftPanel = angular.element(document.querySelector("#leftpanel"));
    leftPanel.css("height", `${leftPanelHeight}px`);
}

RolesController.$inject = ['$scope', '$mdDialog', 'RolesService', '$state', 'Dialogs'];

itisExports.controllers.RolesController = RolesController;