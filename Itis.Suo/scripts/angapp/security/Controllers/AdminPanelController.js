function AdminPanelController($scope, $http, $state) {

    $scope.tabs = $state.current.data.tabs2;

    $scope.tabSelect = function() {
    }

    $scope.userTabSelect = function () {
        $state.go('users');
    }
}

AdminPanelController.$inject = ['$scope', '$http', '$state'];

itisExports.controllers.AdminPanelController = AdminPanelController;