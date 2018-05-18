var TimeSheet = {
    showview: function ShowViewTimeSheet(timesheetid)
    {
        TimeSheet.showViewDeail(timesheetid);
    },
    showViewDeail: function (summaryid)
    {
        window.location.href = '#/Detail/' + summaryid;
    }
};

angular.module('app.controllers').controller('DashBoardCtrl', ['$scope', '$location', '$filter', '$timeout', 'DashboardServices', 'DrawTable', '$sce', '$routeParams', function ($scope, $location, $filter, $timeout, DashboardServices, DrawTable, $sce, $routeParams) {

    $scope.UserGroup = 'user';
        $scope.header = [{ disName_en: 'Link', sortOrder: 0, columnName: '', Width: '20', disType: 'a', isFilterControl: 'false', onclickFunction: TimeSheet.showview, linkparam: 'Id' },
            { disName_en: 'TS ID', sortOrder: 3, columnName: 'Id', Width: '40', disType: 'text', align: 'right' },
            { disName_en: 'Date Opened', sortOrder: 3, columnName: 'DateOpened', dateFormat: 'MM/dd/yyyy', Width: '40', disType: 'date'},
            { disName_en: 'WeekEndingDate', sortOrder: 3, columnName: 'EndDayOfWeek', dateFormat: 'MM/dd/yyyy', Width: '40', disType: 'date'},
            { disName_en: 'Week', sortOrder: 3, columnName: 'YearAndWeek', Width: '40', disType: 'text' },
            { disName_en: 'Employee Name', sortOrder: 3, columnName: 'DisplayName', Width: '40', disType: 'text'},
            { disName_en: 'Dept#', sortOrder: 3, columnName: 'GroupName', Width: '40', disType: 'text' },
            { disName_en: 'Total Hours', sortOrder: 3, columnName: 'TotalHours', Width: '40', disType: 'text' },
            { disName_en: 'Id', sortOrder: 3, columnName: 'Id', Width: '40',disType: 'text',isHidden:true }
        ]

    //for export 
        $scope.detail = [{ disName_en: 'Link', sortOrder: 0, columnName: '', Width: '20', disType: 'a', isFilterControl: 'false', onclickFunction: TimeSheet.showview, linkparam: 'Id' },
            { disName_en: 'Date', sortOrder: 3, columnName: 'TypeDate', dateFormat: 'MM/dd/yyyy', Width: '40', disType: 'date' },
            { disName_en: 'Category Header', sortOrder: 3, columnName: 'CategoryName', Width: '40', disType: 'text' },
            { disName_en: 'Category Name', sortOrder: 3, columnName: 'DetailName', Width: '40', disType: 'text' },
            { disName_en: 'Customer', sortOrder: 3, columnName: 'CategoryCustomerName', Width: '40', disType: 'text' },
            { disName_en: 'Week', sortOrder: 3, columnName: 'WeekToShow', Width: '40', disType: 'text' },
            { disName_en: 'Employee Name', sortOrder: 3, columnName: 'DisplayName', Width: '40', disType: 'text' },
            { disName_en: 'Act Hours', sortOrder: 3, columnName: 'ActHours', Width: '40', disType: 'text' },
            { disName_en: 'Comment', sortOrder: 3, columnName: 'Comment', Width: '40', disType: 'text' }
            ]

        $scope.ExportTimeSheetList = function () {
            var colInfo = $scope.header;
            var fieldsInfo = "[";
            for (var i = 0; i < colInfo.length; i++) {
                if (colInfo[i].isHidden != 'true')
                    fieldsInfo += "{disName_en: '" + colInfo[i].disName_en + "',sortOrder: " + colInfo[i].sortOrder + ", columnName: '" + colInfo[i].columnName + "', disType: '" + colInfo[i].disType + "', isHidden: '" + colInfo[i].isHidden + "'}, ";
            }
            fieldsInfo += "]";
            var fullSearch = $("#fullSearchBox").val();
            var postData = new Array();
            postData.fullSearch = fullSearch;
            postData.exportFileds = fieldsInfo;
            ShowExportInfo('/Export/ExportTimeSheetList', postData, 0);
        };
        $scope.ExportTimeSheetDetailList = function () {
            var colInfo = $scope.detail;
            var fieldsInfo = "[";
            for (var i = 0; i < colInfo.length; i++) {
                if (colInfo[i].isHidden != 'true')
                    fieldsInfo += "{disName_en: '" + colInfo[i].disName_en + "',sortOrder: " + colInfo[i].sortOrder + ", columnName: '" + colInfo[i].columnName + "', disType: '" + colInfo[i].disType + "', isHidden: '" + colInfo[i].isHidden + "'}, ";
            }
            fieldsInfo += "]";
            var fullSearch = $("#fullSearchBox").val();
            var postData = new Array();
            postData.fullSearch = fullSearch;
            postData.exportFileds = fieldsInfo;
            ShowExportInfo('/Export/ExportTimeSheetDetailList', postData, 0);
        };
        $scope.ShowTip = function () {
            $("#tiptip_holder").show();
        };
        $scope.HideTip = function () {
            $("#tiptip_holder").hide();
        };
    $scope.AddNewSummary = function (detailid)
    {
        window.location.href = '#/Detail/0';
    };

    $scope.searchEnter = function ()
    {
        if (window.event.keyCode == 13)
        {
            var searchstring = $("#txtSearch").val();

            t2v_StorageData.SavePageSearchCondition("dashboard", JSON.stringify({ "search": encodeURIComponent(searchstring)}));
            t2v_StorageData.DeleteLocalStorageValue("dashboard");

            window.location.href = "#/DashBoard/List/1/search/" + encodeURIComponent(searchstring);
        }
    };
    $scope.GetTimeSheetView = function (timesheetid)
    {
        DashboardServices.GetTimeSheet(timesheetid).then(function (summaryvalue) {
            //$scope.isSaved = true;
          
            var k = $sce;
                $scope.clearTimeSheet();
                var result = JSON.parse(summaryvalue.result);

                    $scope.isSaved = true;
                    $scope.TimeSheet.CategoryCustomerName = result.CategoryCustomerName;
                    $scope.TimeSheet.CategoryDetailName = result.CategoryDetailName;
                    $scope.TimeSheet.ActHours = result.ActHours;
                    $scope.TimeSheet.Comment = $sce.trustAsHtml(result.Comment);
                    $scope.TimeSheet.TypeDate = result.TypeDate;
                    $scope.$apply();
        });
    };


    $scope.searchContent = function () {

        //get search data by url
        var pageindex = $routeParams.pageindex;
        var searchcontent = $.trim($routeParams.searchcontent);

        $("#txtSearch").val(searchcontent);

        var param = '[{"columnName":"fullsearch","columnValue":"' + searchcontent + '","columnOperator":"cn"}]';

        //get data from storage firstly.
        

        DrawTable("divTimeSheet", $scope.header, "/DashBoard/GetList", param, 'dashboard', pageindex,true);
    };

    $scope.searchContent();

    var ue = UE.getEditor('editor');

}]);