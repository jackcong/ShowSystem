angular.module('app.controllers').controller('DetailCtrl', ['$scope', '$location', 'DetailServices', '$sce', function ($scope, $location, DetailServices, $sce) {

    $scope.CurrentUserId = parseInt($.cookie("UserID"));
    $scope.TimeSheet = new Object();
    $scope.TimeSheet.TempCustomerName = "";
    $scope.TimeSheet.CategoryCustomerID = 0;
    $scope.TimeSheet.CategoryCustomerName = "";
    $scope.TimeSheet.TempCategoryName = "";
    $scope.TimeSheet.CategoryDetailID = 0;
    $scope.TimeSheet.CategoryDetailName = "";
    $scope.TimeSheet.ActHours = "";
    $scope.TimeSheet.Comment = "";
    $scope.TimeSheet.TypeDate = "";
    //$scope.TimeSheet.TypeWeek = t2v_lib.DateCalculate.getWeek($scope.TimeSheet.TypeDate);
    $scope.isSaved = false;
    $scope.UserGroup = 0;
    $scope.TimeSheet.SummaryID = $scope.DetailID;
    $scope.TimeSheet.Id = 0;

    $scope.DetailID = $location.path().split('/')[2];

    //init week range
    $scope.weekRange = [];

    $scope.GetGroupByTimeSheet = function (listTimeSheet)
    {
        var groupTimeSheet = [];
        var categoryDetailName = "";
        var categoryCustomerName = "";

        for (i = 0; i < listTimeSheet.length; i++)
        {
            var isFind = false;
            for (j = 0; j < groupTimeSheet.length; j++)
            {
                if (groupTimeSheet[j].categoryDetailName == listTimeSheet[i].CategoryDetailName && groupTimeSheet[j].categoryCustomerName == listTimeSheet[i].CategoryCustomerName)
                {
                    isFind = true;
                    break;
                }
            }

            if (isFind == false)
            {
                if(listTimeSheet[i].CategoryName=="Project")
                {
                    groupTimeSheet.push({ "categoryDetailName": listTimeSheet[i].CategoryDetailName, "categoryCustomerName": listTimeSheet[i].CategoryCustomerName,"seq":1 });
                }
                else if (listTimeSheet[i].CategoryName == "R&D")
                {
                    groupTimeSheet.push({ "categoryDetailName": listTimeSheet[i].CategoryDetailName, "categoryCustomerName": listTimeSheet[i].CategoryCustomerName, "seq": 2 });
                }
                else if (listTimeSheet[i].CategoryName == "Sales&Marketing") {
                    groupTimeSheet.push({ "categoryDetailName": listTimeSheet[i].CategoryDetailName, "categoryCustomerName": listTimeSheet[i].CategoryCustomerName, "seq": 3 });
                }
                else if (listTimeSheet[i].CategoryName == "Admin&HR") {
                    groupTimeSheet.push({ "categoryDetailName": listTimeSheet[i].CategoryDetailName, "categoryCustomerName": listTimeSheet[i].CategoryCustomerName, "seq": 4 });
                }
                else if (listTimeSheet[i].CategoryName == "Other") {
                    groupTimeSheet.push({ "categoryDetailName": listTimeSheet[i].CategoryDetailName, "categoryCustomerName": listTimeSheet[i].CategoryCustomerName, "seq": 5 });
                }
            }
        }

        groupTimeSheet = groupTimeSheet.OrderByAsc(function (obj) { return obj.seq; });

        groupTimeSheet.push({ "categoryDetailName":"Total", "categoryCustomerName":"" });

        return groupTimeSheet;
    };

    DetailServices.GetDetailData($scope.DetailID).then(function (summaryvalue) {
        //set
        $scope.summary = JSON.parse(summaryvalue.summary);

        //get a new group by timesheet
        $scope.summaryGroup = $scope.GetGroupByTimeSheet($scope.summary.listTimeSheet);

        $scope.TimeSheet.SummaryID = $scope.summary.Id;
        $scope.DetailID = $scope.summary.Id;
        $scope.listcategory = JSON.parse(summaryvalue.category);
        $scope.AnalysisWorker($scope.listcategory, $scope.summary.listTimeSheet);

        if ($scope.summary.DateOpened == null) {

            var date = new Date();
            $scope.weekRange = t2v_lib.DateCalculate.getWeekDays(date);
        }
        else {
            var date = new Date($scope.summary.DateOpened);
            $scope.weekRange = t2v_lib.DateCalculate.getWeekDays(date);
        }
    });

    $scope.AnalysisWorker = function (listcategory, timesheet) {
        //after get timesheet data ,we calculate item.
        $scope.projectlist = new Array();

        var totalHour = 0;
        for (i = 0; i < timesheet.length; i++)
        {
            totalHour += parseFloat(timesheet[i].ActHours);
        }

        var AnalysisWorker = new Worker("Scripts/webworker/StatisticsAnalysis.js");
        AnalysisWorker.onmessage = function (evt) {
            $scope.$apply(function () { $scope.projectlist.push(evt.data); });
        };
        angular.forEach(listcategory, function (item) {
            var categoryArr = { "category": item, "timesheet": timesheet,"totalHour":totalHour };
            AnalysisWorker.postMessage(categoryArr);
        });
    };

    $scope.AddNewTimeSheet = function () {
        $scope.clearTimeSheet();
        $scope.isSaved = false;

        var date = new Date();
        $scope.TimeSheet.TypeDate = (date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate());
        $scope.TimeSheet.TypeWeek = t2v_lib.DateCalculate.getWeek(date);
        $scope.summary.DateOpened = date;

        $('#divMail').modal('show');
    };

    $scope.CheckSummaryExists = function (fullyear,week, detailid) {
        //var newdate = (date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate());
        DetailServices.CheckSummaryExists(fullyear,week, detailid).then(function (summaryvalue) {
            if (summaryvalue.result > 0) {

                if (confirm('Timesheet of week ' + week + ' already exists, do you want to open it?')) {
                    window.location.href = '#/Detail/' + summaryvalue.result;
                }
                else {
                    //clear the date.
                    $("#txtDate").val("");
                    return false;
                }
            }
        });
    };

    $scope.showComment = function (timesheetid) {
        DetailServices.GetCommentData(timesheetid).then(function (commentvalue) {
            $("#divCommentContent").html("");
            $("#divCommentContent").html(commentvalue.result);
            $('#divComment').modal('show');
        });
    };

    $scope.clearTimeSheet = function () {

        $scope.TimeSheet.TempCustomerName = "";
        $scope.TimeSheet.CategoryCustomerID = 0;
        $scope.TimeSheet.CategoryCustomerName = "";

        $scope.TimeSheet.TempCategoryName = "";
        $scope.TimeSheet.CategoryDetailID = 0;
        $scope.TimeSheet.CategoryDetailName = "";

        $scope.TimeSheet.ActHours = "";
        $scope.TimeSheet.Comment = "";
        //$scope.TimeSheet.TypeDate = "";
        //$scope.TimeSheet.TypeWeek = t2v_lib.DateCalculate.getWeek($scope.TimeSheet.TypeDate);
        $scope.TimeSheet.SummaryID = $scope.DetailID;
        $scope.TimeSheet.Id = 0;

        $("#txtCategory").val("");
        //$("#txtDate").val("");
    };

    $scope.CheckDateRange = function (selectedDate)
    {
        var tempWeek = t2v_lib.DateCalculate.getWeek(selectedDate);

        if ($scope.summary.TypeWeek != null && $scope.summary.Id != 0) {
            if (tempWeek != $scope.summary.TypeWeek) {
                alert('The date you selected is in week ' + tempWeek + ',Please select date in week ' + $scope.summary.TypeWeek);
                //$("#txtDate").val("");
                $scope.TimeSheet.TypeWeek = "";
                $scope.$apply();
                return false;
            }
        }
        return true;
    };

    $scope.SaveTimeSheet = function () {
        //with out detail timesheet

        if ($scope.timesheetform.$valid) {

            //check and make sure
            if (!$scope.isSaved) {
                if ((typeof $scope.summary.DateOpened) == "object" && $scope.CheckDateRange($scope.summary.DateOpened) == false) {
                    return;
                }
            }
            //check current select day's week equal or not equal exists week.
            if ($scope.summary.Id != 0)
            {
                if ($scope.summary.TypeWeek != $scope.TimeSheet.TypeWeek)
                {
                    alert('The date you selected is in week ' + $scope.TimeSheet.TypeWeek + ',Please select date in week ' + $scope.summary.TypeWeek);
                    return;
                }
            }
            
            $("#btnSubmit").css("display", 'none');

            DetailServices.SaveTimeSheet($scope.summary, $scope.TimeSheet).then(function (summaryvalue) {
                if (summaryvalue.result > 0) {
                    $scope.DetailID = summaryvalue.result;

                    if (!$scope.isSaved) {
                        $scope.clearTimeSheet();

                        var date = new Date();
                        $scope.TimeSheet.TypeDate = (date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate());
                        $scope.TimeSheet.TypeWeek = t2v_lib.DateCalculate.getWeek(date);
                    }
                    else {
                        alert('Update successful.');
                        $('#divMail').modal('hide');
                    }

                    $("#btnSubmit").css("display", 'block');
                    //reget the data after user input
                    DetailServices.GetDetailData($scope.DetailID).then(function (summaryvalue) {
                        //set 
                        $scope.summary = JSON.parse(summaryvalue.summary);

                        $scope.summaryGroup = $scope.GetGroupByTimeSheet($scope.summary.listTimeSheet);

                        var date = new Date($scope.summary.DateOpened);
                        $scope.weekRange = t2v_lib.DateCalculate.getWeekDays(date);

                        $scope.listcategory = JSON.parse(summaryvalue.category);
                        $scope.AnalysisWorker($scope.listcategory, $scope.summary.listTimeSheet);
                    });
                }
                else if (summaryvalue.result==-1) {
                    alert('Last week need 40 hours.');
                    $("#btnSubmit").css("display", 'block');
                }
            });
        }
    };

    $scope.DeleteSummary = function () {
        if (confirm('Are you sure you want to delete?')) {

            DetailServices.DeleteSummary($scope.DetailID).then(function (resultvalue) {
                window.location.href = '#/DashBoard';
            });
        }
    };

    $scope.EditTimeSheet = function (timesheetid) {

        DetailServices.GetTimeSheet(timesheetid).then(function (summaryvalue) {
            var timesheet = JSON.parse(summaryvalue.result);

            $scope.clearTimeSheet();

            $scope.TimeSheet.UserID = timesheet.UserID;
            $scope.TimeSheet.TempCustomerName = timesheet.DetailName;
            $scope.TimeSheet.CategoryCustomerID = timesheet.CategoryCustomerID;
            $scope.TimeSheet.CategoryCustomerName = timesheet.CategoryCustomerName;
            $scope.TimeSheet.TempCategoryName = timesheet.DetailName;
            $scope.TimeSheet.CategoryDetailID = timesheet.CategoryDetailID;
            $scope.TimeSheet.CategoryDetailName = timesheet.CategoryDetailName;
            $scope.TimeSheet.CategoryName = timesheet.CategoryName;
            $scope.TimeSheet.DetailName = timesheet.DetailName;
            $scope.TimeSheet.ActHours = timesheet.ActHours;
            $scope.TimeSheet.OldActHours = timesheet.ActHours;
            $scope.TimeSheet.Comment = timesheet.Comment;
            $scope.TimeSheet.TypeDate = timesheet.TypeDate;
            $scope.TimeSheet.TypeWeek = timesheet.TypeWeek;
            $scope.TimeSheet.SummaryID = $scope.summary.Id;
            $scope.TimeSheet.TypeYear = timesheet.TypeYear;
            $scope.TimeSheet.Id = timesheet.Id;

            $scope.isSaved = true;

            $('#divMail').modal('show');
        });
    };

    $scope.DeleteTimeSheet = function () {

        if (confirm('Are you sure you want to delete?')) {
            DetailServices.DeleteTimeSheet($scope.TimeSheet.Id).then(function (summaryvalue) {

                $('#divMail').modal('hide');

                DetailServices.GetDetailData($scope.DetailID).then(function (summaryvalue) {
                    //set 
                    $scope.summary = JSON.parse(summaryvalue.summary);
                    $scope.summaryGroup = $scope.GetGroupByTimeSheet($scope.summary.listTimeSheet);
                    $scope.listcategory = JSON.parse(summaryvalue.category);
                    $scope.AnalysisWorker($scope.listcategory, $scope.summary.listTimeSheet);
                });

            });
        }
    };

    $scope.BindAutocompleteClick = function () {
        if ($("#txtCategory").autocomplete("widget").is(":visible")) {
            $("#txtCategory").autocomplete("close"); return;
        }
        $("#txtCategory").autocomplete("search", "!@");
    };

    $scope.getHour = function (timesheet, index) {
        var searchDate = new Date($scope.weekRange[index]);
        var typeDate = new Date(timesheet.TypeDate);

        if (searchDate.getYear() == typeDate.getYear() && searchDate.getMonth() == typeDate.getMonth() && searchDate.getDate() == typeDate.getDate()) {
            return timesheet.ActHours;
        }
        else {
            return "";
        }
    };

    $scope.getTotalHour = function (index) {
        var searchDate = new Date($scope.weekRange[index]);
        var singleDayTotal = 0;

        for (i = 0; i < $scope.summary.listTimeSheet.length; i++) {
            var singleTimeSheet = $scope.summary.listTimeSheet[i];
            var typeDate = new Date(singleTimeSheet.TypeDate);
            if (searchDate.getYear() == typeDate.getYear() && searchDate.getMonth() == typeDate.getMonth() && searchDate.getDate() == typeDate.getDate()) {
                singleDayTotal += singleTimeSheet.ActHours;
            }
        }
        return singleDayTotal;
    };

    $scope.getTotalHourByCategory = function (categoryName)
    {
        var actHour = 0;
        for (i = 0; i < $scope.summary.listTimeSheet.length; i++) {

            if (categoryName == 'Total')
            {
                actHour += $scope.summary.listTimeSheet[i].ActHours;
            }
            else
            {
                if ($scope.summary.listTimeSheet[i].CategoryDetailName == categoryName) {
                    actHour += $scope.summary.listTimeSheet[i].ActHours
                }
            }
            
        }
        return actHour;
    };

    DetailServices.GetCategory().then(function (summaryvalue) {
        $scope.listcategoryforSelect = JSON.parse(summaryvalue.result);
        //set autocomplete
        $("#txtCategory").autocomplete({
            delay: 100,
            open: function () {
                //$(this).data("autocomplete").menu.elemesnt.width(240);
            },
            select: function (event, data) {

                $scope.TimeSheet.CategoryDetailID = data.item.Id;
                $scope.TimeSheet.CategoryDetailName = data.item.CategoryHeaderName + "-" + data.item.CategoryDetailName + (data.item.CategoryCustomerName == "" ? "" : ("-" + data.item.CategoryCustomerName));
                $scope.TimeSheet.CategoryCustomerID = data.item.CategoryCustomerId;
                $scope.TimeSheet.CategoryCustomerName = data.item.CategoryCustomerName;
                $scope.TimeSheet.TempCategoryName = data.item.CategoryHeaderName + "-" + data.item.CategoryDetailName + (data.item.CategoryCustomerName == "" ? "" : ("-" + data.item.CategoryCustomerName));

                $scope.timesheetform.txtCategory.$valid = true;


                $("#txtCategory").val(data.item.CategoryHeaderName + "-" + data.item.CategoryDetailName + (data.item.CategoryCustomerName == "" ? "" : ("-" + data.item.CategoryCustomerName)));

                return false;
            },
            source: function (request, response) {
                var searchContent = request.term;
                var res;
                if (searchContent == "!@") {
                    res = $scope.listcategoryforSelect;
                }
                else {
                    res = $scope.listcategoryforSelect.filter(function (item) { var regex = new RegExp(searchContent, "i"); return (item.CategoryHeaderName.search(regex) >= 0 || item.CategoryDetailName.search(regex) >= 0 || item.CategoryCustomerName.search(regex) >= 0); });
                    if (res.length > 50) {
                        for (i = res.length - 1; i >= 50; i--) {
                            res.pop();
                        }
                    }
                }

                response(res);
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li>").data("ui-autocomplete-item", item)
                                .append("<a>" + item.CategoryHeaderName + "-" + item.CategoryDetailName + (item.CategoryCustomerName == "" ? "" : ("-" + item.CategoryCustomerName)) + "</a>")
                                .appendTo(ul);
        };
    });
}]).filter('to_trusted', ['$sce', function () {
    return function (text) {
        return $sce.trustAsHtml(text);
    }
}]);