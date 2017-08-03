function LoginController($scope, $window, LoginService, SettingService) {
    $scope.login = "";
    $scope.password = "";
    $scope.rememberMe = true;
    $scope.InProcessing = false;
    $scope.MainApplicationLink = null;    

    SettingService.GetMainApplicationHostAndPort().then(function (hostAndPort) {
        if (hostAndPort.Host.length === 0) {
            $scope.MainApplicationLink = '/';
            return;
        }
        if (hostAndPort.Port.length === 0) {
            $scope.MainApplicationLink = hostAndPort.Host + '/LightingSystem';
            return;
        }
        $scope.MainApplicationLink = hostAndPort.Host + ':' + hostAndPort.Port + '/LightingSystem';
    });
    
    $scope.Login = function () {
        $scope.InProcessing = true;
        LoginService.Login($scope.login, $scope.password).then(function (result) {            
            if (result.IsSuccessfull) {
                $window.location.href = result.Url;
            }
            else {
                $scope.InProcessing = false;
                $scope.ErrorMessage = result.ErrorMessage;
            }            
        }, function () {
            $scope.InProcessing = false;
            $scope.ErrorMessage = "Произошла ошибка во время выполнения запроса";
        });
    }
}

LoginController.$inject = ['$scope', '$window', 'LoginService', 'SettingsService'];
itisExports.controllers.LoginController = LoginController;