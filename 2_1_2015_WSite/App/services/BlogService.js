app.factory('BlogService', function ($http, $q) {
    var posts = [];
    
    var getPosts = function () {
        var deferred = $q.defer();
        $http.get("/BlogRead/").success(function (data) {
            posts.length = 0;
            for (var g in data) {
                posts.push(data[g])
            }
            deferred.resolve(posts);
        }).error(function (data, status) {

            deferred.reject();
        })
        return deferred.promise;
    }

     return {
        getPosts: getPosts
    };

});