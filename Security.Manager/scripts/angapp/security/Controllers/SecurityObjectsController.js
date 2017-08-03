function SecObject(objectName) {
    this.IdSecObject = 0;
    this.ObjectName = objectName || "";
    this.Grants = [];
}

function f($scope, secObjectsService, $mdDialog) {
    BaseListController.call(this, $scope);
    var selectedObjects = [];
    $scope.secObjectList = [];
    var deleteAllow = false;
    var addAllow = false;
    var selectedObjectsCount = 0;
    var self = this;
   
    function addNewSecObject(secObject) {
        if ($scope.secObjectList.find(function(item) { return item.ObjectName.toLowerCase() === secObject.toLowerCase(); }))
            return false;

        $scope.secObjectList.push(new SecObject(secObject));
        return true;
    }

    this.getItems = function () {
        return $scope.secObjectList;
    };
   
    this.afterItemClick = function (selectedItems) {
        selectedObjects = selectedItems;
        $scope.DeleteAllow = selectedObjects.length > 0;
        $scope.SelectedObjectsCount = selectedObjects.length;
    };

//    $scope.addSecObject = function () {
//        var allow = !$scope.secObjectList.find(function (item) { return item.ObjectName.toLowerCase() === $scope.newSecObject.toLowerCase(); });
//        var length = $scope.newSecObject.length;
//        allow = length > 0;
//        $scope.AddAllow = allow;
//    };
//
//    $scope.addNewObjects = function () {        
//        $scope.AddAllow = false;                                
//        var newSecObjects = [new SecObject($scope.newSecObject)];
//        $scope.newSecObject = "";
//        secObjectsService.addNewObjects(newSecObjects).then(function () {
//            $scope.secObjectList.push(newSecObjects[0]);           
//        });
//    };

//    $scope.deleteObjects = function() {        
//        secObjectsService.deleteObjects(selectedObjects).then(function () {
//            for(var i in selectedObjects){
//                var selectedObject = selectedObjects[i];
//                var index = $scope.secObjectList.findIndex(function (item) {
//                    return item.ObjectName.toLowerCase()
//                        === selectedObject.ObjectName.toLowerCase();
//                });
//                if (index > -1) {
//                    $scope.secObjectList.splice(index, 1);
//                }
//            }
//        });
//    }

    secObjectsService.getAllSecurityObjects().then(function(data) {
        $scope.secObjectList = data;
    });

    Object.defineProperty(this, "newSecObjects", {
        get: function() {
            var newSecObjects = $scope.secObjectList.filter(function(item) {
                return item.IdSecObject === 0;
            });

            return newSecObjects;
        }
    });

    Object.defineProperty(this, "SelectedObjectsCount", {
        get: function () {
            return selectedObjectsCount;
        },
        set: function (value) {
            selectedObjectsCount = value;
        }
    });

    Object.defineProperty($scope, "AddAllow", {
        get: function() {
            return addAllow;
        },
        set: function (value) {
            addAllow = value;
        }
    });

    Object.defineProperty($scope, "DeleteAllow", {
        get: function() {
            return deleteAllow;
        },
        set: function (value) {
            deleteAllow = value;
        }
    });
}

f.$inject = ['$scope', 'SecObjectsService', '$mdDialog'];

itisExports.controllers.SecurityObjectsController = f;