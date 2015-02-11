app.controller('BlogController', function ($scope, BlogService, $routeParams) {
    $scope.blogs = [];


    $scope.getPosts = function () {
       BlogService.getPosts().then(function (data) {
            $scope.blogs = data;
        }, function () { });
    }

    $scope.getPosts();

    

  
});