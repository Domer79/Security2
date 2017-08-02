function f($scope, groupsService, $state, $stateParams) {
    $scope.item = $scope.$ctrl.group;

    $scope.saveProfile = function () {
        groupsService.updateGroup($scope.item).then(function (group) {
            $state.go($state.current.name, { name: group.Name });
        }, function (err) {
            console.log(err);
            groupsService.getGroupByName($stateParams.name).then(function (group) {
                var properties = Object.getOwnPropertyNames(group);
                properties.forEach(function (item) {
                    $scope.$ctrl.group[item] = group[item];
                });
                $state.go($state.current.name, { name: group.Name });
            });
        });
    };
}

f.$inject = ["$scope", "GroupsService", "$state", "$stateParams"];

itisExports.controllers.GroupProfileController = f;