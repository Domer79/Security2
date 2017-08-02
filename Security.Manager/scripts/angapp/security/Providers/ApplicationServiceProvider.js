function f() {
    function ApplicationServiceFactory($http) {
        this.getApplications = function() {
            return $http.get("GetApplications").then(function(resp) {
                let applications = resp.data;
                for (let i = 0; i < applications.length; i++) {
                    applications[`${applications[i].AppName}`] = applications[i];
                }

                return applications;
            });
        };

//        this.createApplication = function(app) {
//            return $http.post("CreateApplication", app).then(function (response) {
//                return response.data;
//            });
//        };

        this.deleteApplication = function (app) {
            return $http.post("DeleteApplication", app);
        };

        this.getApplicationByName = function(appname) {
            return $http.get("GetApplication", { appname: appname }).then(function(resp) {
                return resp.data;
            });
        }
    }

    return {
        $get: ['$http', function($http) {
            return new ApplicationServiceFactory($http);
        }]
    }
}

itisExports.providers.ApplicationServiceProvider = f;