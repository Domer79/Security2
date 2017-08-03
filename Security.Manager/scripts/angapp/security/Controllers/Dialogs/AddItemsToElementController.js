function AddItemsToElementController($scope, $mdDialog, items) {
    BaseListController.call(this, $scope);

    $scope.items = items;

    this.getItems = function () {
        return items;
    }

    this.dialogOk = function () {
        throw new NotImplementedException();
    }

    this.afterItemClick = function () {
        
    }

    $scope.dialogCancel = function() {
        $mdDialog.cancel();
    };

    $scope.answer = function () {
        var selectedItems = [];

        items.forEach(function (item) {
            if (item.selected)
                selectedItems.push(item);
        });

        $mdDialog.hide(selectedItems);
    };
}

AddItemsToElementController.$inject = ['$scope', '$mdDialog', 'items'];

itisExports.controllers.AddItemsToElementController = AddItemsToElementController;