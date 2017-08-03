function Log(message, log) {
    this.message = message;
    this.saved = false;

    if (typeof log !== "undefined" && log !== null) {
        this.idLog = log.idLog;
        this.saved = true;
        this.dateCreated = log.dateCreated;
        this.link = `GetLog/${log.idLog}/`;
    }

    this.isShow = false;
}

function f() {
    var provider = this;
    var logs = [];

    function saveLog(message) {
        return provider.$http.post("/Security/Log", { message : message}).then(function(resp) {
            return provider.$q.resolve(resp);
        }, function(err) {
            return provider.$q.reject(err);
        });
    }

    function saveUnsavedLogs(logs) {
        return provider.$q(function (resolve, reject) {
            var promises = [];
            logs.forEach(function(item) {
                var savePromise = provider.$q(function(resolve, reject) {
                    saveLog(item.message).then(function (resp) {
                        item.dateCreated = resp.data.dateCreated;
                        item.saved = true;
                        item.idLog = resp.data.idLog;
                        item.link = `GetLog/${resp.data.idLog}/`;

                        //                    var unsavedLogs = logs.filter(function(item) {
                        //                        return !item.saved;
                        //                    });

                        //                    if (unsavedLogs.length === 0)
                        //                        resolve();

                        resolve();
                    }, function (err) {
                        reject(err);
                    });
                });

                promises.push(savePromise);
            });

            provider.$q.all(promises).then(function() {
                resolve();
            }, function(err) {
                reject(err);
            });
        });
    }

    function saveLogsByIteration() {
        var timeoutPromise = provider.$timeout(function() {
            var unsavedLogs = logs.filter(function(item) {
                return !item.saved;
            });

            saveUnsavedLogs(unsavedLogs).then(function() {
                
            }, function(err) {
                saveLogsByIteration();
            });
        }, 10000);
    }

    function LogFactory($q, $timeout) {
        provider.$q = $q;
        provider.$timeout = $timeout;

        this.log = function (message) {
            //var log = new Log(message);

            return $q(function (resolve, reject) {
                var injector = angular.injector(["ng"]);
                provider.$http = injector.get("$http");

                saveLog(message).then(function (resp) {
                    var log = new Log(message, resp.data);
                    logs.push(log);

                    resolve(log);
                }, function (err) {
                    var message = "Произошла ошибка, связанная с сетью. " + err.data;
                    var log = new Log(message);
                    logs.push(log);

                    saveLogsByIteration();

                    resolve(log);
                });
            });
        };

        this.getActiveLogs = function() {
            return logs.filter(function(item) {
                return !item.isShow;
            });
        };      
    }

    return {
        $get: function($q, $timeout) {
            return new LogFactory($q, $timeout);
        }
    };
}

itisExports.providers.LogServiceProvider = f;