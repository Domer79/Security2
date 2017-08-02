function SettingsController($scope, SettingsService) {
    $scope.Settings = [];
    $scope.SettingsInProcessing = [];
    $scope.ShowSomething = 0;

    SettingsService.GetSystemSettings().then(function (settings) {
        $scope.Settings = settings;        
    });

    $scope.SaveSetting = function (setting) {
        $scope.SettingsInProcessing.push(setting.Name);       
        $scope.ShowSomething = 1;        
        SettingsService.SaveSetting(setting).then(function () {
            var index = $scope.SettingsInProcessing.findIndex(function (item) { return item === setting.Name });
            $scope.SettingsInProcessing.splice(index, 1);
        }, function () {
            $scope.SettingsInProcessing.splice(index, 1);
        });        
    }

    //
    $scope.InProgress = function (setting) {      
        var index = $scope.SettingsInProcessing.findIndex(function (item) {
            return item === setting.Name;
        });        
        return index !== -1;
    }
}

SettingsController.$inject = ['$scope', 'SettingsService'];
itisExports.controllers.SettingsController = SettingsController;