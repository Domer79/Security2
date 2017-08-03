(function() {
    "use strict";
    var securityApp = angular.module("SecurityApp", ['ngMaterial', 'ui.router', 'ngSanitize']);

    itisExports.controllers.register(securityApp);
    itisExports.components.register(securityApp);
    itisExports.providers.register(securityApp);
    itisExports.directives.register(securityApp);

    securityApp.config([
        '$stateProvider', '$mdIconProvider', '$httpProvider', function ($stateProvider, $mdIconProvider, $httpProvider) {
            $mdIconProvider.iconSet('navigation', '/Content/images/material-icons/iconsets/navigation-icons.svg');
            $mdIconProvider.iconSet('images', '/Content/images/material-icons/iconsets/image-icons.svg');

            if (!window.location.hasOwnProperty("appname")) {
                Object.defineProperty(window.location, "appname", {
                    get: function () {
                        let regex = /^\/([^\s\/]+)\//;
                        let result;
                        if ((result = regex.exec(window.location.pathname)) !== null)
                            return result[1];

                        throw new Error("URL-адрес не соответствует требуемому шаблону");
                    },
                    set: function (value) {
                        let regex = /^\/([^\s\/]+)\//;
                        window.location.pathname = window.location.pathname.replace(regex, `/${value}/`);
                    }
                });
            };

            let tempAppName = window.location.appname;

            var states = [
                {
                    name: 'adminpanel',
                    url: '/adminpanel',                   
                    views: {
                        "securitymain": {
                            component: "adminpanel"
                        }
                    },
                    data: {
                        tabs2: {
                            current: [
                                { name: "tab0" },
                                { name: "tab1" },
                                { name: "tab2" }
                            ]
                        }
                    }                 
                },
                {
                    name: 'safeobjects',
                    url: '/safeobjects',                    
                    views: {
                        "securitymain": {
                            controller: 'SecurityObjectsController',
                            templateUrl: '/Templates/SafeObjects',
                        }
                    },                   
                },
                 {
                     name: 'settings',
                     url: '/settings',
                     views: {
                         "securitymain": {
                             controller: 'SettingsController',
                             templateUrl: '/Templates/Settings',
                         }
                     },
                 },
                {
                    name: 'log',
                    url: '/log',                    
                    views: {
                        "securitymain": {
                            controller: 'LogController',
                            templateUrl: '/Templates/Log',
                        }
                    },
                },
                {
                    name: 'users',
                    parent: 'adminpanel',
                    url: '/users',
                    component: 'users',
                    resolve: {
                        users: function(UsersService) {
                            var allUsers = UsersService.getAllUsers();
                            return allUsers;
                        }
                    }
                },                
                {
                    name: 'users.detail',
                    url: '/{login}',
                    views: {
                        "tab0@adminpanel": {
                            component: 'userProfile'
                        },
                        "tab1@adminpanel": {
                            component: "userGroupsProfile"
                        },
                        "tab2@adminpanel": {
                            component: "memberRolesProfile"
                        }
                    },
                    resolve: {
                        user: function(users, $stateParams) {
                            return users.find(function(user) {
                                return user.Login === $stateParams.login;
                            });
                        },
                        groups: function($stateParams, UsersService) {
                            var allGroupsByUser = UsersService.getUserGroups($stateParams.login);
                            return allGroupsByUser;
                        },
                        roles: function($stateParams, RolesService) {
                            var roles = RolesService.getMemberRoles($stateParams.login);
                            return roles;
                        }
                    }
                },
                {
                    name: "groups",
                    parent: "adminpanel",
                    url: "/groups",
                    component: "groups",
                    resolve: {
                        groups: function(GroupsService) {
                            var allGroups = GroupsService.getAllGroups();
                            return allGroups;
                        }
                    }
                },
                {
                    name: "groups.detail",
                    url: '/{name}',
                    views: {
                        "tab0@adminpanel": {
                            component: 'groupProfile'
                        },
                        "tab1@adminpanel": {
                            component: "groupUsersProfile"
                        },
                        "tab2@adminpanel": {
                            component: "memberRolesProfile"
                        }
                    },
                    resolve: {
                        group: function (groups, $stateParams) {
                            return groups.find(function(group) {
                                return group.Name === $stateParams.name;
                            });
                        },
                        users: function($stateParams, GroupsService) {
                            var allUsersByGroup = GroupsService.getGroupUsers($stateParams.name);
                            return allUsersByGroup;
                        },
                        roles: function($stateParams, RolesService) {
                            var roles = RolesService.getMemberRoles($stateParams.name);
                            return roles;
                        },
                        groupInfoCollection: function(groups, $stateParams, Tools) {
                            var group = groups.find(function(group) {
                                return group.Name === $stateParams.name;
                            });

                            if (group == null) {
                                group = {};
                                group.info = "Group " + $stateParams.name + " is not found";
                            }

                            return Tools.getObjectInfoCollection(group);
                        }
                    }
                },
                {
                    name: "roles",
                    parent: "adminpanel",
                    url: "/roles",
                    component: "roles",
                    resolve: {
                        roles: function(RolesService) {
                            var allRoles = RolesService.getAllRoles();
                            return allRoles;
                        }
                    }
                },
                {
                    name: "roles.detail",
                    url: '/{name}',
                    views: {
                        "tab0@adminpanel": {
                            component: 'roleProfile'
                        },
                        "tab1@adminpanel": {
                            component: "roleMembersProfile"
                        },
                        "tab2@adminpanel": {
                            component: "roleGrantsProfile"
                        }
                    },
                    resolve: {
                        role: function(roles, $stateParams) {
                            return roles.find(function (role) {
                                return role.Name === $stateParams.name;
                            });
                        },
                        members: function($stateParams, RolesService) {
                            var allMembersByRole = RolesService.getRoleMembers($stateParams.name);
                            return allMembersByRole;
                        },
                        grants: function($stateParams, RolesService) {
                            var roleGrants = RolesService.getRoleGrants($stateParams.name);
                            return roleGrants;
                        },
                        roleInfoCollection: function(roles, $stateParams, Tools) {
                            var role = roles.find(function(role) {
                                return role.Name === $stateParams.name;
                            });

                            if (role == null) {
                                role = {};
                                role.info = "Role " + $stateParams.name + " is not found";
                            }

                            return Tools.getObjectInfoCollection(role);
                        }
                    }
                }
            ];

            $httpProvider.interceptors.push('SecurityHttpInterceptor');
            $httpProvider.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";

            states.forEach(function(state) {
                $stateProvider.state(state);
            });
        }
    ]); 
}());