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

}]);

app.service('StatisticsServices', ['$http', 'transformRequestAsFormPost', '$q', function ($http, transformRequestAsFormPost, $q) {
    this.GetStatisticsData = function () {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/GetStatisticsData',
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };


    this.GetUserHours = function (datetime) {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/GetUserHours',
            data: { dt:datetime }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    
}]);

app.service('DetailServices', ['$http', 'transformRequestAsFormPost', '$q', function ($http, transformRequestAsFormPost, $q) {
    this.GetDetailData = function (detailid) {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/GetDetailData',
            data: { DetailID: detailid }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    this.SaveTimeSheet = function (summary,model) {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/SaveTimeSheet',
            data: { stsm: summary, tsm: model }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    this.GetCategory = function () {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/GetCategory'
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    this.GetCommentData = function (timesheetid)
    {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/GetCommentData',
            data: { timesheetid: timesheetid }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };
    this.DeleteSummary = function (summaryid)
    {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/DeleteSummary',
            data: { summaryid: summaryid }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    this.GetTimeSheet = function (timesheetid) {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/GetTimeSheet',
            data: { timesheetid: timesheetid }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    
    this.DeleteTimeSheet = function (timesheetid) {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/DeleteTimeSheet',
            data: { timesheetid: timesheetid }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    this.CheckSummaryExists = function (year,week,detailid) {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/DashBoard/CheckSummaryExists',
            data: {year:year,week:week,detailid:detailid}
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    }

}]);


app.service('SettingServices', ['$http', 'transformRequestAsFormPost', '$q', function ($http, transformRequestAsFormPost, $q) {

    this.GetFullCategory = function () {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/Setting/GetFullCategory'
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    this.SaveNewNode = function (Id,level,nodeName) {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/Setting/SaveNewNode',
            data: { Id: Id,level:level,nodeName:nodeName}
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    this.SaveNode = function (Id, level, nodeName) {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/Setting/SaveNode',
            data: { Id: Id, level: level, nodeName: nodeName }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    };

    this.DeleteNode = function (Id,level)
    {
        var deferred = $q.defer();
        $http({
            method: 'post',
            url: '/Setting/DeleteNode',
            data: { Id: Id, level: level}
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (d, s, h, c) {
            deferred.resolve(d.message);
        });
        return deferred.promise;
    }

}]);