function f() {
    function SettingsServiceProvider($http) {

        this.GetSystemSettings = function () {
            return $http.get("/Settings/GetSystemSettings").then(function (response) {
                return response.data;
            });
        }

        this.GetMainApplicationHostAndPort = function () {
            return $http.get("/Settings/GetMainApplicationHostAndPort").then(function (response) {
                return response.data;
            });
        }

        this.SaveSetting = function (setting) {
            return $http.post("/Settings/SaveSetting", setting).then(function () {
                return;
            });
        }
    }
    return {
        $get: ['$http', function ($http) {
            return new SettingsServiceProvider($http);
        }]
    }
}

itisExports.providers.SettingsServiceProvider = f;