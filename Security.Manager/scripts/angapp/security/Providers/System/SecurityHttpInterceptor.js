function f() {    
    function SecurityHttpFactory($q, $rootScope, logService, $injector) {
        this.request = function (config) {
            return config;
        };

        this.requestError = function (rejection) {
            return rejection;
        };

        this.response = function (response) {
            return response;
        };

        this.responseError = function (rejection) {
            var message = "{statusText: '" + rejection.statusText + "', data: '" + rejection.data + "'}";
            logService.log(message).then(function(log) {                                
                $rootScope.NewLogMessage = true;
            });
            return $q.reject(rejection);
        };
    }

    return {
        $get: function ($q, $rootScope, LogService, $injector) {
            return new SecurityHttpFactory($q, $rootScope, LogService, $injector);
        }
    };
}

itisExports.providers.SecurityHttpInterceptorProvider = f;