'use strict';

/*
 * Directives
 **/
var app = angular.module('app.directives', []);
app.constant('TypeEnum', {
    AuthMapper: {
        "Draft": "Status_1.png",
        "Pending": "Status_1.png",
        "Requested-Modify": "Status_1.png",
        "Requested-Reject": "Status_1.png",
        "Not Submitted": "Status_2.png",
        "Submitted": "Status_2.png",
        "Requested": "Status_2.png",
        "Requested-TPV": "Status_2.png",
        "Requested-Response": "Status_2.png",
        "Requested-Resubmit": "Status_2.png",
        "Quoted": "Status_3.png",
        "Paid": "Status_3.png",
        "Claim": "Status_4.png",
        "Closed": "Status_5.png",
        "Closed-Requote": "Status_5.png"
    }
});

app.directive('match', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        scope: {
            match: '='
        },
        link: function (scope, elem, attrs, ctrl) {
            scope.$watch(function () {
                var modelValue = ctrl.$modelValue || ctrl.$$invalidModelValue;
                return (ctrl.$pristine && angular.isUndefined(modelValue)) || scope.match === modelValue;
            }, function (currentValue) {
                ctrl.$setValidity('match', currentValue);
            });
        }
    };
});

app.directive('unique', ['$http', function ($http) {
    return {
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.$watch(attrs.ngModel, function (currValue) {
                $http({
                    method: 'get',
                    url: 'http://api.mewemusic.com/group/available',
                    params: { 'name': currValue }
                }).success(function (data, status, headers, cfg) {
                    c.$setValidity('unique', eval(data));
                }).error(function (data, status, headers, cfg) {
                    //c.$setValidity('unique', true);
                });
            });
        }
    }
}]);
app.directive('datepicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                element.addClass('datefield');
                element.datepicker({
                    dateFormat: 'yy-mm-dd',
                    onSelect: function (date) {
                        ngModelCtrl.$setViewValue(date);
                        scope.$apply();
                    }
                });
            });
        }
    }
});

app.directive('dateFormat', ['$filter', function ($filter) {
    var dateFilter = $filter('date');
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {

            function formatter(value) {
                return dateFilter(value, 'dd/MM/yyyy'); //format
            }

            function parser() {
                return ctrl.$modelValue;
            }

            ctrl.$formatters.push(formatter);
            ctrl.$parsers.unshift(parser);
        }
    };
}]);

app.directive('myCurrentTime', ['$interval', 'dateFilter',
    function ($interval, dateFilter) {
        // return the directive link function. (compile function not needed)
        return function (scope, element, attrs) {
            var format,  // date format
                stopTime; // so that we can cancel the time updates

            // used to update the UI
            function updateTime() {
                element.text(dateFilter(new Date(), format));
            }

            // watch the expression, and update the UI on change.
            scope.$watch(attrs.myCurrentTime, function (value) {
                format = value;
                updateTime();
            });

            stopTime = $interval(updateTime, 1000);

            // listen on DOM destroy (removal) event, and cancel the next UI update
            // to prevent updating time after the DOM element was removed.
            element.on('$destroy', function () {
                $interval.cancel(stopTime);
            });
        }
    }]);
app.directive('bindHtmlUnsafe', function ($compile) {
    return function ($scope, $element, $attrs) {
        var compile = function (newHTML) { // Create re-useable compile function
            newHTML = $compile(newHTML)($scope); // Compile html
            $element.html('').append(newHTML); // Clear and append it
        };
        var htmlName = $attrs.bindHtmlUnsafe; // Get the name of the variable 
        // Where the HTML is stored
        $scope.$watch(htmlName, function (newHTML) { // Watch for changes to 
            // the HTML
            if (!newHTML) return;
            compile(newHTML);   // Compile it
        });
    };
});
app.directive('ngFocus', [function () {
    var FOCUS_CLASS = "ng-focused";
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {
            ctrl.$focused = false;
            element.bind('focus', function (evt) {
                element.addClass(FOCUS_CLASS);
                scope.$apply(function () { ctrl.$focused = true; });
            }).bind('blur', function (evt) {
                element.removeClass(FOCUS_CLASS);
                scope.$apply(function () { ctrl.$focused = false; });
            });
        }
    }
}]);
app.directive('compareTo', [function () {
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {

            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue == scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
}]);
app.directive('barchart', function () {

    return {
        restrict: 'E',
        template: '<div></div>',
        replace: true,
        link: function ($scope, element, attrs) {

            var data = $scope[attrs.data],
                xkey = $scope[attrs.xkey],
                ykeys = $scope[attrs.ykeys],
                labels = $scope[attrs.labels];

            var setData = function () {
                console.log('inside setData function');
                Morris.Bar({
                    element: element,
                    data: data,
                    xkey: xkey,
                    ykeys: ykeys,
                    labels: labels
                });
            };

            // The idea here is that when data variable changes, 
            // the setData() is called. But it is not happening.
            attrs.$observe('data', setData);
        }

    };

});
app.directive('mailListAutoComplete', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            $(function () {

                elm.autocomplete({
                    delay: 100,
                    open: function () {
                        //$(".ui-autocomplete").css("z-index", "10000");
                        $(this).autocomplete("widget").css("z-index", "10000");
                    },
                    select: function (event, data) {
                        scope.$apply(scope.mail.Reply = data.item.value);
                        return false;
                    },
                    source: function (request, response) {
                        var searchContent = request.term;
                        if (searchContent == "!@#$%") {
                            var res = scope.mail.ReplyLst.slice(0, 10);
                            response(res);
                            return;
                        }

                        var res = scope.mail.ReplyLst.filter(function (item) { var regex = new RegExp(searchContent, "i"); return item.search(regex) >= 0; });
                        if (res.length > 10) {
                            for (var i = res.length - 1; i >= 10; i--) {
                                res.pop();
                            }
                        }
                        response(res);
                    }
                })
                .autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>")
                        .append("<a>" + item.value + "</a>")
                        .appendTo(ul);
                };

            });
        }
    };
});

app.directive('mailListAutoCompleteButton', function () {
    return {
        link: function (scope, elm, attrs, ctrl) {
            $(function () {
                elm.click(function () {
                    if ($(".txt_input_Email_AutoComplete").autocomplete("widget").is(":visible")) { $(".txt_input_Email_AutoComplete").autocomplete("close"); return; }
                    $(".txt_input_Email_AutoComplete").autocomplete("search", "!@#$%");
                });
            });
        }
    };
});

app.filter('orgtype', function () {
    return function (types) {
        var newtypes = [];
        for (var i = 0; i < types.length; i++) {
            if (i > 1) {
                newtypes.push(types[i]);
            }
        }
        return newtypes;
    }
});
app.filter('toYYYYMMDD', function () {
    return function (str) {
        if (str == null || str == '')
            return "";
        return moment(str).format('YYYY-MM-DD');
    }
});


app.filter('collectasnum', function () {
    return function (colls, num) {
        var newcolls = [];
        for (var i = 0; i < colls.length; i++) {
            if (i < num) {
                newcolls.push(colls[i]);
            }
        }
        return newcolls;
    }
});

app.filter('fromNow1', function () {
    return function (date) {
        return moment(date).fromNow();
    }
});

app.filter('fromNow', function () {
    return function (date) {
        return jQuery.timeago(date);
    }
});

app.filter('ellipsis', function () {
    return function (str) {
        var splitted = str.split(' ');
        var res;
        if (splitted.length > 3) {
            res = splitted.slice(0, 3).join(' ') + '...';
        } else {
            res = splitted.join(' ');
        }
        if (res.length > 20) {
            res = res.substring(0, 17) + '...';
        }
        return res;
    }
});
app.filter('getByProperty', function () {
    return function (propertyName, propertyValue, collection) {
        var i = 0, len = collection.length;
        for (; i < len; i++) {
            if (collection[i][propertyName] == +propertyValue) {
                return collection[i];
            }
        }
        return null;
    }
});
app.filter('trimToLenOrSpc', function () {
    return function (str, len) {
        if (!str) {
            return '';
        }
        if (str.length > len) {
            var pres = str.slice(0, len);
            pres += "...";
            return pres;
        }
        else
            return str;
    }
});
app.filter('decimaldigits', function () {
    return function (str, len) {
        if (!str) {
            return '';
        }
        else {
            return parseFloat(str).toFixed(len);
        }
    }
});
app.filter('ellipsis15', function () {
    return function (str) {
        if (!str)
            return str;

        var splitted = str.split(' ');
        var res;
        if (splitted.length > 30) {
            res = splitted.slice(0, 29).join(' ') + '...';
        } else {
            res = splitted.join(' ');
        }
        if (res.length > 150) {
            res = res.substring(0, 147) + '...';
        }
        return res;
    }
});
app.filter('convertToDate', function () {
    return function (str) {
        if (str == undefined || str == null)
            return "";
        var strSplit = str.split('-');
        if (strSplit.length > 0) {
            var arr = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec']
            var month = (arr.indexOf(strSplit[1]) + 1);
            if (month < 10)
                month = "0" + month;
            return strSplit[2] + "-" + month + "-" + strSplit[0];
        }
        return str;
    }
});
app.filter('nl2p', function () {
    return function (text) {
        text = String(text).trim();
        return (text.length > 0 ? '<p>' + text.replace(/[\r\n]+/g, '</p><p>') + '</p>' : null);
    }
});

app.filter('unsafe', function ($sce) { return $sce.trustAsHtml; });
