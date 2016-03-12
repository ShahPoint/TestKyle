
function SetTimeControl(thisControl) {

    var val = $(thisControl).val();

    val = val.replace(":", "");

    var timeNumber = parseInt(val);

    var hours = Math.floor(timeNumber / 100);

    var minutes = timeNumber - (hours * 100);

    if (hours <= 23 && hours >= 0 && minutes >= 0 && minutes <= 59) {
        
        var originalTime = (hours * 100) + minutes;

        if(hours == 0)
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
    SavePcrList(pcrList)
    {
        Utils.SetStorageObject("pcrList", pcrList);
    },
    GetPcrList()
    {
        var pcrList = Utils.GetStorageObject("pcrList");

        if (pcrList == null)
            pcrList = {};

        return pcrList;
    },
    GetPcrByOfflineId(offlineId)
    {
        var pcrList = Utils.GetPcrList();

        return pcrList[offlineId];
    },
    SavePcr(pcr)
    {
        if ( pcr.offlineId == null)
            pcr.offlineId = Utils.GetGuid();

        var pcrList = Utils.GetPcrList();

        pcrList[pcr.offlineId] = pcr;

        Utils.SavePcrList( pcrList);
    },
    RemovePcrByOfflineId(offlineId)
    {
        var pcrList = Utils.GetPcrList();

        delete pcrList[offlineId];

        Utils.SavePcrList(pcrList);
    }


}


var app = angular.module('myApp', []);
app.controller('myCtrl', function ($scope) {
    $scope.firstName = "John";
    $scope.lastName = "Doe";

    $scope.pcr = {};
    

    //var medications = GetStorageObject("PatientMedicationList");

    //alert(JSON.stringify( GetStorageObject("PatientMedicationList1") ) ) ;
    $scope.TestDefaultDispositions = function () {
        alert(JSON.stringify($scope.pcr.E20));
    }

    $scope.SaveCurrentPcr = function () {

        Utils.SavePcr($scope.pcr);
        $scope.LoadPcrArray();
    }

    $scope.LoadPcrArray = function () {

        var pcrListObject = Utils.GetPcrList();

        $scope.pcrArray = [];

        $.each(pcrListObject, function (index, value) {
            $scope.pcrArray.push(value);
        });
    }

    $scope.Load = function (index) {
        $scope.pcr = $scope.pcrArray[index];
    }

    $scope.Delete = function (index) {
        Utils.RemovePcrByOfflineId($scope.pcrArray[index].offlineId);
        $scope.LoadPcrArray();
    }

    $scope.pcrArray = [];
    
    //$scope.DeleteAllPcrs = function () {

    //    var emptyObject = {};

    //    Utils.SavePcrList(emptyObject);
    //}

    //$scope.ApplyDisposition = function () {
    //    // disposition_config pattern
    //    //alert(JSON.stringify($scope.pcr.currentDisposition));
    //    if ($scope.dispositions == undefined) return; // happens before dispositions request completes, on load of form
    //    var currentDisposition = $.grep($scope.dispositions, function (element, index) {
    //        return (element.outcome == $scope.pcr.currentDisposition.name)
    //    })[0];

    //    // defaults
    //    try {
    //        $scope.oldDisposition.defaults.forEach(function (element, index, array) {
    //            try {
    //                if ($("[data-ng-model='" + element.ngModel + "']").hasClass("select2-offscreen")) {
    //                    var currVals = jQuery.makeArray($("[data-ng-model='" + element.ngModel + "']").select2('val'));
    //                    if ($(element.value).not(currVals).length == 0 && $(currVals).not(element.value).length == 0) {
    //                        $("[data-ng-model='" + element.ngModel + "']").select2("val", []);
    //                        $("[data-ng-model='" + element.ngModel + "']").trigger("change");
    //                    }
    //                } else {
    //                    if ($("[data-ng-model='" + element.ngModel + "']").val() == element.value[0]) {
    //                        $("[data-ng-model='" + element.ngModel + "']").val("");
    //                    }
    //                }
    //            } catch (ex) {
    //                logError(ex);
    //                //logError({ message: "Clearing of old disposition defaults failed: " + element.ngModel + "\n\nApplyDisposition(...), PcrFormController.js", timestamp: new Date().toDateTime() });
    //            }
    //        });
    //    } catch (ex) {
    //        logError(ex);
    //    }

    //    try {
    //        currentDisposition.defaults.forEach(function (element, index, array) {
    //            try {
    //                if ($("[data-ng-model='" + element.ngModel + "']").hasClass("select2-offscreen")) {
    //                    $("[data-ng-model='" + element.ngModel + "']").select2("val", element.value);
    //                    $("[data-ng-model='" + element.ngModel + "']").trigger("change");

    //                } else {

    //                    $("[data-ng-model='" + element.ngModel + "']").val(element.value[0]);
    //                }
    //            } catch (ex) {
    //                logError(ex);
    //            }
    //        });
    //    } catch (ex) {
    //        logError(ex);
    //    }

    //    // hidden
    //    try {
    //        $scope.oldDisposition.hidden.forEach(function (element, index, array) {
    //            try {
    //                if (element.hasOwnProperty("id")) {
    //                    $("#" + element.id).show();
    //                }
    //                else {
    //                    $("[data-ng-model='" + element.ngModel + "']").closest("section").slideDown();
    //                }
    //            } catch (ex) {
    //                logError(ex);
    //            }
    //        });
    //    } catch (ex) {
    //        logError(ex);
    //    }
    //    try {
    //        currentDisposition.hidden.forEach(function (element, index, array) {
    //            try {
    //                if (element.hasOwnProperty("id")) {
    //                    $("#" + element.id).hide();
    //                }
    //                else {
    //                    $("[data-ng-model='" + element.ngModel + "']").closest("section").slideUp();
    //                }
    //            } catch (ex) {
    //                logError(ex);
    //            }
    //        });
    //    } catch (ex) {
    //        logError(ex);
    //    }

    //    // required
    //    try {
    //        $scope.oldDisposition.required.forEach(function (element, index, array) {
    //            try {
    //                $("[data-ng-model='" + element + "']").rules("add", {
    //                    required: false
    //                });
    //                $("[data-ng-model='" + element + "']").removeClass("RequiredRedBorder");
    //            } catch (ex) {
    //                logError(ex);
    //            }
    //        });
    //    } catch (ex) {
    //        logError(ex);
    //    }
    //    try {
    //        currentDisposition.required.forEach(function (element, index, array) {
    //            try {
    //                $("[data-ng-model='" + element + "']").rules("add", {
    //                    required: true
    //                });
    //                $("[data-ng-model='" + element + "']").addClass("RequiredRedBorder");
    //            } catch (ex) {
    //                logError(ex);
    //            }
    //        });
    //    } catch (ex) {
    //        logError(ex);
    //    }





});