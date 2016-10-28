function UsersController($scope, $http, $state) {

    debugger;

    var tabs = $state.current.data.tabs2;
    tabs[0] = "Профиль";
    tabs[1] = "Группы";
    tabs[2] = "Роли";

    $scope.itemClick = function ($event, user) {
        $state.go('detail', { login: user.Login });
    }
}

UsersController.$inject = ['$scope', '$http', '$state'];

itisExports.controllers.UsersController = UsersController;