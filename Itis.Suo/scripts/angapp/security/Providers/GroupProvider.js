function GroupProvider() {
    function GroupFactory($http) {
        this.getAllGroupsByUser = function(user) {
            return $http.get("/Security/GetGroupListByUser/" + user).then(function(resp) {
                 return resp.data;
            });
        }
    }

    return {
        $get: ['$http', function($http) {
            return new GroupFactory($http);
        }]
    };
}

itisExports.providers.GroupProvider = GroupProvider;