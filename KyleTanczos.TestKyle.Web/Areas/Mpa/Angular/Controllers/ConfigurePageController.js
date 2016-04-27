
function SetTimeControl(thisControl) {

    var val = $(thisControl).val();

    val = val.replace(":", "");

    var timeNumber = parseInt(val);

    var hours = Math.floor(timeNumber / 100);

    var minutes = timeNumber - (hours * 100);

    if (hours <= 23 && hours >= 0 && minutes >= 0 && minutes <= 59) {

        var originalTime = (hours * 100) + minutes;

        if (hours == 0)
            originalTime = "00" + originalTime;
        else if (hours < 10)
            originalTime = "0" + originalTime;

        originalTime = String(originalTime);

        var timeWithColon = originalTime.substr(0, 2) + ":" + originalTime.substr(2);

        $(thisControl).val(timeWithColon)
    }
}



var Utils = {

    GetGuid() {
        var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);

            return v.toString(16);
        });

        return guid;
    },
    SetStorageObject(key, value) {
        localStorage.setItem(key, JSON.stringify(value))
    },

    GetStorageObject(key) {
        return JSON.parse(localStorage.getItem(key));
    },
    SavePcrList(pcrList) {
        Utils.SetStorageObject("pcrList", pcrList);
    },
    GetPcrList() {
        var pcrList = Utils.GetStorageObject("pcrList");

        if (pcrList == null)
            pcrList = {};

        return pcrList;
    },
    GetPcrByOfflineId(offlineId) {
        var pcrList = Utils.GetPcrList();

        return pcrList[offlineId];
    },
    SavePcr(pcr) {
        alert(JSON.stringify(pcr));
        // var stuff = [];
        abp.services.app[webServiceName].update(pcr[configurationName], { //override jQuery's ajax parameters
            timeout: 30000
        }).done(function (msg) {
            alert(JSON.stringify(msg));
            abp.notify.success('successfully created a task!');
        });
        //$.ajax({
        //    url: "/api/services/app/stations/createorupdatestations",
        //    method: "POST",
        //    data: { "": stuff }
        //})
        // .done(function (msg) {
        //     alert(JSON.stringify(msg));
        //     $scope.$apply();
        // });;
    },
    SaveUser(user) {
        alert(JSON.stringify(user));
        // var stuff = [];
        return abp.services.app[webServiceName].update(user, { //override jQuery's ajax parameters
            timeout: 30000
        });
        //$.ajax({
        //    url: "/api/services/app/stations/createorupdatestations",
        //    method: "POST",
        //    data: { "": stuff }
        //})
        // .done(function (msg) {
        //     alert(JSON.stringify(msg));
        //     $scope.$apply();
        // });;
    },
    RemovePcrByOfflineId(offlineId) {
        var pcrList = Utils.GetPcrList();

        delete pcrList[offlineId];

        Utils.SavePcrList(pcrList);
    }


}


var app = angular.module('myApp', []);
app.controller('myCtrl', function ($scope) {
    $scope.pcr = { forms: {} };
    $scope.pcrArray = [];

    $(document).ready(function () {
        //$.ajax({
        //    url: "/api/services/app/stations/getstations",
        //    method: "POST"
        //})
        //.done(function (msg) {
        //    alert(JSON.stringify(msg));
        //    $scope.$apply();
        //});;
        abp.services.app[webServiceName].get({
        }, { //override jQuery's ajax parameters
            timeout: 30000
        }).done(function (msg) {
            $scope.pcr[configurationName] = msg;
            $scope.$apply();
            abp.notify.success('successfully created a task!');
        });
    });

    $scope.SaveCurrentPcr = function () {

        Utils.SavePcr($scope.pcr);
        //$scope.LoadPcrArray();
    }












    function logError(customObject) {

    }

    $scope.AddItemToDatabase = function (listName) {
        alert("save");
        if ($scope.pcr[listName] == null)
            $scope.pcr[listName] = [];

        var formItem = $scope.pcr.forms[listName];

        var item = JSON.parse(JSON.stringify(formItem));


        Utils.SaveUser(item).done(function (result) {


            var formId = formItem.ItemIndex;

            if (formId >= 0) {
                $scope.pcr[listName].splice(formId, 1, item);
            }
            else {
                $scope.pcr[listName].push(item);
            }

            if ($scope.pcr.forms[listName].keepOpen != true)
                CloseModal("#" + listName);

            ClearModal(listName);
            $scope.$apply();
            // Code depending on result
        }).fail(function (msg) {
            // An error occurred
            alert(JSON.stringify(msg));

        });




    }

    $scope.AddItemToList = function (listName) {
        if ($scope.pcr[listName] == null)
            $scope.pcr[listName] = [];

        var formItem = $scope.pcr.forms[listName];

        var item = JSON.parse(JSON.stringify(formItem));

        var formId = formItem.ItemIndex

        if (formId >= 0) {
            $scope.pcr[listName].splice(formId, 1, item);
        }
        else {
            $scope.pcr[listName].push(item);
        }



        if ($scope.pcr.forms[listName].keepOpen != true)
            CloseModal("#" + listName);

        ClearModal(listName);


    }




    $scope.RemoveItem = function (index, listName) {




        //bootbox.confirm("Are you sure you want to DELETE the item?", function (result) {
        //    $scope.pcr[listName].splice(index, 1);

        //    $scope.$apply();
        //});


    };


    $scope.LoadItemModal = function (index, listname) {

        var angularItem = angular.copy($scope.pcr[listname][index]);

        var strng = JSON.stringify(angularItem)

        var itemToLoad = JSON.parse(strng);

        itemToLoad.ItemIndex = index;


        $scope.pcr.forms[listname] = itemToLoad;

        $("#" + listname).modal("show");
        //$scope.$apply();
    };

    $scope.ClearCloseModal = function (modalTargetQuery, listName) {
        ClearModal(listName);
        CloseModal(modalTargetQuery);
    }

    function ClearModal(listName) {

        $scope.pcr.forms[listName] = {};
    }

    function CloseModal(modalTargetQuery) {

        $(modalTargetQuery).modal('hide');
    }
});