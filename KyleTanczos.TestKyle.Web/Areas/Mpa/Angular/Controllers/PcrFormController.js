
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


var app = angular.module('myApp', ["ngAutocomplete"]);
app.controller('myCtrl', function ($scope) {
    $scope.firstName = "John";
    $scope.lastName = "Doe";

    $scope.pcr = { forms : { ImmunForm : {}  } };
    

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

    $scope.LoadPcr = function (index) {
        $scope.pcr = $scope.pcrArray[index];
    }

    $scope.DeletePcr = function (index) {
        Utils.RemovePcrByOfflineId($scope.pcrArray[index].offlineId);
        $scope.LoadPcrArray();
    }

    $scope.pcrArray = [];


    $scope.dispositions = [
         {
             name :"Cancelled"
             , defaults: [{ ngModel: "E02_02", value: "aaaa" }]
             , required: ['pcr.E07_33', 'pcr.E20_10']
             , hidden: ['pcr.E07_33']
         },
         {
                      name: "Dead at Scene"
             , defaults: []
             , required: []
             , hidden: []
         }
    ];


    $( document ).ready(function() {
        $("[ng-model='pcr.E20_10']").change(function () {

            $scope.ApplyDisposition();

            //alert("Handler for .change() called.");
        });
    });



    function logError(customObject)
    {

    }

    $scope.AddItemToList = function (listName, formItemName)
    {
        //alert(JSON.stringify(listName));
        //alert(JSON.stringify(formItemName));

        //alert($(this).html());

        if ($scope.pcr[listName] == null)
            $scope.pcr[listName] = [];

        var formItem = $scope.pcr.forms[formItemName];

        var item = JSON.parse(JSON.stringify(formItem));

        $scope.pcr[listName].push(item);

        $scope.pcr.forms[formItemName] = {};

       

        //$scope.$apply(); // only called on item being added

        //$("[ng-model='pcr.forms.ImmunForm.ImmunType']").val(null).trigger('change');

    }

    
    $scope.ApplyDisposition = function () {
        // disposition_config pattern
        //alert(JSON.stringify($scope.pcr.currentDisposition));
        //if ($scope.pcr.E20_10 == undefined) return; // happens before dispositions request completes, on load of form

        //$scope.oldDisposition = $scope.dispositions[0];



        var currentDisposition = $.grep($scope.dispositions, function (element, index) {
            return (element.name == "Cancelled")
        })[0];

        //alert(JSON.stringify(currentDisposition));

        //$scope.oldDisposition.defaults.forEach(function (element, index, array) {
        //    try {
        //        var $control = $("[data-ng-model='" + element.ngModel + "']");


        //        if ($control.hasClass("select2-offscreen")) {
        //            var currVals = jQuery.makeArray($control.select2('val'));
        //            if ($(element.value).not(currVals).length == 0 && $(currVals).not(element.value).length == 0) {
        //                $control.select2("val", []);
        //                $control.trigger("change");
        //            }
        //        } else {
        //            if ($control.val() == element.value[0]) {
        //                $control.val("");
        //            }
        //        }
        //    } catch (ex) {

        //        alert({ message: "Clearing of old disposition defaults failed: " + element.ngModel + "\n\nApplyDisposition(...), PcrFormController.js", timestamp: new Date().toDateTime() });
        //    }
        //});


        currentDisposition.defaults.forEach(function (element, index, array) {
            try {

                $scope.pcr[element.ngModel] = element.value;

                //$scope.pcr["E07_33"] = 'Not Applicable';



                //$("[ng-model='" + element.ngModel + "'] option[value='Not Applicable']").attr('selected', 'selected').trigger('input');

                //$('.id_100 ').attr('selected', 'selected');

                //var $control = $("[ng-model='" + element.ngModel + "'] option[value='Not Applicable']");

                

                //if ($control.hasClass("select2-offscreen")) {
                //    $control.select2("val", element.value);
                //    $control.trigger("change");

                //} else {
                //    $control.val(element.value);                   
                //}
            } catch (ex) {
                alert(ex);
            }
        });


        //$scope.oldDisposition.hidden.forEach(function (element, index, array) {
        //    try {
        //        if (element.hasOwnProperty("id")) {
        //            $("#" + element.id).show();
        //        }
        //        else {
        //            $("[data-ng-model='" + element.ngModel + "']").parents(".form-group").parent().slideDown();
        //        }
        //    } catch (ex) {
        //        alert(ex);
        //    }
        //});


        currentDisposition.hidden.forEach(function (element, index, array) {
            try {

                //alert('hidden: ' + element);

                //if (element.hasOwnProperty("id")) {
                //    $("#" + element.id).hide();
                //}
                //else {
                    $("[ng-model='" + element+ "']").parents(".form-group").parent().slideUp();
                //}
            } catch (ex) {
                alert(ex);
            }
        });

        //$scope.oldDisposition.required.forEach(function (element, index, array) {
        //    try {
        //        var $control = $("[data-ng-model='" + element + "']");

        //        $control.rules("add", { required: false });

        //        $control.removeClass("RequiredRedBorder");
        //    } catch (ex) {
        //        alert(ex);
        //    }
        //});


        currentDisposition.required.forEach(function (element, index, array) {
            try {

                var $control = $("[ng-model='" + element + "']");

                $control.parents('form').validate();

                if ($control.hasClass('select2-hidden-accessible')) //handles red box on select2
                    $control.parent().children('.select2').addClass("RequiredRedBorder");

                $control.rules("add", {
                    required: true,
                    messages: { required: "" }
                });

                $control.addClass("RequiredRedBorder");

                //alert("valid: " + $control.parents('form').valid());

            } catch (ex) {
                alert(ex);
            }
        });

        $scope.$apply();
    }




});