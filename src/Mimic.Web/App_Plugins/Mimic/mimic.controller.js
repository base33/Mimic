angular.module("umbraco").controller("mimicCodeGenerator", [
    "$scope", "$http", "$timeout", function ($scope, $http, $timeout) {
        $scope.documentTypes = [];
        $scope.documentTypeCode = '';
        $scope.parameters = [];
        $scope.loading = false;


        $scope.loadDocumentTypes = function () {
            $scope.loading = true;
            $http.get("/umbraco/backoffice/api/mimicapi/GetDocumentTypes").then(function (resp) {
                $scope.documentTypes = resp.data;
                $scope.loading = false;
            });
        };

        $scope.generateCode = function (documentTypeId) {
            $scope.loading = true;
            $scope.documentTypeCode = '';
            $http.get("/umbraco/backoffice/api/mimicapi/GetDocumentTypeCode?documentTypeId=" + documentTypeId).then(function (resp) {
                var trimmed = trimChar(resp.data, '"');
                var modified = trimmed.replace(/\\/g, '')
                $scope.documentTypeCode = modified;
                $scope.loading = false;
            });
        };

    }
]).filter('splitLines', function () {
    return function (text) {
        var modified = text.replace(/"/g, '').split(/\\r\\n/g);
        return modified;
    };
});


function trimChar(string, charToRemove) {
    while (string.charAt(0) == charToRemove) {
        string = string.substring(1);
    }

    while (string.charAt(string.length - 1) == charToRemove) {
        string = string.substring(0, string.length - 1);
    }

    return string;
}