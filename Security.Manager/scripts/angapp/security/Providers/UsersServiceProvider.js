var f = function () {
    function UsersFactory($http) {

        this.getAllUsers = function () {
            return $http.get("/Security/GetUserList").then(function (resp) {
                resp.data.forEach(function(item) {
                    item.DateCreated = new Date(parseInt(item.DateCreated.substr(6)));
                    if (item.LastActivityDate !== null)
                        item.LastActivityDate = new Date(parseInt(item.LastActivityDate.substr(6)));
                });
                return resp.data;
            });
        };

        this.getUserByLogin = function(login) {
            return $http.get(`GetUserByLogin/${login}/`).then(function(resp) {
                resp.data.DateCreated = new Date(parseInt(resp.data.DateCreated.substr(6)));
                if (resp.data.LastActivityDate !== null)
                    resp.data.LastActivityDate = new Date(parseInt(resp.data.LastActivityDate.substr(6)));

                return resp.data;
            });
        };

        this.addNewUserId = function (user) {
            return $http.post("AddNewUserId", { user: user, password: user.password }).then(function (resp) {
                return resp.data;
            });
        };

        this.updateUser = function (user) {
            return $http.post("UpdateUser", user).then(function(resp) {
                return resp.data;
            });
        };

        this.removeUsers = function (users) {
            return $http.post("RemoveUsers", users);
        };

        this.setUserPassword = function(user, password) {
            return $http.post("SetUserPassword", {login: user.Login, password: password});
        };

        this.getUserGroups = function(user) {
            return $http.get(`GetGroupListByUser/${user}/`).then(function(resp) {
                return resp.data;
            });
        };

        this.getNonUserGroups = function(user) {
            return $http.get(`GetNonUserGroups/${user}/`).then(function(resp) {
                return resp.data;
            });
        };

        this.addGroups = function(user, groups) {
            return $http.post("AddGroupsToUser", { userId: user, groups: groups });
        };

        this.removeGroups = function(user, groups) {
            return $http.post("DeleteGroupsFromUser", { user: user, groups: groups });
        };
    }

    return {
        $get: ['$http', function ($http) {
            return new UsersFactory($http);
        }]
    };
}

itisExports.providers.UsersServiceProvider = f;