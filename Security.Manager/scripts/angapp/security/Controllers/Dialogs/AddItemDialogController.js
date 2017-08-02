function AddItemDialogController($scope, $mdDialog) {
    $scope.item = {};
    $scope.dialogCancel = function () {
        $mdDialog.cancel();
    }

    $scope.answer = function (answer) {
        $mdDialog.hide(answer);
    }
}

AddItemDialogController.$inject = ['$scope', '$mdDialog'];

itisExports.controllers.AddItemDialogController = AddItemDialogController;