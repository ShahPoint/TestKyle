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

    $scope.SendAlert = function () {
        alert(JSON.stringify($scope.pcr.E20));
    }

    
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