function f() {
    function LoginServiceFactory($http) {

        this.Login = function (login, password) {
            return $http.post('/Auth/Login', {login, password}).then(function (response) {
                return response.data;
            });
        }      
    }

    return {
        $get: ['$http', function ($http) {
            return new LoginServiceFactory($http);
        }]
    }
}

itisExports.providers.LoginServiceProvider = f;