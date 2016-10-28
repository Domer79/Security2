﻿//#region Helpers

/**
 * Возвращает имя функции. Взято с http://stackoverflow.com/questions/2648293/javascript-get-function-name
 * @param {} func 
 * @returns {} 
 */
function getFunctionName(func) {
    // Match:
    // - ^          the beginning of the string
    // - function   the word 'function'
    // - \s+        at least some white space
    // - ([\w\$]+)  capture one or more valid JavaScript identifier characters
    // - \s*        optionally followed by white space (in theory there won't be any here,
    //              so if performance is an issue this can be omitted[1]
    // - \(         followed by an opening brace
    //
    var result = /^function\s+([\w\$]+)\s*\(/.exec(func.toString());

    return result ? result[1] : '' // for an anonymous function there won't be a match
}

function NotImplementedException(message) {
    this.message = message;
    var lastPart = new Error().stack.match(/[^\s]+$/);
    this.stack = `${this.name} at ${lastPart}`;
}

NotImplementedException.prototype = Object.create(Error.prototype);
NotImplementedException.prototype.name = "NotImplementedException";
NotImplementedException.prototype.message = "";
NotImplementedException.prototype.constructor = NotImplementedException;

//#endregion

function AbstractFeature() { }

AbstractFeature.prototype.__getFeatureName = function(ctor) {
    if (typeof ctor !== "function")
        throw new Error("Controller parameter must be a Function");

    var funcName = getFunctionName(ctor);

    if (funcName === "")
        throw new Error("No name of the controller function");

    return funcName;
}

AbstractFeature.prototype.registerFeatures = function(angularApp) {
    var featureNames = Object.getOwnPropertyNames(this);
    for (var i = 0; i < featureNames.length ; i++) {
        var feature = this[featureNames[i]];
        this.registerFeature(angularApp, featureNames[i], feature);
    }
}

AbstractFeature.prototype.registerFeature = function (angularApp, featureName, feature) {
    throw new NotImplementedException();
}

function Components() { }

Components.prototype = Object.create(AbstractFeature.prototype);
Components.prototype.constructor = Components;

Components.prototype.registerFeature = function (angularApp, componentName, component) {
    componentName = componentName.substr(0, componentName.lastIndexOf("Component"));
    angularApp.component(componentName, new component());
}

function Controllers() { }

Controllers.prototype = Object.create(AbstractFeature.prototype);
Controllers.prototype.constructor = Controllers;

Controllers.prototype.registerFeature = function (angularApp, controllerName, controller) {
    angularApp.controller(controllerName, controller);
}

function Providers() { }

Providers.prototype = Object.create(AbstractFeature.prototype);
Providers.prototype.constructor = Providers;

Providers.prototype.registerFeature = function(angularApp, providerName, provider) {
    angularApp.provider(providerName, provider);
}

function Exports() {
    this.components = new Components();
    this.controllers = new Controllers();
    this.providers = new Providers();
}

var itisExports = new Exports();