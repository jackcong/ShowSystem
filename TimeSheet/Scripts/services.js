var app = angular.module('app.services', []);
app.service('SharedService', ['$http', 'transformRequestAsFormPost', '$q', function ($http, transformRequestAsFormPost, $q) {
    this.LogOut = function () {
        var deferred = $q.defer();
        $http({
            method: 'get',
            url: '/Account/LogOff',
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };
}]);

app.service('DashboardServices', ['$http', 'transformRequestAsFormPost', '$q', function ($http, transformRequestAsFormPost, $q) {
    this.GetStatisticsData = function () {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/GetEditInfo',
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };
}]);