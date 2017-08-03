function f($scope, usersService, $state, $stateParams) {
    $scope.item = $scope.$ctrl.user;

    $scope.changePassword = function() {
        usersService.setUserPassword($scope.item, $scope.password).then(function () {
            $scope.password = "";
        });
    }

    $scope.saveProfile = function() {
        usersService.updateUser($scope.item).then(function (user) {
            $state.go($state.current.name, {login: user.Login});
        }, function(err) {
            console.log(err);
            usersService.getUserByLogin($stateParams.login).then(function(user) {
                var properties = Object.getOwnPropertyNames(user);
                properties.forEach(function(item) {
                    $scope.$ctrl.user[item] = user[item];
                });
                $state.go($state.current.name, { login: user.Login });
            });
        });
    };

    $scope.canSavePassword = function() {
        return angular.isDefined($scope.password) && $scope.password !== "";
    }
}

f.$inject = ["$scope", "UsersService", "$state", "$stateParams"];

itisExports.controllers.UserProfileController = f;