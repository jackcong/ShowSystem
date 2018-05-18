angular.module('app.controllers').controller('SettingCtrl', ['$scope', 'SettingServices', function ($scope, SettingServices) {

    SettingServices.GetFullCategory().then(function (fullcategory) {

        $scope.category = JSON.parse(fullcategory.category);



        $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');
        $('.tree li.parent_li > span').on('click', function (e) {
            var children = $(this).parent('li.parent_li').find(' > ul > li');
            if (children.is(":visible")) {
                children.hide('fast');
                $(this).attr('title', 'Expand this branch').find(' > i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
            } else {
                children.show('fast');
                $(this).attr('title', 'Collapse this branch').find(' > i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
            }
            e.stopPropagation();
        });

    });

    $scope.AddSubNode = function (nodeObject,level) {

        if (level == 'header')
        {
            //add detail
            var detaiobj = { CategoryDetailName: "", Id: 0,isNewAdd:true };
            nodeObject.CategoryDetails.push(detaiobj);

        }
        else if (level == 'detail')
        {
            //add customer
            var detaiobj = { CategoryCustomerName: "", Id: 0, isNewAdd: true };
            nodeObject.CategoryCustomers.push(detaiobj);
        }
    };
    $scope.SaveNewNode = function (Id,dataObj,level,obj)
    {
        var nodeName = $.trim($(obj.currentTarget).prev().val());
        if (nodeName == "")
        {
            return;
        }

        SettingServices.SaveNewNode(Id,level, nodeName).then(function (result) {

            if (result.success == true) {
                //update node by return id.
                dataObj.Id = result.Id;
 
                dataObj.isNewAdd = false;
                //alert('Successful.');
            }
            else {
                alert(result.content);
            }

        });
    };

    $scope.EditNode = function (nodeObject) {
        nodeObject.isEdit = true;
    };

    $scope.SaveNode = function (nodeName, nodeObject, level) {

        var nodeNamein = $.trim(nodeName);
        if (nodeNamein == "") {
            return;
        }
        var Id = nodeObject.Id;

        SettingServices.SaveNode(Id, level, nodeNamein).then(function (result) {

            if (result.success == true) {
                nodeObject.isEdit = false;
            }
            else {
                alert(result.content);
            }

        });

        
    };

    $scope.DeleteNode = function (Id,level,index,parentObj)
    {
        if (confirm('Are you sure you want to delete?')) {

            SettingServices.DeleteNode(Id,level).then(function(result)
            {
                if (result.success == true)
                {
                    if (level == 'detail')
                    {
                        parentObj.CategoryDetails.splice(index, 1);
                    }
                    if (level == 'customer')
                    {
                        parentObj.CategoryCustomers.splice(index, 1);
                    }
                }
            });
        }
    }

}]);