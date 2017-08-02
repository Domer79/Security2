function logController($scope,$http, $rootScope, logService, toast) {

    var logInfo = '';

    Object.defineProperty($scope, "LogInfo", {
        get: function () {
            return logInfo;
        },
        set: function (value) {
            logInfo = value;
        }
    });   

    Object.defineProperty($scope, "logs", {
        get: function() {
            return logService.getActiveLogs();
        }
    });

    $rootScope.NewLogMessage = false;

    $scope.logClick = function (log) {
        if (log.link) {
            $http.get(log.link).then(function (respone) {
                $scope.LogInfo = respone.data;
                log.isShow = true;
            });
        }        
    };
}

logController.$inject = ['$scope', '$http', '$rootScope', 'LogService', 'Toast'];

itisExports.controllers.LogController = logController;