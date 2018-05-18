angular.module('app.controllers')
    .controller('LoginCtrl', ['$scope', '$http', 'transformRequestAsFormPost', '$location', 'dialogs', function ($scope, $http, transformRequestAsFormPost, $location, dialogs) {

        $scope.PasswordModel = { "OldPassword": "", "NewPassword": "", "ConfirmPassword":"" };

        $scope.ChangePassword = function ()
        {
            if ($scope.changpasswordform.$valid) {
                if ($.trim($scope.PasswordModel.NewPassword) != $.trim($scope.PasswordModel.ConfirmPassword))
                {
                    alert('Please confirm new password.');
                    $scope.PasswordModel.NewPassword = "";
                    $scope.PasswordModel.ConfirmPassword = "";
                    return;
                }

                $http({
                    method: 'post',
                    url: '/Account/SendChangePassword',
                    transformRequest: transformRequestAsFormPost,
                    data: { oldPassword: $scope.PasswordModel.OldPassword, newPassword: $scope.PasswordModel.NewPassword }
                }).success(function (response) {
                    if (response.result === 0) {
                        //old password wrong.
                        alert('Old password incorrect , please confirm.');
                        return;
                    }
                    else {
                        alert('Successful.');
                        window.location.href = '#/DashBoard';
                    }
                }).error(function (d, s, h, c) {
                    if (d != null) {
                        alert(d.message);
                    }
                    else {
                        alert("error");
                    }
                });
            }
        }
    }]);
