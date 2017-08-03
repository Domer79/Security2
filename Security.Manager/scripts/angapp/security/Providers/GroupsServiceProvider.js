function f() {
    function GroupsFactory($http) {
        this.getAllGroups = function() {
            return $http.get("GetGroupList").then(function(resp) {
                return resp.data;
            });
        };

        this.getGroupByName = function(groupName) {
            return $http.post(`GetGroupByName/${groupName}/`).then(function(resp) {
                return resp.data;
            });
        };

        this.addGroup = function(group) {
            return $http.post("AddGroup", { name: group.name, description: group.description });
        };

        this.updateGroup = function (group) {
            return $http.post("UpdateGroup", group).then(function (resp) {
                return resp.data;
            });
        };

        this.removeGroups = function (groups) {
            return $http.post("RemoveGroups", groups);
        };

        this.getGroupUsers = function(group) {
            return $http.get(`GetUserListByGroup/${group}/`).then(function(resp) {
                return resp.data;
            });
        };

        this.getNonGroupUsers = function(group) {
            return $http.get(`GetNonGroupUsers/${group}/`).then(function (resp) {
                resp.data.forEach(function(item) {
                    item.Name = item.Login;
                });

                return resp.data;
            });
        };

        this.addUsers = function(group, users) {
            return $http.post("AddUsersToGroup", { group: group, users: users });
        };

        this.removeUsers = function(group, users) {
            return $http.post("DeleteUsersFromGroup", {group: group, users: users});
        };

    }

    return {
        $get: ['$http', function($http) {
            return new GroupsFactory($http);
        }]
    };
}

itisExports.providers.GroupsServiceProvider = f;