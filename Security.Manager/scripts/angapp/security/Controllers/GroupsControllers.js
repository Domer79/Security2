function GroupsController($scope, $mdDialog, $state, groupsService, dialogs) {
    BaseListController.call(this, $scope, $state, $mdDialog);
    var self = this;
    this.selectedItems;

    var groupTabs = [
        { name: "Профиль", active: true },
        { name: "Пользователи" },
        { name: "Роли" }
    ];

    var tabs = $state.current.data.tabs2;

    if (typeof tabs.groups === "undefined") {
        tabs.groups = groupTabs;
        tabs.current = groupTabs;
    }

    this.getItems = function () {
        return $scope.$ctrl.groups;
    };

    this.dialogOk = function (group) {
        groupsService.addGroup(group).then(function () {
            reloadState();
        });
    };

    this.afterItemClick = function (selectedItems) {
        self.selectedItems = selectedItems;
        var group = selectedItems[selectedItems.length - 1];
        if (group !== null && typeof group !== "undefined")
            $state.go('groups.detail', { name: group.Name });
    };

    $scope.isSelectedItem = function (name) {
        let selectedItem = $state.is('groups.detail', { name: `${name}` });
        return selectedItem;
    };

    function reloadState() {
        $state.reload("groups");
    }

    $scope.deleteGroups = function($event) {
        var textContent = "";

        self.selectedItems.forEach(function (item) {
            textContent += item.Name + "; ";
        });

        dialogs.confirm($event, "Подтвердите удаление следующих элементов: ", textContent).then(function () {
            groupsService.removeGroups(self.selectedItems).then(function () {
                $state.reload("groups");
            });
        });
    };

    $scope.$emit("leftpanelload", "groups");

    var leftPanel = angular.element(document.querySelector("#leftpanel"));
    leftPanel.css("height", `${leftPanelHeight}px`);
}

GroupsController.$inject = ['$scope', '$mdDialog', '$state', 'GroupsService', 'Dialogs'];

itisExports.controllers.GroupsController = GroupsController;


