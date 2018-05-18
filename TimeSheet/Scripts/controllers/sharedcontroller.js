angular.module('app.controllers')
    .controller('SharedCtrl', ['$scope', '$timeout', '$http', '$window', 'transformRequestAsFormPost', '$location', '$rootScope', 'TypeEnum', 'dialogs', function ($scope, $timeout, $http, $window, transformRequestAsFormPost, $location, $upload, $rootScope, TypeEnum, dialogs) {
        $scope.compares = [];
        $scope.userType = "";
        $scope.displayName = "";
        $scope.emailCount = 0;

        if (sessionStorage.getItem("usertype") == 0)
            $scope.userType = "Creator";
        else
            $scope.userType = "Reviewer";
        $scope.displayName = sessionStorage.getItem("displayname");
        $scope.logout = function () {
            $http.get('/Account/SignOut')
            .success(function (response) {
                if (response.Code === "Success") {
                    sessionStorage.clear();
                    $location.path('/index');
                }
            }).error(function (d, s, h, c) {
                if (d != null) {
                    alert(d.message);
                }
                else {
                    alert("error");
                }
            });
        };
        $scope.GetUserInfo = function () {

        };
    }])
    .controller('StaticCtrl', ['$scope', '$timeout', '$http', '$window', 'transformRequestAsFormPost', '$location', '$upload', '$rootScope', 'TypeEnum', 'dialogs', function ($scope, $timeout, $http, $window, transformRequestAsFormPost, $location, $upload, $rootScope, TypeEnum, dialogs) {

    }])
    .controller('CommonCtrl', ['$scope', function ($scope) {
        $scope.navbarMaxWidth = true;
        $scope.navbarMinWidth = false;
        $scope.navbarColMax = true;
        $scope.navbarColMin = false;
        $scope.navbarText = false;
        $scope.pagemax = true;
        $scope.pagemin = false;
        $scope.CollapseMenu = function () {
            $scope.navbarMaxWidth = false;
            $scope.navbarMinWidth = true;
            $scope.navbarColMax = false;
            $scope.navbarColMin = true;
            $scope.navbarText = true;
            $scope.pagemax = false;
            $scope.pagemin = true;
        };
        $scope.currentStatus = true;

        $scope.treeWithoutText = [
{
    text: "",
    icon: "glyphicon glyphicon-time",
    selectable: true,
    href: '#/DashBoard',
    shouldJump: true
},
{
    text: "",
    icon: "glyphicon glyphicon-signal",
    selectable: true,
    href: "#/Statistics",
    shouldJump: true
},
{
    text: "",
    icon: "glyphicon glyphicon-list-alt",
    selectable: true,
    state: { expanded: false },
    href: "#/CategoryEdit",
    shouldJump: false,
    nodes: [
        {
            text: "",
            icon: "glyphicon glyphicon-edit",
            href: "#/CategoryEdit",
            shouldJump: true
        }
    ]
}];

        $scope.treeWithText = [
              {
                  text: "TimeSheet",
                  icon: "glyphicon glyphicon-time",
                  selectable: true,
                  href: '#/DashBoard',
                  shouldJump: true
              },
              {
                  text: "Statistics",
                  icon: "glyphicon glyphicon-signal",
                  selectable: true,
                  href: "#/Statistics",
                  shouldJump: true
              },
                {
                    text: "Setting",
                    icon: "glyphicon glyphicon-list-alt",
                    selectable: true,
                    state: { expanded: true },
                    href: "#/CategoryEdit",
                    shouldJump: false,
                    nodes: [
                        {
                            text: "Category",
                            icon: "glyphicon glyphicon-edit",
                            href: "#/CategoryEdit",
                            shouldJump: true
                        }
                    ]
                }
        ];


        $scope.ReversalMenu = function () {

            if ($scope.currentStatus == true) {
                $scope.currentStatus = false;
                
                var tree = $scope.treeWithoutText;

                $('#tree').treeview({ data: tree, enableLinks: false, highlightSelected: false,isAddEmptyIcon:false });
            }
            else {

                $scope.currentStatus = true;
                var tree = $scope.treeWithText;

                $('#tree').treeview({ data: tree, enableLinks: true, highlightSelected: false,isAddEmptyIcon:true });

            }

            if ($scope.navbarMaxWidth) {
                $scope.navbarMaxWidth = false;
                $scope.navbarMinWidth = true;
                $scope.navbarColMax = false;
                $scope.navbarColMin = true;
                $scope.navbarText = true;
                $scope.pagemax = false;
                $scope.pagemin = true;
            }
            else {
                $scope.navbarMaxWidth = true;
                $scope.navbarMinWidth = false;
                $scope.navbarColMax = true;
                $scope.navbarColMin = false;
                $scope.navbarText = false;
                $scope.pagemax = true;
                $scope.pagemin = false;
            }
        };

    }]);