function f() {
    function DialogsFactory($http, $mdDialog, $q) {
        this.showAddItemDialog = function(ev, options) {
            return $q(function (resolve, reject) {
                var dialogOptions = {
                    templateUrl: options.templateUrl || "selectitems.tmpl.html",
                    parent: angular.element(document.body),
                    controller: options.controller || "AddItemsToElementController",
                    resolve: options.resolve ? { items: options.resolve } : "undefined",
                    targetEvent: ev,
                    clickOutsideToClose: options.clickOutsideToClose || true,
                    fullscreen: options.customFullScreen || true
                };

                $mdDialog.show(dialogOptions).then(function(answer) {
                    resolve(answer);
                }, function(err) {
                    reject(err);
                });
            });
        };

        this.confirm = function(ev, title, textContent) {
            return $q(function(resolve, reject) {
                var confirm = $mdDialog.confirm()
                    .title(title)
                    .textContent(textContent)
                    .targetEvent(ev)
                    .ok("Подтвердить")
                    .cancel("Отмена");

                $mdDialog.show(confirm).then(function() {
                    resolve();
                }, function() {
                    reject.apply(this, arguments);
                });
            });
        };
    }

    return {
        $get: [
            '$http', '$mdDialog', '$q', function($http, $mdDialog, $q) {
                return new DialogsFactory($http, $mdDialog, $q);
            }
        ]
    };
}

itisExports.providers.DialogsProvider = f;