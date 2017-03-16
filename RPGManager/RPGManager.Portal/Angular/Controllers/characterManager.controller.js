rpgManagerApp.controller('characterManagerController', function ($scope) {
    $scope.characters = [{Key: 'StubKey', Name: 'StubName', Race: 'StubRace', Class: 'StubClass', Level: 'StubLevel'}];

    $scope.setActiveCharacter = function (event, characterKey) {
    };

    $scope.deleteCharacter = function (event, characterKey) {
    };
});