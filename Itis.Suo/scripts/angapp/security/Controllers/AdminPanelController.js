function AdminPanelController($scope, $http, $state, tabs) {

    $scope.tabs = tabs;

    $scope.tabSelect = function() {
    }

    $scope.userTabSelect = function () {
        $state.go('users');
    }
}

AdminPanelController.$inject = ['$scope', '$http', '$state', 'tabs'];

exports.controllers.AdminPanelController = AdminPanelController;