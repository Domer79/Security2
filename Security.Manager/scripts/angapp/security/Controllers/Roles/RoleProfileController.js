function f($scope, rolesService, $state, $stateParams) {
    $scope.item = $scope.$ctrl.role;

    $scope.saveProfile = function () {
        rolesService.updateRole($scope.item).then(function (role) {
            $state.go($state.current.name, { name: role.Name });
        }, function (err) {
            console.log(err);
            rolesService.getRoleByName($stateParams.name).then(function (role) {
                var properties = Object.getOwnPropertyNames(role);
                properties.forEach(function (item) {
                    $scope.$ctrl.role[item] = role[item];
                });
                $state.go($state.current.name, { name: role.Name });
            });
        });
    };
}

f.$inject = ["$scope", "RolesService", "$state", "$stateParams"];

itisExports.controllers.RoleProfileController = f;