function f() {
    function ToolsFactory() {
        this.getObjectInfoCollection = function(object) {
            function link(key, value) {
                this.key = key;
                this.value = value;
            }

            var linkCollection = [];

            var keys = Object.getOwnPropertyNames(object);
            for (var i = 0; i < keys.length; i++) {
                linkCollection.push(new link(keys[i], object[keys[i]]));
            }

            return linkCollection;
        };
    }

    return {
        $get: [function() {
            return new ToolsFactory();
        }]
    };
}

itisExports.providers.ToolsProvider = f;