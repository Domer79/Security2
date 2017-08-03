function AppController($scope, $rootScope, $http, $state, $mdDialog, secObjectsService, settingService, toast, appService, dialogs, $transitions, $urlRouter) {
    
    var applications = [];
    var newLogMessage = false;    

    var labelEl = angular.element(document.querySelector('#app_name_label'));
    
    $scope.MainApplicationLink = null;
    $scope.ApplicationsAreLoaded = false;

    settingService.GetMainApplicationHostAndPort().then(function (hostAndPort) {
        if (hostAndPort.Host.length === 0) {  
            $scope.MainApplicationLink = '/';
            return;
        }
        if (hostAndPort.Port.length === 0){
            $scope.MainApplicationLink = hostAndPort.Host + '/LightingSystem';
            return;
        }
        $scope.MainApplicationLink = hostAndPort.Host + ':' + hostAndPort.Port + '/LightingSystem';
    });

    appService.getApplications().then(function(data){
        $scope.Applications = data;
        $scope.ApplicationsAreLoaded = true;                
        labelEl.text($scope.currentApp().Description);
    });

    Object.defineProperty($scope, 'Applications', {
        get: function () {            
            return applications;
        },
        set: function (value) {
            applications = value;
        }
    });

    $rootScope.NewLogMessage = false;
    
    $scope.currentApp = function () {        
        return $scope.Applications[window.location.appname];
    }
  
    $scope.gotoApplication = function (appName) {
        window.location.appname = appName;
    };

    $scope.deleteApp = function ($event, app) {        
        dialogs.confirm($event, "", `Подтвердите удаление приложения ${app.AppName}`).then(function () {
            appService.deleteApplication(app).then(function () {                               
                var index = $scope.Applications.findIndex(function (item) { return item.IdApplication === app.IdApplication; });
                $scope.Applications.splice(index,1);                
            });
        });
    }
//
//    $scope.createApplication = function ($event) {
//        var dialogOptions = {
//            templateUrl: "createapp.tmpl.html",
//            controller: "AddApplicationDialogController",
//            targetEvent: $event,
//            clickOutsideToClose: true,
//            fullscreen: true
//        };
//
//        dialogs.showAddItemDialog($event, dialogOptions).then(function (answer) {
//            appService.createApplication(answer).then(function (application) {                                
//                $scope.Applications.push(application);                
//            });
//        });
//    };

    $scope.goToState = function (name) {
        $state.go(name);
    }

    $transitions.onBefore({ to: "adminpanel" }, function () {
        return $state.target("users");
    });

    if ($urlRouter.location === "")
        $state.go("adminpanel");
}

AppController.$inject = ['$scope', '$rootScope', '$http', '$state', '$mdDialog', 'SecObjectsService', 'SettingsService', 'Toast', 'ApplicationService', 'Dialogs', '$transitions', '$urlRouter'];
itisExports.controllers.AppController = AppController;