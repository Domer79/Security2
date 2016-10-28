function RoleProvider() {
    function RoleFactory($http) {
        this.getRolesByUser = function (user) {
            return $http.get("/Security/GetRoleListByUser/" + user).then(function(resp) {
                return resp.data;
            });
        }
    }

    return {
        $get: ['$http', function($http) {
            return new RoleFactory($http);
        }]
    };
}

itisExports.providers.RoleProvider = RoleProvider;