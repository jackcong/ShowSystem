angular.module('app.factories', [])
.factory("authHttpResponseInterceptor", function ($q, $location) {
        return {
            response: function (response) {
                if (response.status === 401) {
                    console.log("Response 401");
                }
                return response || $q.when(response);
            },
            responseError: function (rejection,rc,rd) {
                if (rejection.status === 401) {
                    console.log("Response Error 401", rejection);
                    var returnPath = $location.path().replace('/', '');
                    $location.path('/login').search('returnUrl', returnPath);
                }
                else if (rejection.status == 404)
                {
                    $location.path('/NotFound');
                }
                else if (rejection.status == 500)
                {
                    alert('Error:' + rejection.data.message);
                }
                return $q.reject(rejection);
            }
        }
})
.factory("transformRequestAsFormPost", function () {
    function transformRequest(data, getHeaders) {
        var headers = getHeaders();
        headers['Content-Type'] = 'application/x-www-form-urlencoded; charset=utf-8';
        console.log(headers['Content-Type']);
        console.log(serializeData(data));
        return serializeData(data);
    }
    // Return the factory value.
    return (transformRequest);
    // ---
    // PRVIATE METHODS.
    // --
    // I serialize the given Object into a key-value pair string. This
    // method expects an object and will default to the toString() method.
    // --
    // NOTE: This is an altered version of the jQuery.param() method which
    // will serialize a data collection for Form posting.
    // --
    // https://github.com/jquery/jquery/blob/master/src/serialize.js#L45
    function serializeData(data) {

        // If this is not an object, defer to native stringification.
        if (!angular.isObject(data)) {
            return ((data == null) ? "" : data.toString());
        }
        var buffer = [];
        // Serialize each key in the object.
        for (var name in data) {
            if (!data.hasOwnProperty(name)) {
                continue;
            }
            var value = data[name];
            buffer.push(encodeURIComponent(name) + "=" + encodeURIComponent((value == null) ? "" : value));
        }
        // Serialize the buffer and clean it up for transportation.
        var source = buffer.join("&").replace(/%20/g, "+");
        return source;
    }
})
.factory('exceptionHandler', function () {
    return function (exception, cause) {
        alert(exception.message);
    }
});