function AddUserDialogController($scope, $mdDialog) {
    $scope.item = {};
    $scope.dialogCancel = function () {
        $mdDialog.cancel();
    }

    $scope.answer = function (answer) {
        $mdDialog.hide(answer);
    }

    $scope.submit = function(itemModelValid) {
        if (itemModelValid)
            $mdDialog.hide($scope.item);
    }
}

AddUserDialogController.$inject = ['$scope', '$mdDialog'];

itisExports.controllers.AddUserDialogController = AddUserDialogController;