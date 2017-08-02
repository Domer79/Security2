function MemberRolesController($scope, dialogs, rolesService, $stateParams, $state) {
    var member = $stateParams.login || $stateParams.name;

    BaseListController.call(this, $scope);

    this.getItems = function () {
        return $scope.$ctrl.roles;
    };

    this.dialogOk = function () {
        throw new NotImplementedException();
    };

    this.afterItemClick = function () { };

    $scope.items = this.getItems();

    $scope.showAddRolesDialog = function($event) {
        var options = {
            resolve: function () { return rolesService.getNonMemberRoles(member); },
            customFullScreen: true
        };

        dialogs.showAddItemDialog($event, options).then(function(answer) {
            rolesService.addMemberRoles(member, answer).then(function () {
                $state.reload($state.current.name);
            });
        });
    }

    $scope.confirm = function ($event) {
        var textContent = "";
        var selectedItems = [];

        this.items.forEach(function (item) {
            if (item.selected) {
                textContent += item.Name + "; ";
                selectedItems.push(item);
            }
        });

        dialogs.confirm($event, "Подтвердите удаление следующих элементов: ", textContent).then(function () {
            rolesService.removeMemberRoles(member, selectedItems).then(function () {
                $state.reload($state.current.name);
            });
        });
    };
}

MemberRolesController.$inject = ['$scope', 'Dialogs', 'RolesService', '$stateParams', '$state'];

itisExports.controllers.MemberRolesController = MemberRolesController;