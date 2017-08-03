function AdminPanelController($scope, $http, $state, $transitions) {
    $scope.tabs = $state.current.data.tabs2;

    $scope.profileTabSelect = function (tab) {
        if (typeof $scope.tabs === "undefined")
            return;

        $scope.tabs.current.forEach(function(tab) {
            tab.active = false;
        });

        tab.active = true;
    };

    $scope.profileSelectedNavItem = $scope.tabs.current[0];

    $scope.$on("leftpanelload", function ($event, stateName) {
        $scope.tabs.current = $state.current.data.tabs2[stateName];

        if (typeof $scope.tabs === "undefined")
            $scope.profileSelectedNavItem = stateName;

        $scope.tabs.current.forEach(function (tab) {
            if (tab.active)
                $scope.profileSelectedNavItem = tab.name;
        });            
    });

    if ($state.includes("users.**")) {
        selectedNav = "users";
        $scope.profileSelectedNavItem = "users";
    }
    if ($state.includes("groups.**")) {
        selectedNav = "groups";
        $scope.profileSelectedNavItem = "groups";
    }
    if ($state.includes("roles.**")) {
        selectedNav = "roles";
        $scope.profileSelectedNavItem = "roles";
    }

    Object.defineProperty($scope, "selectedNavItem",
        {
            get: function () {
                return selectedNav;
            },
            set: function(value) {
                selectedNav = value;
            }
        });
}

AdminPanelController.$inject = ['$scope', '$http', '$state', '$transitions'];

itisExports.controllers.AdminPanelController = AdminPanelController;