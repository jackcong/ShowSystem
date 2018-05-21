
angular.module('app.controllers').controller('DashBoardCtrl', ['$scope', '$location', '$filter', '$timeout', 'DashboardServices', 'DrawTable', '$sce', '$routeParams', function ($scope, $location, $filter, $timeout, DashboardServices, DrawTable, $sce, $routeParams) {
    var ue = UE.getEditor('editor');

    ue.addListener('contentChange', function () {

        $scope.ShowSys[$scope.CategoryIndex].listSubCategory[$scope.SubCategoryIndex].Content=ue.getContent();
    });
 


    $scope.ShowSys = [];
    $scope.CategoryIndex = 0;
    $scope.SubCategoryIndex = 0;

    DashboardServices.GetStatisticsData().then(function (result) {
        $scope.ShowSys = JSON.parse(result.showsys);
        $scope.ShowContent();
    });

    $scope.SelectCategory = function (index) {
        $scope.CategoryIndex = index;
        $scope.SubCategoryIndex = 0;
        $scope.ShowContent();
        
    };

    $scope.ShowContent = function ()
    {
        var content = $scope.ShowSys[$scope.CategoryIndex].listSubCategory[$scope.SubCategoryIndex].Content;
        if (content == null)
        {
            content = "";
        }
        ue.setContent(content);
    };

    $scope.SelectSubCategory = function (index)
    {
        $scope.SubCategoryIndex = index;

        $scope.ShowContent();
    };

}]);