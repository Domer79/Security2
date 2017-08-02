function GroupUsersController($scope, Dialogs, groupsService, $stateParams, $state) {
    BaseListController.call(this, $scope);

    this.getItems = function() {
        return $scope.$ctrl.users;
    };

    this.dialogOk = function() {
        throw new NotImplementedException();
    };

    this.afterItemClick = function () { };

    $scope.items = this.getItems();

    $scope.showAddUsersDialog = function ($event) {
        var options = {
            resolve: function () { return groupsService.getNonGroupUsers($stateParams.name); },
            customFullScreen: true
        };

        Dialogs.showAddItemDialog($event, options).then(function (answer) {
            groupsService.addUsers($stateParams.name, answer).then(function () {
                $state.reload($state.current.name);
            });
        });
    }

    $scope.confirm = function ($event) {
        var textContent = "";
        var selectedItems = [];

        this.items.forEach(function (item) {
            if (item.selected) {
                textContent += item.Login + "; ";
                selectedItems.push(item);
            }
        });

        Dialogs.confirm($event, "Подтвердите удаление следующих элементов: ", textContent).then(function () {
            groupsService.removeUsers($stateParams.name, selectedItems).then(function () {
                $state.reload($state.current.name);
            });
        });
    };
}

GroupUsersController.$inject = ['$scope', 'Dialogs', 'GroupsService', '$stateParams', '$state'];

itisExports.controllers.GroupUsersController = GroupUsersController;