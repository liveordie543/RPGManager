var rpgManagerApp = angular.module('rpgManagerApp', ['ngRoute']);

rpgManagerApp.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'Angular/Views/CharacterManager.html',
            controller: 'characterManagerController'
        });
});