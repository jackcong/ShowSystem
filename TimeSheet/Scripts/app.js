angular.module('app', ['ngRoute', 'app.controllers', 'app.directives', 'app.services', 'app.factories', 'chieffancypants.loadingBar', 'ui.bootstrap', 'dialogs.main'])
    .run(function () { })
    .config(['$routeProvider', '$locationProvider', '$httpProvider',
              function ($routeProvider, $locationProvider, $httpProvider) {

                  $httpProvider.interceptors.push('authHttpResponseInterceptor');

                  //Todo: Maybe change to use 'ui-router'.
                  //Routes
                  $routeProvider.when('/ChangePassword', {
                      templateUrl: 'Account/ChangePassword',
                      controller: 'LoginCtrl'
                  }).when('/index', {
                           templateUrl: 'Home/Landing',
                           controller: 'HomeCtrl'
                       }).when('/activeaccount/:activeCode',{
                          templateUrl: function (params) {
                              return 'Account/ActiveAccount?activeCode=' + params.activeCode;
                          },
                          controller: 'UserCtrl'
                       }).when('/DashBoard/List/:pageindex/search/:searchcontent', {
                          templateUrl: function (params) {
                              return 'DashBoard/Index?page='+params.pageindex+'&search='+params.searchcontent
                          },
                          controller: 'DashBoardCtrl'
                      }).when('/Detail/:detailid', {
                          templateUrl: function (params) {
                              return 'DashBoard/Detail?DetailID=' + params.detailid
                          },
                          controller: 'DetailCtrl'
                  }).when('/Statistics', {
                          templateUrl: 'DashBoard/Statistics',
                          controller: 'StatisticsCtrl'
                      }).when('/Exception', {
                          templateUrl: 'Exception/Index',
                          controller: 'ExceptionCtrl'
                      }).when('/CategoryEdit', {

                          templateUrl: 'Setting/CategoryEdit',
                          controller:'SettingCtrl'
                      })
                      .otherwise({ redirectTo: '/DashBoard/List/1/search/ ' });
              }]);
