function UserGroupsController($scope, Dialogs, UsersService, $stateParams, $state) {
    BaseListController.call(this, $scope);

    this.getItems = function() {
        return $scope.$ctrl.groups;
    };

    this.dialogOk = function() {
        throw new NotImplementedException();
    };

    this.afterItemClick = function () {};

    $scope.items = this.getItems();

    $scope.showAddGroupsDialog = function($event) {
        var options = {
            resolve: function() { return UsersService.getNonUserGroups($stateParams.login); },
            customFullScreen: true
        };

        Dialogs.showAddItemDialog($event, options).then(function(answer) {
            UsersService.addGroups($stateParams.login, answer).then(function() {
                $state.reload($state.current.name);
            });
        });
    };

    $scope.confirm = function($event) {
        var textContent = "";
        var selectedItems = [];

        this.items.forEach(function (item) {
            if (item.selected) {
                textContent += item.Name + "; ";
                selectedItems.push(item);
            }
        });

        Dialogs.confirm($event, "Подтвердите удаление следующих элементов: ", textContent).then(function() {
            UsersService.removeGroups($stateParams.login, selectedItems).then(function() {
                $state.reload($state.current.name);
            });
        });
    };
}

UserGroupsController.$inject = ['$scope', 'Dialogs', 'UsersService', '$stateParams', '$state'];

itisExports.controllers.UserGroupsController = UserGroupsController;