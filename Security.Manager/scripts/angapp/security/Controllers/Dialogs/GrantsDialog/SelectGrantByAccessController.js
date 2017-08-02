function SelectGrantByAccessController($scope, rolesService, $stateParams) {
    BaseListController.call(this, $scope);

    $scope.items = null;

    this.getItems = function () {
        return $scope.items;
    };

    this.dialogOk = function () {
        throw new NotImplementedException();
    };

    this.afterItemClick = function (selectedItems) {
        $scope.$emit("secObjectsSelect", selectedItems);
    };

    rolesService.getNonRoleGrants($stateParams.name).then(function (data) {
        $scope.items = data;
    });
}

SelectGrantByAccessController.$inject = ['$scope', 'RolesService', '$stateParams'];

itisExports.controllers.SelectGrantByAccessController = SelectGrantByAccessController;