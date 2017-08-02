function GrantsDialogController($scope, $mdDialog) {
    $scope.secObjects = undefined;

    Object.defineProperty($scope, "saveAllow", {
        get: function() {
            if (typeof $scope.secObjects === "undefined")
                return false;

            if ($scope.secObjects.length === 0)
                return false;

            return true;
        }
    });

    $scope.dialogCancel = function () {
        $mdDialog.cancel();
    };

    $scope.answer = function () {
        $mdDialog.hide({secObjects: $scope.secObjects});
    };

    $scope.$on("secObjectsSelect", function (event, selectedItems) {
        $scope.secObjects = selectedItems;
    });
}

GrantsDialogController.$inject = ['$scope', '$mdDialog'];

itisExports.controllers.GrantsDialogController = GrantsDialogController;