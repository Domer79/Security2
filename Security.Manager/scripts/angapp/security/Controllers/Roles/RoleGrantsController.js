function RoleGrantsController($scope, dialogs, rolesService, $stateParams, $state) {
    BaseListController.call(this, $scope);

    this.getItems = function () {
        return $scope.$ctrl.grants;
    };

    this.dialogOk = function () {
        throw new NotImplementedException();
    };

    var selectedGrants;

    this.afterItemClick = function(selectedItems) {
        selectedGrants = selectedItems;
    };

    $scope.showAddGrantsDialog = function ($event) {
        var options = {
            templateUrl: 'grants-dialog.tmpl.html',
            controller: 'GrantsDialogController',
            customFullScreen: true
        };

        dialogs.showAddItemDialog($event, options).then(function (answer) {
            rolesService.setGrants($stateParams.name, answer.secObjects).then(function() {
                $state.reload($state.current.name);
            });
        });
    };

    $scope.confirm = function ($event) {
        var textContent = "";

        selectedGrants.forEach(function (item) {
            textContent += `${item.SecObject}; `;
        });

        dialogs.confirm($event, "Подтвердите удаление следующих элементов: ", textContent).then(function () {
            rolesService.removeGrants(selectedGrants).then(function () {
                $state.reload($state.current.name);
            });
        });
    };

    $scope.items = this.items;
}

RoleGrantsController.$inject = ['$scope', 'Dialogs', 'RolesService', '$stateParams', '$state'];

itisExports.controllers.RoleGrantsController = RoleGrantsController;