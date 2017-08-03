function BaseListController($scope, $state, $mdDialog) {
    var self = this;
    var oldSelectedItem;

    Object.defineProperty(this, "items", {
        get: function () {
            return self.getItems();
        }
    });

    var selectItem = function (item, $event) {
        if (!(typeof $event === "undefined" || $event === null) && !$event.ctrlKey)
            allItemsUnSelect();

        if (!(typeof $event === "undefined" || $event === null) && $event.shiftKey) {
            selectSelectedItems(item, oldSelectedItem);
            return;
        }

        if (typeof item.selected === "undefined") {
            item.selected = true;
            return;
        }

        item.selected = !item.selected;
        oldSelectedItem = item;
    }

    function selectSelectedItems(beginItem, endItem) {
        let beginItemPosition;
        let endItemPosition;

        let allFinded = 0;
        for (var i = 0; i < self.items.length; i++) {
            if (self.items[i] == beginItem) {
                beginItemPosition = i;
                allFinded++;
            }

            if (self.items[i] == endItem) {
                endItemPosition = i;
                allFinded++;
            }

            if (allFinded === 2)
                break;
        }

        let inc = endItemPosition - beginItemPosition > 0 ? 1 : -1;

        for (var i = beginItemPosition; i !== endItemPosition; i=i+inc) {
            self.items[i].selected = !self.items[i].selected;
        }
        self.items[i].selected = !self.items[i].selected;
    }

    function allItemsUnSelect() {
        self.items.forEach(function (item) {
            item.selected = false;
        });
    }

    this.getItems = function() {
        throw new NotImplementedException();
    };

    this.dialogOk = function() {
        throw new NotImplementedException();
    };

    this.afterItemClick = function(selectedItems) {
        throw new NotImplementedException();
    };

    this.selectedExist = function () {
        return self.items.some(function (item) {
            return item.selected;
        });
    };

    $scope.selectedExist = this.selectedExist;

    $scope.showNewItemDialog = function (ev, options) {
        var dialogOptions = {
            templateUrl: options.templateUrl,
            parent: angular.element(document.body),
            controller: options.ctrl,
            targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen
        };

        $mdDialog.show(dialogOptions).then(function (answer) {
            self.dialogOk(answer);
        });
    };

    $scope.itemClick = function ($event, item) {
        selectItem(item, $event);
            
        var selectedItems = [];
        self.items.forEach(function (item) {
            if (item.selected)
                selectedItems.push(item);
        });

        self.afterItemClick(selectedItems);
    };
}