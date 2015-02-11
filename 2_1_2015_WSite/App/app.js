var app = angular.module('app', ['ng', 'ngRoute', 'ngSanitize']);

app.config(function ($routeProvider, $httpProvider) {

    $routeProvider
       .when('/', {
           templateUrl: '/App/views/BlogSummary.html',
           controller: 'SearchController'
       })
       .when('/Home', {
           templateUrl: '/App/views/BlogSummary.html',
           controller: 'SearchController'
       })
       .when('/editsongs', {
           templateUrl: '/App/views/EditSongsView.html',
           controller: 'SearchController'
       })
       .when('/genres', {
           templateUrl: '/app/views/GenresView.html',
           controller: 'SearchController'
       })
       .otherwise({
           redirectTo: '/'
       });

})

