function f() {
    function SecObjectFactory($http) {
        this.getAllSecurityObjects = function() {
            return $http.get("GetSecurityObjects").then(function (resp) {
                return resp.data;
            });
        };

//        this.addNewObjects = function(newSecObjects) {
//            return $http.post("AddSecObjects", { secObjects: newSecObjects });
//        };
//
//        this.deleteObjects = function(deletedObjects) {
//            return $http.post("DeleteSecObjects", { secObjects: deletedObjects });
//        };

//        this.setupSecurityObjects = function() {
//            return $http.post("SetUpSecObjects");
//        };
    }

    return {
        $get: ['$http', function($http) {
            return new SecObjectFactory($http);
        }]
    };
}

itisExports.providers.SecObjectsServiceProvider = f;