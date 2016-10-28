function UserProvider() {
    function UserFactory($http) {

        this.getAllUsers = function() {
            return $http.get("/Security/GetUserList").then(function(resp) {
                return resp.data;
            });
        };


    }

    return {
        $get: ['$http', function($http) {
            return new UserFactory($http);
        }]
    };
}

itisExports.providers.UserProvider = UserProvider;