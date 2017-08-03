function f() {
    function RolesFactory($http) {
        this.getAllRoles = function() {
            return $http.get("GetRoleList").then(function(resp) {
                return resp.data;
            });
        };

        this.getRoleByName = function (roleName) {
            return $http.post(`GetRoleByName/${roleName}/`).then(function (resp) {
                return resp.data;
            });
        };

        this.addRole = function(role) {
            return $http.post("AddRole", { name: role.name, description: role.description });
        };

        this.updateRole = function (role) {
            return $http.post("UpdateRole", role).then(function (resp) {
                return resp.data;
            });
        };

        this.removeRoles = function(roles) {
            return $http.post("RemoveRoles", roles);
        }

        this.getMemberRoles = function(member) {
            return $http.get(`GetRoleListByMember/${member}/`).then(function(resp) {
                return resp.data;
            });
        };

        this.getNonMemberRoles = function(member) {
            return $http.get(`GetNonMemberRoles/${member}/`).then(function(resp) {
                return resp.data;
            });
        };

        this.addMemberRoles = function(member, roles) {
            return $http.post("AddRolesToMember", { memberId: member, roles: roles });
        };

        this.removeMemberRoles = function(member, roles) {
            return $http.post("DeleteRolesFromMember", { member: member, roles: roles });
        };

        this.getRoleGrants = function(role) {
            return $http.get(`GetGrantsByRole/${role}/`).then(function(resp) {
                return resp.data;
            });
        };

        this.getRoleMembers = function(role) {
            return $http.get(`GetMembersByRole/${role}/`).then(function(resp) {
                return resp.data;
            });
        };

        this.getNonRoleGrants = function (role) {
            return $http.post(`GetNonRoleGrants/${role}/`).then(function(resp) {
                return resp.data;
            });
        }

        this.setGrants = function (role, secObjects) {
            return $http.post("SetGrants", {role: role, secObjects: secObjects});
        };

        this.removeGrants = function (grants) {
            return $http.post("RemoveGrants", { grants: grants });
        }
    }

    return {
        $get: ['$http', function($http) {
            return new RolesFactory($http);
        }]
    };
}

itisExports.providers.RolesServiceProvider = f;