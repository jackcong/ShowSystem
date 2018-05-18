angular.module('app.controllers').controller('StatisticsCtrl', ['$scope', 'StatisticsServices', function ($scope, StatisticsServices) {

    var tempDate = new Date();
    var firstTime = tempDate.getDate() + '/' + (tempDate.getMonth() + 1) + '/' + tempDate.getFullYear();
    $scope.datesearch = firstTime;

    StatisticsServices.GetStatisticsData().then(function (summaryvalue) {
        $scope.listcategory = JSON.parse(summaryvalue.category);
        $scope.timesheet = JSON.parse(summaryvalue.timesheet);

        //after get timesheet data ,we calculate item.
        $scope.projectlist = new Array();

        var AnalysisWorker = new Worker("Scripts/webworker/StatisticsAnalysis.js");

        AnalysisWorker.onmessage = function (evt) {
            $scope.$apply(function () { $scope.projectlist.push(evt.data); });
        };
        angular.forEach($scope.listcategory, function (item)
        {
            var categoryArr = { "category": item, "timesheet": $scope.timesheet };
            AnalysisWorker.postMessage(categoryArr);
        });
    });

    $scope.GetDateSource = function () {

        var dateSource = [];

        for (i = 0; i < 15; i++) {
            var d = new Date();
            var newdate = new Date(d.setDate(d.getDate() - i));
            dateSource.push(newdate.getDate() + '/' + (newdate.getMonth() + 1) + '/' + newdate.getFullYear());
        }
        return dateSource;
    };

    $scope.selectUserHours = function () {
        StatisticsServices.GetUserHours($scope.datesearch).then(function (result) {
            $scope.userhours = JSON.parse(result.result);
        });
    };

    StatisticsServices.GetUserHours($scope.datesearch).then(function (result) {
        try{
            $scope.userhours = JSON.parse(result.result);
        }
        catch (e) {
            dialogs.notify('T2VSoft!', "Object parse error, please find administrator to get help.");
            return;
        }
    });

}]);
