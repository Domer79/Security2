function AdminPanelController($scope, $http, $state, tab1, tab2, tab3) {

    $scope.tab1 = tab1;
    $scope.tab2 = tab2;
    $scope.tab3 = tab3;


    $scope.tabSelect = function() {
    }

    $scope.userTabSelect = function () {
        $state.go('users');
    }
}

AdminPanelController.$inject = ['$scope', '$http', '$state'];

exports.controllers.AdminPanelController = AdminPanelController;