function provider() {
    var showed = false;

    function ToastFactory($mdToast, $q) {
        this.show = function(options) {
            return $q(function (resolve, reject) {
                showed = true;
                $mdToast.show(options).then(function (response, arg2) {
                    return resolve(response);
                }, function(arg1, arg2, arg3) {
                    debugger;
                });
            });
        };

        this.simple = function() {
            return $mdToast.simple();
        };

        this.hide = function(response) {
            showed = false;
            $mdToast.hide(response);
        }

        Object.defineProperty(this, "showed", {
            get: function() {
                return showed;
            }
        });
    }

    return {
        $get: function ($mdToast, $q) {
            return new ToastFactory($mdToast, $q);
        }
    };
}

itisExports.providers.ToastProvider = provider;