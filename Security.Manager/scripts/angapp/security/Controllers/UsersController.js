function UsersController($scope, $state, $mdDialog, usersService, dialogs, $transitions) {
    BaseListController.call(this, $scope, $state, $mdDialog);
    var self = this;
    this.selectedItems;

    this.getItems = function () {
        return $scope.$ctrl.users;
    }

    this.dialogOk = function(user) {
        usersService.addNewUserId(user).then(function () {
            $state.go($state.current.name, $state.params, { reload: true });
        });
    }

    this.afterItemClick = function (selectedItems) {
        self.selectedItems = selectedItems;
        var user = selectedItems[selectedItems.length - 1];
        if (user !== null && typeof user !== "undefined")
            $state.go('users.detail', { login: user.Login });
    }

    var userTabs = [
        { name: "Профиль", active: true },
        { name: "Группы" },
        { name: "Роли" }
    ];

    $scope.isSelectedItem = function (login) {
        let selectedItem = $state.is('users.detail', { login: `${login}` });
        return selectedItem;
    };

    var tabs = $state.current.data.tabs2;

    if (typeof tabs.users === "undefined") {
        tabs.users = userTabs;
        tabs.current = userTabs;
    }

    $scope.deleteUsers = function ($event) {
        var textContent = "";

        self.selectedItems.forEach(function (item) {
            textContent += item.Login + "; ";
        });

        dialogs.confirm($event, "Подтвердите удаление следующих элементов: ", textContent).then(function () {
            usersService.removeUsers(self.selectedItems).then(function () {
                $state.reload("users");
            });
        });
    };

    $scope.$emit("leftpanelload", "users");

    var leftPanel = angular.element(document.querySelector("#leftpanel"));
    leftPanel.css("height", `${leftPanelHeight}px`);
}

UsersController.$inject = ['$scope', '$state', '$mdDialog', 'UsersService', 'Dialogs', '$transitions'];

itisExports.controllers.UsersController = UsersController;