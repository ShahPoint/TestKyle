var app = angular.module('myApp', []);
app.controller('myCtrl', function ($scope) {
    $scope.firstName = "John";
    $scope.lastName = "Doe";



    //var medications = GetStorageObject("PatientMedicationList");

    //alert(JSON.stringify( GetStorageObject("PatientMedicationList1") ) ) ;

    $scope.SendAlert = function () {
        alert('here');
    }






    $scope.currentUser = currentUser;
    DataService.pcr.LastSync = null;
    var allowAutofips = false;

    ////Jay: this looks like a culprit of cost, i will use 4 comments for pieces
    //// I dont think we will reuse, or just things we are not going to support
    //// The new project is not going to have services like LoadingGif
    ////$("#contentWrapper").hide();
    ////LoadingGifService.ShowLoading();

    var attemptingSync = false;
    var loadedPcrDocs = false;
    var pcrLoaded = false;

    $scope.cache = {};

    //// Review are we using this?
    $scope.ZipChanged = function (zipToSearch) {

        var targetAddress = $scope.cache.targetAddress;

        if (zipToSearch) {
            $.ajax({
                type: "GET",
                url: "/api/FIPS/?zip=" + zipToSearch + "&fipslist=" + $scope.demo2["ConfigFIPSList"]
            })
                    .done(function (msg) {

                        $scope.pcr[targetAddress].fipsOptions = msg;

                        $scope.$apply();
                    });
        }
    };

    ////What does this watcher do? I wrote this, i can look back.... culprit
    $scope.$watch('details',
	function (newValue, oldValue) {
	    alert(JSON.stringify( newValue ));
	    $scope.updateAutoComplete(newValue);

	});

    function lookupFips() {
        var targetAddress = $scope.cache.targetAddress;
        if ($scope.pcr[targetAddress] && !$scope.pcr[targetAddress].fips && !$scope.pcr[targetAddress].fipsCounty) {
            fipsLookupRequest(targetAddress);
        }
    }


    ////This can be deleted, we no longer get this from the server, I believe
    function fipsLookupRequest(address, deferred) {
        return $.ajax({
            type: "POST",
            url: "/api/FIPS/TDS",
            data: {
                streetAddress: $scope.pcr[address].combinedStreet,
                city: $scope.pcr[address].city,
                state: $scope.pcr[address].state,
                zip: $scope.pcr[address].zip
            },
            success: function (fipsData) {
                $scope.pcr[address].fips = fipsData.FipsCity;
                $scope.pcr[address].fipsCounty = fipsData.FipsCounty;
                $scope.$apply();
                if (deferred != null)
                    deferred.resolve();
            },
            fail: function () {
                if (deferred != null)
                    deferred.reject();
            }
        });
    }

    //// I believe this also can be deleted
    function tryAllFipsLookup() {
        var addresses = ["SceneAddress", "PatientAddress", "DestinationAddress", "GuardianAddress", "InsuranceAddress", "EmployerAddress"];
        var deferreds = [];

        for (var i = 0; i < addresses.length; i++) {
            if ($scope.pcr[addresses[i]] && !$scope.pcr[addresses[i]].fips && !$scope.pcr[addresses[i]].fipsCounty) {
                var dObject = new $.Deferred();
                fipsLookupRequest(addresses[i], dObject);
                deferreds.push(dObject);
            }
        }

        return $.when.apply($, deferreds);
    }

    //// Maybe change name to updateInfoOnAutoCompleteChange
    $scope.updateAutoComplete = function (details) {
        if (details == null) // happens on page load
            return;

        var info = details.address_components;

        var targetAddress = $scope.cache.targetAddress;
        var advanced = $scope.pcr[targetAddress].advancedFips;

        $scope.pcr[targetAddress] = {};

        for (i = 0; i < details.address_components.length; i++) {
            if (info[i].types[0] == "street_number")
                $scope.pcr[targetAddress].number = info[i].long_name || "";

            if (info[i].types[0] == "route")
                $scope.pcr[targetAddress].street = "" + info[i].long_name || "";

            if (info[i].types[0] == "locality")
                $scope.pcr[targetAddress].city = "" + info[i].long_name || "";

            if (info[i].types[0] == "administrative_area_level_1")
                $scope.pcr[targetAddress].state = "" + info[i].short_name || "";

            if (info[i].types[0] == "postal_code")
                $scope.pcr[targetAddress].zip = "" + info[i].long_name || "";
        }

        $scope.pcr[targetAddress].combinedStreet =
			($scope.pcr[targetAddress].number || "")
			+ " " +
			($scope.pcr[targetAddress].street || "");

        $scope.pcr[targetAddress].municipality = "";
        $scope.pcr[targetAddress].advancedFips = advanced;
        $("[ng-model='pcr." + targetAddress + ".municipality']").select2("val", null);
        $("[name='pcr_" + targetAddress + "_state']").select2("val", $scope.pcr[targetAddress].state);
        $("[ng-model='pcr." + targetAddress + ".city']").trigger("change");
    }

    $scope.SetFips = function (fipsOptionObject) {

        var targetAddress = $scope.cache.targetAddress;

        if ($scope.pcr[targetAddress] == null)
            $scope.pcr[targetAddress] = {};

        $scope.pcr[targetAddress].fips = fipsOptionObject.FipsCode;
        $scope.pcr[targetAddress].fipsCounty = fipsOptionObject.FipsCounty;

        $scope.$apply();
    };



    $('.aaCustomFips').on("change", function (e) {
        var selectData = $(this).select2('val');
        $parse($(this).attr("data-ng-model")).assign($scope, $(this).select2('data').text);
        //alert(JSON.stringify(selectData));

        if (!selectData) {
            $scope.SetFips({ FipsCode: '', FipsCounty: '' });
            return;
        }

        var fipsOption = selectData.split('||');

        while (fipsOption[1].length < 3) // pad 0's
            fipsOption[1] = "0" + fipsOption[1];
        while (fipsOption[0].length < 5) // pad 0's
            fipsOption[0] = "0" + fipsOption[0];

        var fipsObject = { FipsCode: fipsOption[0], FipsCounty: fipsOption[1] };

        //alert("greg:" + JSON.stringify(fipsObject));

        $scope.SetFips(fipsObject);
    });


    ////This is not being used as we are now a spa, but might be able to be used if go back to a standalone spa
    ////window.onbeforeunload = function () {
    ////    if ($scope.oldPcrReport != angular.toJson($scope.pcr)) {
    ////        return 'Your data is not saved!';
    ////    }
    ////    else {

    ////        //nothing happens if already saved
    ////        return null;
    ////    }
    ////};

    //even though it is suggested to use an event listener for beforeunload handling
    //this is currently not being fired
    //the above code fixes this
    //window.removeEventListener('beforeunload');
    //window.addEventListener('beforeunload', function (e) {
    //    if ($scope.oldPcrReport != angular.toJson($scope.pcr)) {
    //        return 'Your data is not saved!';
    //    }
    //    else {
    //        //nothing happens if already saved
    //    }
    //});


    ////All the next 50 lines and similar lines could be moved to a generic object, 
    //// maybe that initiates it else where just for code cleanliness
    //// example a getInitPcr and getInitDemo 
    $scope.pcrid = getParameterByName("id");

    $scope.ShowAllVitals = false;
    $scope.ShowAllProcedures = false;
    $scope.form = {};
    $scope.pcr = {};


    $scope.pcr.devices = [];
    $scope.pcr.notes = [];
    $scope.showAllMedications = false;
    $scope.showAllProcedures = false;
    $scope.form.validate = false;
    $scope.form.vitalsShowAnchor = "Show More";
    $scope.form.medsShowAnchor = "Show More";
    $scope.form.procsShowAnchor = "Show More";
    $scope.globalLastUpdated = new Date();
    //************


    //initialize all GPS Coordinates to 0.  YAY NEMSIS!!
    $scope.pcr.incidentLocationLat = 0.0;
    $scope.pcr.incidentLocationLong = 0.0;
    $scope.pcr.vehicleDispatchLat = 0.0;
    $scope.pcr.vehicleDispatchLong = 0.0;
    $scope.pcr.incidentDestinationLong = 0.0;
    $scope.pcr.incidentDestinationLat = 0.0;
    $scope.pcr.mvcRow = 1;
    $scope.pcr.injuryHeightOfFall = 1;

    $scope.pcr.googleAddress = {};
    $scope.vitalViewToggle = true;
    $scope.oldPcrReport = "";
    $scope.pcr.vitals = [];
    $scope.pcr.treatmentMedications = [];
    $scope.pcr.procedures = [];
    $scope.mergedVitals = [];
    $scope.pcr.currentSignature = {};

    $scope.dispositionsObject = dynamicDispositionsObject;


    //// this was already not used by the app, but the snippet might be the lightest way of dynamically applying required fields if need be
    // org custom required fields
    //success(DataService.demographics.GetDemo());
    //function success(msg) {
    //    $scope.demo = JSON.parse(msg.DataAsJson);
    //    if ($scope.demo.AgencyRequiredFieldsArr) {
    //        alert(JSON.stringify($scope.demo.AgencyRequiredFieldsArr));
    //        $scope.demo.AgencyRequiredFieldsArr.forEach(function (element, index, array) {
    //            $("[data-ng-model=" + "'" + element + "'" + "]").rules("add", {
    //                required: true
    //            });
    //            $("[data-ng-model=" + "'" + element + "'" + "]").addClass("RequiredRedBorder");
    //        });
    //    }
    //}


    //// make this a local function, not a public $scope function
    //// maybe put these in a helper JS file 
    $scope.GetTimeStampString = function () {
        var now = new Date();
        var year = now.getFullYear();
        var month = now.getMonth() + 1;
        var day = now.getDate();
        var hour = now.getHours();
        var minute = now.getMinutes();
        var second = now.getSeconds();
        if (month.toString().length == 1) {
            var month = '0' + month;
        }
        if (day.toString().length == 1) {
            var day = '0' + day;
        }
        if (hour.toString().length == 1) {
            var hour = '0' + hour;
        }
        if (minute.toString().length == 1) {
            var minute = '0' + minute;
        }
        if (second.toString().length == 1) {
            var second = '0' + second;
        }
        var dateTimeStamp = month + '/' + day + ' ' + hour + ':' + minute;

        return dateTimeStamp;

    }

    // helper function
    function getDate() {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd
        }

        if (mm < 10) {
            mm = '0' + mm
        }

        today = mm + '/' + dd + '/' + yyyy;
        return today;
    }

    // Is this still used?
    $scope.EditNote = function (index) {

        $scope.note = $scope.pcr.notes[index].body;

        $scope.DeleteNote(index);

    };

    $scope.CalculateFullName = function () {
        $scope.pcr.patientFullName = ($scope.pcr.patientFirstName || "").trim() + " " + ($scope.pcr.patientLastName || "").trim();
    };

    $scope.DeleteNote = function (index) {
        $scope.pcr.notes.splice(index, 1);
    };

    $scope.AddNote = function () {

        //this if is just to fix a bug where notes is null
        if ($scope.pcr.notes == null)
            $scope.pcr.notes = [];

        if ($scope.note != '') {

            var noteObj = { body: $scope.note, dateTime: $scope.GetTimeStampString(), guid: $scope.GetGuid(), user: currentUser };

            $scope.pcr.notes.push(noteObj);

            $scope.note = '';

            $scope.$apply();
        }
    };

    //// this manually triggers changes to nhtsa widget, since we do not have it, does this matter?
    $scope.FillNhtsaExam = function (value) {
        $("#wid-id-nhtsa input").each(function (index) {
            $(this).val(value).trigger("change");
        });
    };


    //// used for filling the normal on button click?
    $scope.FillExamSelect2s = function (tabId, value) {
        $("#" + tabId + " input").each(function (index) {
            var select2_list = $(this).attr("data-select2_list");
            var oldVal = $(this).val();

            if (select2_list && oldVal.length == 0) {
                var listName = select2_list.split('|')[1];
                var normalOption = $.grep(window[listName], function (element, index) {
                    return (element.text == "Normal");
                })[0];
                var reactiveOption = $.grep(window[listName], function (element, index) {
                    return (element.text == "Reactive");
                })[0];
                var notApplicableOption = $.grep(window[listName], function (element, index) {
                    return (element.text == "***Not Applicable");
                })[0];

                if (normalOption && normalOption.text) {
                    $(this).val("Normal").trigger("change");
                }
                else if (reactiveOption && reactiveOption.text) {
                    $(this).val("Reactive").trigger("change");
                }
                else if (notApplicableOption && notApplicableOption.text) {
                    $(this).val("***Not Applicable").trigger("change");
                }
            }
            else {
                $(this).val(oldVal).trigger("change");
            }

        });

        if (tabId == 'tabs-head') {
            // $("[data-ng-model='ExamForm.examRightEye'],[data-ng-model='ExamForm.examLeftEye']").val("Reactive").trigger("change");
            //$("[data-ng-model='ExamForm.examHead']").val("***Not Applicable").trigger("change");
        }
        if (tabId == 'tabs-back') {
            // $("[data-ng-model='ExamForm.examUnspecified']").val("***Not Applicable").trigger("change");
        }
    };


    $scope.pcr.DestinationAddress = {};
    $scope.pcr.DestinationAddress.currentfipsOption;
    $scope.pcr.DestinationAddress.fipsOptions = [];

    $scope.pcr.GuardianAddress = {};
    $scope.pcr.GuardianAddress.currentfipsOption;
    $scope.pcr.GuardianAddress.fipsOptions = [];

    $scope.pcr.EmployerAddress = {};
    $scope.pcr.EmployerAddress.currentfipsOption;
    $scope.pcr.EmployerAddress.fipsOptions = [];

    $scope.pcr.SceneAddress = {};
    $scope.pcr.SceneAddress.currentfipsOption;
    $scope.pcr.SceneAddress.fipsOptions = [];

    $scope.pcr.PatientAddress = {};
    $scope.pcr.PatientAddress.currentfipsOption;
    $scope.pcr.PatientAddress.fipsOptions = [];

   // $('#tabs222').tabs();

    $scope.UpdateCurrentCrewDropdowns = function () {
        var newOptions = [];
        //Get current user
        if ($scope.pcr.crewPrimary) {
            newOptions.push({ id: $scope.pcr.crewPrimary, text: $scope.pcr.crewPrimary });
        }
        if ($scope.pcr.crewDriver) {
            newOptions.push({ id: $scope.pcr.crewDriver, text: $scope.pcr.crewDriver });
        }

        if ($scope.pcr.crewSecondary) {
            var secondarySplit = ("" + $scope.pcr.crewSecondary).split(',');
            for (var i = 0; i < secondarySplit.length; i++) {
                if (secondarySplit[i]) {
                    newOptions.push({ id: secondarySplit[i], text: secondarySplit[i] });
                }

            }
        }
        if ($scope.pcr.crewThird) {
            var thirdSplit = ("" + $scope.pcr.crewThird).split(',');
            for (var i = 0; i < thirdSplit.length; i++) {
                if (thirdSplit[i]) {
                    newOptions.push({ id: thirdSplit[i], text: thirdSplit[i] });
                }

            }
        }
        if ($scope.pcr.crewOther) {
            var otherSplit = ("" + $scope.pcr.crewOther).split(',');
            for (var i = 0; i < otherSplit.length; i++) {
                if (otherSplit[i]) {
                    newOptions.push({ id: otherSplit[i], text: otherSplit[i] });
                }

            }
        }


        $scope.AttachSelect2ToControl(Select2Single, "whoGeneratedThisReport", newOptions);
        $scope.AttachSelect2ToControl(Select2Single, "medCrewMemberId", newOptions);
        $scope.AttachSelect2ToControl(Select2Single, "procCrewMemberId", newOptions);
    };



    
    $scope.SetCreatorToPrimary = function () {
        $scope.pcr.whoGeneratedThisReport = $scope.pcr.crewPrimary;
        $("[data-ng-model='pcr.whoGeneratedThisReport']").select2("val", $scope.pcr.crewPrimary);
    };


    //// This is being called but I think can be killed?
    $scope.FormActionSetAgencyDefaultValues = function (str) {

        //$scope.pcr['incidentReportNumber'] = 'q';
        //alert("asdf");
        //var str = "incidentReportNumber:P;incidentNumber:T3434";

        var strArray = str.split(';');

        var message = "";

        for (var i = 0 ; i < strArray.length ; i++) {
            var namesPair = strArray[i].split(':');
            //alert(namesPair[0] + ":" + namesPair[1]);
            $scope.pcr[namesPair[0]] = namesPair[1];

            var $control = $("[data-ng-model='pcr." + namesPair[0] + "']").first();
            var attr = $control.attr('data-select2_list');
            if (typeof attr !== typeof undefined && attr !== false) {
                //alert("select2 + " + namesPair[0]);
                $control.select2("val", namesPair[1]);
            }

        }

        toastr.success('Note: ' + strArray.length + " fields modified.", '');
    };


    //// This was not being called from anywhere
    //// This i believe was our pattern for when we shared a dialog for all addresses
    //// google  address code
    ////$scope.SetAddress = function () {

    ////    $("#street_number").trigger('change');
    ////    $("#route").trigger('change');
    ////    $("#locality").trigger('change');
    ////    $("#administrative_area_level_1").trigger('change');
    ////    $("#postal_code").trigger('change');
    ////    $("#country").trigger('change');

    ////    var tempAddressObject = {

    ////        street_number: "" + $scope.pcr.googleAddress.street_number,
    ////        street_name: "" + "" + $scope.pcr.googleAddress.street_name,
    ////        city: "" + $scope.pcr.googleAddress.city,
    ////        state: "" + $scope.pcr.googleAddress.state,
    ////        postal_code: "" + $scope.pcr.googleAddress.postal_code,
    ////        country: "" + $scope.pcr.googleAddress.country,
    ////        fips: "" + $scope.pcr.googleAddress.fips,
    ////        fipsCounty: "" + $scope.pcr.googleAddress.fipsCounty,
    ////        notes: "" + $scope.pcr.googleAddress.notes,
    ////        valid: true

    ////    };

    ////    var addressName = "" + $scope.pcr.googleAddress.targetAddress;
    ////    if (addressName == "SceneAddress") {
    ////        $scope.pcr.SceneAddress = tempAddressObject;
    ////    }
    ////    else if (addressName == "PatientAddress") {
    ////        $scope.pcr.PatientAddress = tempAddressObject;
    ////    }
    ////    else if (addressName == "DestinationAddress") {
    ////        $scope.pcr.DestinationAddress = tempAddressObject;
    ////    }
    ////    else if (addressName == "GuardianAddress") {
    ////        $scope.pcr.GuardianAddress = tempAddressObject;
    ////    }
    ////    else if (addressName == "InsuranceAddress") {
    ////        $scope.pcr.InsuranceAddress = tempAddressObject;
    ////    }
    ////    else if (addressName == "EmployerAddress") {
    ////        $scope.pcr.EmployerAddress = tempAddressObject;
    ////    }
    ////};


    $scope.UpdateTimelineDates = function () {
        var d = $scope.pcr.incidentDate;

        $scope.pcr.incidentOnsetDate = d;
        $scope.pcr.incidentCallRecievedDate = d;
        $scope.pcr.incidentDispatchNotifiedDate = d;
        $scope.pcr.incidentDispatchedDate = d;
        $scope.pcr.incidentEnrouteDate = d;
        $scope.pcr.incidentArriveSceneDate = d;
        $scope.pcr.incidentPtContactDate = d;
        $scope.pcr.incidentTransferPtDate = d;
        $scope.pcr.incidentDepartSceneDate = d;
        $scope.pcr.incidentArriveDestinationDate = d;
        $scope.pcr.incidentInServiceDate = d;
        $scope.pcr.incidentAtBaseDate = d;
        $scope.pcr.incidentUnitCanceledDate = d;
    }

    $scope.UpdateNumberOfPatients = function (mci) {
        if (mci == "Yes") {
            $scope.pcr.numberOfPts = "Multiple";
            $("[data-ng-model='pcr.numberOfPts']").select2("val", "Multiple");
        }
    }


    //// helper class or moved close to where it is used with the id
    function getParameterByName(name) {
        var item = $location.search();
        return (item != null ? item[name] : "");
    }

    $scope.calculateAge = function () {
        var dateValue = new Date($scope.pcr.patientDOB);
        var today = new Date();
        var ageYears = Math.floor((today - dateValue) / 1000 / 60 / 60 / 24 / 365.25);
        var ageMonths = Math.floor((today - dateValue) / 1000 / 60 / 60 / 24 / 30);
        $scope.pcr.patientAge = ageYears;

        $scope.pcr.patientAgeMonths = ageMonths;
    }


    //// this needs rewritten, as one 
    ////$scope.updatePcrOffline = function () {
    ////    var PcrOfflineArray = JSON.parse(localStorage.getItem("PcrOfflineArray"));
    ////    if (PcrOfflineArray == null) {
    ////        PcrOfflineArray = [];
    ////    }

    ////    var x2js = new X2JS();
    ////    var tempXml = x2js.json2xml_str($.parseJSON(angular.toJson($scope.pcr)));
    ////    var id = parseInt($scope.pcrid);
    ////    var offlinePcr = {
    ////        Version: $scope.Version + 1,
    ////        ID: id,//$scope.GetGuid(),
    ////        Name: "No Name",
    ////        DataAsJson: JSON.stringify($scope.pcr),
    ////        DataAsXml: tempXml,
    ////        Offline: $scope.OfflinePcr,
    ////        Status: "0"
    ////    };

    ////    var index;
    ////    var splice = false;
    ////    for (var i = 0; i < PcrOfflineArray.length; i++) {
    ////        if (PcrOfflineArray[i].ID == id) {
    ////            //alert("match: " + i);
    ////            index = i;
    ////            splice = true;
    ////            offlinePcr["Created"] = PcrOfflineArray[i].Created;
    ////            offlinePcr["CreatedBy"] = PcrOfflineArray[i].CreatedBy;
    ////            offlinePcr["PrettyCreated"] = PcrOfflineArray[i].PrettyCreated;
    ////            break;
    ////        }
    ////    }

    ////    if (splice) {
    ////        PcrOfflineArray.splice(index, 1, offlinePcr);
    ////    }
    ////    else {
    ////        PcrOfflineArray.push(offlinePcr);
    ////    }


    ////    localStorage.setItem("PcrOfflineArray", JSON.stringify(PcrOfflineArray));
    ////}

    //// What is this used for?
    $scope.appendQueryValue = function (id, value) {
        $location.search(id, value);
    }

    //// This needs rethought and rewritten
    ////$scope.updatePcrToList = function () {

    ////    var x2js = new X2JS();
    ////    var tempXml = x2js.json2xml_str($.parseJSON(angular.toJson($scope.pcr)));
    ////    $scope.extraPcrData.DataAsXml = tempXml;
    ////    DataService.pcr.SetPcr($scope.pcr, $scope.pcrid, $scope.extraPcrData);

    ////};

    //// this can be rewritten to just call teh validateAndSubmit from teh client
    ////var submitHandler = $scope.$on('HandleSubmitPcr', function (event, args) {
    ////    $scope.validateAndSubmit(args.url);
    ////});

    //// This looks fine, not sure why we overloaded the submit url, where else do we submit and go to?
    //// Except teh set nullable values, i would like to rethink this for 2.1
    $scope.validateAndSubmit = function (url) {
        // tryAllFipsLookup().done(function () {
        if (!url)
            url = "/App/Index/#/Submit?id=";
        if (isPcrFormValid()) {
            $scope.SetNullableValues();
            $scope.SavePcrThenSubmit(url);
        }
        else {
            ShowValidationPanel();
        }
        //  });
    }


    $scope.SavePcrThenSubmit = function (url) {
        var x2js = new X2JS();
        var tempXml = x2js.json2xml_str($.parseJSON(angular.toJson($scope.pcr)));
        $scope.extraPcrData.DataAsXml = tempXml;
        DataService.pcr.SetPcr($scope.pcr, $scope.pcrid, $scope.extraPcrData);

        var promise = DataService.pcr.SyncPcr($scope.pcrid);
        if (promise != null) {
            toastr.info("Attempting to sync PCR to server...")
            promise.done(function (newPcr) {
                if ($scope.pcrid != newPcr.ID) {
                    $scope.pcrid = newPcr.ID;
                    //$state.transitionTo('PcrForm', { id: newPcr.ID }, { notify: false });
                }
                toastr.success("PCR saved successfully");
                window.location = url + $scope.pcrid;
            }).fail(function (msg) {
                toastr.error("PCR could not be synced. Please submit when you have a stable internet connection.");
            });
        }

    };






    //// I dont think this is being used
    ////$scope.UpdatePatientAddress = function (addr) {
    ////    $scope.pcr.PatientAddress = angular.copy(addr);
    ////};

    //// I do not think this being used
    ////$scope.UpdateGuardianAddress = function (addr) {
    ////    $scope.pcr.GuardianAddress = angular.copy(addr);
    ////};


    //// Is this the submit, or the submit above, we should only need one?
    $scope.SubmitPcr = function () {

        if ($scope.PageOnline) {

            var x2js = new X2JS();
            var tempXml = x2js.json2xml_str($.parseJSON(angular.toJson($scope.pcr)));
            $scope.extraPcrData.DataAsXml = tempXml;
            $scope.extraPcrData.Status = "1";
            DataService.pcr.SetPcr($scope.pcr, $scope.pcrid, $scope.extraPcrData);
            window.location = "/Angular/index.html#/ListView";
        }
        else {
            toastr.error("You are not permitted to submit reports when offline");
        }
    };

    $scope.CalculateTotalTime = function () {
        try {
            if ($scope.pcr.incidentDispatchedTime && $scope.pcr.incidentInServiceTime) {
                //alert("calc");
                //var year1 = $scope.pcr.incidentDispatchedDate.split('/');
                //var year2 = $scope.pcr.incidentInServiceDate.split('/');
                var time1 = $scope.pcr.incidentDispatchedTime.split(':');
                var time2 = $scope.pcr.incidentInServiceTime.split(':');

                var totalTimeMinutes = parseInt(time2[1]) - parseInt(time1[1]);
                var totalTimeHours = parseInt(time2[0]) - parseInt(time1[0]);

                if (isNaN(totalTimeHours) || isNaN(totalTimeMinutes)) {
                    $scope.pcr.totalTime = "00:00";
                    return;
                }

                if (totalTimeMinutes < 0) {
                    totalTimeMinutes = totalTimeMinutes + 60;
                    totalTimeHours = totalTimeHours - 1;
                }
                if (totalTimeHours < 0) {
                    totalTimeHours = totalTimeHours + 24;
                }

                //pad single digits
                if (totalTimeHours < 10) {
                    totalTimeHours = "0" + totalTimeHours;
                }
                //pad single digits
                if (totalTimeMinutes < 10) {
                    totalTimeMinutes = "0" + totalTimeMinutes;
                }

                $scope.pcr.totalTime = totalTimeHours + ":" + totalTimeMinutes
            }
        }
        catch (ex) {

        }
    };

    $scope.CalculateOdometerTotal = function () {
        if ($scope.pcr.odometerScene && $scope.pcr.odometerDest) {
            $scope.pcr.odometerTotal = $scope.pcr.odometerDest - $scope.pcr.odometerScene;
        }
    };

    //// what is this used for?
    $scope.TrustThisHTML = function (str) {
        return $sce.trustAsHtml(str);
    }

    //// Is this working?
    $scope.CalculateTotalGcs = function () {

        $scope.VitalForm.vitalGcsTotal =
        parseInt($scope.VitalForm.vitalGcs) +
        parseInt($scope.VitalForm.vitalGcsM) +
        parseInt($scope.VitalForm.vitalGcsV);

    }

    //// We have to code revew and rethink the offline story
    ////
    ////$scope.RemovePcrsFromLocalStorage = function (results) {
    ////    var PcrOfflineArray = JSON.parse(localStorage.getItem("PcrOfflineArray"));

    ////    if (PcrOfflineArray && PcrOfflineArray.length > 0 && results && results.length > 0) {
    ////        for (var j = 0; j < results.length; j++) {
    ////            for (var i = 0; i < PcrOfflineArray.length; i++) {
    ////                if (PcrOfflineArray[i].ID == results[j]) {
    ////                    //alert("Removing " + i);
    ////                    PcrOfflineArray.splice(i, 1);
    ////                    //alert(angular.toJson(PcrOfflineArray));
    ////                    break;
    ////                }
    ////            }
    ////        }

    ////        localStorage.setItem("PcrOfflineArray", JSON.stringify(PcrOfflineArray));
    ////    }
    ////}

    //// What do these do, culprit
    ////var saveThenOpenDisplayPageHandler = $scope.$on('HandleSaveThenOpenDisplayPage', function (event, args) {
    ////    $scope.SaveOrUpdateHandler();
    ////    window.location = args.url + $scope.pcrid;
    ////});

    //// What do these do, culprit
    ////var saveOrUpdateHandler = $scope.$on('HandleSaveOrUpdateHandler', function (event, args) {
    ////    $scope.SaveOrUpdateHandler();
    ////});



    //// Rewrite offline saving and submitting logic
    ////$scope.SaveOrUpdateHandler = function () {
    ////    if ($scope.extraPcrData.Status == "0") {
    ////        if ($scope.oldPcrReport != JSON.stringify($scope.pcr)) {


    ////            $scope.oldPcrReport = JSON.stringify($scope.pcr);

    ////            $scope.updatePcrToList();
    ////        }
    ////        else {

    ////            //toastr.info('No updates to save.', "");
    ////        }

    ////        if (!attemptingSync)
    ////            sync();
    ////    } else {
    ////        toastr.warning("You are viewing a submitted PCR. Edits will not be saved.")
    ////    }
    ////};

    $scope.calculateWeightInKg = function () {
        $scope.pcr.patientWeightLbs = $scope.pcr.patientWeightLbs.replace(/[^\0-9]/ig, "");
        $scope.pcr.patientWeightKg = ($scope.pcr.patientWeightLbs == "" ? 0 : (parseInt(parseInt($scope.pcr.patientWeightLbs) / 2.2)));
    }


    //// I would like to move demo based dropdowns to the server side, 
    //// update the cache-manifest for that agency and re-install resources
    ////$scope.CreateSelect2ArrayFromDelimitedString = function (demographics, property) {
    ////    var data = demographics[property];
    ////    if (!data) {
    ////        var empty = [];
    ////        return empty;
    ////    }
    ////    var dataArray = data.split(',');
    ////    var select2Array = [];
    ////    var i;
    ////    for (i = 0; i < dataArray.length; ++i) {
    ////        var select2Object = { id: dataArray[i], text: dataArray[i] };
    ////        select2Array.push(select2Object);
    ////    }
    ////    return select2Array;
    ////};

    //// I dont think we will need this either
    ////$scope.CreateSelect2ArrayFromObjectArray = function (demographics, objectName, property) {
    ////    var objectArray = demographics[objectName];
    ////    if (!objectArray) {
    ////        var empty = [];
    ////        return empty;
    ////    }
    ////    var select2Array = [];
    ////    var i;
    ////    for (i = 0; i < objectArray.length; ++i) {
    ////        var medicationName = "" + objectArray[i][property];
    ////        var select2Object = { id: medicationName, text: medicationName };
    ////        select2Array.push(select2Object);
    ////    }
    ////    return select2Array;
    ////};

    //// This will all get moved to a new pattern for offline resources
    ////$scope.CreateArrayFromObjectArray = function (demographics, objectName, property) {
    ////    var objectArray = [];
    ////    objectArray = demographics[objectName];

    ////    var array = [];
    ////    var i;
    ////    for (i = 0; i < objectArray.length; ++i) {
    ////        array.push("" + objectArray[i][property]);
    ////    }
    ////    return array;
    ////};

    //// Move to server side, either deliver as ready to use resources or deliver in mvc partial controller
    ////$scope.CreateSelect2ArrayForPersonnels = function (demographics) {
    ////    var objectArray = [];
    ////    objectArray = demographics["Personnels"];

    ////    var select2Array = [];
    ////    var i;
    ////    for (i = 0; i < objectArray.length; ++i) {
    ////        if (!objectArray[i].activeEmt)
    ////            continue;
    ////        var PersonnelFirstName = "" + objectArray[i]["PersonnelFirstName"];
    ////        var PersonnelLastName = "" + objectArray[i]["PersonnelLastName"];
    ////        var PersonnelStateLicensureID = "" + objectArray[i]["PersonnelStateLicensureID"];
    ////        var PersonnelServiceLevel;
    ////        if (typeof objectArray[i].PersonnelAgencyCertifications === "undefined") {
    ////            PersonnelServiceLevel = "Nothing Found";
    ////        }
    ////        else {
    ////            PersonnelServiceLevel = $scope.GetMostRecentPersonnelCertification(objectArray[i].PersonnelAgencyCertifications);
    ////        }

    ////        var FormattedText = PersonnelFirstName + " " + PersonnelLastName + " (" + PersonnelStateLicensureID + "/" + PersonnelServiceLevel + ")";
    ////        var select2Object = { id: FormattedText, text: FormattedText };
    ////        select2Array.push(select2Object);
    ////    }
    ////    return select2Array;
    ////}

    var emptyarr = [];

    // what is this?  Guessing this will not be needed if we rethink COnley
    try {
        $scope.dispositions = JSON.parse(AgencyDispositions);
        $scope.oldDisposition = $scope.dispositions[0];
    } catch (e) { } // in case the parse fails

    //// This really needs rethough and simplified (Jay Shah)
    ////$scope.ApplyDisposition = function () {
    ////    // disposition_config pattern
    ////    //alert(JSON.stringify($scope.pcr.currentDisposition));
    ////    if ($scope.dispositions == undefined) return; // happens before dispositions request completes, on load of form
    ////    var currentDisposition = $.grep($scope.dispositions, function (element, index) {
    ////        return (element.outcome == $scope.pcr.currentDisposition.name)
    ////    })[0];

    ////    // defaults
    ////    try {
    ////        $scope.oldDisposition.defaults.forEach(function (element, index, array) {
    ////            try {
    ////                if ($("[data-ng-model='" + element.ngModel + "']").hasClass("select2-offscreen")) {
    ////                    var currVals = jQuery.makeArray($("[data-ng-model='" + element.ngModel + "']").select2('val'));
    ////                    if ($(element.value).not(currVals).length == 0 && $(currVals).not(element.value).length == 0) {
    ////                        $("[data-ng-model='" + element.ngModel + "']").select2("val", []);
    ////                        $("[data-ng-model='" + element.ngModel + "']").trigger("change");
    ////                    }
    ////                } else {
    ////                    if ($("[data-ng-model='" + element.ngModel + "']").val() == element.value[0]) {
    ////                        $("[data-ng-model='" + element.ngModel + "']").val("");
    ////                    }
    ////                }
    ////            } catch (ex) {
    ////                logError(ex);
    ////                //logError({ message: "Clearing of old disposition defaults failed: " + element.ngModel + "\n\nApplyDisposition(...), PcrFormController.js", timestamp: new Date().toDateTime() });
    ////            }
    ////        });
    ////    } catch (ex) {
    ////        logError(ex);
    ////    }

    ////    try {
    ////        currentDisposition.defaults.forEach(function (element, index, array) {
    ////            try {
    ////                if ($("[data-ng-model='" + element.ngModel + "']").hasClass("select2-offscreen")) {
    ////                    $("[data-ng-model='" + element.ngModel + "']").select2("val", element.value);
    ////                    $("[data-ng-model='" + element.ngModel + "']").trigger("change");

    ////                } else {

    ////                    $("[data-ng-model='" + element.ngModel + "']").val(element.value[0]);
    ////                }
    ////            } catch (ex) {
    ////                logError(ex);
    ////            }
    ////        });
    ////    } catch (ex) {
    ////        logError(ex);
    ////    }

    ////    // hidden
    ////    try {
    ////        $scope.oldDisposition.hidden.forEach(function (element, index, array) {
    ////            try {
    ////                if (element.hasOwnProperty("id")) {
    ////                    $("#" + element.id).show();
    ////                }
    ////                else {
    ////                    $("[data-ng-model='" + element.ngModel + "']").closest("section").slideDown();
    ////                }
    ////            } catch (ex) {
    ////                logError(ex);
    ////            }
    ////        });
    ////    } catch (ex) {
    ////        logError(ex);
    ////    }
    ////    try {
    ////        currentDisposition.hidden.forEach(function (element, index, array) {
    ////            try {
    ////                if (element.hasOwnProperty("id")) {
    ////                    $("#" + element.id).hide();
    ////                }
    ////                else {
    ////                    $("[data-ng-model='" + element.ngModel + "']").closest("section").slideUp();
    ////                }
    ////            } catch (ex) {
    ////                logError(ex);
    ////            }
    ////        });
    ////    } catch (ex) {
    ////        logError(ex);
    ////    }

    ////    // required
    ////    try {
    ////        $scope.oldDisposition.required.forEach(function (element, index, array) {
    ////            try {
    ////                $("[data-ng-model='" + element + "']").rules("add", {
    ////                    required: false
    ////                });
    ////                $("[data-ng-model='" + element + "']").removeClass("RequiredRedBorder");
    ////            } catch (ex) {
    ////                logError(ex);
    ////            }
    ////        });
    ////    } catch (ex) {
    ////        logError(ex);
    ////    }
    ////    try {
    ////        currentDisposition.required.forEach(function (element, index, array) {
    ////            try {
    ////                $("[data-ng-model='" + element + "']").rules("add", {
    ////                    required: true
    ////                });
    ////                $("[data-ng-model='" + element + "']").addClass("RequiredRedBorder");
    ////            } catch (ex) {
    ////                logError(ex);
    ////            }
    ////        });
    ////    } catch (ex) {
    ////        logError(ex);
    ////    }

    ////    // deep copy setting oldDisposition to currentDisposition
    ////    $scope.oldDisposition = jQuery.extend(true, {}, currentDisposition);
    ////    // END: disposition_config pattern

    ////    $scope.pcr.assessmentOutcome = $scope.pcr.currentDisposition["results"].outcome;

    ////    //$scope.SaveOrUpdateHandler();

    ////    //$('section').show();
    ////    //$('.jarviswidget').show();
    ////    //$('nav > ul > li > a').show();


    ////    //$("[data-ng-model]").each(function (index) {
    ////    //    var ngModel = $(this).attr("data-ng-model");
    ////    //    var indexOf = $.inArray(ngModel, $scope.pcr.currentDisposition["results"].sections);
    ////    //    if (indexOf != -1) {
    ////    //        $(this).parent().parent().slideUp();
    ////    //    }
    ////    //});

    ////    //$("[data-addressTable]").each(function (index) {
    ////    //    var ngModel = $(this).attr("data-addressTable");
    ////    //    var indexOf = $.inArray(ngModel, $scope.pcr.currentDisposition["results"].addressTables);
    ////    //    if (indexOf != -1) {
    ////    //        $(this).parent().parent().slideUp();
    ////    //    }
    ////    //});

    ////    //$('.jarviswidget > header > h2').each(function (index) {
    ////    //    var jarvisTitle = $(this).html();
    ////    //    var indexOf = $.inArray(jarvisTitle, $scope.pcr.currentDisposition["results"].tables);
    ////    //    if (indexOf != -1) {
    ////    //        $(this).parent().parent().slideUp();
    ////    //    }
    ////    //});



    ////    //$('nav > ul > li > a').each(function (index) {

    ////    //    var tabName = $(this).attr('name');

    ////    //    var indexOf = $.inArray(tabName, $scope.pcr.currentDisposition["results"].tabs);
    ////    //    if (indexOf != -1) {
    ////    //        //alert(tabId);
    ////    //        $(this).hide();
    ////    //    }
    ////    //});

    ////    ////$scope.FormActionSetAgencyDefaultValues($scope.pcr.currentDisposition.fieldValuePairs)

    ////    //$scope.pcr.assessmentOutcome = $scope.pcr.currentDisposition["results"].outcome;
    ////    //$("[data-ng-model='pcr.assessmentOutcome']").select2("val", $scope.pcr.currentDisposition["results"].outcome);


    ////    //rerender select / show selection
    ////    for (i = 0; i < $scope.dispositionsObject.length; i++) {
    ////        if ($scope.pcr.currentDisposition.name == $scope.dispositionsObject[i].name) {
    ////            $scope.pcr.currentDisposition = $scope.dispositionsObject[i];
    ////        }
    ////    }
    ////    //end rerender select (by jay shah), not sure why this made it work 


    ////    changeTab();
    ////};

    //// Part of the stuff that just needs rethought
    ////$scope.ApplyDispositionChange = function () {
    ////    //any code in here will automatically have an apply run afterwards
    ////    $scope.ApplyDisposition();
    ////    //$scope.$apply();
    ////    $scope.SaveOrUpdateHandler();
    ////    //$scope.$apply();
    ////};

    //// I want these attachments of options moved to the serverside for the most part
    $scope.AttachSelect2ToControl = function (select2Function, title, optionsArray) {
        var $control = $("input[title='" + title + "']").first();
        $control.attr("data-select2_list", "|" + title);
        select2Function($control, optionsArray);
        return optionsArray;
    };

    //// I see this is being used, when does this trigger, we will maybe need to do this on the server
    function removeNullAndUndefinedFromList(list) {
        for (var i = 0; i < list.length; i++) {
            if (list[i].id == "null" || list[i].id == "undefined")
                list.splice(i--, 1);
        }
        return list;
    }

    //// Move this all to the serverside, except meds, procs, and protocols, and move to a deffered select2 pattern
    ////$scope.AttachDemographicSelect2s = function () {
    ////    if ($scope.demo) {
    ////        $scope.AttachSelect2ToControl(Select2Single, "LocationZone", $scope.demo.ConfigZoneNumbers);
    ////        $scope.AttachSelect2ToControl(Select2Single, "VehicleDispatchZone", $scope.demo.ConfigZoneNumbers);
    ////        $scope.AttachSelect2ToControl(Select2Single, "DestinationZone", $scope.demo.ConfigZoneNumbers);

    ////        $scope.AttachSelect2ToControl(Select2Single, "LocationState", $scope.demo.AgencyStates);

    ////        $scope.AttachSelect2ToControl(Select2Single, "LocationCounty", $scope.demo.AgencyCountys);

    ////        $scope.AttachSelect2ToControl(Select2Single, "PAMedicationList", $scope.demo.ConfigMedications.concat(NullValuesList));
    ////        $scope.AttachSelect2ToControl(Select2Single, "ConfigProceduresList", $scope.demo.ConfigProcedures);

    ////        // $scope.AttachSelect2ToControl(Select2_TagsMultiple, "Scene-Prior AID", $scope.demo.ConfigProcedures.concat($scope.demo.ConfigMedications).concat(NullValuesList));

    ////        $scope.AttachSelect2ToControl(Select2Multiple, "Protocols", $scope.demo.ConfigProtocols);

    ////        $scope.AttachSelect2ToControl(Select2Single, "incidentUnitVehicleNumber", $scope.demo.VehicleNumbers);

    ////        $scope.AttachSelect2ToControl(Select2Single, "Unit-Call Sign", $scope.demo.VehicleCallSigns);

    ////        $scope.AttachSelect2ToControl(Select2_TagsSingle, "VehicleDispatchLocation", $scope.demo.StationNames.concat($scope.demo.ConfigOtherFacilityNames).concat($scope.demo.ConfigHospitalNames));
    ////        $scope.AttachSelect2ToControl(Select2_TagsSingle, "Incident-Destination", $scope.demo.ConfigOtherFacilityNames.concat($scope.demo.ConfigHospitalNames).concat(NullValuesList));

    ////        $scope.AttachSelect2ToControl(Select2_TagsMultiple, "AgenciesOnScene", $scope.demo.AgencyOtherAgenciesInArea.concat(NullValuesList));

    ////        $scope.AttachSelect2ToControl(Select2_TagsSingle, "LocationFacilityCode", removeNullAndUndefinedFromList($scope.demo.ConfigHospitalNumbers.concat($scope.demo.ConfigOtherFacilityNumbers).concat(NullValuesList)));
    ////        $scope.AttachSelect2ToControl(Select2_TagsSingle, "DestinationCode", removeNullAndUndefinedFromList($scope.demo.ConfigHospitalNumbers.concat($scope.demo.ConfigOtherFacilityNumbers).concat(NullValuesList)));

    ////        $scope.AttachSelect2ToControl(Select2Single, "Crew - Primary", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    ////        $scope.AttachSelect2ToControl(Select2Multiple, "Crew - Secondary", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    ////        $scope.AttachSelect2ToControl(Select2Multiple, "Crew - Third", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    ////        $scope.AttachSelect2ToControl(Select2Multiple, "Crew - Other", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    ////        $scope.AttachSelect2ToControl(Select2Single, "Crew - Driver", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    ////        $scope.AttachSelect2ToControl(Select2Single, "whoGeneratedThisReport", $scope.demo.Personnels);
    ////        $scope.AttachSelect2ToControl(Select2Single, "medCrewMemberId", $scope.demo.Personnels);
    ////        $scope.AttachSelect2ToControl(Select2Single, "procCrewMemberId", $scope.demo.Personnels);
    ////    } //title="vitals-procedure"
    ////    else {
    ////        //alert("no demo object");
    ////        if ($scope.pcr && $scope.pcr.demo) {
    ////            //alert(false);
    ////            $scope.readDemographicsFromList(false);
    ////        }
    ////        else {
    ////            //alert(true);
    ////            $scope.readDemographicsFromList(true);
    ////        }
    ////    }

    ////};

    ////Move this all to the serverside, except meds, procs, and protocols, and move to a deffered select2 pattern
    $scope.autoFips = false;
    //$scope.readDemographicsFromList = function (isNew) {
    //    success(DataService.demographics.GetDemo());
    //    function success(msg) {
    //        //alert("Data Saved: " + JSON.stringify(msg));

    //        var demographics = JSON.parse(msg.DataAsJson);
    //        //alert(JSON.stringify(demographics["Stations"]));
    //        $scope.demo2 = demographics;
    //        $scope.demo = {};
    //        $scope.hospital = {};

    //        $scope.demo.ConfigZoneNumbers = $scope.CreateSelect2ArrayFromDelimitedString(demographics, "ConfigZoneNumber");

    //        window["LocationZone"] = $scope.AttachSelect2ToControl(Select2Single, "LocationZone", $scope.demo.ConfigZoneNumbers);

    //        window["VehicleDispatchZone"] = $scope.AttachSelect2ToControl(Select2Single, "VehicleDispatchZone", $scope.demo.ConfigZoneNumbers);

    //        window["DestinationZone"] = $scope.AttachSelect2ToControl(Select2Single, "DestinationZone", $scope.demo.ConfigZoneNumbers);

    //        $scope.demo.AgencyStates = $scope.CreateSelect2ArrayFromDelimitedString(demographics, "AgencyState"); //

    //        window["LocationState"] = $scope.AttachSelect2ToControl(Select2Single, "LocationState", $scope.demo.AgencyStates);

    //        $scope.demo.AgencyCountys = $scope.CreateSelect2ArrayFromDelimitedString(demographics, "AgencyCounty");

    //        window["LocationCounty"] = $scope.AttachSelect2ToControl(Select2Single, "LocationCounty", $scope.demo.AgencyCountys);


    //        $scope.demo.ConfigProcedures = $scope.CreateSelect2ArrayFromObjectArray(demographics, "ConfigProcedures", "ConfigProcedure");

    //        $scope.demo.ConfigMedications = $scope.CreateSelect2ArrayFromObjectArray(demographics, "ConfigMedications", "ConfigMedicationsGiven");

    //        window["PAMedicationList"] = $scope.AttachSelect2ToControl(Select2Single, "PAMedicationList", $scope.demo.ConfigMedications.concat(NullValuesList));

    //        window["ConfigProceduresList"] = $scope.AttachSelect2ToControl(Select2Single, "ConfigProceduresList", $scope.demo.ConfigProcedures);



    //        //window["Scene-Prior AID"] = $scope.AttachSelect2ToControl(Select2_TagsMultiple, "Scene-Prior AID", $scope.demo.ConfigProcedures.concat($scope.demo.ConfigMedications).concat(NullValuesList));

    //        $scope.demo.ConfigProtocols = $scope.CreateSelect2ArrayFromObjectArray(demographics, "ConfigProtocols", "ConfigProtocols");

    //        window["Protocols"] = $scope.AttachSelect2ToControl(Select2Multiple, "Protocols", $scope.demo.ConfigProtocols.concat(NullValuesList));


    //        $scope.demo.VehicleNumbers = $scope.CreateSelect2ArrayFromObjectArray(demographics, "Vehicles", "VehicleNumber");

    //        window["incidentUnitVehicleNumber"] = $scope.AttachSelect2ToControl(Select2Single, "incidentUnitVehicleNumber", $scope.demo.VehicleNumbers);

    //        //$scope.demo.ConfigEMSUnitNumber = $scope.CreateSelect2ArrayFromDelimitedString(demographics, "ConfigEMSUnitNumber");
    //        $scope.demo.VehicleCallSigns = $scope.CreateSelect2ArrayFromObjectArray(demographics, "Vehicles", "VehicleCallSign");

    //        window["Unit-Call Sign"] = $scope.AttachSelect2ToControl(Select2Single, "Unit-Call Sign", $scope.demo.VehicleCallSigns);

    //        $scope.demo.StationNames = $scope.CreateSelect2ArrayFromObjectArray(demographics, "Stations", "StationName");
    //        $scope.demo.ConfigOtherFacilityNames = $scope.CreateSelect2ArrayFromObjectArray(demographics, "ConfigOtherFacilitys", "ConfigOtherFacilityName");
    //        $scope.demo.ConfigSceneNames = $scope.CreateSelect2ArrayFromObjectArray(demographics, "ConfigScenes", "ConfigSceneName");
    //        $scope.demo.ConfigHospitalNames = $scope.CreateSelect2ArrayFromObjectArray(demographics, "ConfigHospitals", "ConfigHospitalName");
    //        window["VehicleDispatchLocation"] = $scope.AttachSelect2ToControl(Select2_TagsSingle, "VehicleDispatchLocation", $scope.demo.StationNames.concat($scope.demo.ConfigOtherFacilityNames).concat($scope.demo.ConfigHospitalNames));
    //        window["Incident-Destination"] = $scope.AttachSelect2ToControl(Select2_TagsSingle, "Incident-Destination", $scope.demo.ConfigOtherFacilityNames.concat($scope.demo.ConfigHospitalNames).concat(NullValuesList));
    //        window["quick-scene"] = $scope.AttachSelect2ToControl(Select2Single, "quick-scene", $scope.demo.ConfigSceneNames.concat($scope.demo.ConfigOtherFacilityNames.concat($scope.demo.ConfigHospitalNames)));

    //        $scope.demo.AgencyOtherAgenciesInArea = $scope.CreateSelect2ArrayFromDelimitedString(demographics, "AgencyOtherAgenciesInArea");
    //        window["AgenciesOnScene"] = $scope.AttachSelect2ToControl(Select2_TagsMultiple, "AgenciesOnScene", $scope.demo.AgencyOtherAgenciesInArea.concat(NullValuesList));

    //        $scope.demo.ConfigHospitalNumbers = $scope.CreateSelect2ArrayFromObjectArray(demographics, "ConfigHospitals", "ConfigHospitalNumber");
    //        $scope.demo.ConfigOtherFacilityNumbers = $scope.CreateSelect2ArrayFromObjectArray(demographics, "ConfigOtherFacilitys", "ConfigOtherFacilityNumber");
    //        window["LocationFacilityCode"] = $scope.AttachSelect2ToControl(Select2_TagsSingle, "LocationFacilityCode", $scope.demo.ConfigHospitalNumbers.concat($scope.demo.ConfigOtherFacilityNumbers).concat(NullValuesList));
    //        window["DestinationCode"] = $scope.AttachSelect2ToControl(Select2_TagsSingle, "DestinationCode", removeNullAndUndefinedFromList($scope.demo.ConfigHospitalNumbers.concat($scope.demo.ConfigOtherFacilityNumbers).concat(NullValuesList)));


    //        $scope.hospital.PersonnelFirstNames = $scope.CreateArrayFromObjectArray(demographics, "Personnels", "PersonnelFirstName");
    //        $scope.hospital.PersonnelLastNames = $scope.CreateArrayFromObjectArray(demographics, "Personnels", "PersonnelLastName");
    //        $scope.hospital.ConfigProcedures = $scope.CreateArrayFromObjectArray(demographics, "ConfigProcedures", "ConfigProcedure");
    //        $scope.hospital.ConfigMedications = $scope.CreateArrayFromObjectArray(demographics, "ConfigMedications", "ConfigMedicationsGiven");


    //        $scope.demo.Personnels = $scope.CreateSelect2ArrayForPersonnels(demographics);

    //        window["Crew - Primary"] = $scope.AttachSelect2ToControl(Select2Single, "Crew - Primary", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    //        window["Crew - Secondary"] = $scope.AttachSelect2ToControl(Select2Multiple, "Crew - Secondary", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    //        window["Crew - Third"] = $scope.AttachSelect2ToControl(Select2Multiple, "Crew - Third", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    //        window["Crew - Other"] = $scope.AttachSelect2ToControl(Select2Multiple, "Crew - Other", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    //        window["Crew - Driver"] = $scope.AttachSelect2ToControl(Select2Single, "Crew - Driver", $scope.demo.Personnels/*.concat(NullValuesList)*/);
    //        window["whoGeneratedThisReport"] = $scope.AttachSelect2ToControl(Select2Single, "whoGeneratedThisReport", $scope.demo.Personnels);
    //        window["medCrewMemberId"] = $scope.AttachSelect2ToControl(Select2Single, "medCrewMemberId", $scope.demo.Personnels);
    //        window["procCrewMemberId"] = $scope.AttachSelect2ToControl(Select2Single, "procCrewMemberId", $scope.demo.Personnels);

    //        $scope.autoFips = allowAutofips = defaultedValue(demographics.AutoFips, true);
    //        $scope.allowCustomOptions = defaultedValue(demographics.allowCustomOptions, true);

    //        var disabledDispositions = demographics.AgencyDisabledDispositions;
    //        if (disabledDispositions) {
    //            var disabledDispoList = disabledDispositions.split(";");
    //            for (var i = 0; i < $scope.dispositionsObject.length; i++) {
    //                for (var j = 0; j < disabledDispoList.length; j++) {
    //                    if (disabledDispoList[j].trim() == $scope.dispositionsObject[i].name) {
    //                        $scope.dispositionsObject.splice(i--, 1);
    //                        break;
    //                    }
    //                }
    //            }
    //        }

    //        // replace this with a demographics list?
    //        $scope.signatures = [
    //            {
    //                nodeTitle: "Crew_Primary",
    //                title: "Crew Primary",
    //                required: true,
    //                disclaimer: "",
    //                defaultValue: function () {
    //                    return $scope.pcr.crewPrimary ? $scope.pcr.crewPrimary.substr(0, $scope.pcr.crewPrimary.indexOf('(')).trim() : "";
    //                }
    //            },
    //            {
    //                nodeTitle: "Crew_Secondary",
    //                title: "Crew Secondary",
    //                required: false,
    //                disclaimer: "",
    //                defaultValue: function () {
    //                    return $scope.pcr.crewSecondary ? $scope.pcr.crewSecondary.substr(0, $scope.pcr.crewSecondary.indexOf('(')).trim() : "";
    //                }
    //            },
    //            {
    //                nodeTitle: "ETT_Confirmation",
    //                title: "ETT Confirmation",
    //                required: false,
    //                disclaimer: "My signature acknowledges that I have assessed the placement of the endotracheal tube upon the arrival to my facility.",
    //                defaultValue: function () {
    //                    return "";
    //                }
    //            },
    //            {
    //                nodeTitle: "Patient_RMA",
    //                title: "Patient, Refused Medical Assistance (RMA)",
    //                required: false,
    //                disclaimer: "I hereby refuse (treatment/transport to a hospital) and I acknowledge that such treatment/transportation was advised by the ambulance crew or physician. I hereby release such persons from liability for respecting and following my express wishes."
	//					+ "\n\n"
	//					+ "Mediante la presente declaro que me niego a aceptar el tratamiento/traslado a un hospital y reconozco asimismo que el medical o el personal de la ambulancia recomendaron ese tratamiento/traslado. Consiguientemente, eximo a dichas personas de toda responsibilidad por haber respetado y cumplido mis deseos expresos.",
    //                //	"I understand that the EMS personnel are not physicians and are not qualified or authorized to make a diagnosis and that their care is not a substitute for that of a physician. I recognize that I may have a serious injury or illness which could get worse without medical attention even though I (or the patient on whose behalf I legally sign this document) may feel fine at the present time.\n\nI understand that I may change my mind and call 9-1-1 if treatment or assistance is needed later. I also understand that treatment is available at an emergency department 24 hours a day or from my physician. If I have insisted on being transported to a destination other than that recommended by the EMS personnel, I understand and have been informed that there may be a significant delay in receiving care at the emergency room, that the emergency room may lack the staff, equipment, beds or resources to care for me promptly, and/or that I might not be able to be admitted to that hospital.\n\nI acknowledge that this advice has been explained to me by the ambulance crew and that I have read this form completely and understand its provisions. I agree, on my own behalf (and on behalf of the patient for whom I legally sign this document), to release, indemnify and hold harmless the ambulance service and its officers, members, employees or other agents, and the medical command physician and medical command facility, from any and all claims, actions, causes of action, damages, or legal liabilities of any kind arising out of my decision, or from any act or omission of the ambulance service or its crew, or the medical command physician or medical command facility.\n\nI also acknowledge receipt of the ambulance service's Notice of Privacy Practices.",
    //                defaultValue: function () {
    //                    return ($scope.pcr.patientFirstName || "") + " " + ($scope.pcr.patientLastName || "").trim();
    //                }
    //            },
    //            {
    //                nodeTitle: "Patient",
    //                title: "Patient",
    //                required: false,
    //                disclaimer: "I authorize the submission of a claim to Medicare, Medicaid, or any other payer for any services provided to me by EMS now, in the past, or in the future, until such time as I revoke this authorization in writing. I understand that I am financially responsible for the services and supplies provided to me by EMS, regardless of my insurance coverage, and in some cases, may be responsible for an amount in addition to that which was paid by my insurance. I agree to immediately remit to EMS any payments that I receive directly from insurance or any source whatsoever for the services provided to me and I assign all rights to such payments to EMS. I authorize EMS to appeal payment denials or other adverse decisions on my behalf. I authorize and direct any holder of medical, insurance, billing or other relevant information about me to release such information to EMS and its billing agents, the Centers for Medicare and Medicaid Services, and/or any other payers or insurers, and their respective agents or contractors, as may be necessary to determine these or other benefits payable for any services provided to me by EMS, now, in the past, or in the future.  I also authorize EMS to obtain medical, insurance, billing and other relevant information about me from any party, database or other source that maintains such information. . Privacy Practices Acknowledgement, by signing below, I acknowledge that I have received a copy of EMS' Notice of Privacy Practices.",
    //                defaultValue: function () {
    //                    return ($scope.pcr.patientFirstName || "") + " " + ($scope.pcr.patientLastName || "").trim();
    //                }
    //            },
    //            {
    //                nodeTitle: "Student_Author",
    //                title: "Student Author",
    //                required: false,
    //                disclaimer: "Signature of the student that completed the documentation. I have written the patient care report and can attest that the content is true and accurate to the best of my knowledge.",
    //                defaultValue: function () {
    //                    return "";
    //                }
    //            },
    //            {
    //                nodeTitle: "Peer_Review",
    //                title: "Peer Review",
    //                required: false,
    //                disclaimer: "I have reviewed the patient care report and can attest that the content is true and accurate to the best of my knowledge.",
    //                defaultValue: function () {
    //                    return "";
    //                }
    //            },
    //            {
    //                nodeTitle: "AuthRep",
    //                title: "Patient Representative",
    //                required: false,
    //                disclaimer: "I am signing on behalf of the patient. I recognize that signing on behalf of the patient is not an acceptance of financial responsibility for the services rendered. Notice of Privacy Practices applicable only to the following: Patient's Legal Guardian or Patient's Durable Power of Attorney who arranges treatments or handles the patient's affairs. Privacy Practices Acknowledgement, by signing below, I acknowledge that I have received a copy of EMS' Notice of Privacy Practices.",
    //                defaultValue: function () {
    //                    return "";
    //                }
    //            },
    //            {
    //                nodeTitle: "Witness",
    //                title: "Witness",
    //                required: false,
    //                disclaimer: "I acknowledge that my signature is in witness to the above person(s) signing patient care report or refusal.",
    //                defaultValue: function () {
    //                    return "";
    //                }
    //            },
    //            {
    //                nodeTitle: "Received_By",
    //                title: "Received By",
    //                required: false,
    //                disclaimer: "I acknowledge that I have received report and patient care for the above listed patient. I understand that my signature is only that I have received the transfer of care from EMS. My signature is not acceptance of financial responsibility for the services rendered.",
    //                defaultValue: function () {
    //                    return "";
    //                }
    //            },
    //            {
    //                nodeTitle: "Guardian",
    //                title: "Parent/Guardian",
    //                required: false,
    //                disclaimer: "",
    //                defaultValue: function () {
    //                    return "";
    //                }
    //            }
    //        ];


    //        if (demographics.AgencyRequiredFieldsArr) {
    //            //alert(JSON.stringify($scope.demo.AgencyRequiredFieldsArr));
    //            demographics.AgencyRequiredFieldsArr.forEach(function (element, index, array) {
    //                $("[data-ng-model=" + "'" + element + "'" + "]").rules("add", {
    //                    required: true
    //                });
    //                $("[data-ng-model=" + "'" + element + "'" + "]").addClass("RequiredRedBorder");
    //            });
    //        }

    //        //add the fips list based on configured fips
    //        var fipsUrl9 = "/api/FIPSQuery?fipslist=" + $scope.demo2["ConfigFIPSList"] + "&";
    //        $('.aaCustomFips').select2(
    //             {
    //                 placeholder: 'Enter City or County',
    //                 //Does the user have to enter any data before sending the ajax request
    //                 minimumInputLength: 3,
    //                 width: "100%",
    //                 allowClear: true,
    //                 data: offlineFipsList
    //                 //ajax: {
    //                 //    How long the user has to pause their typing before sending the next request
    //                 //    quietMillis: 150,
    //                 //    The url of the json service
    //                 //    type: "GET",
    //                 //    url: fipsUrl9,
    //                 //    Our search term and what page we are on
    //                 //    data: function (term, page) {
    //                 //        return {
    //                 //            q: term,
    //                 //        };
    //                 //    },
    //                 //    results: function (data, page) {
    //                 //        return { results: data };
    //                 //    }
    //                 //}
    //             });


    //        $scope.pcr.demo = {};
    //        $scope.pcr.demo.agencyNumber = "" + demographics["AgencyNumber"];
    //        $scope.pcr.demo.agencyLevelOfService = "" + demographics["AgencyLevelOfService"];
    //        $scope.pcr.demo.agencyOrganizationalType = "" + demographics["AgencyOrganizationalType"];
    //        $scope.pcr.demo.agencyOrganizationStatus = "" + demographics["AgencyOrganizationStatus"];
    //        $scope.pcr.demo.agencyNationalProviderIdentifier = "" + demographics["AgencyNationalProviderIdentifier"];
    //        $scope.pcr.demo.agencyStates = "" + demographics["AgencyState"];
    //        $scope.pcr.demo.agencyCountys = "" + demographics["AgencyCounty"];
    //        $scope.pcr.demo.AgencyName = demographics["AgencyName"];
    //        $scope.pcr.demo.AutoGenNarOpt = demographics["AutoGenNarOpt"];
    //        //   alert("New");

    //        if (demographics["AgencyContactAddresss"] && demographics["AgencyContactAddresss"][0]) {
    //            $scope.pcr.demo.agencyContactDemographicZip = "" + demographics["AgencyContactAddresss"][0]["AgencyContactDemographicZip"];
    //        }

    //        if (isNew) {
    //            //if (!($scope.pcr)) {
    //            //    $scope.InitializePcr();
    //            //}
    //            $scope.pcr.googleAddress = {};
    //            $scope.pcr.whoGeneratedThisReportCheckbox = true;
    //            if (!($scope.pcr.incidentDate) || $scope.pcr.incidentDate == "") {
    //                $scope.pcr.incidentDate = getDate();
    //                $scope.UpdateTimelineDates();
    //            }
    //            //  alert(JSON.stringify(demographics));
    //            $scope.FormActionSetAgencyDefaultValues("" + demographics["AgencyDefaultValues"]);
    //            $scope.pcr.currentDisposition = $scope.dispositionsObject[0];

    //            // var $LocationZoneNumber = $("input[title='']").first();
    //            // Select2Single($LocationZoneNumber, $scope.demo.ConfigZoneNumbers);
    //        }



    //        $scope.$apply();
    //        //}

    //        //$scope.ApplyDisposition();
    //    }
    //}

    $scope.sign = function (sigData) {
        var src = $scope.pcr.signatures[sigData.nodeTitle] ? $scope.pcr.signatures[sigData.nodeTitle].src : "";
        var name = $scope.pcr.signatures[sigData.nodeTitle] ? $scope.pcr.signatures[sigData.nodeTitle].name : sigData.defaultValue();
        signatureModal({
            src: src,
            name: name,
            nameProvided: sigData.defaultValue(),
            disclaimer: sigData.disclaimer || "",
            success: function (newSrc, name) {
                $scope.$evalAsync(function () {
                    $scope.pcr.signatures[sigData.nodeTitle] = {
                        title: sigData.title,
                        name: name,
                        src: newSrc,
                        date: new Date().toDateTime()
                    };
                    $scope.SaveOrUpdateHandler();
                });
            }
        });
    }
    $scope.clearSig = function (sigData) {
        // add confirmation
        $scope.$evalAsync(function () {
            delete $scope.pcr.signatures[sigData.nodeTitle];
        });
    }

    $scope.ProceduresNA = function () {
        $scope.pcr.procedures = [];
        $scope.pcr.procedures.push(
                   {
                       procProcedure: "***Not Applicable",
                       procComplication: "***Not Applicable",
                       procNumberOfAttempts: "***Not Applicable",
                       procSuccess: "***Not Applicable",
                   }
        );
        buildTimeline();
    };

    $scope.TreatmentMedicationsNA = function () {
        $scope.pcr.treatmentMedications = [];
        $scope.pcr.treatmentMedications.push(
                   {
                       medComplication: "***Not Applicable",
                       medName: "***Not Applicable"
                   }
        );
        buildTimeline();
    }

    $scope.UpdateVehicleDispatchZone = function () {
        var name = $scope.pcr.vehicleDispatchLocation;
        var stations = $scope.demo2["Stations"];
        var i;

        if (stations) {
            for (i = 0; i < stations.length; ++i) {
                if (stations[i]["StationName"] == name) {
                    $scope.pcr.vehicleDispatchZone = "" + stations[i]["StationZoneNumber"];
                    //$scope.AttachSelect2ToControl(Select2Single, "VehicleDispatchZone", $scope.demo.ConfigZoneNumbers);
                    $("input[title='VehicleDispatchZone']").select2("val", $scope.pcr.vehicleDispatchZone);
                    break;
                }
            }
        }

    };

    $scope.UpdateCallSign = function () {
        var VehicleNumber = $scope.pcr.incidentVehicleNumber;
        var Vehicles = $scope.demo2["Vehicles"];
        var i;
        if (Vehicles && Vehicles.length > 0) {
            for (i = 0; i < Vehicles.length; ++i) {
                if (Vehicles[i]["VehicleNumber"] == VehicleNumber) {
                    $scope.pcr.callSign = Vehicles[i]["VehicleCallSign"];
                    $("input[data-ng-model='pcr.callSign']").select2("val", $scope.pcr.callSign);
                    return;
                }
            }
        }
    };

    //// Since we allow defaulting based on disposition, i dont think this pattern is needed anymore
    ////$scope.NullableDefaultObjectProperties = {
    ////    "googleAddress": {
    ////        "targetAddress": "DestinationAddress",
    ////        "number": "",
    ////        "street": "***Not Applicable",
    ////        "city": "***Not Applicable",
    ////        "state": "***Not Applicable",
    ////        "zip": "***Not Applicable",
    ////        "fipsCounty": "***Not Applicable",
    ////        "fips": "***Not Applicable",
    ////        "country": "",
    ////        "combinedStreet": "***Not Applicable"
    ////    },
    ////    "whoGeneratedThisReportCheckbox": true,
    ////    "incidentDate": "",
    ////    "incidentOnsetDate": "",
    ////    "incidentCallRecievedDate": "",
    ////    "incidentDispatchNotifiedDate": "",
    ////    "incidentDispatchedDate": "",
    ////    "incidentEnrouteDate": "",
    ////    "incidentArriveSceneDate": "",
    ////    "incidentPtContactDate": "",
    ////    "incidentTransferPtDate": "",
    ////    "incidentDepartSceneDate": "",
    ////    "incidentArriveDestinationDate": "",
    ////    "incidentInServiceDate": "",
    ////    "incidentAtBaseDate": "",
    ////    "incidentUnitCanceledDate": "",
    ////    "currentDisposition": { "name": "Default", "results": { "outcome": "", "sections": [], "tables": [], "tabs": [] } },
    ////    "demo": { "agencyNumber": "29", "agencyLevelOfService": "EMT-Paramedic", "agencyOrganizationalType": "Community, Non-Profit", "agencyOrganizationStatus": "Mixed", "agencyNationalProviderIdentifier": "undefined" },
    ////    "SceneAddress": { "number": "", "street": "***Not Applicable", "combinedStreet": "***Not Applicable", "city": "***Not Applicable", "state": "***Not Applicable", "zip": "***Not Applicable", "country": "", "fips": "***Not Applicable", "fipsCounty": "***Not Applicable", "notes": "undefined", "valid": true },
    ////    "incidentVehicleResponseNumber": "***Not Applicable",
    ////    "servicesOnScene": "***Not Applicable",
    ////    "agenciesOnScene": "***Not Applicable",
    ////    "emsSystem": "***Not Applicable",
    ////    "delaysSwitch": true,
    ////    "delayDispatch": "***Not Applicable",
    ////    "delayTransport": "***Not Applicable",
    ////    "delayResponse": "***Not Applicable",
    ////    "delayScene": "***Not Applicable",
    ////    "delayTurnAround": "***Not Applicable",
    ////    "emdContact": "***Not Applicable",
    ////    "vehicleDispatchLocation": "***Not Applicable",
    ////    "incidentLocationFacilityCode": "***Not Applicable",
    ////    "incidentLocationLong": "",
    ////    "odometerStart": null,
    ////    "odometerScene": null,
    ////    "odometerDest": null,
    ////    "odometerTotal": null,
    ////    "odometerservice": null,
    ////    "crewPrimary": "***Not Applicable",
    ////    "whoGeneratedThisReport": "***Not Applicable",
    ////    "crewDriver": "***Not Applicable",
    ////    "crewSecondary": "***Not Applicable",
    ////    "crewThird": "***Not Applicable",
    ////    "crewOther": "***Not Applicable",
    ////    "patientFirstName": "***Not Applicable",
    ////    "patientLastName": "***Not Applicable",
    ////    "patientPhoneNumber": "***Not Applicable",
    ////    "patientMiddleName": "***Not Applicable",
    ////    "patientDOB": "",
    ////    "patientAge": null,
    ////    "patientAgeMonths": null,
    ////    "patientEthnicity": "***Not Applicable",
    ////    "patientGender": "Female",
    ////    "patientFemalePregant": "***Not Applicable",
    ////    "patientFemalePregnancyDuration": "N/A",
    ////    "patientDriverLicenseNumber": "***Not Applicable",
    ////    "practitionerLast": "***Not Applicable",
    ////    "patientDriverLicenseState": "***Not Applicable",
    ////    "patientMedicalHistory": "***Not Applicable",
    ////    "patientMedicationAllergies": "***Not Applicable",
    ////    "patientEnvironmentalAllergies": "***Not Applicable",
    ////    "advancedDirectives": "***Not Applicable",
    ////    "emergencyInfoFormSelect": "***Not Applicable",
    ////    "patientMedications": [{ "medicationName": "***Not Applicable", "medicationRoute": null }],
    ////    "primaryComplaint": "***Not Applicable",
    ////    "secondaryComplaint": "***Not Applicable",
    ////    "primaryComplaintDuration": "",
    ////    "secondaryComplaintDuration": "",
    ////    "barriersPatientCare": "***Not Applicable",
    ////    "secondarySymptoms": "***Not Applicable",
    ////    "primaryImpression": "***Not Applicable",
    ////    "secondaryImpressions": "***Not Applicable",
    ////    "signsOfDrugs": "***Not Applicable",
    ////    "cardiacArrestSelect": false,
    ////    "cardiacArrest": "***Not Applicable",
    ////    "mvc": "***Not Applicable",
    ////    "trauma": "***Not Applicable",
    ////    "traumaSelect": true,
    ////    "nhtsaNeck": "***Not Applicable",
    ////    "nhtsaThorax": "***Not Applicable",
    ////    "nhtsaAbdomen": "***Not Applicable",
    ////    "nhtsaSpine": "***Not Applicable",
    ////    "nhtsaPelvis": "***Not Applicable",
    ////    "nhtsaHead": "***Not Applicable",
    ////    "nhtsaFace": "***Not Applicable",
    ////    "nhtsaUpperExtremities": "***Not Applicable",
    ////    "nhtsaLowerExtremities": "***Not Applicable",
    ////    "nhtsaExternalSkin": "***Not Applicable",
    ////    "nhtsaUnscpecified": "***Not Applicable",
    ////    "exams": [{ "examDate": "", "examTime": "", "examSkin": "***Not Applicable", "examFace": "***Not Applicable", "examNeck": "***Not Applicable", "examLeftEye": "***Not Applicable", "examRightEye": "***Not Applicable", "examMental": "***Not Applicable", "examNeuro": "***Not Applicable", "examChest": "***Not Applicable", "examHeart": "***Not Applicable", "examAbsLeftUpper": "***Not Applicable", "examAbsLeftLower": "***Not Applicable", "examAbsRightUpper": "***Not Applicable", "examAbsRightLower": "***Not Applicable", "examGU": "***Not Applicable", "examBackCervical": "***Not Applicable", "examBackThoracic": "***Not Applicable", "examBackLumbar": "***Not Applicable", "examExtremRightUpper": "***Not Applicable", "examExtremRightLower": "***Not Applicable", "examExtremLeftLower": "***Not Applicable", "examExtremLeftUpper": "***Not Applicable" }],
    ////    "conditionCodes": "***Not Applicable",
    ////    "primaryPaymentMethod": "***Not Applicable",
    ////    "billingWorkRelatedSelect": "***Not Applicable",
    ////    "patientEmployer": "***Not Applicable",
    ////    "EmployerAddress": { "number": "", "street": "***Not Applicable", "combinedStreet": "***Not Applicable", "city": "***Not Applicable", "state": "***Not Applicable", "zip": "***Not Applicable", "country": "", "fips": "***Not Applicable", "fipsCounty": "***Not Applicable", "notes": "undefined", "valid": true },
    ////    "patientOccupationIndustry": "***Not Applicable",
    ////    "certificateMedNecessitySelect": "***Not Applicable",
    ////    "billingDifferentThanPatientSelect": false,
    ////    "billingDifferentThanPatient": "***Not Applicable",
    ////    "treatmentMedications": [{ "medComplication": "***Not Applicable", "medName": "***Not Applicable" }],
    ////    "procedures": [{ "procProcedure": "***Not Applicable", "procComplication": "***Not Applicable", "procNumberOfAttempts": "***Not Applicable", "procSuccess": "***Not Applicable" }],
    ////    "procSuccessfulIvSite": "***Not Applicable",
    ////    "procEtTubeConfirm": "***Not Applicable",
    ////    "protocols": "***Not Applicable",
    ////    "incidentDestination": "***Not Applicable",
    ////    "DestinationAddress": { "number": "", "street": "***Not Applicable", "combinedStreet": "***Not Applicable", "city": "***Not Applicable", "state": "***Not Applicable", "zip": "***Not Applicable", "country": "", "fips": "***Not Applicable", "fipsCounty": "***Not Applicable", "notes": "undefined", "valid": true },
    ////    "conditionOfPtAtDest": "***Not Applicable",
    ////    "incidentDestinationType": "***Not Applicable",
    ////    "incidentDestinationCode": "***Not Applicable",
    ////    "incidentDestinationReason": "***Not Applicable",
    ////    "massCasualitySelect": "***Not Applicable",
    ////    "numberOfPts": "***Not Applicable",
    ////    "edDisposition": "***Not Applicable",
    ////    "hospitalDisposition": "***Not Applicable",
    ////    "modeFromScene": "***Not Applicable",
    ////    "nuerologicalOutcomeAtDischarge": "***Not Applicable",
    ////    "reviewRequestedSelect": "***Not Applicable",
    ////    "fluidContactSelect": "***Not Applicable",
    ////    "emsDeathSelect": "***Not Applicable",
    ////    "requiredReportableConditionsSelect": "***Not Applicable",
    ////    "disasters": "***Not Applicable",
    ////    "fluidContactType": "***Not Applicable",
    ////    "emsDeathType": "***Not Applicable"
    ////};

    //// as stated above, i dont think this is needed 
    ////$scope.SetNullableValues = function () {
    ////    var nullableObject = $scope.NullableDefaultObjectProperties;

    ////    for (var key in nullableObject) {

    ////        if (nullableObject.hasOwnProperty(key)) {
    ////            var attr = nullableObject[key];

    ////            //alert(""  key  " : "  attr);

    ////            //alert("[data-ng-model='pcr."  key  "']");

    ////            var $thisControl = $("[data-ng-model='pcr." + key + "']").first();
    ////            // alert(JSON.stringify($thisControl));

    ////            if ($thisControl.length == 1 && !ControlIsVisible($thisControl)) {
    ////                //  alert("before - " + key  + " : " +$scope.pcr[key]);
    ////                $scope.pcr[key] = nullableObject[key];
    ////                // alert("after - " + key + " : " + $scope.pcr[key]);

    ////            }


    ////        }
    ////    }

    ////    //Exception Cases

    ////    //Patient Medications
    ////    if (!JarvisWidgetIsVisible($("#PatientMedicationsJWidget"))) {
    ////        $scope.pcr.patientMedications = nullableObject.patientMedications;
    ////    }

    ////    //exams
    ////    if (!JarvisWidgetIsVisible($("#ExamsJWidget"))) {
    ////        $scope.pcr.exams = nullableObject.exams;
    ////    }

    ////    //Treatment Meds        
    ////    if (!JarvisWidgetIsVisible($("#TreatmentMedsJWidget"))) {
    ////        $scope.pcr.treatmentMedications = nullableObject.treatmentMedications;
    ////    }

    ////    //procedures    
    ////    if (!JarvisWidgetIsVisible($("#ProceduresJWidget"))) {
    ////        $scope.pcr.procedures = nullableObject.procedures;
    ////    }

    ////    //Scene Address
    ////    if (!ControlIsVisible($("[data-ng-click=\"SetTargetAddress('SceneAddress');\"]"))) {
    ////        $scope.pcr.SceneAddress = nullableObject.SceneAddress;
    ////    }

    ////    //employeraddress
    ////    if (!ControlIsVisible($("[data-ng-click=\"SetTargetAddress('EmployerAddress');\"]"))) {
    ////        $scope.pcr.EmployerAddress = nullableObject.EmployerAddress;
    ////    }

    ////    //DestinationAddress
    ////    if (!ControlIsVisible($("[data-ng-click=\"SetTargetAddress('DestinationAddress');\"]"))) {
    ////        $scope.pcr.DestinationAddress = nullableObject.DestinationAddress;
    ////    }

    ////    //MVC
    ////    if (!ControlIsVisible($("[data-ng-model='pcr.mvcSelect']"))) {
    ////        $scope.pcr.mvc = nullableObject.mvc;
    ////    }

    ////    //MVC
    ////    if (!ControlIsVisible($("[data-ng-model='pcr.traumaSelect']"))) {
    ////        $scope.pcr.trauma = nullableObject.trauma;
    ////    }

    ////    //EMD Cotnact
    ////    if (!ControlIsVisible($("[data-ng-model='pcr.emdContactSelect']"))) {
    ////        $scope.pcr.emdContact = nullableObject.emdContact;
    ////    }

    ////    //billingDifferentThanPatient
    ////    if (!ControlIsVisible($("[data-ng-model='pcr.billingDifferentThanPatientSelect']"))) {
    ////        $scope.pcr.billingDifferentThanPatient = nullableObject.billingDifferentThanPatient;
    ////    }

    ////    //pcr.cardiacArrestSelect
    ////    if (!ControlIsVisible($("[data-ng-model='pcr.cardiacArrestSelect']"))) {
    ////        $scope.pcr.cardiacArrest = nullableObject.cardiacArrest;
    ////    }

    ////    //OdometerTotal
    ////    if (!JarvisWidgetIsVisible($("#OdometerJWidget"))) {
    ////        $scope.pcr.odometerTotal = nullableObject.odometerTotal;
    ////    }

    ////    //patient age
    ////    if (!ControlIsVisible($("[data-ng-model='pcr.patientDOB']"))) {
    ////        $scope.pcr.patientAge = nullableObject.patientAge;
    ////        $scope.pcr.patientAgeMonths = nullableObject.patientAgeMonths;

    ////    }


    ////};




    //// This wont work in the new ui framework, and probably is not as relevent
    ////function JarvisWidgetIsVisible(thisControl) {
    ////    var isVisible = true;
    ////    if ("none" == $(thisControl).closest('.jarviswidget').css('display')) {
    ////        isVisible = false;
    ////    }
    ////    else {
    ////        var tabId = $(thisControl).closest('[tabs]').attr('id');

    ////        //alert("tabId:"  tabId);

    ////        var tabBtnDisplayProperty = $("nav [onclick=" + '"' + "ShowTab('" + tabId + "')" + ';"' + "]").css('display');

    ////        //alert("nav [onclick="  '"'  "ShowTab('"  tabId  "')"  '"'  "]");

    ////        if ("none" == tabBtnDisplayProperty) {
    ////            isVisible = false;
    ////            // alert("tabs");
    ////        }
    ////    }

    ////    return isVisible;

    ////}

    //// This wont work in the new ui framework, and probably is not as relevent
    ////function ControlIsVisible(thisControl) {
    ////    var isVisible = true;

    ////    if ("none" == $(thisControl).closest('section').css('display')) {
    ////        isVisible = false;
    ////    }

    ////    else if ("none" == $(thisControl).closest('.jarviswidget').css('display')) {
    ////        isVisible = false;
    ////    }
    ////    else {
    ////        var tabId = $(thisControl).closest('[tabs]').attr('id');

    ////        //alert("tabId:"  tabId);

    ////        var tabBtnDisplayProperty = $("nav [onclick=" + '"' + "ShowTab('" + tabId + "')" + ';"' + "]");

    ////        if ($rootScope.UI.isMobile)
    ////            tabBtnDisplayProperty = tabBtnDisplayProperty.eq(1).css('display');
    ////        else
    ////            tabBtnDisplayProperty = tabBtnDisplayProperty.eq(0).css('display');

    ////        //alert("nav [onclick="  '"'  "ShowTab('"  tabId  "')"  '"'  "]");

    ////        if ("none" == tabBtnDisplayProperty) {
    ////            isVisible = false;
    ////            //  alert("tabs");
    ////        }
    ////    }

    ////    return isVisible;

    ////}












    $scope.UpdateVehicleNumber = function () {
        var CallSign = $scope.pcr.callSign;
        var Vehicles = $scope.demo2["Vehicles"];
        var i;
        if (Vehicles && Vehicles.length > 0) {
            for (i = 0; i < Vehicles.length; ++i) {
                if (Vehicles[i]["VehicleCallSign"] == CallSign) {
                    $scope.pcr.incidentVehicleNumber = Vehicles[i]["VehicleNumber"];
                    $("input[data-ng-model='pcr.incidentVehicleNumber']").select2("val", $scope.pcr.incidentVehicleNumber);
                    return;
                }
            }
        }
    };


    //// This is deprecated if we do not have multiple certs for each medic
    ////$scope.GetMostRecentPersonnelCertification = function (certs) {

    ////    if (certs == null)
    ////        return;

    ////    var i;
    ////    var mostRecentCert = { PersonnelAgencyCertificationStatus: "Nothing Found", day: 0, month: 0, year: 0 };
    ////    for (i = 0; i < certs.length; ++i) {
    ////        if (certs[i].PersonnelAgencyCertificationStatusDate) {
    ////            var date = certs[i].PersonnelAgencyCertificationStatusDate.split('/');
    ////            var day = date[1];
    ////            var month = date[0];
    ////            var year = date[2];
    ////            //alert(certs[i].PersonnelAgencyCertificationStatusDate);
    ////            //alert("new: " + year + "|old: " + mostRecentCert.year);
    ////            if ((year > mostRecentCert.year) || (year == mostRecentCert.year && month > mostRecentCert.month) || (year == mostRecentCert.year && month == mostRecentCert.month && day == mostRecentCert.day)) {
    ////                mostRecentCert = certs[i];
    ////                mostRecentCert.year = year;
    ////                mostRecentCert.month = month;
    ////                mostRecentCert.day = day;
    ////            }
    ////        }
    ////    }

    ////    return mostRecentCert.PersonnelAgencyCertificationStatus;
    ////};



    /*
        Timeline
    */

    //// this is valid code i belive we can reuse
    function getTimelineItems() {
        var timeline = [];
        if ($scope.pcr.incidentOnsetTime && $scope.pcr.incidentOnsetDate)
            timeline.push({ timelineTime: $scope.pcr.incidentOnsetTime, timelineDate: $scope.pcr.incidentOnsetDate, text: "Incident Onset", icon: "fa-play-circle" });
        if ($scope.pcr.incidentCallRecievedTime && $scope.pcr.incidentCallRecievedDate)
            timeline.push({ timelineTime: $scope.pcr.incidentCallRecievedTime, timelineDate: $scope.pcr.incidentCallRecievedDate, text: "Call Recieved", icon: "fa-phone" });
        if ($scope.pcr.incidentDispatchNotifiedTime && $scope.pcr.incidentDispatchNotifiedDate)
            timeline.push({ timelineTime: $scope.pcr.incidentDispatchNotifiedTime, timelineDate: $scope.pcr.incidentDispatchNotifiedDate, text: "Dispatch Notified", icon: "fa-comment-o" });
        if ($scope.pcr.incidentDispatchedTime && $scope.pcr.incidentDispatchedDate)
            timeline.push({ timelineTime: $scope.pcr.incidentDispatchedTime, timelineDate: $scope.pcr.incidentDispatchedDate, text: "Unit Dispatched", icon: "fa-comments-o" });
        if ($scope.pcr.incidentEnrouteTime && $scope.pcr.incidentEnrouteDate)
            timeline.push({ timelineTime: $scope.pcr.incidentEnrouteTime, timelineDate: $scope.pcr.incidentEnrouteDate, text: "Unit Enroute", icon: "fa-compass" });
        if ($scope.pcr.incidentArriveSceneTime && $scope.pcr.incidentArriveSceneDate)
            timeline.push({ timelineTime: $scope.pcr.incidentArriveSceneTime, timelineDate: $scope.pcr.incidentArriveSceneDate, text: "Unit Arrived at Scene", icon: "fa-thumbs-o-up" });
        if ($scope.pcr.incidentPtContactTime && $scope.pcr.incidentPtContactDate)
            timeline.push({ timelineTime: $scope.pcr.incidentPtContactTime, timelineDate: $scope.pcr.incidentPtContactDate, text: "Patient Contact", icon: "fa-user" });
        if ($scope.pcr.incidentTransferPtTime && $scope.pcr.incidentTransferPtDate)
            timeline.push({ timelineTime: $scope.pcr.incidentTransferPtTime, timelineDate: $scope.pcr.incidentTransferPtDate, text: "Transferred Patient", icon: "fa-external-link-square" });
        if ($scope.pcr.incidentDepartSceneTime && $scope.pcr.incidentDepartSceneDate)
            timeline.push({ timelineTime: $scope.pcr.incidentDepartSceneTime, timelineDate: $scope.pcr.incidentDepartSceneDate, text: "Scene Departure", icon: "fa-mail-forward" });
        if ($scope.pcr.incidentArriveDestinationTime && $scope.pcr.incidentArriveDestinationDate)
            timeline.push({ timelineTime: $scope.pcr.incidentArriveDestinationTime, timelineDate: $scope.pcr.incidentArriveDestinationDate, text: "Unit Arrived at Destination", icon: "fa-map-marker" });
        if ($scope.pcr.incidentInServiceTime && $scope.pcr.incidentInServiceDate)
            timeline.push({ timelineTime: $scope.pcr.incidentInServiceTime, timelineDate: $scope.pcr.incidentInServiceDate, text: "Unit Back in Service", icon: "fa-ambulance" });
        if ($scope.pcr.incidentAtBaseTime && $scope.pcr.incidentAtBaseDate)
            timeline.push({ timelineTime: $scope.pcr.incidentAtBaseTime, timelineDate: $scope.pcr.incidentAtBaseDate, text: "Unit Back at Base", icon: "fa-home" });
        if ($scope.pcr.incidentUnitCanceledTime && $scope.pcr.incidentUnitCanceledDate)
            timeline.push({ timelineTime: $scope.pcr.incidentUnitCanceledTime, timelineDate: $scope.pcr.incidentUnitCanceledDate, text: "Unit Cancelled", icon: "fa-ban" });
        return $filter('orderBy')($filter('orderBy')(timeline, 'timelineTime'), 'timelineDate');;
    }

    // build timeline on form change, if form is the procs page
    //// again probably can reuse, but why is it equaling a local variable of buildTimeLine?
    //// remove distracting times that from getTimeLineItems that do not matter, like backAtBase
    buildTimeline = $scope.buildTimeline = function () {
        var timeline = getTimelineItems();

        var list = JSON.parse(JSON.stringify(timeline.concat($scope.pcr.exams || []).concat($scope.pcr.vitals || []).concat($scope.pcr.procedures || []).concat($scope.pcr.treatmentMedications || [])));
        var timeline = document.getElementById('timeline');
        timeline.innerHTML = '';

        var vitalIndex = 0, procIndex = 0, medIndex = 0, examIndex = 0, timelineIndex = 0, dates = [];
        for (var i = 0; i < list.length; i++) {
            if (list[i].medTime)
                timelineItem = medsTable(list[i], medIndex++);
            else if (list[i].vitalTime)
                timelineItem = vitalTable(list[i], vitalIndex++);
            else if (list[i].procTime)
                timelineItem = procTable(list[i], procIndex++);
            else if (list[i].examTime)
                timelineItem = examTable(list[i], examIndex++);
            else if (list[i].timelineTime)
                timelineItem = timelineTable(list[i], timelineIndex++);
            else continue;
            $compile(timelineItem)($scope); // compile here to avoid compiling in the 3 different functions
            // only 1 time and its corresponding date will be non-null for any element, so just use the null coalescing op to get the existing properties 
            var date = new Date((list[i].examDate || list[i].medDate || list[i].procDate || list[i].vitalDate || list[i].timelineDate) + " " + (list[i].examTime || list[i].medTime || list[i].procTime || list[i].vitalTime || list[i].timelineTime));
            for (var index = 0; index < dates.length; index++)
                if (date < dates[index]) break;

            if (index < dates.length)
                timeline.insertBefore(timelineItem, timeline.childNodes[index * 2]); // index * 2 to skip over timeline breaks
            else
                timeline.appendChild(timelineItem);
            $(timelineItem).after(document.createElement("br"));
            dates.splice(index, 0, date);
        }
        $('.data').dataTable({ searching: false, info: false, paging: false, ordering: false, autoWidth: true });
        +$('.table-outline').css("border", "3px solid silver");
    };

    /*
        gets the decimal representation of a time string
    */
    function timeAsNumber(time) {
        var temp;
        temp = time.split(':');
        return Number(temp[0]) + Number(temp[1]) / 60;
    }

    /*
        checks if ob's time/date are less than test's time/date
        ob < test
        accepts two data objects and their types
        valid types are 'med' or 'proc' or 'vital'
    */
    function compareTimes(ob, obType, test, testType) {
        if (ob[obType + "Date"] < test[testType + "Date"])
            return true;
        if (ob[obType + "Date"] > test[testType + "Date"])
            return false;
        return timeAsNumber(ob[obType + "Time"]) < timeAsNumber(test[testType + "Time"]);
    }

    /*
        adds an arrow element between timeline events
    */
    function timelineBreak() {
        var template = document.createElement("tr");
        template.innerHTML = '<td class="text-center" colspan="2"><h1 class="text-center"><span class="fa fa-arrow-down"></span></h1></td>';
        return template;
    }

    /*
        adds a timeline datetime
    */
    function timelineTable(data, i) {
        var template = document.createElement("tr");
        template.innerHTML = '<td class="text-center" style="width: 40px"><h1><span class="widget-icon fa fw ' + data.icon + '"></span></h1></td> <td class="table-outline text-center"><label style="margin:0; font-size: larger;">' + data.text + ' (' + data.timelineTime + ')</label></td>';
        return template;
    };

    /*
        adds a vital timeline event
    */
    function vitalTable(data, i) {
        var template = document.createElement("tr");
        template.innerHTML = '<td class="text-center" style="width: 40px"><h1><span class="widget-icon fa fa-heart"></span></h1></td> <td class="table-outline"> <table class="datatables data no-padding" style="vertical-align: top;"> <thead> <tr> <th style="width: 40px;">Edit</th> <th style="width: 100px;">Date</th> <th style="width: 60px;">Time</th> <th>Pulse</th> <th>Resp</th> <th>S-BP</th> <th>OX</th> <th>AVPU</th> <th>Delete</th> </tr> </thead> <tbody> <tr> <td> <span class="widget-icon"><i class="fa fa-edit" data-toggle="modal" data-ng-click="EditModalVital(' + i + ');InstantiateSelect2sByEvent($event);" data-target="[modaltarget=\'VitalModal\']"></i></span> </td> <td><span>' + $filter('date')(data.vitalDate, 'mm/dd/yy') + '</span></td> <td> <span>' + (data.vitalTime ? data.vitalTime : '') + '</span> </td> <td> <span>' + (data.vitalPulse ? data.vitalPulse : '') + '</span> </td> <td> <span>' + (data.vitalResp ? data.vitalResp : '') + '</span> </td> <td> <span>' + (data.vitalSbp ? data.vitalSbp : '') + '</span> </td> <td> <span>' + (data.vitalOx ? data.vitalOx : '') + '</span> </td> <td> <span>' + (data.vitalAVPU ? data.vitalAVPU : '') + '</span> </td> <td><i class="fa fa-eraser" data-ng-click="removeVital(' + i + ')"></i></td> </tr> </tbody> </table> <span class="col-xs-12"><b>Notes:</b> ' + (data.Notes || "") + '</span> </td>';
        return template;
    };

    /*
        adds a med timeline event
    */
    function medsTable(data, i) {
        var template = document.createElement("tr");
        template.innerHTML = '<td class="text-center" style="width: 40px"><h1><span class="widget-icon fa fa-medkit"></span></h1></td> <td class="table-outline"><table style="vertical-align: top" class="datatables data no-padding"> <thead> <tr> <th style="width: 40px;">Edit</th> <th style="width: 100px;">Date</th> <th style="width: 60px;">Time</th> <th>Name</th> <th>Dosage</th> <th>Route</th> <th>Crew</th> <th>Delete</th> </tr> </thead> <tbody> <tr vw-required="true" vw-minstringconstraint="2" vw-maxstringconstraint="30"> <td> <span class="widget-icon"><i class="fa fa-edit" data-toggle="modal" data-ng-click="EditModalTreatmentMedication(' + i + ');InstantiateSelect2sByEvent($event);" data-target="[modaltarget=\'TreatmentMedicationModal\']"></i></span> </td> <td> <span>' + (data.medDate ? data.medDate : '') + '</span> </td> <td> <span>' + (data.medTime ? data.medTime : '') + '</span> </td> <td> <span>' + (data.medName ? data.medName : '') + '</span> </td> <td> <span>' + (data.medDosage ? data.medDosage : '') + '-' + (data.medDosageUnits ? data.medDosageUnits : '') + '</span> </td> <td> <span>' + (data.medRoute ? data.medRoute : '') + '</span> </td> <td> <span>' + (data.medCrewMemberId ? data.medCrewMemberId : '') + '</span> </td> <td><i class="fa fa-eraser" data-ng-click="removeTreatmentMedication(' + i + ')"></i></td> </tr> </tbody> </table><span class="col-xs-12"><b>Notes:</b> ' + (data.Notes || "") + '</span> </td>';
        return template;
    };

    /*
        adds an exam timeline event
    */
    function examTable(data, i) {
        var template = document.createElement("tr");
        template.innerHTML = '<td class="text-center" style="width: 40px"><h1><span class="widget-icon fa fa-stethoscope"></span></h1></td> <td class="table-outline"> <table class="datatables data no-padding" style="vertical-align: top;"> <thead> <tr> <th style="width: 40px;">Edit</th> <th style="width: 100px;">Date</th> <th style="width: 60px;">Time</th> <th>Head</th> <th>Mental</th> <th>Neuro</th> <th>Chest</th> <th>Heart</th> <th>Delete</th> </tr> </thead> <tbody> <tr> <td> <span class="widget-icon"><i class="fa fa-edit" data-toggle="modal" data-ng-click="EditModalExam(' + i + ');InstantiateSelect2sByEvent($event);" data-target="[modaltarget=\'ExamModal\']"></i></span> </td> <td><span>' + $filter('date')(data.examDate, 'mm/dd/yy') + '</span></td> <td> <span>' + (data.examTime ? data.examTime : '') + '</span> </td> <td> <span>' + (data.examHead ? data.examHead : '') + '</span> </td> <td> <span>' + (data.examMental ? data.examMental : '') + '</span> </td> <td> <span>' + (data.examNeuro ? data.examNeuro : '') + '</span> </td> <td> <span>' + (data.examChest ? data.examChest : '') + '</span> </td> <td> <span>' + (data.examHeart ? data.examHeart : '') + '</span> </td> <td><i class="fa fa-eraser" data-ng-click="removeExam(' + i + ')"></i></td> </tr> </tbody> </table> <span class="col-xs-12"><b>Notes:</b> ' + (data.examNotes || "") + '</span> </td>';
        return template;
    };

    /*
        adds a procedure timeline event
    */
    function procTable(data, i) {
        var template = document.createElement("tr");
        template.innerHTML = '<td class="text-center" style="width: 40px"><h1><span class="widget-icon fa fa-user-md"></span></h1></td> <td class="table-outline"><table style="vertical-align: top" class="datatables data no-padding"> <thead> <tr> <th style="width: 40px;">Edit</th> <th style="width: 100px;">Date</th> <th style="width: 60px;">Time</th> <th>Name</th> <th>Size</th> <th>Response</th> <th>Success</th> <th>Delete</th> </tr> </thead> <tbody> <tr vw-required="true" vw-minstringconstraint="0" vw-maxstringconstraint="1000"> <td> <span class="widget-icon"><i class="fa fa-edit" data-toggle="modal" data-ng-click="EditModalProcedure(' + i + ');InstantiateSelect2sByEvent($event);" data-target="[modaltarget=\'ProcedureModal\']"></i></span> </td> <td><span>' + $filter('date')(data.procDate, 'mm/dd/yy') + '</span></td> <td> <span>' + (data.procTime ? data.procTime : '') + '</span> </td> <td> <span>' + (data.procProcedure ? data.procProcedure : '') + '</span> </td> <td> <span>' + (data.procSizeOfEquipment ? data.procSizeOfEquipment : '') + '</span> </td> <td> <span>' + (data.procResponse ? data.procResponse : '') + '</span> </td> <td> <span>' + (data.procSuccess ? data.procSuccess : '') + '</span> </td> <td><i class="fa fa-eraser" data-ng-click="removeProcedure(' + i + ')"></i></td> </tr> </tbody> </table><span class="col-xs-12"><b>Notes:</b> ' + (data.Notes || "") + '</span> </td>';
        return template;
    };

    /* On document load, create Vital jQuery Tabs */
    $("#vitalTabs").tabs();

    //$scope.ShowTab = ShowTab;
    //$scope.activeTabs = {
    //    prevIcon: '',
    //    prev: '',
    //    current: '',
    //    next: '',
    //    nextIcon: ''
    //}

    //changeTab('tabs-Incident');
    //changeTab = function (tabID) {
    //    if (!tabID) tabID = $scope.activeTabs.current;
    //    $scope.activeTabs.prev = '';
    //    $scope.activeTabs.next = '';
    //    $scope.activeTabs.current = tabID;
    //    var _this = $("#" + tabID + "-btn").parent();
    //    var parent = _this.parent();
    //    var children = parent.children();
    //    var numChildren = children.length;

    //    var thisIndex = -1;
    //    var prevIndex = -1;
    //    var nextIndex = -1;
    //    var substr;
    //    for (var i = 0; i < numChildren; i++) {
    //        if ($(children[i].innerHTML).attr("id")) {
    //            substr = $(children[i].innerHTML).attr("id");
    //            substr = substr.substr(0, substr.length - 4);
    //            if (tabID === substr) {
    //                thisIndex = i;
    //                continue;
    //            }
    //            if ($($(children[i]).children()[0]).is(":visible") && thisIndex === -1) {
    //                prevIndex = i;
    //                $scope.activeTabs.prev = substr;
    //            }
    //            if (thisIndex >= 0 && $($(children[i]).children()[0]).is(":visible")) {
    //                nextIndex = i;
    //                $scope.activeTabs.next = substr;
    //                break;
    //            }
    //        }
    //    }
    //    if (prevIndex >= 0) {
    //        $scope.activeTabs.prevIcon = $($($($(children[prevIndex]).children()[0])).children()[0]).attr("class");
    //    }
    //    if (nextIndex >= 0) {
    //        $scope.activeTabs.nextIcon = $($($($(children[nextIndex]).children()[0])).children()[0]).attr("class");
    //    }

    //    //$("#tabText").text(tabID.replace("tabs-", ""));
    //}


    //$scope.tabShown = function (tab) {
    //    return $(tab).is(":visible");
    //};

    //$scope.setScroll = function (posX, posY) {
    //    if (!posX) posX = 0;
    //    if (!posY) posY = 0;
    //    window.scrollTo(posX, posY);
    //};

    /*
     ******************
     ************************
     ************************
     ************************
     ******************



     AFWEFJ WEFCEH H ER

     Nates One to Manys

     ******/

    //// Submitting new synonyms is being deprecated
    ////$scope.SubmitSynonyms = function ($event) {

    ////    $scope.Synonym.Ngmodel = $($event.target).parent().parent().parent().find('.helpModalTargetModel').html()
    ////    $scope.synonymsShow = false;
    ////    $scope.Synonym.Status = "Approved";

    ////    $.ajax({
    ////        type: "POST",
    ////        url: "/api/Synonym",
    ////        data: $scope.Synonym
    ////    })
	////	.done(function (msg) {

	////	})
	////	.fail(function (xhr, textStatus, errorThrown) {

	////	});

    ////    $scope.Synonym.Synonym1 = "";
    ////}




    $scope.HideAndClearModal2 = function (jqueryTargetModal) {

        $(jqueryTargetModal + " input").val('');


        $(jqueryTargetModal + " input").each(function (index) {


            $(this).select2("val", null);
            //$('[data-clear-select2="' + modalId + '"]').select2("val", null)
            //var thisValue = angular.element(this).data('$ngModelController');
            //alert("" + thisValue);
            //thisValue = "";
        });


        $(jqueryTargetModal).modal('hide');




    };

    //// This is achieved by the above function
    ////$scope.HideAndClearModalSelect2 = function (jqueryTargetModal) {

    ////    $(jqueryTargetModal + " input").each(function (index) {
    ////        $(this).select2("val", null);
    ////    });

    ////    $(jqueryTargetModal).modal('hide');
    ////};

    //// This looks like exactly above
    ////$scope.ClearModal2 = function (jqueryTargetModal) {


    ////    $(jqueryTargetModal + " input").val('');


    ////    $(jqueryTargetModal + " input").each(function (index) {


    ////        $(this).select2("val", null);
    ////        //$('[data-clear-select2="' + modalId + '"]').select2("val", null)
    ////        //var thisValue = angular.element(this).data('$ngModelController');
    ////        //alert("" + thisValue);
    ////        //thisValue = "";
    ////    });



    ////};

    $scope.ClearModalSelect2 = function (jqueryTargetModal) {
        $(jqueryTargetModal + " input").each(function (index) {
            $(this).select2("val", null);
        });
    };

    $scope.HideModal = function (jqueryTargetModal) {
        $(jqueryTargetModal).modal('hide');
    };

    //Immunization Functions
    $scope.AddImmunization = function () {

        if ($scope.pcr.immunizations == null)
            $scope.pcr.immunizations = [];


        var formId = parseInt($scope.ImmunizationForm.ModalFormId);

        var ImmunizationObject = {
            immunizationDate: $scope.ImmunizationForm.immunizationDate,
            immunizationType: $scope.ImmunizationForm.immunizationType
        };

        $scope.ImmunizationForm = {};


        if (formId >= 0) {
            $scope.pcr.immunizations.splice(formId, 1, ImmunizationObject
            );
        }
        else {
            $scope.pcr.immunizations.push(
                    ImmunizationObject

            );
        }

    };

    $scope.removeImmunization = function (index) {
        if (confirm("Are you sure you want to DELETE the item?") == true) {
            $scope.pcr.immunizations.splice(index, 1);
        }
    };

    $scope.EditModalImmunization = function (itemId) {



        $scope.ImmunizationForm = {
            immunizationDate: $scope.pcr.immunizations[itemId].immunizationDate,
            immunizationType: $scope.pcr.immunizations[itemId].immunizationType,
            ModalFormId: itemId
        };

        $scope.$apply();

    };

    $scope.ClearImmunizationForm = function () {
        $scope.ImmunizationForm = {};
    };

    $scope.SubmitImmunizationForm = function (keepOpen) {
        $scope.AddImmunization();
        if (keepOpen) {
            $scope.ClearModal2('[modaltarget=ImmunizationModal]');
            $("[name='ImmunizationForm_immunizationDate']").focus();
            $scope.setScroll();
        }
        else {
            $scope.HideAndClearModal2('[modaltarget=ImmunizationModal]');
            $scope.ClearImmunizationForm();
            toastr.success('Item Added', '');
            $scope.SaveOrUpdateHandler();
        }
    };

    //Insurance Functions
    $scope.AddInsurance = function (keepOpen) {

        if ($scope.pcr.insurances == null)
            $scope.pcr.insurances = [];


        var formId = parseInt($scope.InsuranceForm.ModalFormId);

        var InsuranceObject = {
            insuranceCompanyBillingPriority: $scope.InsuranceForm.insuranceCompanyBillingPriority,
            insuranceCompany: $scope.InsuranceForm.insuranceCompany,
            insuranceGroup: $scope.InsuranceForm.insuranceGroup,
            insurancePolicy: $scope.InsuranceForm.insurancePolicy,
            insuranceDifferentThanPatientSelect: $scope.InsuranceForm.insuranceDifferentThanPatientSelect,
            insuranceLastName: $scope.InsuranceForm.insuranceLastName,
            insuranceFirstName: $scope.InsuranceForm.insuranceFirstName,
            insuranceMiddleName: $scope.InsuranceForm.insuranceMiddleName,
            insuranceRelationshipToPatient: $scope.InsuranceForm.insuranceRelationshipToPatient,
            hasAdditionalInsuranceInfo: $scope.InsuranceForm.hasAdditionalInsuranceInfo,
            additionalInsuranceInfo: $scope.InsuranceForm.additionalInsuranceInfo,
            streetAddress: $scope.InsuranceForm.streetAddress,
            city: $scope.InsuranceForm.city,
            state: $scope.InsuranceForm.state,
            zip: $scope.InsuranceForm.zip

        };

        $scope.InsuranceForm = {};


        if (formId >= 0) {
            $scope.pcr.insurances.splice(formId, 1, InsuranceObject
            );
        }
        else {
            $scope.pcr.insurances.push(
                    InsuranceObject

            );
        }

        if (keepOpen) {
            $scope.ClearModal2('[modaltarget=InsuranceModal]');
            $("[name='InsuranceForm_insuranceCompany']").focus();
            $scope.setScroll();
        }
        else {
            $scope.HideAndClearModal2('[modaltarget=InsuranceModal]');
            $scope.ClearInsuranceForm();
            toastr.success('Item Added', '');
            $scope.SaveOrUpdateHandler();
        }

    };

    $scope.removeInsurance = function (index) {
        if (confirm("Are you sure you want to DELETE the item?") == true) {
            $scope.pcr.insurances.splice(index, 1);
        }
    };

    $scope.EditModalInsurance = function (itemId) {



        $scope.InsuranceForm = {
            insuranceCompanyBillingPriority: $scope.pcr.insurances[itemId].insuranceCompanyBillingPriority,
            insuranceCompany: $scope.pcr.insurances[itemId].insuranceCompany,
            insuranceGroup: $scope.pcr.insurances[itemId].insuranceGroup,
            insurancePolicy: $scope.pcr.insurances[itemId].insurancePolicy,
            insuranceDifferentThanPatientSelect: $scope.pcr.insurances[itemId].insuranceDifferentThanPatientSelect,
            insuranceLastName: $scope.pcr.insurances[itemId].insuranceLastName,
            insuranceFirstName: $scope.pcr.insurances[itemId].insuranceFirstName,
            insuranceMiddleName: $scope.pcr.insurances[itemId].insuranceMiddleName,
            insuranceRelationshipToPatient: $scope.pcr.insurances[itemId].insuranceRelationshipToPatient,
            hasAdditionalInsuranceInfo: $scope.pcr.insurances[itemId].hasAdditionalInsuranceInfo,
            additionalInsuranceInfo: $scope.pcr.insurances[itemId].additionalInsuranceInfo,
            streetAddress: $scope.pcr.insurances[itemId].streetAddress,
            city: $scope.pcr.insurances[itemId].city,
            state: $scope.pcr.insurances[itemId].state,
            zip: $scope.pcr.insurances[itemId].zip,
            ModalFormId: itemId
        };

        $scope.$apply();

    };

    $scope.ClearInsuranceForm = function () {
        $scope.InsuranceForm = {};
    };

    //PatientMedication Functions

    $scope.ConfirmPatientMedication = function () {
        if (!$scope.pcr.patientMedications) {
            $scope.pcr.patientMedications = [];
        }
        //$scope.meds = $('.PatientMedicationsSelect2').select2('val');

        //alert(JSON.stringify($scope.meds));

        //for (i = 0; i < $scope.meds.length; i++) {
        var info = $('.PatientMedicationsSelect2').select2('data').text.split(" | ");
        var dosage = info[1].split(" ");

        $scope.pcr.patientMedications.push({
            medicationName: info[0],
            medicationDosage: dosage[0],
            medicationDosageUnits: dosage[1],
            medicationRoute: info[2]
        });
        //}

        $('.PatientMedicationsSelect2').select2('data', null);
    }

    $scope.AddPatientMedicationToDB = function (keepOpen) {
        $scope.PatientMedicationDB = {};

        $scope.PatientMedicationDB.Name = $('.aaMedsSelect2').select2('data').text;
        $scope.PatientMedicationDB.Dosage = $scope.PatientMedicationForm.medicationDosage;
        $scope.PatientMedicationDB.Units = $scope.PatientMedicationForm.medicationDosageUnits;
        $scope.PatientMedicationDB.Route = $scope.PatientMedicationForm.medicationRoute;
        $scope.PatientMedicationDB.Notes = $scope.PatientMedicationForm.medicationNotes;
        $scope.PatientMedicationDB.Status = "Pending";

        $.ajax({
            type: "POST",
            url: "/api/PatientMedication",
            data: $scope.PatientMedicationDB
        })
		.done(function (msg) {

		})
		.fail(function (xhr, textStatus, errorThrown) {
		    toastr.error("Error");
		});

        $scope.AddPatientMedication(keepOpen);
    }

    $scope.AddPatientMedication = function (keepOpen) {
        //if (!($("#PatientMedication").valid())) {
        //    return;
        //}

        if ($scope.pcr.patientMedications == null)
            $scope.pcr.patientMedications = [];

        $scope.PatientMedicationObject = {
            medicationName: $('.aaMedsSelect2').select2('data').text,
            medicationDosage: $scope.PatientMedicationForm.medicationDosage,
            medicationDosageUnits: $scope.PatientMedicationForm.medicationDosageUnits,
            medicationRoute: $scope.PatientMedicationForm.medicationRoute,
            medicationNotes: $scope.PatientMedicationForm.medicationNotes
        };

        var formId = parseInt($scope.PatientMedicationForm.ModalFormId);

        var PatientMedicationObject = $scope.PatientMedicationObject;

        if (formId >= 0) {
            $scope.pcr.patientMedications.splice(formId, 1, PatientMedicationObject
            );
        }
        else {
            $scope.pcr.patientMedications.push(
                    PatientMedicationObject

            );
        }

        if (keepOpen) {

            $scope.ClearModal2('[modaltarget=PatientMedicationModal]');
            $("[name='PatientMedicationForm_medicationName']").select2('open');
            //$scope.setScroll();
            $scope.ClearPatientMedicationForm();
            $scope.$apply();
        }
        else {
            $scope.HideAndClearModal2('[modaltarget=PatientMedicationModal]');
            $scope.ClearPatientMedicationForm();
            toastr.success('', 'Medications Updated');
        }
    };

    $scope.removePatientMedication = function (index) {
        if (confirm("Are you sure you want to DELETE the item?") == true) {
            $scope.pcr.patientMedications.splice(index, 1);
        }

    };

    $scope.EditModalPatientMedication = function (itemId) {

        $scope.PatientMedicationForm = {
            medicationName: $scope.pcr.patientMedications[itemId].medicationName,
            medicationDosage: $scope.pcr.patientMedications[itemId].medicationDosage,
            medicationDosageUnits: $scope.pcr.patientMedications[itemId].medicationDosageUnits,
            medicationRoute: $scope.pcr.patientMedications[itemId].medicationRoute,
            medicationNotes: $scope.pcr.patientMedications[itemId].medicationNotes,
            ModalFormId: itemId

        };
        $("#aaMedsSelect2").select2("val", $scope.PatientMedicationForm.medicationName);
        $scope.$apply();

    };

    $scope.ClearPatientMedicationForm = function () {
        $scope.PatientMedicationForm = {};
    };


    //Exam Functions
    $scope.AddExam = function (clear) {

        if ($scope.pcr.exams == null)
            $scope.pcr.exams = [];


        var formId = parseInt($scope.ExamForm.ModalFormId);

        var ExamObject = {

            examDate: $scope.ExamForm.examDate,
            examTime: $scope.ExamForm.examTime,
            examSkin: $scope.ExamForm.examSkin,
            examFace: $scope.ExamForm.examFace,
            examNeck: $scope.ExamForm.examNeck,
            examLeftEye: $scope.ExamForm.examLeftEye,
            examRightEye: $scope.ExamForm.examRightEye,
            examMental: $scope.ExamForm.examMental,
            examNeuro: $scope.ExamForm.examNeuro,
            examChest: $scope.ExamForm.examChest,
            examHeart: $scope.ExamForm.examHeart,
            examAbsLeftUpper: $scope.ExamForm.examAbsLeftUpper,
            examAbsLeftLower: $scope.ExamForm.examAbsLeftLower,
            examAbsRightUpper: $scope.ExamForm.examAbsRightUpper,
            examAbsRightLower: $scope.ExamForm.examAbsRightLower,
            examGU: $scope.ExamForm.examGU,
            examBackCervical: $scope.ExamForm.examBackCervical,
            examBackThoracic: $scope.ExamForm.examBackThoracic,
            examBackLumbar: $scope.ExamForm.examBackLumbar,
            examExtremRightUpper: $scope.ExamForm.examExtremRightUpper,
            examExtremRightLower: $scope.ExamForm.examExtremRightLower,
            examExtremLeftLower: $scope.ExamForm.examExtremLeftLower,
            examExtremLeftUpper: $scope.ExamForm.examExtremLeftUpper,
            examNotes: $scope.ExamForm.examNotes,
            examUnspecified: $scope.ExamForm.examUnspecified,
            examHead: $scope.ExamForm.examHead
        };

        $("[href='#tabs-head']").click();
        $scope.ExamForm = {};
        if (clear) {
            $scope.HideAndClearModal2('[modaltarget=ExamModal]');
            toastr.success('Item Added', '');
            $scope.SaveOrUpdateHandler();
        }
        else {
            $scope.SetExamDateTime();
            $("[name='ExamForm_examMental']").siblings(".select2-container").select2("open");
            $scope.ClearModalSelect2('[modaltarget=ExamModal]');
        }

        if (formId >= 0) {
            $scope.pcr.exams.splice(formId, 1, ExamObject);
        }
        else {
            $scope.pcr.exams.push(ExamObject);
        }
        $scope.pcr.exams = $filter('orderBy')($filter('orderBy')($scope.pcr.exams, 'examTime'), 'examDate');
        $scope.buildTimeline();
    };

    $scope.removeExam = function (index) {
        if (confirm("Are you sure you want to DELETE the item?") == true) {
            $scope.pcr.exams.splice(index, 1);
        }
        $scope.buildTimeline();
    };

    $scope.EditModalExam = function (itemId) {



        $scope.ExamForm = {
            examDate: $scope.pcr.exams[itemId].examDate,
            examTime: $scope.pcr.exams[itemId].examTime,
            examSkin: $scope.pcr.exams[itemId].examSkin,
            examFace: $scope.pcr.exams[itemId].examFace,
            examNeck: $scope.pcr.exams[itemId].examNeck,
            examLeftEye: $scope.pcr.exams[itemId].examLeftEye,
            examRightEye: $scope.pcr.exams[itemId].examRightEye,
            examMental: $scope.pcr.exams[itemId].examMental,
            examNeuro: $scope.pcr.exams[itemId].examNeuro,
            examChest: $scope.pcr.exams[itemId].examChest,
            examHeart: $scope.pcr.exams[itemId].examHeart,
            examAbsLeftUpper: $scope.pcr.exams[itemId].examAbsLeftUpper,
            examAbsLeftLower: $scope.pcr.exams[itemId].examAbsLeftLower,
            examAbsRightUpper: $scope.pcr.exams[itemId].examAbsRightUpper,
            examAbsRightLower: $scope.pcr.exams[itemId].examAbsRightLower,
            examGU: $scope.pcr.exams[itemId].examGU,
            examBackCervical: $scope.pcr.exams[itemId].examBackCervical,
            examBackThoracic: $scope.pcr.exams[itemId].examBackThoracic,
            examBackLumbar: $scope.pcr.exams[itemId].examBackLumbar,
            examExtremRightUpper: $scope.pcr.exams[itemId].examExtremRightUpper,
            examExtremRightLower: $scope.pcr.exams[itemId].examExtremRightLower,
            examExtremLeftLower: $scope.pcr.exams[itemId].examExtremLeftLower,
            examExtremLeftUpper: $scope.pcr.exams[itemId].examExtremLeftUpper,
            examNotes: $scope.pcr.exams[itemId].examNotes,
            examHead: $scope.pcr.exams[itemId].examHead,
            examUnspecified: $scope.pcr.exams[itemId].examUnspecified,
            ModalFormId: itemId

        };

        $scope.$apply();

    };

    $scope.ClearExamForm = function () {
        $scope.ExamForm = {};
    };

    $scope.SetExamDateTime = function () {
        var date = "";
        var time = "";
        if ($scope.pcr.exams && $scope.pcr.exams.length > 0) {
            date = $scope.pcr.exams[$scope.pcr.exams.length - 1].examDate;
            time = $scope.pcr.exams[$scope.pcr.exams.length - 1].examTime;
        }
        else {
            if ($scope.pcr.incidentDate) {
                date = $scope.pcr.incidentDate;
            }
            else {
                date = getDate();
            }
            if ($scope.pcr.incidentPtContactTime) {
                time = $scope.pcr.incidentPtContactTime;
            }
            else {
                time = moment().format('HH:mm');
            }

        }

        $scope.ExamForm = {
            examDate: date,
            examTime: time
        };
    };

    //Vital Functions
    $scope.pcr.previousVital = null;

    $scope.VitalForm = {};
    $scope.VitalForm.vitalGcs = '1';
    $scope.VitalForm.vitalGcsM = '1';
    $scope.VitalForm.vitalGcsV = '1';
    $scope.ReloadVital = function () {
        if ($scope.pcr.previousVital != null) {
            $scope.VitalForm = angular.copy($scope.pcr.previousVital);
            $scope.InstantiateSelect2sBySelector('[modaltarget=VitalModal]', true);
            //$scope.$apply();
        }
    };

    $scope.calculateTemp = function () {
        $scope.VitalForm.vitalTemp = $scope.VitalForm.vitalTempF ? parseFloat((($scope.VitalForm.vitalTempF - 32) * 5 / 9).toFixed(1)) : null;
    }

    $scope.AddVital = function () {

        if ($scope.pcr.vitals == null)
            $scope.pcr.vitals = [];


        var formId = parseInt($scope.VitalForm.ModalFormId);

        var VitalObject = {
            vitalDate: $scope.VitalForm.vitalDate,
            vitalTime: $scope.VitalForm.vitalTime,
            vitalTemp: $scope.VitalForm.vitalTemp,
            vitalTempF: $scope.VitalForm.vitalTempF,
            vitalTempMethod: $scope.VitalForm.vitalTempMethod,
            vitalSbp: $scope.VitalForm.vitalSbp,
            vitalDbp: $scope.VitalForm.vitalDbp,
            vitalBpDevice: $scope.VitalForm.vitalBpDevice,
            vitalCardRhythm: $scope.VitalForm.vitalCardRhythm,
            vitalPulse: $scope.VitalForm.vitalPulse,
            vitalPulseRhythm: $scope.VitalForm.vitalPulseRhythm,
            vitalOx: $scope.VitalForm.vitalOx,
            vitalCo2: $scope.VitalForm.vitalCo2,
            vitalResp: $scope.VitalForm.vitalResp,
            vitalRespEffort: $scope.VitalForm.vitalRespEffort,
            vitalGcs: $scope.VitalForm.vitalGcs,
            vitalGcsM: $scope.VitalForm.vitalGcsM,
            vitalGcsV: $scope.VitalForm.vitalGcsV,
            vitalGcsQual: $scope.VitalForm.vitalGcsQual,
            vitalGcsTotal: $scope.VitalForm.vitalGcsTotal,
            vitalStroke: $scope.VitalForm.vitalStroke,
            vitalPain: $scope.VitalForm.vitalPain,
            vitalThrombScreen: $scope.VitalForm.vitalThrombScreen,
            vitalApgar: $scope.VitalForm.vitalApgar,
            vitalRevisedTrauma: $scope.VitalForm.vitalRevisedTrauma,
            vitalPediatricTrauma: $scope.VitalForm.vitalPediatricTrauma,
            vitalBg: $scope.VitalForm.vitalBg,
            vitalPriorEms: $scope.VitalForm.vitalPriorEms,
            vitalElectricMonitorRate: $scope.VitalForm.vitalElectricMonitorRate,
            Notes: $scope.VitalForm.Notes,
            vitalAVPU: $scope.VitalForm.vitalAVPU
        };

        $scope.pcr.previousVital = VitalObject;

        //$scope.VitalForm = {};



        if (formId >= 0) {
            $scope.pcr.vitals.splice(formId, 1, VitalObject
            );
        }
        else {
            $scope.pcr.vitals.push(
                    VitalObject

            );
        }
        $scope.pcr.vitals = $filter('orderBy')($filter('orderBy')($scope.pcr.vitals, 'vitalTime'), 'vitalDate');
    };

    $scope.SubmitVitalForm = function (keepOpen) {
        if ($("#VitalsForm").valid()) {
            $scope.AddVital();
            //$scope.ClearModal2('[modaltarget=VitalModal]');

            if (keepOpen) {
                $scope.SetVitalDateTime();
                $scope.ClearModal2('[modaltarget=VitalModal]');
                $("[name='VitalForm_vitalSbp']").focus();
                $("[name=VitalForm_vitalDate").val($scope.VitalForm.vitalDate);
                $("[name=VitalForm_vitalTime").val($scope.VitalForm.vitalTime);
            }
            else {
                $scope.HideAndClearModalSelect2('[modaltarget=VitalModal]');
                $scope.ClearVitalForm();
                toastr.success('Item Added', '');
                $scope.SaveOrUpdateHandler();
            }

            $scope.buildTimeline();
        }
    };

    $scope.removeVital = function (index) {
        if (confirm("Are you sure you want to DELETE the item?") == true) {
            $scope.pcr.vitals.splice(index, 1);
            $scope.buildTimeline();
        }
    };

    $scope.EditModalVital = function (itemId) {
        $scope.VitalForm = {
            vitalDate: $scope.pcr.vitals[itemId].vitalDate,
            vitalTime: $scope.pcr.vitals[itemId].vitalTime,
            vitalTemp: $scope.pcr.vitals[itemId].vitalTemp,
            // if the vital has a tempF, use it. Otherwise, use the vitalTemp calculation
            vitalTempF: $scope.pcr.vitals[itemId].vitalTempF || ($scope.pcr.vitals[itemId].vitalTemp ? parseFloat(($scope.pcr.vitals[itemId].vitalTemp / 5.0 * 9.0 + 32.0).toFixed(1)) : ""),
            vitalTempMethod: $scope.pcr.vitals[itemId].vitalTempMethod,
            vitalSbp: $scope.pcr.vitals[itemId].vitalSbp,
            vitalDbp: $scope.pcr.vitals[itemId].vitalDbp,
            vitalBpDevice: $scope.pcr.vitals[itemId].vitalBpDevice,
            vitalCardRhythm: $scope.pcr.vitals[itemId].vitalCardRhythm,
            vitalPulse: $scope.pcr.vitals[itemId].vitalPulse,
            vitalPulseRhythm: $scope.pcr.vitals[itemId].vitalPulseRhythm,
            vitalOx: $scope.pcr.vitals[itemId].vitalOx,
            vitalCo2: $scope.pcr.vitals[itemId].vitalCo2,
            vitalResp: $scope.pcr.vitals[itemId].vitalResp,
            vitalRespEffort: $scope.pcr.vitals[itemId].vitalRespEffort,
            vitalGcs: $scope.pcr.vitals[itemId].vitalGcs,
            vitalGcsM: $scope.pcr.vitals[itemId].vitalGcsM,
            vitalGcsV: $scope.pcr.vitals[itemId].vitalGcsV,
            vitalGcsQual: $scope.pcr.vitals[itemId].vitalGcsQual,
            vitalGcsTotal: $scope.pcr.vitals[itemId].vitalGcsTotal,
            vitalStroke: $scope.pcr.vitals[itemId].vitalStroke,
            vitalPain: $scope.pcr.vitals[itemId].vitalPain,
            vitalThrombScreen: $scope.pcr.vitals[itemId].vitalThrombScreen,
            vitalApgar: $scope.pcr.vitals[itemId].vitalApgar,
            vitalRevisedTrauma: $scope.pcr.vitals[itemId].vitalRevisedTrauma,
            vitalPediatricTrauma: $scope.pcr.vitals[itemId].vitalPediatricTrauma,
            vitalBg: $scope.pcr.vitals[itemId].vitalBg,
            vitalPriorEms: $scope.pcr.vitals[itemId].vitalPriorEms,
            vitalElectricMonitorRate: $scope.pcr.vitals[itemId].vitalElectricMonitorRate,
            vitalAVPU: $scope.pcr.vitals[itemId].vitalAVPU,
            Notes: $scope.pcr.vitals[itemId].Notes,
            ModalFormId: itemId
        };

        $scope.$apply();

    };

    $scope.ClearVitalForm = function () {
        $scope.VitalForm = {};
    };

    $scope.SetVitalDateTime = function () {
        var date = "";
        var time = "";
        if ($scope.pcr.vitals && $scope.pcr.vitals.length > 0) {
            date = $scope.pcr.vitals[$scope.pcr.vitals.length - 1].vitalDate;
            time = $scope.pcr.vitals[$scope.pcr.vitals.length - 1].vitalTime;
        }
        else {
            if ($scope.pcr.incidentDate) {
                date = $scope.pcr.incidentDate;
            }
            else {
                date = getDate();
            }
            if ($scope.pcr.incidentPtContactTime) {
                //alert("incident pt contact time");
                time = $scope.pcr.incidentPtContactTime;
            }
            else {
                //alert("moment js");
                time = moment().format('HH:mm');
            }

        }

        $scope.VitalForm = {
            vitalDate: date,
            vitalTime: time
        };
        //alert(time);
        //$scope.$apply();
    };

    //TreatmentMedication Functions
    $scope.AddTreatmentMedication = function () {

        if ($scope.pcr.treatmentMedications == null)
            $scope.pcr.treatmentMedications = [];


        var formId = parseInt($scope.TreatmentMedicationForm.ModalFormId);

        var TreatmentMedicationObject = {
            medDate: $scope.TreatmentMedicationForm.medDate,
            medTime: $scope.TreatmentMedicationForm.medTime,
            medCrewMemberId: $scope.TreatmentMedicationForm.medCrewMemberId,
            medName: $scope.TreatmentMedicationForm.medName,
            medDosage: $scope.TreatmentMedicationForm.medDosage,
            medDosageUnits: $scope.TreatmentMedicationForm.medDosageUnits,
            medRoute: $scope.TreatmentMedicationForm.medRoute,
            medResponse: $scope.TreatmentMedicationForm.medResponse,
            medComplication: $scope.TreatmentMedicationForm.medComplication,
            medAuth: $scope.TreatmentMedicationForm.medAuth,
            medAuthPhysician: $scope.TreatmentMedicationForm.medAuthPhysician,
            Notes: $scope.TreatmentMedicationForm.Notes,
            medPriorEms: $scope.TreatmentMedicationForm.medPriorEms
        };

        // $scope.TreatmentMedicationForm = {};


        if (formId >= 0) {
            $scope.pcr.treatmentMedications.splice(formId, 1, TreatmentMedicationObject
            );
        }
        else {
            $scope.pcr.treatmentMedications.push(
                    TreatmentMedicationObject

            );
        }
        $scope.pcr.treatmentMedications = $filter('orderBy')($filter('orderBy')($scope.pcr.treatmentMedications, 'medTime'), 'medDate');
    };

    $scope.removeTreatmentMedication = function (index) {
        if (confirm("Are you sure you want to DELETE the item?") == true) {
            $scope.pcr.treatmentMedications.splice(index, 1);
            $scope.buildTimeline();
        }
    };

    $scope.EditModalTreatmentMedication = function (itemId) {



        $scope.TreatmentMedicationForm = {
            medDate: $scope.pcr.treatmentMedications[itemId].medDate,
            medTime: $scope.pcr.treatmentMedications[itemId].medTime,
            medCrewMemberId: $scope.pcr.treatmentMedications[itemId].medCrewMemberId,
            medName: $scope.pcr.treatmentMedications[itemId].medName,
            medDosage: $scope.pcr.treatmentMedications[itemId].medDosage,
            medDosageUnits: $scope.pcr.treatmentMedications[itemId].medDosageUnits,
            medRoute: $scope.pcr.treatmentMedications[itemId].medRoute,
            medResponse: $scope.pcr.treatmentMedications[itemId].medResponse,
            medComplication: $scope.pcr.treatmentMedications[itemId].medComplication,
            medAuth: $scope.pcr.treatmentMedications[itemId].medAuth,
            medAuthPhysician: $scope.pcr.treatmentMedications[itemId].medAuthPhysician,
            medPriorEms: $scope.pcr.treatmentMedications[itemId].medPriorEms,
            Notes: $scope.pcr.treatmentMedications[itemId].Notes,
            ModalFormId: itemId

        };

        $scope.$apply();

    };

    $scope.ClearTreatmentMedicationForm = function () {
        $scope.TreatmentMedicationForm = {};
    };

    $scope.SetTreatmentMedicationDateTime = function () {
        var date = "";
        var time = "";
        if ($scope.pcr.treatmentMedications && $scope.pcr.treatmentMedications.length > 0) {
            date = $scope.pcr.treatmentMedications[$scope.pcr.treatmentMedications.length - 1].medDate;
            time = $scope.pcr.treatmentMedications[$scope.pcr.treatmentMedications.length - 1].medTime;
        }
        else {
            if ($scope.pcr.incidentDate) {
                date = $scope.pcr.incidentDate;
            }
            else {
                date = getDate();
            }
            if ($scope.pcr.incidentPtContactTime) {
                time = $scope.pcr.incidentPtContactTime;
            }
            else {
                time = moment().format('HH:mm');
            }

        }

        $scope.TreatmentMedicationForm = {
            medDate: date,
            medTime: time
        };

        //$scope.$apply();
    };

    $scope.SubmitTreatmentMedicationForm = function (keepOpen) {
        if ($("#TreatMedForm").valid()) {
            $scope.AddTreatmentMedication();
            //$scope.ClearModal2('[modaltarget=TreatmentMedicationModal]');
            if (keepOpen) {
                $scope.SetTreatmentMedicationDateTime();
                //$("[name='TreatmentMedicationForm_medName']").siblings(".select2-container").select2("open");
                $scope.ClearModal2('[modaltarget=TreatmentMedicationModal]');
                $("[name=TreatmentMedicationForm_medDate").val($scope.TreatmentMedicationForm.medDate);
                $("[name=TreatmentMedicationForm_medTime").val($scope.TreatmentMedicationForm.medTime);
            }
            else {
                $scope.HideAndClearModalSelect2('[modaltarget=TreatmentMedicationModal]');
                $scope.ClearTreatmentMedicationForm();
                toastr.success('Item Added', '');
                $scope.SaveOrUpdateHandler();
            }

            $scope.buildTimeline();
        }
    };

    //Procedure Functions
    $scope.AddProcedure = function () {

        if ($scope.pcr.procedures == null)
            $scope.pcr.procedures = [];

        //alert("th7777777is");


        var formId = parseInt($scope.ProcedureForm.ModalFormId);

        var ProcedureObject = {
            procDate: $scope.ProcedureForm.procDate,
            procTime: $scope.ProcedureForm.procTime,
            procCrewMemberId: $scope.ProcedureForm.procCrewMemberId,
            procProcedure: $scope.ProcedureForm.procProcedure,
            procSizeOfEquipment: $scope.ProcedureForm.procSizeOfEquipment,
            procResponse: $scope.ProcedureForm.procResponse,
            procComplication: $scope.ProcedureForm.procComplication,
            procAuth: $scope.ProcedureForm.procAuth,
            procAuthPhysician: $scope.ProcedureForm.procAuthPhysician,
            procSuccess: $scope.ProcedureForm.procSuccess,
            procPriorEms: $scope.ProcedureForm.procPriorEms,
            Notes: $scope.ProcedureForm.Notes,
            procNumberOfAttempts: $scope.ProcedureForm.procNumberOfAttempts

        };
        //$scope.ProcedureForm = {};


        if (formId >= 0) {
            $scope.pcr.procedures.splice(formId, 1, ProcedureObject
            );
        }
        else {
            $scope.pcr.procedures.push(
                    ProcedureObject
            );
        }
        $scope.pcr.procedures = $filter('orderBy')($filter('orderBy')($scope.pcr.procedures, 'procTime'), 'procDate');
    };

    $scope.removeProcedure = function (index) {
        if (confirm("Are you sure you want to DELETE the item?") == true) {
            $scope.pcr.procedures.splice(index, 1);
            $scope.buildTimeline();
        }
    };

    $scope.EditModalProcedure = function (itemId) {



        $scope.ProcedureForm = {
            procDate: $scope.pcr.procedures[itemId].procDate,
            procTime: $scope.pcr.procedures[itemId].procTime,
            procCrewMemberId: $scope.pcr.procedures[itemId].procCrewMemberId,
            procProcedure: $scope.pcr.procedures[itemId].procProcedure,
            procSizeOfEquipment: $scope.pcr.procedures[itemId].procSizeOfEquipment,
            procResponse: $scope.pcr.procedures[itemId].procResponse,
            procComplication: $scope.pcr.procedures[itemId].procComplication,
            procAuth: $scope.pcr.procedures[itemId].procAuth,
            procAuthPhysician: $scope.pcr.procedures[itemId].procAuthPhysician,
            procSuccess: $scope.pcr.procedures[itemId].procSuccess,
            procPriorEms: $scope.pcr.procedures[itemId].procPriorEms,
            procNumberOfAttempts: $scope.pcr.procedures[itemId].procNumberOfAttempts,
            Notes: $scope.pcr.procedures[itemId].Notes,
            ModalFormId: itemId

        };

        $scope.$apply();

    };

    $scope.ClearProcedureForm = function () {
        $scope.ProcedureForm = {};
    };

    $scope.SetProcedureDateTime = function () {
        var date = "";
        var time = "";
        if ($scope.pcr.procedures && $scope.pcr.procedures.length > 0) {
            date = $scope.pcr.procedures[$scope.pcr.procedures.length - 1].procDate;
            time = $scope.pcr.procedures[$scope.pcr.procedures.length - 1].procTime;
        }
        else {
            if ($scope.pcr.incidentDate) {
                date = $scope.pcr.incidentDate;
            }
            else {
                date = getDate();
            }
            if ($scope.pcr.incidentPtContactTime) {
                time = $scope.pcr.incidentPtContactTime;
            }
            else {
                time = moment().format('HH:mm');
            }

        }

        $scope.ProcedureForm = {
            procDate: date,
            procTime: time
        };

        //$scope.$apply();
    };

    $scope.SubmitProcedureForm = function (keepOpen) {
        if ($("#ProcedureForm").valid()) {
            $scope.AddProcedure();
            //$scope.ClearModal2('[modaltarget=ProcedureModal]');
            if (keepOpen) {
                $scope.SetProcedureDateTime();
                $("[name='ProcedureForm_procProcedure']").siblings(".select2-container").select2("open");
                $scope.ClearModal2('[modaltarget=ProcedureModal]');
                $("[name=ProcedureForm_procDate").val($scope.ProcedureForm.procDate);
                $("[name=ProcedureForm_procTime").val($scope.ProcedureForm.procTime);
            }
            else {
                $scope.HideAndClearModalSelect2('[modaltarget=ProcedureModal]');
                $scope.ClearProcedureForm();
                toastr.success('Item Added', '');
                $scope.SaveOrUpdateHandler();
            }

            $scope.buildTimeline();
        }
    };

    //FORMS

    /*
        General stuff
    */
    // formControls is the list of controls for the form
    $scope.formControls = [];

    var copiedControl = null;

    /*
        formTypeDefs is the static object that contains element creation algorithms for each control type, for each form type
        
        ex:
        [_controlTag]: {
            "build": [_buildTypeFunction],
            "edit": [_editTypeFunction],
            "view": [_viewTypeFunction]
        },

        where:
            _controlTag is the 'type' of object, as defined in the form controls object,
            _buildTypeFunction is a function that accepts a control data object, and returns either null or a child element to display when building forms,
            _editTypeFunction is a function that accepts a control data object, and returns either null or a child element to display when editing forms,
            _viewTypeFunction is a function that accepts a control data object, and returns either null or a child element to display when viewing forms
    */


//// Is this for display form?
    ////var formTypeDefs = {
    ////    "text": (function textFormDef() {
    ////        return {
    ////            "properties": {
    ////                "fixedWidth": false,
    ////                "fixedHeight": false,
    ////                "ngModel": true,
    ////                "clicks": 2
    ////            },
    ////            "build": function (data) {
    ////                var child = document.createElement("span");
    ////                $(child).html(data["ng-model"]);
    ////                return child;
    ////            },
    ////            "edit": function (data) {
    ////                var child = document.createElement("input");
    ////                if (data["ng-model"]) {
    ////                    var storedValue = getNgModelValue(data["ng-model"]);
    ////                    $parse(data["ng-model"]).assign($scope.formData, storedValue);
    ////                    $(child).attr("data-ng-model", "formData." + data["ng-model"]);
    ////                } else {
    ////                    var storedValue = getNgModelValue(data.title.replace(/ +/g, ""));
    ////                    $parse(data.title.replace(/ +/g, "")).assign($scope.formData, storedValue);
    ////                    $(child).attr("data-ng-model", "formData." + data.title.replace(/ +/g, ""));
    ////                }
    ////                $(child).attr("type", "text");
    ////                return child;
    ////            },
    ////            "view": function (data) {
    ////                var storedValue;
    ////                if (data["ng-model"])
    ////                    storedValue = getNgModelValue(data["ng-model"]);
    ////                else
    ////                    storedValue = getNgModelValue(data.title.replace(/ +/g, ""));
    ////                var child = document.createElement("span");
    ////                $(child).html(storedValue);
    ////                return child;
    ////            }
    ////        };
    ////    })(),
    ////    "checkbox": (function checkboxFormDef() {
    ////        return {
    ////            "properties": {
    ////                "fixedWidth": "height",
    ////                "fixedHeight": "width",
    ////                "ngModel": true,
    ////                "clicks": 2
    ////            },
    ////            "build": function (data) {
    ////                var child = document.createElement("span");
    ////                $(child).html(data["ng-model"]);
    ////                return child;
    ////            },
    ////            "edit": function (data) {
    ////                var child = document.createElement("input");
    ////                if (data["ng-model"]) {
    ////                    var storedValue = getNgModelValue(data["ng-model"]);
    ////                    $parse(data["ng-model"]).assign($scope.formData, storedValue == true);
    ////                    $(child).attr("data-ng-model", "formData." + data["ng-model"]);
    ////                } else {
    ////                    var storedValue = getNgModelValue(data.title.replace(/ +/g, ""));
    ////                    $parse(data.title.replace(/ +/g, "")).assign($scope.formData, storedValue == true);
    ////                    $(child).attr("data-ng-model", "formData." + data.title.replace(/ +/g, ""));
    ////                }
    ////                $(child).attr("type", "checkbox");
    ////                return child;
    ////            },
    ////            "view": function (data) {
    ////                var storedValue;
    ////                if (data["ng-model"])
    ////                    storedValue = getNgModelValue(data["ng-model"]);
    ////                else
    ////                    storedValue = getNgModelValue(data.title.replace(/ +/g, ""));
    ////                var child = document.createElement("span");
    ////                if (storedValue == true) {
    ////                    $(child).html("&#x2714;");
    ////                    $(child).css("font-size", data.height + "px");
    ////                    $(child).css("font-weight", "bold");
    ////                    $(child).css("top", "-15px");
    ////                }
    ////                return child;
    ////            }
    ////        };
    ////    })(),
    ////    "signature": (function checkboxFormDef() {
    ////        return {
    ////            "properties": {
    ////                "fixedWidth": false,
    ////                "fixedHeight": false,
    ////                "ngModel": false,
    ////                "clicks": 2
    ////            },
    ////            "build": function (data) {
    ////                var child = document.createElement("span");
    ////                return child;
    ////            },
    ////            "edit": function (data) {
    ////                var child = document.createElement("div");
    ////                $(child).addClass("formSignature");
    ////                var $img = $(document.createElement("img"));
    ////                $(child).append($img);
    ////                var $name = $(document.createElement("span"));
    ////                $(child).append($name);
    ////                $name.css("position", "absolute");
    ////                $name.css("left", "0");
    ////                $name.css("top", "0");
    ////                $name.css("font-size", "10px");

    ////                var model = data.title.replace(/ +/g, "") + "_Signature";
    ////                var storedValue = getNgModelValue(model);
    ////                var storedName = getNgModelValue(model + "_Name");
    ////                $img.attr("src", storedValue || "");
    ////                $name.html(storedName);

    ////                $(child).click({ model: model, $img: $img, $name: $name }, function ($event) {
    ////                    signatureModal({
    ////                        src: getNgModelValue($event.data.model),
    ////                        success: function (newSrc, name) {
    ////                            $parse($event.data.model + "_Name").assign($scope.formData, name);
    ////                            $parse($event.data.model).assign($scope.formData, newSrc);
    ////                            $event.data.$img.attr("src", newSrc || "");
    ////                            $name.html(name);
    ////                        }
    ////                    });
    ////                });

    ////                return child;
    ////            },
    ////            "view": function (data) {
    ////                var child = document.createElement("div");
    ////                $(child).addClass("formSignature");
    ////                var $img = $(document.createElement("img"));
    ////                $(child).append($img);
    ////                var $name = $(document.createElement("span"));
    ////                $(child).append($name);
    ////                $name.css("position", "absolute");
    ////                $name.css("left", "0");
    ////                $name.css("top", "0");
    ////                $name.css("font-size", "10px");

    ////                var model = data.title.replace(/ +/g, "") + "_Signature";
    ////                var storedValue = getNgModelValue(model);
    ////                var storedName = getNgModelValue(model + "_Name");
    ////                $img.attr("src", storedValue || "");
    ////                $name.html(storedName);

    ////                return child;
    ////            }
    ////        };
    ////    })()
    ////}


    //// Goes with dynamic 
    ////function getNgModelValue(ngModel) {
    ////    var formValue = $parse("formData." + ngModel)($scope);
    ////    if (formValue != null && formValue != undefined)
    ////        return formValue;
    ////    return $parse(ngModel)($scope);
    ////}

    /* ////This is all dynamic form
    $scope.formData = {};

    // formDefs defines the form pages
    $scope.formDefs = [];

    // scale is a floating point that defines form zoom
    $scope.scale = 1;
    // contentType can be "build", "edit", or "view", and defines the classes applied to controls and the type definitions functions executed
    $scope.contentType = "edit";

    $scope.formType = "Sample";
    // droppingControl is a flag indicating whether the user is currently adding a field
    $scope.droppingControl = false;

    var currentControlProps = null;
    // mousedown and mousepoint are a part of the field drop cycle
    var mousedown, mousepoint = null;

    
    //    setupOverlays() clears all of the pages and rebuilds all of the controls defined in $scope.formControls
    
    function setupOverlays() {
        $(".page .controlLayer").children("div").remove();
        $(".buildControlContainer").off("click");
        for (var i = 0; i < $scope.formControls.length; i++) {
            buildControl(i);
        }
    }

    
    //    buildControl() builds an individual control defined by the element at index, i, in the $scope.formControls array.
    //    Optionally accepts a temporary form control data object as a second parameter, so a sample object can be rendered without modifying the original definition
    
    function buildControl(i, tempData) {
        $("#element_" + i).off("click");
        $("#element_" + i).remove(); // if the element has already been rendered, this removes it

        var data = tempData || $scope.formControls[i]; // looks for tempData first, and if null or undefined, falls back to the form controls definition at index i
        var page = $("#page" + data.page + " .controlLayer"); // page element

        var $el = $(document.createElement("div")); // creats a new div for the container and applies all relevent properties
        $el.addClass($scope.contentType + "ControlContainer");
        $el.css("width", data.width + "px");
        $el.css("height", data.height + "px");
        $el.css("top", data.y + "px");
        $el.css("left", data.x + "px");
        $el.attr("id", "element_" + i);

        var child = formTypeDefs[data.type][$scope.contentType](data); // runs the form type definition function to create a child object

        if (child != null && child != undefined) { // if the child object exists, add it to the form control container
            $(child).addClass($scope.contentType + "Control");
            $el.append(child);
        }

        if ($scope.contentType == "build" && tempData == undefined) // if the form is in build mode, add the edit functionality on click
            $el.on("click", { i: i }, function (e) { $scope.getElementDetails(e.data.i); });

        page.append($el); // adds the element to the page
        $compile($el.get(0))($scope); // apply $scope to control
    }

    $scope.printForm = function () {
        var $content = $("#docContent");
        var $form = $("#form");
        var $printForm = $("#printForm");
        $printForm.append($content);
        $content.addClass("save");
        window.print();
        $content.removeClass("save");
        $form.append($content);
    }

    function getPDFs() {
        $.ajax({
            type: "GET",
            url: "/api/CustomForms"
        }).done(function (data) {
            $scope.$evalAsync(function () {
                $scope.ActiveForms = data;
            });
        });
    };
    getPDFs();

    $scope.getPdfData = function () {
        //This breaks when it is half online, half offline, and it never goes away
        //LoadingGifService.ShowLoading();

        $.ajax({
            type: "GET",
            url: "/api/CustomForms/" + $scope.formId
        }).done(function (data) {
            $scope.$evalAsync(function () {
                $scope.formTitle = data.Name;
                $scope.formControls = JSON.parse(data.FieldData);
                $scope.formDefs = [];
                data.ImageData = JSON.parse(data.ImageData);
                for (var key in data.ImageData)
                    $scope.formDefs.push(data.ImageData[key]);
                $timeout(setupOverlays);

                $("#tabs-Documents").children().hide();
                $("#tabs-Documents #FormContainer").show();

                $("#main").addClass("")
            });
        }).fail(function (msg) {
            toastr.error("Failed To Load Form Over Network Connection.");
        });

        //.always(LoadingGifService.HideLoading);
    }

    $scope.newPdfData = function () {
        if ($scope.formId) {
            $scope.contentType = "edit";
            $scope.formData = {};
            $scope.formIndex = $scope.pcr.customForms.length;
            $scope.getPdfData();
        }
    }

    $scope.GoToPdf = function (index) {
        if (index < $scope.pcr.customForms.length) {
            $scope.contentType = "edit";
            $scope.formData = angular.copy($scope.pcr.customForms[index].data);
            $scope.formId = $scope.pcr.customForms[index].id;
            $scope.formIndex = index;
            $scope.getPdfData();
        }
    }

    $scope.ViewPdf = function (index) {
        if (index < $scope.pcr.customForms.length) {
            $scope.contentType = "view";
            $scope.formData = angular.copy($scope.pcr.customForms[index].data);
            $scope.formId = $scope.pcr.customForms[index].id;
            $scope.formIndex = index;
            $scope.getPdfData();
        }
    }

    $scope.CloseForm = function () {
        $("#tabs-Documents").children().show();
        $("#tabs-Documents #FormContainer").hide();

        $scope.formIndex = -1;
    }

    $scope.SaveForm = function () {
        $scope.pcr.customForms[$scope.formIndex] = {
            data: angular.copy($scope.formData),
            id: angular.copy($scope.formId),
            title: $scope.formTitle
        };
    }

    $scope.SaveCloseForm = function () {
        $scope.SaveForm();
        $scope.CloseForm();
    }
    */
    //END FORMS

    //Device Functions
    $scope.AddDevice = function (keepOpen) {

        if ($scope.pcr.devices == null)
            $scope.pcr.devices = [];


        var formId = parseInt($scope.DeviceForm.ModalFormId);

        var DeviceObject = {
            readingDate: $scope.DeviceForm.readingDate,
            readingTime: $scope.DeviceForm.readingTime,
            eventName: $scope.DeviceForm.eventName,
            waveFormType: $scope.DeviceForm.waveFormType,
            waveFormData: $scope.DeviceForm.waveFormData,
            typeOfShock: $scope.DeviceForm.typeOfShock,
            shockEnergy: $scope.DeviceForm.shockEnergy,
            numberOfShocks: $scope.DeviceForm.numberOfShocks,
            pacingRate: $scope.DeviceForm.pacingRate,
            heartRate: $scope.DeviceForm.heartRate,
            pulseRate: $scope.DeviceForm.pulseRate,
            sbp: $scope.DeviceForm.sbp,
            dbp: $scope.DeviceForm.dbp,
            resp: $scope.DeviceForm.resp,
            ox: $scope.DeviceForm.ox,
            co2: $scope.DeviceForm.co2,
            co2Units: $scope.DeviceForm.co2Units,
            invasivePressureMean: $scope.DeviceForm.invasivePressureMean,
            ecgInterp: $scope.DeviceForm.ecgInterp,
            ecgLead: $scope.DeviceForm.ecgLead,
            mode: $scope.DeviceForm.mode
        };

        $scope.DeviceForm = {};


        if (formId >= 0) {
            $scope.pcr.devices.splice(formId, 1, DeviceObject
            );
        }
        else {
            $scope.pcr.devices.push(
                    DeviceObject

            );
        }

        if (keepOpen) {
            $scope.ClearModal2('[modaltarget=DeviceModal]');
            $("[name='DeviceForm_eventName']").siblings(".select2-container").select2("open");
            $scope.setScroll();
        }
        else {
            $scope.HideAndClearModal2('[modaltarget=DeviceModal]');
            $scope.ClearDeviceForm();
            toastr.success('Item Added', '');
            $scope.SaveOrUpdateHandler();
        }

    };

    $scope.removeDevice = function (index) {
        if (confirm("Are you sure you want to DELETE the item?") == true) {
            $scope.pcr.devices.splice(index, 1);
        }
    };

    $scope.EditModalDevice = function (itemId) {



        $scope.DeviceForm = {
            readingTime: $scope.pcr.devices[itemId].readingTime,
            readingDate: $scope.pcr.devices[itemId].readingDate,
            eventName: $scope.pcr.devices[itemId].eventName,
            waveFormType: $scope.pcr.devices[itemId].waveFormType,
            waveFormData: $scope.pcr.devices[itemId].waveFormData,
            typeOfShock: $scope.pcr.devices[itemId].typeOfShock,
            shockEnergy: $scope.pcr.devices[itemId].shockEnergy,
            numberOfShocks: $scope.pcr.devices[itemId].numberOfShocks,
            pacingRate: $scope.pcr.devices[itemId].pacingRate,
            heartRate: $scope.pcr.devices[itemId].heartRate,
            pulseRate: $scope.pcr.devices[itemId].pulseRate,
            sbp: $scope.pcr.devices[itemId].sbp,
            dbp: $scope.pcr.devices[itemId].dbp,
            resp: $scope.pcr.devices[itemId].resp,
            ox: $scope.pcr.devices[itemId].ox,
            co2: $scope.pcr.devices[itemId].co2,
            co2Units: $scope.pcr.devices[itemId].co2Units,
            invasivePressureMean: $scope.pcr.devices[itemId].invasivePressureMean,
            ecgInterp: $scope.pcr.devices[itemId].ecgInterp,
            ecgLead: $scope.pcr.devices[itemId].ecgLead,
            mode: $scope.pcr.devices[itemId].mode,
            ModalFormId: itemId
        };

        $scope.$apply();

    };

    $scope.ClearDeviceForm = function () {
        $scope.DeviceForm = {};
    };

    $scope.SetDeviceDateTime = function () {
        var date = "";
        var time = "";
        if ($scope.pcr.devices && $scope.pcr.devices.length > 0) {
            date = $scope.pcr.devices[$scope.pcr.devices.length - 1].readingDate;
            time = $scope.pcr.devices[$scope.pcr.devices.length - 1].readingTime;
        }
        else {
            if ($scope.pcr.incidentDate) {
                date = $scope.pcr.incidentDate;
            }
            else {
                date = getDate();
            }
            if ($scope.pcr.incidentPtContactTime) {
                time = $scope.pcr.incidentPtContactTime;
            }
            else {
                time = moment().format('HH:mm');
            }

        }

        $scope.DeviceForm = {
            readingDate: date,
            readingTime: time
        };
    };


    /*  END Nates One to Many */
    /*  END Nates One to Many */
    /*  END Nates One to Many */
    /*  END Nates One to Many */
    /*  END Nates One to Many */
    /*  END Nates One to Many */


    //// What have a new dropdown and select2 pattern of using selects in v2.1
    //// curious why we are passing in $event instead of this, which wuold make more sense
    ////$scope.InstantiateSelect2sByEvent = function ($event) {
    ////    //alert($event.currentTarget);
    ////    //var modalTarget = 
    ////    var modaltarget = $($event.currentTarget).attr('data-target');
    ////    //$.ajax({
    ////    //    type: "GET",
    ////    //    url: "/api/DropDownSelect2Options/"
    ////    //})
    ////    //.done(function (msg) {
    ////    $(modaltarget + " input[data-select2_list]").each(function (index) {



    ////        var array = $(this).attr('data-select2_list').split('|');



    ////        var functionName = array[0];

    ////        var listArrayName = array[1];
    ////        //alert(listArrayName);
    ////        var booles = (null == listArrayName);

    ////        //alert( "|" + functionName + "|" + listArrayName + "|" + booles);

    ////        var arrayToPass = window[listArrayName].sort();

    ////        if ("Select2Single" == functionName) {

    ////            Select2Single(this, arrayToPass);

    ////        }
    ////        else if ("Select2_TagsMultiple" == functionName) {
    ////            //if(msg[listArrayName])
    ////            //{
    ////            //    Select2_TagsMultiple(this, window[listArrayName].concat(msg[listArrayName]));
    ////            //}
    ////            //else
    ////            //{
    ////            Select2_TagsMultiple(this, arrayToPass);
    ////            //}

    ////        }
    ////        else if ("Select2Multiple" == functionName) {
    ////            Select2Multiple(this, arrayToPass);
    ////        }
    ////        else if ("Select2_TagsSingle" == functionName) {
    ////            //if(msg[listArrayName])
    ////            //{
    ////            //    Select2_TagsSingle(this, window[listArrayName].concat(msg[listArrayName]));
    ////            //}
    ////            //else {
    ////            Select2_TagsSingle(this, arrayToPass);
    ////            // }
    ////        }

    ////    });
        ////});


    ////    $scope.AttachDemographicSelect2s();
    ////    $scope.UpdateCurrentCrewDropdowns();
    ////};

    //// rethink instantiation of select2s
    ////$scope.InstantiateSelect2sBySelector = function (selector, attachDemographics) {
    ////    //alert($event.currentTarget);
    ////    //var modalTarget = 
    ////    //var modaltarget = $($event.currentTarget).attr('data-target');
    ////    //$.ajax({
    ////    //    type: "GET",
    ////    //    url: "/api/DropDownSelect2Options/"
    ////    //})
    ////    //.done(function (msg) {
    ////    $(selector + " input[data-select2_list]").each(function (index) {



    ////        var array = $(this).attr('data-select2_list').split('|');


    ////        var functionName = array[0];

    ////        var listArrayName = array[1];
    ////        //alert(listArrayName);
    ////        var booles = (null == listArrayName);

    ////        var arrayToPass = window[listArrayName].sort();

    ////        //alert(JSON.stringify(arrayToPass));
    ////        if ("Select2Single" == functionName) {
    ////            Select2Single(this, arrayToPass);

    ////        }
    ////        else if ("Select2_TagsMultiple" == functionName) {
    ////            //if(msg[listArrayName])
    ////            //{
    ////            //    Select2_TagsMultiple(this, window[listArrayName].concat(msg[listArrayName]));
    ////            //}
    ////            //else
    ////            //{
    ////            Select2_TagsMultiple(this, arrayToPass);
    ////            //}

    ////        }
    ////        else if ("Select2Multiple" == functionName) {
    ////            Select2Multiple(this, arrayToPass);
    ////        }
    ////        else if ("Select2_TagsSingle" == functionName) {
    ////            //if(msg[listArrayName])
    ////            //{
    ////            //    Select2_TagsSingle(this, window[listArrayName].concat(msg[listArrayName]));
    ////            //}
    ////            //else {
    ////            Select2_TagsSingle(this, arrayToPass);
    ////            // }
    ////        }

    ////    });
    ////    //});

    ////    if (attachDemographics) {
    ////        $scope.AttachDemographicSelect2s();
    ////    }
    ////    $scope.UpdateCurrentCrewDropdowns();
    ////};

    ////$scope.InstantiateSelect2s = function () {

    ////    //$.ajax({
    ////    //    type: "GET",
    ////    //    url: "/api/DropDownSelect2Options/"
    ////    //})
    ////    //.done(function (msg) {
    ////    $("input[data-Select2_list]").each(function (index) {



    ////        var array = $(this).attr('data-Select2_list').split('|');

    ////        var functionName = array[0];

    ////        var listArrayName = array[1];

    ////        var booles = (null == listArrayName);

    ////        //alert( "|" + functionName + "|" + listArrayName + "|" + booles);

    ////        var arrayToPass = window[listArrayName].sort();

    ////        if ("Select2Single" == functionName) {

    ////            Select2Single(this, arrayToPass);

    ////        }
    ////        else if ("Select2_TagsMultiple" == functionName) {
    ////            //if(msg[listArrayName])
    ////            //{
    ////            //    Select2_TagsMultiple(this, window[listArrayName].concat(msg[listArrayName]));
    ////            //}
    ////            //else
    ////            //{
    ////            Select2_TagsMultiple(this, arrayToPass);
    ////            //}

    ////        }
    ////        else if ("Select2Multiple" == functionName) {
    ////            Select2Multiple(this, arrayToPass);
    ////        }
    ////        else if ("Select2_TagsSingle" == functionName) {
    ////            //if(msg[listArrayName])
    ////            //{
    ////            //    Select2_TagsSingle(this, window[listArrayName].concat(msg[listArrayName]));
    ////            //}
    ////            //else {
    ////            Select2_TagsSingle(this, arrayToPass);
    ////            // }
    ////        }

    ////    });
    ////    //});

    ////    $scope.AttachDemographicSelect2s();


    ////};


    //// Do not see this being used anywhere
    ////$scope.CreateMergedView = function () {
    ////    if ($scope.vitalViewToggle == false) {
    ////        $scope.tempVitals = [];
    ////        $scope.mergedVitals = [];
    ////        if ($scope.pcr.vitals && $scope.pcr.vitals.length > 0) {
    ////            $scope.tempVitals = $scope.tempVitals.concat($scope.pcr.vitals);
    ////        }
    ////        if ($scope.pcr.treatmentMedications && $scope.pcr.treatmentMedications.length > 0) {
    ////            $scope.tempVitals = $scope.tempVitals.concat($scope.pcr.treatmentMedications);
    ////        }
    ////        if ($scope.pcr.procedures && $scope.pcr.procedures.length > 0) {
    ////            $scope.tempVitals = $scope.tempVitals.concat($scope.pcr.procedures);
    ////        }

    ////        for (var i = 0; i < $scope.tempVitals.length; i++) {
    ////            $scope.mergedVitals.push(
    ////                {
    ////                    Time: i
    ////                }
    ////            );
    ////        }

    ////    }
    ////};


    //// belongs in Javascript utils
    $scope.GetGuid = function () {
        var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);

            return v.toString(16);
        });

        return guid;
    };

    //// Is this even being used?
    ////$scope.LoadFromOffline = function () {

    ////    var PcrOfflineArray = JSON.parse(localStorage.getItem("PcrOfflineArray"));
    ////    var offlineId = $scope.pcrid;


    ////    //alert("offlineId: |" + offlineId + "|");
    ////    //alert("Length:" + PcrOfflineArray.length);
    ////    if (PcrOfflineArray) {
    ////        for (var i = 0; i < PcrOfflineArray.length; i++) {

    ////            if (PcrOfflineArray[i].ID == offlineId) {
    ////                // alert(PcrOfflineArray[i].DataAsJson);
    ////                $scope.Version = PcrOfflineArray[i].Version;
    ////                $scope.pcr = JSON.parse(PcrOfflineArray[i].DataAsJson);
    ////                $scope.oldPcrReport = JSON.stringify($scope.pcr);
    ////                $scope.OfflinePcr = PcrOfflineArray[i].Offline;
    ////                var pcrLoaded = true;
    ////                break;
    ////            }
    ////            //dataSet.push(["<a href='/SmartAdmin15/Pcr_form.aspx?offlineId=" + item.ID + "' >Show Details</a>", item.ID, item.Name, item.Created, item.CreatedBy]);
    ////        }
    ////    }
    ////};

    $scope.readFrequentFlyerInfo = function () {

        $.ajax({
            type: "GET",
            url: "/api/CloudPCR/" + $('#aafrequentFlyerSelect2').val()
        })
        .done(function (msg) {


            //alert(msg.DataAsJson);

            var ffpcr = JSON.parse(msg.DataAsJson);

            //alert(tempPcr.patientFirstName);

            //alert("" + JSON.stringify(ffpcr) );


            $scope.pcr.patientFirstName = ffpcr.patientFirstName;
            $scope.pcr.patientLastName = ffpcr.patientLastName;
            $scope.pcr.patientMiddleName = ffpcr.patientMiddleName;
            $scope.pcr.patientDOB = ffpcr.patientDOB;
            $scope.pcr.patientAge = ffpcr.patientAge;
            $scope.pcr.patientAgeMonths = ffpcr.patientAgeMonths;
            $scope.pcr.PatientAddress = ffpcr.PatientAddress;
            $scope.pcr.patientPhoneNumber = ffpcr.patientPhoneNumber;
            $scope.pcr.patientWeight = ffpcr.patientWeight;
            $scope.pcr.patientRace = ffpcr.patientRace;
            $scope.pcr.patientEthnicity = ffpcr.patientEthnicity;
            $scope.pcr.patientGender = ffpcr.patientGender;
            $scope.pcr.patientSsn = ffpcr.patientSsn;
            $scope.pcr.patientDriverLicenseNumber = ffpcr.patientDriverLicenseNumber;
            $scope.pcr.patientDriverLicenseState = ffpcr.patientDriverLicenseState;
            $scope.pcr.practitionerLast = ffpcr.practitionerLast;
            $scope.pcr.patientMedicalHistory = ffpcr.patientMedicalHistory;
            $scope.pcr.patientMedicationAllergies = ffpcr.patientMedicationAllergies;
            $scope.pcr.patientEnvironmentalAllergies = ffpcr.patientEnvironmentalAllergies;
            $scope.pcr.advancedDirectives = ffpcr.advancedDirectives;
            $scope.pcr.patientFemalePastPregnancies = ffpcr.patientFemalePastPregnancies;
            $scope.pcr.patientFemalePregant = ffpcr.patientFemalePregant;
            $scope.pcr.patientFemalePregnancyDuration = ffpcr.patientFemalePregnancyDuration;
            $scope.pcr.emergencyInfoFormSelect = ffpcr.emergencyInfoFormSelect;
            $scope.pcr.medHxObtainedFrom = ffpcr.medHxObtainedFrom;
            $scope.pcr.conditionCodes = ffpcr.conditionCodes;
            $scope.pcr.primaryPaymentMethod = ffpcr.primaryPaymentMethod;
            $scope.pcr.billingWorkRelatedSelect = ffpcr.billingWorkRelatedSelect;
            $scope.pcr.patientEmployer = ffpcr.patientEmployer;
            $scope.pcr.patientWorkPhone = ffpcr.patientWorkPhone;
            $scope.pcr.patientOccupation = ffpcr.patientOccupation;
            $scope.pcr.patientOccupationIndustry = ffpcr.patientOccupationIndustry;
            $scope.pcr.certificateMedNecessitySelect = ffpcr.certificateMedNecessitySelect;
            $scope.pcr.responseUrgency = ffpcr.responseUrgency;
            $scope.pcr.billingLastName = ffpcr.billingLastName;
            $scope.pcr.billingFirstName = ffpcr.billingFirstName;
            $scope.pcr.billingRelationshipToPatient = ffpcr.billingRelationshipToPatient;
            $scope.pcr.billingPhonenumber = ffpcr.billingPhonenumber;
            $scope.pcr.EmployerAddress = ffpcr.EmployerAddress;
            $scope.pcr.patientMedications = ffpcr.patientMedications;
            $scope.pcr.immunizations = ffpcr.immunizations;
            $scope.pcr.insurances = ffpcr.insurances;
            $scope.pcr.GuardianAddress = ffpcr.GuardianAddress;
            $scope.$apply();
            $scope.InstantiateSelect2sBySelector("#tabs-Billing", false);
            $scope.InstantiateSelect2sBySelector("#tabs-Patient", true);



        });

    };

    //// Most of this will not be needed if we are always and only working out of local memory
    //// But still not sure what is going on with the setting of the pcr data
    ////$scope.readReportFromList = function () {
    ////    var data = DataService.pcr.GetPcr($scope.pcrid);

    ////    if (data.promise == null)
    ////        initReport(data);
    ////    else
    ////        data.promise.done(initReport);

    ////    function initReport(msg) {

    ////        $timeout(function () {
    ////            loadNotes();

    ////            if (!msg || angular.isString(msg)) {
    ////                alert("Error loading pcr\n" + msg)
    ////                $state.go("ListView");
    ////            }

    ////            if (msg.ModifiedBy != currentUser && msg.CreatedBy != currentUser
    ////                && $rootScope.user.roles.indexOf("Admin") == -1 && $rootScope.user.roles.indexOf("Approver") == -1) {
    ////                $state.go("Error", { id: "invalidPCR" });
    ////                return;
    ////            }

    ////            try {
    ////                $scope.pcr = JSON.parse(msg.DataAsJson);
    ////                if (!$scope.pcr)
    ////                    $scope.pcr = {};
    ////            }
    ////            catch (ex) {
    ////                $scope.pcr = {};
    ////            }

    ////            $scope.pcr.customForms = defaultedValue($scope.pcr.customForms, []);
    ////            $scope.pcr.signatures = defaultedValue($scope.pcr.signatures, {});

    ////            $scope.Modified = msg.Modified;
    ////            $scope.ModifiedBy = msg.ModifiedBy;
    ////            $scope.Created = msg.Created;
    ////            $scope.CreatedBy = msg.CreatedBy;
    ////            $scope.Version = msg.Version;

    ////            $scope.extraPcrData = {
    ////                Cache: msg.Cache,
    ////                isActive: msg.IsActive,
    ////                Created: msg.Created,
    ////                CreatedBy: msg.CreatedBy,
    ////                Modified: msg.Modified, // update this somewhere, probably pcr service
    ////                ModifiedBy: msg.ModifiedBy, // update this somewhere, probably pcr service
    ////                //StateXml: msg.StateXml,
    ////                //NemsisXml: msg.NemsisXml,
    ////                Version: msg.Version,
    ////                Status: msg.Status
    ////            };

    ////            $scope.oldPcrReport = JSON.stringify($scope.pcr);

    ////            //if (msg.Status != "0") {
    ////            //    alert("You are viewing a submitted PCR.  You will not be able to save changes to this report");
    ////            //}

    ////            $scope.offlinePCR = false;
    ////            //$scope.$apply();
    ////            if ($scope.pcr.currentDisposition) {
    ////                $scope.ApplyDisposition();
    ////            }
    ////            $scope.$apply();
    ////            $scope.InstantiateSelect2s();


    ////            $('[data-page-loading="waiting"]').hide();
    ////            $('[data-page-loading="form"]').slideDown();

    ////            ShowTab('tabs-Incident');
    ////            $('.pcrtabs').show();
    ////            var pcrLoaded = true;
    ////            $("#contentWrapper").show();
    ////            LoadingGifService.HideLoading();

    ////            if ($scope.pcr.SceneAddress)
    ////                $scope.pcr.SceneAddress.advancedFips = $scope.pcr.SceneAddress.advancedFips != false;
    ////            else
    ////                $scope.pcr.SceneAddress = { advancedFips: true };

    ////            if ($scope.pcr.DestinationAddress)
    ////                $scope.pcr.DestinationAddress.advancedFips = $scope.pcr.DestinationAddress.advancedFips != false;
    ////            else
    ////                $scope.pcr.DestinationAddress = { advancedFips: true };

    ////            if ($scope.pcr.PatientAddress)
    ////                $scope.pcr.PatientAddress.advancedFips = $scope.pcr.PatientAddress.advancedFips != false;
    ////            else
    ////                $scope.pcr.PatientAddress = { advancedFips: true };

    ////            if ($scope.pcr.EmployerAddress)
    ////                $scope.pcr.EmployerAddress.advancedFips = $scope.pcr.EmployerAddress.advancedFips != false;
    ////            else
    ////                $scope.pcr.EmployerAddress = { advancedFips: true };

    ////            if ($scope.pcr.GuardianAddress)
    ////                $scope.pcr.GuardianAddress.advancedFips = $scope.pcr.GuardianAddress.advancedFips != false;
    ////            else
    ////                $scope.pcr.GuardianAddress = { advancedFips: true };

    ////            $timeout(function () {
    ////                $(".select2-offscreen").each(function () {
    ////                    var $control = $(this);
    ////                    var ng_model = $control.attr("ng-model") || $control.attr("data-ng-model");
    ////                    var value = $parse(ng_model)($scope);
    ////                    if (!$(".select2-container", $control.parent()).hasClass("select2-container-multi")) {
    ////                        if (value != ($control.select2("data") || { id: null }).id) {
    ////                            $control.select2("data", { text: value, id: value }, true);
    ////                        }
    ////                    } else {
    ////                        var valueArray = (value != null ? value.split(",") : []);
    ////                        var data = $control.select2("data");
    ////                        for (var i = 0; i < valueArray.length; i++) {
    ////                            if (valueArray[i] && $.inArray(valueArray[i], data)) {
    ////                                data.push({ text: valueArray[i], id: valueArray[i] });
    ////                            }
    ////                        }
    ////                        $control.select2("data", data, true);
    ////                    }
    ////                })
    ////            }, 1);

    ////            $scope.updatePcrToList();
    ////            $timeout(sync, 1);
    ////            buildTimeline();

    ////        });
    ////    }
    ////};

    /*
        Help info
    */


    //// is this still used? 
    $scope.ShowHelpInfo = ShowHelpInfo;
    function ShowHelpInfo(event) {
        $('[modaltarget = "HelpModeModal"]').modal('toggle');
        var ngModel = $("[data-ng-model]", $(event.target).parent()).last().attr("data-ng-model");
        if (!ngModel) {
            ngModel = $("[help-field]", $(event.target).parent()).attr("help-field");
        }
        //var ngModel = $(event.target).parent().parent().find('[data-ng-model]').attr('data-ng-model');

        //alert("" +  ngModel);

        $('.helpModalTargetCode').html("");
        $('.helpModalTargetDataElement').html("");
        $('.helpModalTargetName').html("");
        $('.helpModalTargetDefinition').html("");
        $('.helpModalTargetModel').html("");
        $('.helpModalTargetSynonyms').html("");

        for (var i = 0; i < helpInfoObjectArray.length ; i++) {
            if (helpInfoObjectArray[i].model == ngModel) {

                $('.helpModalTargetCode').html(helpInfoObjectArray[i].code);
                $('.helpModalTargetDataElement').html(helpInfoObjectArray[i].dataElement);
                $('.helpModalTargetName').html(helpInfoObjectArray[i].name);
                $('.helpModalTargetDefinition').html(helpInfoObjectArray[i].definition);
                $('.helpModalTargetModel').html(helpInfoObjectArray[i].model);
                $('.helpModalTargetSynonyms').html(helpInfoObjectArray[i].synonyms);
                break;
            }
        }
        if (i == helpInfoObjectArray.length) {
            // If we make it in here, the help info wasn't found
            //alert(ngModel + " has no help info")
        }


    }

    /*
        **********************************************************
        RequestAlias
    */
    $scope.nullableAction = function (event, ui) {
        var newItem = { id: ui.cmd, text: ui.cmd };
        var select2 = $(".select2-container", $(event.target).parent());
        if (select2.length > 0) {
            if (select2.hasClass("select2-container-multi")) {
                var selectData = select2.select2("data");
                if (!selectData) selectData = [];
                else selectData.push(newItem);
                select2.select2("data", selectData, true);
            }
            else {
                select2.select2('data', newItem, true);
            }
        }
        else {
            select2 = $("input", $(event.target).parent());
            select2.val(ui.cmd);
            select2.trigger("change");
        }
    }
    
    //// No more alias requests
    ////$scope.resetAliasReq = function () {
    ////    $scope.aliasReq = {
    ////        field: '',
    ////        alias: '',
    ////        entry: '',
    ////        model: '',
    ////        comments: '',
    ////        context: '',
    ////        listName: '',
    ////        list: []
    ////    };
    ////}
    ////$scope.resetAliasReq();
    ////var aliasObj;

    //// no more alias requesting
    ////$scope.requestAlias = function (event) {

    ////    try {
    ////        if (event.type != "click") return;
    ////        $scope.resetAliasReq();
    ////        var target = $(event.target);
    ////        var select2 = $("#aliasSelect2");
    ////        var list = $("[data-select2_list]", target.parent()).last().attr("data-select2_list");
    ////        $scope.aliasReq.field = target.parent().children().first().text().replace(':', "").trim();
    ////        if (list) list = list.split('|')[1];
    ////        else list = $("[data-listname]", target.parent()).last().attr("data-listname"); // data-listname is used when the item is not a traditional select2 list ?
    ////        $scope.aliasReq.list = $filter('filter')(window[list], '');
    ////        for (var i = 0; i < $scope.aliasReq.list.length; i++) {
    ////            if ($scope.aliasReq.list[i].alias) {
    ////                $scope.aliasReq.list.splice(i--, 1);
    ////            }
    ////        }
    ////        $scope.aliasReq.listName = list;
    ////        $scope.aliasReq.model = ($("input.select2-offscreen", target.parent()).last().attr("data-ng-model") || $("input.select2-offscreen", target.parent()).last().attr("ng-model"));
    ////        $scope.aliasReq.context = target.parent();
    ////        Select2Single(select2, $scope.aliasReq.list);
    ////        var options = $filter('filter')($scope.aliasReq.list, 'other');
    ////        if (options.length > 0) {
    ////            select2.select2('val', options[0].text);
    ////            $scope.aliasReq.entry = options[0].text;
    ////        }
    ////        else
    ////            $scope.aliasReq.entry = '';
    ////        $('#aliasFieldBox').val($scope.aliasReq.field);

    ////        $("#requestAliasModal").modal('show');
    ////    } catch (e) {
    ////        //alert(e.message + "\n\n" + e.stack);
    ////    }
    ////}


    //// no more ajax of alias
    ////$scope.validateAlias = function () {
    ////    try {
    ////        if ($("#aliasValidForm").valid()) {
    ////            aliasObj = {
    ////                ID: 0,
    ////                NameOfField: $scope.aliasReq.field,
    ////                NewAliasedValue: $scope.aliasReq.alias,
    ////                NemsisExportedValue: $scope.aliasReq.entry,
    ////                NgModelOfField: $scope.aliasReq.model,
    ////                Comments: $scope.aliasReq.comments,
    ////                Select2ListName: $scope.aliasReq.listName,
    ////                Status: "Pending"
    ////            }
    ////            $scope.HideAndClearModal2('[modalTarget=requestAliasModal]');
    ////            $.ajax({
    ////                type: "PUT",
    ////                url: "/api/alias/0", // using zero signifies a new request
    ////                data: aliasObj
    ////            }).done(function (msg) {
    ////                if (msg) {
    ////                    var newItem = {
    ////                        id: aliasObj.NewAliasedValue + " [" + aliasObj.NemsisExportedValue + "]",
    ////                        text: aliasObj.NewAliasedValue + " [" + aliasObj.NemsisExportedValue + "]",
    ////                        alias: true
    ////                    };
    ////                    var select2 = $(".select2-container", $scope.aliasReq.context);
    ////                    window[$scope.aliasReq.listName].push(newItem);
    ////                    if (select2.hasClass("select2-container-multi")) {
    ////                        var selectData = select2.select2("data");
    ////                        if (!selectData) selectData = [];
    ////                        else selectData.push(newItem);
    ////                        select2.select2("data", selectData, true);
    ////                    }
    ////                    else {
    ////                        select2.select2('data', newItem, true);
    ////                    }
    ////                    //select2.trigger("change");
    ////                    //$('<option value=' + aliasObj.NemsisExportedValue + '>' + aliasObj.NewAliasedValue + '</option>').appendTo(select2);
    ////                    $scope.clearAliasValidator();
    ////                    toastr.success("", "Request Submitted Successfully");
    ////                } else
    ////                    toastr.error("", "An error occurred while submitting your request");
    ////            });
    ////        }
    ////    } catch (e) {
    ////        toastr.error("", "An error occurred while submitting your request");
    ////    }
    ////}

    ////$scope.clearAliasValidator = function () {
    ////    $("#aliasValidForm").validate().resetForm();
    ////}

    /*
        *********************************************************
        END RequestAlias
    */

    $scope.autofillScene = function () {
        var name = $scope.pcr.quickScene;
        var scenes = $scope.demo2.ConfigScenes;
        var hospitals = $scope.demo2.ConfigHospitals;
        var otherFacilities = $scope.demo2.ConfigOtherFacilitys;

        if (scenes) {
            for (var i = 0; i < scenes.length; i++) {
                if (scenes[i].ConfigSceneName == name) {
                    $scope.pcr.SceneAddress = {
                        combinedStreet: scenes[i].ConfigSceneStreetAddress,
                        city: scenes[i].ConfigSceneCity,
                        state: scenes[i].ConfigSceneState,
                        zip: scenes[i].ConfigSceneZip,
                        fips: scenes[i].ConfigSceneFips,
                        fipsCounty: scenes[i].ConfigSceneCountyFips,
                        advancedFips: $scope.pcr.SceneAddress.advancedFips
                    }
                    $scope.$apply();
                    $("[name='pcr_SceneAddress_state']").select2("val", $scope.pcr.SceneAddress.state);
                    $("[ng-model='pcr.pcr_SceneAddress_state.city']").trigger("change");
                    return;
                }
            }
        }
        if (hospitals) {
            for (i = 0; i < hospitals.length; ++i) {
                if (hospitals[i].ConfigHospitalName == name) {
                    $scope.pcr.SceneAddress = {
                        combinedStreet: hospitals[i]["ConfigHospitalStreetNumber"] + " " + hospitals[i]["ConfigHospitalStreet"],
                        city: hospitals[i]["ConfigHospitalCity"],
                        state: hospitals[i]["ConfigHospitalState"],
                        zip: hospitals[i]["ConfigHospitalPostalCode"],
                        fips: hospitals[i]["ConfigHospitalFipsCode"],
                        fipsCounty: hospitals[i]["ConfigHospitalFipsCountyCode"],
                        advancedFips: $scope.pcr.SceneAddress.advancedFips
                    };
                    $scope.$apply();
                    $("[name='pcr_SceneAddress_state']").select2("val", $scope.pcr.SceneAddress.state);
                    $("[ng-model='pcr.pcr_SceneAddress_state.city']").trigger("change");
                    return;
                }
            }
        }
        if (otherFacilities) {
            for (i = 0; i < otherFacilities.length; ++i) {
                if (otherFacilities[i].ConfigOtherFacilityName == name) {
                    $scope.pcr.SceneAddress = {
                        combinedStreet: otherFacilities[i]["ConfigOtherFacilityStreetNumber"] + " " + otherFacilities[i]["ConfigOtherFacilityStreet"],
                        city: otherFacilities[i]["ConfigOtherFacilityCity"],
                        state: otherFacilities[i]["ConfigOtherFacilityState"],
                        zip: otherFacilities[i]["ConfigOtherFacilityPostalCode"],
                        fips: otherFacilities[i]["ConfigOtherFacilityFipsCode"],
                        fipsCounty: otherFacilities[i]["ConfigOtherFacilityFipsCountyCode"],
                        advancedFips: $scope.pcr.SceneAddress.advancedFips
                    };
                    $scope.$apply();
                    $("[name='pcr_SceneAddress_state']").select2("val", $scope.pcr.SceneAddress.state);
                    $("[ng-model='pcr.pcr_SceneAddress_state.city']").trigger("change");
                    return;
                }
            }
        }
        $scope.pcr.SceneAddress = { combinedStreet: "", city: "", state: "", zip: "", fips: "", fipsCounty: "", advancedFips: $scope.pcr.SceneAddress.advancedFips };
        $scope.$apply();
        $("[name='pcr_SceneAddress_state']").select2("val", $scope.pcr.SceneAddress.state);
        $("[ng-model='pcr.pcr_SceneAddress_state.city']").trigger("change");
    }

    /*
    Function to auto fill destination information based on selected item in Destination
    */

    $scope.autoFillDest = function () {
        var name = $scope.pcr.incidentDestination;
        var hospitals = $scope.demo2["ConfigHospitals"];
        var otherFacilities = $scope.demo2["ConfigOtherFacilitys"];
        var i;
        var found = false;

        if (hospitals) {
            //same function as UpdateDestinationCode, used here just to simplify my having to type xD
            for (i = 0; i < hospitals.length; ++i) {
                //alert("|" + hospitals[i]["ConfigHospitalName"] + "|" + hospitals[i]["ConfigHospitalNumber"] + "|");
                if (hospitals[i]["ConfigHospitalName"] == name) {
                    $scope.pcr.incidentDestinationCode = hospitals[i]["ConfigHospitalNumber"] || "";
                    //$scope.AttachSelect2ToControl(Select2Single, "VehicleDispatchZone", $scope.demo.ConfigZoneNumbers);
                    $("input[title='DestinationCode']").select2("val", $scope.pcr.incidentDestinationCode);
                    var tempStreetAddressObj = {
                        combinedStreet: hospitals[i]["ConfigHospitalStreetNumber"] + " " + hospitals[i]["ConfigHospitalStreet"],
                        city: hospitals[i]["ConfigHospitalCity"],
                        state: hospitals[i]["ConfigHospitalState"],
                        zip: hospitals[i]["ConfigHospitalPostalCode"],
                        fips: hospitals[i]["ConfigHospitalFipsCode"],
                        fipsCounty: hospitals[i]["ConfigHospitalFipsCountyCode"],
                        advancedFips: $scope.pcr.DestinationAddress.advancedFips
                    };
                    $scope.pcr.destinationFaxNumber = hospitals[i]["ConfigHospitalFaxNumber"];
                    $scope.pcr.DestinationAddress = tempStreetAddressObj;
                    $scope.pcr.incidentDestinationType = hospitals[i]["ConfigHospitalType"];
                    $("input[title='Incident-Type']").select2("val", hospitals[i]["ConfigHospitalType"]);
                    found = true;
                    break;
                }
                else {
                    var tempStreetAddressObj = {
                        combinedStreet: "",
                        city: "",
                        state: "",
                        zip: "",
                        fips: "",
                        fipsCounty: "",
                        advancedFips: $scope.pcr.DestinationAddress.advancedFips
                    };
                    $scope.pcr.DestinationAddress = tempStreetAddressObj;
                    $("input[title='Incident-Type']").select2("val", "");
                    $("input[title='DestinationCode']").select2("val", "");
                    found = false;
                }
            }
        }
        if (otherFacilities && !found) {
            //same function as UpdateDestinationCode, used here just to simplify my having to type xD
            for (i = 0; i < otherFacilities.length; ++i) {
                if (otherFacilities[i]["ConfigOtherFacilityName"] == name) {
                    $scope.pcr.incidentDestinationCode = otherFacilities[i]["ConfigOtherFacilityNumber"] || "";
                    //$scope.AttachSelect2ToControl(Select2Single, "VehicleDispatchZone", $scope.demo.ConfigZoneNumbers);
                    $("input[title='DestinationCode']").select2("val", $scope.pcr.incidentDestinationCode);
                    var tempStreetAddressObj = {
                        combinedStreet: otherFacilities[i]["ConfigOtherFacilityStreetNumber"] + " " + otherFacilities[i]["ConfigOtherFacilityStreet"],
                        city: otherFacilities[i]["ConfigOtherFacilityCity"],
                        state: otherFacilities[i]["ConfigOtherFacilityState"],
                        zip: otherFacilities[i]["ConfigOtherFacilityPostalCode"],
                        fips: otherFacilities[i]["ConfigOtherFacilityFipsCode"],
                        fipsCounty: otherFacilities[i]["ConfigOtherFacilityFipsCountyCode"]
                    };
                    $scope.pcr.destinationFaxNumber = otherFacilities[i]["ConfigOtherFacilityFaxNumber"];
                    $scope.pcr.DestinationAddress = tempStreetAddressObj;
                    $scope.pcr.incidentDestinationType = otherFacilities[i]["ConfigOtherFacilityType"];
                    $("input[title='Incident-Type']").select2("val", otherFacilities[i]["ConfigOtherFacilityType"]);
                    break;
                }
                else {
                    var tempStreetAddressObj = {
                        combinedStreet: "",
                        city: "",
                        state: "",
                        zip: "",
                        fips: "",
                        fipsCounty: ""
                    };
                    $scope.pcr.DestinationAddress = tempStreetAddressObj;
                    $("input[title='Incident-Type']").select2("val", "");
                    $("input[title='DestinationCode']").select2("val", "");
                }
            }
        }
        $("[name='pcr_DestinationAddress_state']").select2("val", $scope.pcr.DestinationAddress.state);
        $("[ng-model='pcr.DestinationAddress.city']").trigger("change");
    };

    /*
        End of auto fill functions
    */

    // START Modal overlap

    //var openModals = [];
    //$(".modal").on('show.bs.modal', function () {
    //    if ($.inArray(openModals, $(".modal:visible").attr("modaltarget"))) {
    //        openModals.push($(".modal:visible").attr("modaltarget"));
    //        //$(".modal:visible").modal('hide');
    //    }
    //})
    //$(".modal").on('hidden.bs.modal', function () {
    //    if (openModals[openModals.length - 1] !== $(this).attr("modaltarget"))
    //    if (openModals.length > 0) {
    //        var nextModalTarget = openModals.pop();
    //        $("[modaltarget=" + nextModalTarget + "]").modal('show');
    //    }
    //})

    // END modal overlap

    $scope.nullOptions = ['', 'Yes', 'No', '***Not Known', '***Not Available', '***Not Applicable'];

    $scope.pcr.emergencyInfoFormSelect = $scope.nullOptions[0]; // red
    $scope.pcr.priorAidBoolSelect = $scope.nullOptions[0];

    $scope.pcr.massCasualitySelect = $scope.nullOptions[0]; // red
    $scope.pcr.cardiacArrestSelect = $scope.nullOptions[0];
    $scope.pcr.possibleInjurySelect = $scope.nullOptions[0]; // red
    $scope.pcr.billingWorkRelatedSelect = $scope.nullOptions[0];
    $scope.pcr.certificateMedNecessitySelect = $scope.nullOptions[0]; // red
    $scope.pcr.reviewRequestedSelect = $scope.nullOptions[0];
    $scope.pcr.fluidContactSelect = $scope.nullOptions[0]; // red
    $scope.pcr.emsDeathSelect = $scope.nullOptions[0];
    $scope.pcr.requiredReportableConditionsSelect = $scope.nullOptions[0];
    $scope.pcr.emdContactSelect = $scope.nullOptions[0];
    $scope.pcr.mvcSelect = $scope.nullOptions[0];
    $scope.pcr.insuranceDifferentThanPatientSelect = $scope.nullOptions[0];
    $scope.pcr.billingDifferentThanPatientSelect = $scope.nullOptions[0];

    //// this whole function needs explained
    $(document).ready(function () {
        if (agencyEditFormJS) {
            agencyEditFormJS();
            $("#content .customJS").each(function () {
                $compile(this)($scope);
            });
        }

        try {
            var recognition = new webkitSpeechRecognition();
        } catch (e) {
            var recognition = Object;
        }
        recognition.continuous = true;
        recognition.interimResults = true;

        var interimResult = '';
        var textArea = $('#speech-page-content');
        var textAreaID = 'speech-page-content';

        $('.fa-microphone').click(function () {
            startRecognition();
        });

        $('.fa-microphone-slash').click(function () {
            recognition.stop();
        });

        var startRecognition = function () {
            $('.speech-content-mic').removeClass('fa-microphone').addClass('fa-microphone-slash');
            textArea.focus();
            recognition.start();
        };

        recognition.onresult = function (event) {
            var pos = textArea.getCursorPosition() - interimResult.length;
            textArea.val(textArea.val().replace(interimResult, ''));
            interimResult = '';
            textArea.setCursorPosition(pos);
            for (var i = event.resultIndex; i < event.results.length; ++i) {
                if (event.results[i].isFinal) {
                    insertAtCaret(textAreaID, event.results[i][0].transcript);
                } else {
                    isFinished = false;
                    insertAtCaret(textAreaID, event.results[i][0].transcript + '\u200B');
                    interimResult += event.results[i][0].transcript + '\u200B';
                }
            }
        };

        recognition.onend = function () {
            $('.speech-content-mic').removeClass('fa-microphone-slash').addClass('fa-microphone');
        };

        insertAtCaret = function (areaId, text) {
            var txtarea = document.getElementById(areaId);
            var scrollPos = txtarea.scrollTop;
            var strPos = 0;
            var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ?
                "ff" : (document.selection ? "ie" : false));
            if (br == "ie") {
                txtarea.focus();
                var range = document.selection.createRange();
                range.moveStart('character', -txtarea.value.length);
                strPos = range.text.length;
            }
            else if (br == "ff") strPos = txtarea.selectionStart;

            var front = (txtarea.value).substring(0, strPos);
            var back = (txtarea.value).substring(strPos, txtarea.value.length);
            txtarea.value = front + text + back;
            strPos = strPos + text.length;
            if (br == "ie") {
                txtarea.focus();
                range = document.selection.createRange();
                range.moveStart('character', -txtarea.value.length);
                range.moveStart('character', strPos);
                range.moveEnd('character', 0);
                range.select();
            }
            else if (br == "ff") {
                txtarea.selectionStart = strPos;
                txtarea.selectionEnd = strPos;
                txtarea.focus();
            }
            txtarea.scrollTop = scrollPos;
        };

        $.fn.getCursorPosition = function () {
            var el = $(this).get(0);
            var pos = 0;
            if ('selectionStart' in el) {
                pos = el.selectionStart;
            } else if ('selection' in document) {
                el.focus();
                var Sel = document.selection.createRange();
                var SelLength = document.selection.createRange().text.length;
                Sel.moveStart('character', -el.value.length);
                pos = Sel.text.length - SelLength;
            }
            return pos;
        };

        $.fn.setCursorPosition = function (pos) {
            if ($(this).get(0).setSelectionRange) {
                $(this).get(0).setSelectionRange(pos, pos);
            } else if ($(this).get(0).createTextRange) {
                var range = $(this).get(0).createTextRange();
                range.collapse(true);
                range.moveEnd('character', pos);
                range.moveStart('character', pos);
                range.select();
            }
        }
        //window.addEventListener("offline", function (e) {
        //    alert("offline"); $scope.PageOnline = false; $scope.$apply();
        //}, false);

        //window.addEventListener("online", function (e) {
        //    alert("online");
        //    $scope.PageOnline = true;
        //    //$scope.$apply();

        //    //alert("Syncing....");


        //    $scope.SyncOfflineReports();

        //}, false);
        pageSetUp();

        // initialize();

        $("#aliasValidForm").validate({
            validClass: "valid",
            errorClass: "invalid",
            rules: {
                aliasText: "required",
                aliasSelect2: "required"
            },
            messages: {
                aliasText: "Please enter an alias",
                aliasSelect2: "Please select the alias' target label"
            }
        });

        window.addEventListener("keydown", function (e) {
            if (e.keyCode === 114 || (e.ctrlKey && e.keyCode === 70)) {
                e.preventDefault();
                ToggleFieldSearch();
            }
        });

        window.addEventListener("keydown", function (e) {
            if (e.ctrlKey && e.keyCode === 83) {
                e.preventDefault();
                //$('#saveButtonId').trigger('click');
                $scope.SaveOrUpdateHandler();
            }
        });

        $('#searchTextBox').on("change", function (e) {
            ToggleFieldSearch();
            GoToField2(e.val);
            $('#searchTextBox').select2('val', '');

            // $('#searchTextBox').parent().hide();
        });

        /*
        *
        *Ctrl+F synonyms pattern
        *
        */

        //function to get all synonyms for merge
        function getSearchData() {

        };

        /*
        *
        *Current CTRL-F pattern to include synonyms
        *
        */

        //function to merge synonyms and current data

        //// help should be offline
        ////$.ajax({
        ////    type: "GET",
        ////    url: "/api/Synonym",
        ////    success: function (msg) {
        ////        $scope.searchData = msg;
        ////        for (i = 0; i < $scope.searchData.length; i++) {
        ////            if ($scope.searchData[i].Synonym1 != null && $scope.searchData[i].NgModel != null)
        ////                fieldsForSearchArray.push({ id: $scope.searchData[i].NgModel, text: $scope.searchData[i].Synonym1 });
        ////        }
        ////        //push data to select2
        ////        $("#searchTextBox").select2({
        ////            placeholder: "",
        ////            width: "100%",
        ////            multiple: false,
        ////            data: fieldsForSearchArray
        ////        });
        ////    }
        ////});




        /*
        *
        *End CTRL-F pattern
        *
        */

        $("#mymarkdown").markdown({
            autofocus: false

        });


        $('.sigPad').signaturePad();

        $('#menu').slicknav({ 'label': '', 'prependTo': '#slick_nav_placeholder' });
        $('#menu').slicknav('open');
        //alert('i am here');

        /**
         * Created by jayshah on 10/24/14.
         */
        //ONLY WORKS IN WEBKIT BROWSERS
        $("label.select > i").on("click", function () {
            var element = this[0], worked = false;
            if (document.createEvent) { // all browsers
                var e = document.createEvent("MouseEvents");
                e.initMouseEvent("mousedown", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
                worked = element.dispatchEvent(e);
            } else if (element.fireEvent) { // ie
                worked = element.fireEvent("onmousedown");
            }
            if (!worked) { // unknown browser / error
                alert("It didn't worked in your browser.");
            }

        });

        $('body').removeClass("hidden-menu").addClass("minified");

        //$(".datatables").dataTable({
        //    "searching": false,
        //    "info": false,
        //    paging: false
        //});

        $("[timex]").each(function (index) {


            $(this).timepicker({
                defaultTime: "current",
                showMeridian: false,
                template: false,
                showInputs: false,
                minuteStep: 1
            });

        });

        $('.helpicon').click(function () {
            if ($('.helpiconDialog').length) {
                $('.helpiconDialog').hide().remove();
            }
            var fieldShow = $(this).attr('data-rangeInfo'),
                vertPos = $(this).offset().top;
            console.log(fieldShow);
            $('body').append('<div class="helpiconDialog"><div class="exitIcon">X</div></div>');
            switch (fieldShow) {
                case 'Eyes':
                    $('.helpiconDialog').append('<table class="helpiconTable"><thead><th>1</th><th>2</th><th>3</th><th>4</th></thead><tbody><tr><td><ul><li>None</li></ul></td><td><ul><li>Pain</li></ul></td><td><ul><li>Speech</li></ul></td><td><ul><li>Spontaneous</li></ul></td></tr></tbody></table>');
                    break;
                case 'Verbal':
                    $('.helpiconDialog').append('<table class="helpiconTable"><thead><th>1</th><th>2</th><th>3</th><th>4</th><th>5</th></thead><tbody><tr><td><ul><li>None</li></ul></td><td><ul><li>Incomprehensible</li></ul></td><td><ul><li>Inappropriate</li></ul></td><td><ul><li>Confused</li></ul></td><td><ul><li>Oriented</li></ul></td></tr></tbody></table>');
                    break;
                case 'Motor':
                    $('.helpiconDialog').append('<table class="helpiconTable"><thead><th>1</th><th>2</th><th>3</th><th>4</th><th>5</th><th>6</th></thead><tbody><tr><td><ul><li>None</li></ul></td><td><ul><li>Decerebrate</li></ul></td><td><ul><li>Decorticate</li></ul></td><td><ul><li>Withdraws</li></ul></td><td><ul><li>Localized</li></ul></td><td><ul><li>Normal</li></ul></td></tr></tbody></table>');
                    break;
            }
            $('.helpiconDialog').css('top', vertPos);
            $('.helpiconDialog').delegate('.exitIcon', 'click', function () {
                $('.helpiconDialog').hide().remove();
            })
        })
        //window.addEventListener("offline", function (e) {
        //   //alert("offline");
        //}, false);

        //window.addEventListener("online", function (e) {
        //   //alert("online");
        //}, false);

        var $aside = $("#left-aside").first();

        if (!($aside.hasClass('minified'))) {
            $aside.addClass('minified');
        }

        $('.pcrtabs').tabs();

        $('.pcrtabs').show();

        $scope.PageOnline = true;
        $scope.readReportFromList();

        Dropzone.autoDiscover = false;
        $("#mydropzone").dropzone({
            url: "/api/PcrDocuments",
            addRemoveLinks: false,
            maxFilesize: 10,
            dictDefaultMessage: '<span class="text-center"><span class="font-lg visible-xs-block visible-sm-block visible-lg-block"><span class="font-lg"><i class="fa fa-caret-right text-danger"></i> Drop files <span class="font-xs">to upload</span></span><span>&nbsp&nbsp<h4 class="display-inline"> (Or Click)</h4></span>',
            dictResponseError: 'Error uploading file!',
            init: function () {
                $scope.dpzone = this;
                this.on("success", function (file, responseText) {
                    // $("#StudentDocumentsRefresh").trigger("click");
                    // Handle the responseText here. For example, add the text to the preview element:
                    //file.previewTemplate.appendChild(document.createTextNode(responseText));
                    loadPcrDocs();
                });
            }
        });

        $scope.ClearDropzone = function () {
            $scope.dpzone.removeAllFiles(true);
        };



        $('.aafrequentFlyerSelect2').select2(
                {
                    placeholder: 'Enter name',
                    //Does the user have to enter any data before sending the ajax request
                    minimumInputLength: 3,
                    width: "450px",
                    allowClear: true,
                    ajax: {
                        //How long the user has to pause their typing before sending the next request
                        quietMillis: 150,
                        //The url of the json service
                        type: "GET",
                        url: "/api/FrequentFlyer/",
                        //Our search term and what page we are on
                        data: function (term, page) {
                            return {
                                q: term
                            };
                        },
                        results: function (data, page) {
                            return { results: data };
                        }
                    }
                });

        var medsSelect2Options = {
            multiple: false,
            createSearchChoice: function (term, data) {
                if ($(data).filter(function () {
                        return this.text.localeCompare(term) === 0;
                }).length === 0) {
                    return { id: term, text: term };
                }
            },
            placeholder: 'Enter medication name',
            minimumInputLength: 3,
            width: "100%",
            allowClear: true,
            initSelection: function (element, callback) {
                callback({ id: 1, text: $('#aaMedsSelect2').val() });
            },
            data: offlineMedicationsList
        };

        $('.aaMedsSelect2').select2(medsSelect2Options);
        $('.PatientMedicationsSelect2').select2(medsSelect2Options).on("change", function () {
            $timeout(function () {
                if (!$scope.pcr.patientMedications)
                    $scope.pcr.patientMedications = [];

                $scope.pcr.patientMedications.push({
                    medicationName: $('.PatientMedicationsSelect2').select2('data').text
                });

                $('.PatientMedicationsSelect2').select2('data', null);
            });
        });

        //readDemographicsFromList(true);
        /*if ($scope.pcrid != "") {
         //alert("ID PRESENT");
         //$scope.readReportFromList($scope.pcrid);



         //I commented this out cause i dont see a definition
         //ReadPcr($scope.pcrid, $scope.readReportFromList);
         }
         else {

         $scope.readDemographicsFromList(true);
         }*/
        $(".modal").on('show.bs.modal', function () { $scope.continueSubmit = false; });
        $(".modal").on('hide.bs.modal', function () { $scope.continueSubmit = false; });

        /* 
      Attach Spinner to all spinner controls by class
  */
        $timeout(function () {
            $('.spinner').spinner({
                spin: function (event, ui) {
                    var mdlAttr = $(this).attr('data-ng-model').split(".");

                    if (mdlAttr.length > 1) {

                        var objAttr = mdlAttr[mdlAttr.length - 1];
                        var s = $scope[mdlAttr[0]];

                        for (var i = 0; i < mdlAttr.length - 2; i++) {
                            s = s[mdlAttr[i]];
                        }

                        s[objAttr] = ui.value;
                    } else {
                        $scope[mdlAttr[0]] = ui.value;
                    }
                }
            }, 0);

            $('[name="VitalForm_vitalTemp"].spinner').spinner({
                spin: function (event, ui) {
                    $parse($(this).attr('data-ng-model')).assign($scope, ui.value);
                    $scope.$evalAsync($scope.calculateTemp);
                },
                step: .1
            }, 0);
        });
        /* 
            END Attach Spinner to all spinner controls by class
        */

        $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('bFilter', false).withOption('bInfo', false).withOption('bPaginate', false).withOption('bLengthChange', false);

        //// org custom required fields
        //success(DataService.demographics.GetDemo());
        //function success(msg) {
        //    $scope.demo = JSON.parse(msg.DataAsJson);


        //}


        function ValidatePcrForm() {

            $('#Disposition').valid();
            $("#Incident").valid();
            $("#Dispatch1").valid();
            $("#IncidentTiming").valid()
            $("#Delays").valid()
            $("#Location").valid()
            $("#Odometer").valid()
            $("#Crew").valid();

            $("#patientInfoForm").valid();
            $("#Personal").valid();
            $('#MedicalInfoForm').valid();

            $("#PatientComplaints").valid();
            $("#VehicleCollision").valid();
            $("#Trauma").valid();
            $('#NHTSA-form').valid();
            $('#ImpressionForm').valid();

            $("#Assessment2").valid();

            $('#Billing1Form').valid();
            $('#EmployerForm').valid();
            $('#Billing3Form').valid();

            $("#Narrative-form").valid();
            $("#note-form").valid();


            $("#Location").valid();
            $("#Treatment").valid();
            $("#DestinationInfo").valid();
            $("#TransportInformation").valid();
            $("#ReportingInfo").valid();
            //$("#Narrative").valid();
            $("#NewDevice").valid();

            $("#Other1-form").valid();

            $("#SceneAddressForm").valid();
        }

        $(document).ready(function () {
            $("[name='current_Disposition'], [name='pcr_incidentNumber'], [name = pcr_callSign], [name = pcr_crewPrimary], [name = pcr_assessmentOutcome], [name = pcr_typeOfServiceRequested], [name = pcr_incidentDate], [name = pcr_incidentDispatchedTime], [name = pcr_incidentInServiceTime],[name = pcr_cmsLevel], [name = pcr_locationType], [name = pcr_modeToScene], [name = pcr_role], [name = pcr_whoGeneratedThisReport] ")
                .addClass("RequiredRedBorder");
        });

        $scope.NullableCheckBoxChange = function (value, name) {
            //alert(value + "|" + name);
            if (name == "emdContact") {
                $scope.pcr.emdContact = "" + value;
            }
            else if (name == "priorAidBool") {
                $scope.pcr.priorAidBool = "" + value;
            }
            else if (name == "mvc") {
                $scope.pcr.mvc = "" + value;
            }
            else if (name == "cardiacArrest") {
                $scope.pcr.cardiacArrest = "" + value;
            }
            else if (name == "trauma") {
                $scope.pcr.trauma = "" + value;
            }
            else if (name == "billingDifferentThanPatient") {
                $scope.pcr.billingDifferentThanPatient = "" + value;
            }
        };


        $.validator.setDefaults({
            ignore: ""
        });

        $.validator.addMethod("fipsCity", function (value, element, param) {
            var re = /(^\d{5}$)|(\*{3}Not (Applicable)|(Available)|(Known))/;
            return this.optional(element) || re.test(value);
        }, jQuery.format("FIPS City code must be 5 characters"));

        $.validator.addMethod("fipsCounty", function (value, element, param) {
            var re = /(^\d{3}$)|(\*{3}Not (Applicable)|(Available)|(Known))/;
            return this.optional(element) || re.test(value);
        }, jQuery.format("FIPS County code must be 3 characters"));




        jQuery.validator.addMethod("ssn", function (value, element) {
            return this.optional(element) || /^(\d{3})-?\d{2}-?\d{4}$/i.test(value) || value == "***-**-****"
        }, "Invalid Tax ID");


        $.validator.addMethod('RequiredDisposition',
            function (value) {
                return value != "0";
                // return value != "Default";
            }, 'Disposition is required'
            );

        $('#DestinationAddressForm').validate({
            errorLabelContainer: "#validation-placeholder-div34",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + errorList[i].element.getAttribute('data-ng-model') + "' onclick='GoToField(this);'>" + errorList[i].message + " is required." + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#GuardianAddressForm').validate({
            errorLabelContainer: "#validation-placeholder-div33",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + errorList[i].element.getAttribute('data-ng-model') + "' onclick='GoToField(this);'>" + errorList[i].message + " is required." + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#EmployerAddressForm').validate({
            errorLabelContainer: "#validation-placeholder-div32",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + errorList[i].element.getAttribute('data-ng-model') + "' onclick='GoToField(this);'>" + errorList[i].message + " is required." + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#patientAddressForm').validate({
            errorLabelContainer: "#validation-placeholder-div31",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + errorList[i].element.getAttribute('data-ng-model') + "' onclick='GoToField(this);'>" + errorList[i].message + " is required." + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#note-form').validate({
            errorLabelContainer: "#validation-placeholder-div30",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + errorList[i].element.getAttribute('data-ng-model') + "' onclick='GoToField(this);'>" + errorList[i].message + " is required." + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#Other1-form').validate({
            errorLabelContainer: "#validation-placeholder-div29",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + errorList[i].element.getAttribute('data-ng-model') + "' onclick='GoToField(this);'>" + errorList[i].message + " is required." + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#Narrative-form').validate({
            errorLabelContainer: "#validation-placeholder-div28",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + errorList[i].element.getAttribute('data-ng-model') + "' onclick='GoToField(this);'>" + errorList[i].message + " is required." + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#NHTSA-form').validate({
            errorLabelContainer: "#validation-placeholder-div27",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + errorList[i].element.getAttribute('data-ng-model') + "' onclick='GoToField(this);'>" + errorList[i].message + " is required." + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $.validator.addMethod("exactlength", function (value, element, param) {
            return this.optional(element) || value.length == param;
        }, jQuery.format("Please enter exactly {0} characters."));

        $.validator.addMethod("nullablePhoneNumber", function (value, element, param) {
            var re = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
            return this.optional(element) || value == '***Not Known' || value == '***Not Available' || value == '***Not Applicable' || re.test(value) || value == "(***) ***-****";
        }, jQuery.format("Please enter exactly {0} characters."));

        $.validator.addMethod('daterange', function (value, element) {
            if (this.optional(element)) {
                return true;
            }
            var startDate = new Date("1890", "00", "01");
            var endDate = new Date("2030", "11", "31");
            enteredDate = Date.parse(value);
            return ((startDate <= enteredDate) && (enteredDate <= endDate));
        });


        $('#Billing3Form').validate({
            errorLabelContainer: "#validation-placeholder-div25",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            rules: {
                pcr_billingLastName: {
                    minlength: 2//,
                    //  maxlength: 20
                },
                /*      pcr_billingFirstName: {
                          minlength: 1,
                          maxlength: 20
                      },
                      pcr_billingMiddleName: {
                          minlength: 1,
                          maxlength: 20
                      },*/
                pcr_billingPhonenumber: {
                    nullablePhoneNumber: true
                }
            },
            messages: {
                pcr_billingLastName: {
                    minlength: "Guardian Last Name must be at least 2 characters"//,
                    // maxlength: "Guardian Last Name can't be more than 20 characters"
                },
                /*  pcr_billingFirstName: {
                      minlength: "Guardian First Name must be at least 1 character",
                      maxlength: "Guardian First Name can't be more than 20 characters"
                  },
                  pcr_billingMiddleName: {
                      minlength: "Guardian Middle Name must be at least 1 character",
                      maxlength: "Guardian Middle Name can't be more than 20 characters"
                  }, */
                pcr_billingPhonenumber: {
                    nullablePhoneNumber: "Billing Phone must be a 10-digit number"
                }
            }
        });

        $('#EmployerForm').validate({
            errorLabelContainer: "#validation-placeholder-div24",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            rules: {
                pcr_patientEmployer: {
                    minlength: 2//,
                    //    maxlength: 30
                },
                pcr_patientWorkPhone: {
                    nullablePhoneNumber: true
                }
            },
            messages: {
                pcr_patientEmployer: {
                    minlength: "Employer Name must be at least 2 characters"//,
                    //maxlength: "Employer Name cannot be more than 30 characters",
                },
                pcr_patientWorkPhone: {
                    nullablePhoneNumber: "Employer Phone must be a 10-digit number"
                }
            }
        });

        $('#Billing1Form').validate({
            errorLabelContainer: "#validation-placeholder-div23",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#ImpressionForm').validate({
            errorLabelContainer: "#validation-placeholder-div22",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#MedicalInfoForm').validate({
            errorLabelContainer: "#validation-placeholder-div21",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#Disposition').validate({
            errorLabelContainer: "#validation-placeholder-div17",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            rules: {
                current_Disposition: {
                    RequiredDisposition: true
                }
            },
            messages: {
                current_Disposition: {
                    RequiredDisposition: "Disposition must be selected"
                }
            }
        });

        $('#Delays').validate({
            errorLabelContainer: "#validation-placeholder-div18",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            }
        });

        $('#Odometer').validate({
            errorLabelContainer: "#validation-placeholder-div20",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            rules: {
                pcr_odometerStart: {
                    number: true,
                    range: [0, 1000000]
                },
                pcr_odometerScene: {
                    number: true,
                    range: [0, 1000000]
                },
                pcr_odometerDest: {
                    number: true,
                    range: [0.1, 1000000]
                },
                pcr_odometerService: {
                    number: true,
                    range: [0.1, 1000000]
                }
            },
            messages: {
                pcr_odometerStart: {
                    number: "Starting Odometer must be a decimal number with at most one decimal place",
                    range: "Starting Odometer must at least 0 and less than 1,000,000."
                },
                pcr_odometerScene: {
                    number: "Scene Odometer must be a decimal number with at most one decimal place",
                    range: "Scene Odometer must at least 0 and less than 1,000,000."
                },
                pcr_odometerDest: {
                    number: "Destination Odometer must be a decimal number with at most one decimal place",
                    range: "Destination Odometer must at least 0.1 and less than 1,000,000."
                },
                pcr_odometerService: {
                    number: "Service Odometer must be a decimal number with at most one decimal place",
                    range: "Service Odometer must at least 0.1 and less than 1,000,000."
                }
            }
        });

        $("#Incident").validate({
            errorLabelContainer: "#validation-placeholder-div1",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_incidentNumber: {
                    required: true,
                    minlength: 2,
                    maxlength: 15

                },
                pcr_cmsLevel: {
                    required: true,
                },
                pcr_locationType: {
                    required: true,
                }
            },
            messages: {
                pcr_incidentNumber: {
                    required: "Incident Number is required",
                    minlength: "Incident Number must be at least 2 characters",
                    maxlength: "Incident Number may be no more than 15 characters"
                },
                pcr_cmsLevel: {
                    required: "CMS Level is required."
                },
                pcr_locationType: {
                    required: "Location Type is required."
                }
            }
            //// Do not change code below
            //errorPlacement: function (error, element) {
            //    //alert(JSON.stringify(error));
            //    error.insertAfter($("#validation-placeholder").siblings(":last"));
            //    //$("#validation-placeholder").append('<div class="alert alert-info fade in"> <a href="#" data-input-ng-model="' + element.attr('data-ng-model') + '"  onclick="GoToField(this);"><strong>' + element.parent().parent().children().first().html() + '</strong>' + error + '</a></div>');
            //}

        });

        $("#SceneAddressForm").validate({
            errorLabelContainer: "#validation-placeholder-div26",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_SceneAddress_state: {
                    required: true
                },
                pcr_SceneAddress_city: {
                    required: true
                },
                pcr_SceneAddress_fips: {
                    required: true,
                    fipsCity: true
                },
                pcr_SceneAddress_fipsCounty: {
                    required: true,
                    fipsCounty: true
                },
                pcr_SceneAddress_zip: {
                    required: true,
                    regx: /(^(\d{5}(-?\d{4})?)?$)|(\*{3}Not (Applicable)|(Available)|(Known))/
                }
            },
            messages: {
                pcr_SceneAddress_state: {
                    required: "Scene Address: State is a required field"
                },
                pcr_SceneAddress_city: {
                    required: "Scene Address: City is a required field"
                },
                pcr_SceneAddress_fips: {
                    required: "Scene Address: Municipality City Code is a required field",
                    fipsCity: "Scene Address: Must be a 5-digit City FIPS Code"
                },
                pcr_SceneAddress_fipsCounty: {
                    required: "Scene Address: County Code is a required field",
                    fipsCounty: "Scene Address: Must be a 3-digit County FIPS Code"
                },
                pcr_SceneAddress_zip: {
                    regx: "Scene Address Zip Code must contain only digits, and be either 5 or 9 digits long",
                    required: "Scene Address Zip Code is a required field"
                }
            }
        });

        $("#PatientAddressForm").validate({
            errorLabelContainer: "#validation-placeholder-div27",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_PatientAddress_fips: {
                    fipsCity: true
                },
                pcr_PatientAddress_fipsCounty: {
                    fipsCounty: true
                },
                pcr_PatientAddress_zip: {
                    regx: /(^(\d{5}(-?\d{4})?)?$)|(\*{3}Not (Applicable)|(Available)|(Known))|(^$)/
                }
            },
            messages: {
                pcr_PatientAddress_fips: {
                    fipsCity: "Patient Address Fips City Must be a 5-digit City FIPS Code"
                },
                pcr_PatientAddress_fipsCounty: {
                    fipsCounty: "Patient Address Fips County Must be a 3-digit County FIPS Code"
                },
                pcr_PatientAddress_zip: {
                    regx: "Patient Address Zip Code must contain only digits, and be either 5 or 9 digits long"
                }
            }
        });

        $("#GuardianAddressForm").validate({
            errorLabelContainer: "#validation-placeholder-div28",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_GuardianAddress_fips: {
                    fipsCity: true
                },
                pcr_GuardianAddress_fipsCounty: {
                    fipsCounty: true
                },
                pcr_GuardianAddress_zip: {
                    regx: /(^(\d{5}(-?\d{4})?)?$)|(\*{3}Not (Applicable)|(Available)|(Known))|(^$)/
                }
            },
            messages: {
                pcr_GuardianAddress_fips: {
                    fipsCity: "Guardian Address Fips City Must a 5-digit City FIPS Code"
                },
                pcr_GuardianAddress_fipsCounty: {
                    fipsCounty: "Guardian Address Fips County Must a 3-digit County FIPS Code"
                },
                pcr_GuardianAddress_zip: {
                    regx: "Guardian Address Zip Code must contain only digits, and be either 5 or 9 digits long"
                }
            }
        });

        $("#EmployerAddressForm").validate({
            errorLabelContainer: "#validation-placeholder-div29",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_EmployerAddress_fips: {
                    fipsCity: true
                },
                pcr_EmployerAddress_fipsCounty: {
                    fipsCounty: true
                },
                pcr_EmployerAddress_zip: {
                    regx: /(^(\d{5}(-?\d{4})?)?$)|(\*{3}Not (Applicable)|(Available)|(Known))|(^$)/
                }
            },
            messages: {
                pcr_EmployerAddress_fips: {
                    fipsCity: "Employer Address Fips City Must be a 5-digit City FIPS Code"
                },
                pcr_EmployerAddress_fipsCounty: {
                    fipsCounty: "Employer Address Fips County Must be a 3-digit County FIPS Code"
                },
                pcr_EmployerAddress_zip: {
                    regx: "Employer Address Zip Code must contain only digits, and be either 5 or 9 digits long"
                }
            }
        });

        $("#DestinationAddressForm").validate({
            errorLabelContainer: "#validation-placeholder-div30",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_DestinationAddress_fips: {
                    fipsCity: true
                },
                pcr_DestinationAddress_fipsCounty: {
                    fipsCounty: true
                },
                pcr_DestinationAddress_zip: {
                    regx: /(^(\d{5}(-?\d{4})?)?$)|(\*{3}Not (Applicable)|(Available)|(Known))|(^$)/
                }
            },
            messages: {
                pcr_DestinationAddress_fips: {
                    fipsCity: "Destination Address: Must be a 5-digit City FIPS Code"
                },
                pcr_DestinationAddress_fipsCounty: {
                    fipsCounty: "Destination Address: Must be a 3-digit County FIPS Code"
                },
                pcr_DestinationAddress_zip: {
                    regx: "Destination Address Zip Code must contain only digits, and be either 5 or 9 digits long"
                }

            }
            //,
            //wrapper: "div",
            //rules: {
            //    pcr_SceneAddress_state: {
            //        required: true
            //    },
            //    pcr_SceneAddress_fips: {
            //        required: true
            //    },
            //    pcr_SceneAddress_fipsCounty: {
            //        required: true
            //    }
            //},
            //messages: {
            //    pcr_SceneAddress_state: {
            //        required: "Scene Address: State is a required field"
            //    },
            //    pcr_SceneAddress_fips: {
            //        required: "Scene Address: City FIPS code is a required field"
            //    },
            //    pcr_SceneAddress_fipsCounty: {
            //        required: "Scene Address: County FIPS code is a required field"
            //    }
            //}
        });


        $("#Dispatch1").validate({
            errorLabelContainer: "#validation-placeholder-div2",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {
                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_modeToScene: {
                    required: true
                },
                pcr_callSign: {
                    required: true,
                    minlength: 2//,
                    //  maxlength: 15
                },
                pcr_typeOfServiceRequested: {
                    required: true
                },
                pcr_role: {
                    required: true
                },
                pcr_incidentVehicleNumber: {
                    minlength: 2 //,
                    // maxlength: 30
                },
                /*  pcr_incidentVehicleResponseNumber: {
                      minlength: 1,
                      maxlength: 15
                  }, */
                pcr_initialResponderDate: {
                    date: true
                }
            },

            messages: {
                pcr_callSign: {
                    required: "Call Sign is required",
                    minlength: "Call Sign must be at least 2 characters"//,
                    //maxlength: "Call Sign must be no more than 15 characters"
                },
                pcr_typeOfServiceRequested: {
                    required: "Type of Service is required"
                },
                pcr_role: {
                    required: "Role is required"
                },
                pcr_incidentVehicleNumber: {
                    minlength: "Incident Vehicle Number must be no more than 2 characters"//,
                    // maxlength: "Incident Vehicle Number must be no more than 30 characters"
                },
                /*  pcr_incidentVehicleResponseNumber: {
                      minlength: "Incident Vehicle Response Number must be at least 1 characters",
                      maxlength: "Incident Vehicle Response Number must be no more than 15 characters"
                  }, */
                pcr_initialResponderDate: {
                    date: "Responder Date be a valid date"
                },
                pcr_modeToScene: {
                    required: "Mode To Scene field is required"
                }
            }
        });

        $("#IncidentTiming").validate({
            errorLabelContainer: "#validation-placeholder-div3",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_incidentDate: {
                    required: true,
                    date: true
                },
                pcr_incidentDispatchedTime: {
                    required: true
                },
                pcr_incidentInServiceTime: {
                    required: true
                },
            },
            messages: {
                pcr_incidentDate: {
                    required: "Incident Date is required",
                    date: "Incident Date must be a valid date"
                },
                pcr_incidentDispatchedTime: {
                    required: "Dispatch Time is required"
                },
                pcr_incidentInServiceTime: {
                    required: "Available Time is required"
                },
            }
        });

        $("#Crew").validate({
            errorLabelContainer: "#validation-placeholder-div4",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_whoGeneratedThisReport: {
                    required: true
                },
                pcr_crewPrimary: {
                    required: true
                }
            },
            messages: {
                pcr_whoGeneratedThisReport: {
                    required: "Creator is required"
                },
                pcr_crewPrimary: {
                    required: "Primary Crew Member is required"
                }
            }
        });


        $("#patientInfoForm").validate({
            errorLabelContainer: "#validation-placeholder-div5",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                /*  pcr_patientFirstName: {
                      minlength: 1,
                      maxlength: 20
                  }, */
                pcr_patientLastName: {
                    minlength: 2//,
                    // maxlength: 20
                },
                //pcr_patientMiddleName: {
                //    minlength: 1,
                //    maxlength: 20
                //},
                pcr_patientWeight: {
                    number: true
                },
                pcr_patientDOB: {
                    date: true,
                    daterange: true
                },
                pcr_patientPhoneNumber: {
                    nullablePhoneNumber: true
                },
                pcr_patientSsn: {
                    ssn: true
                },
            },
            messages: {
                //pcr_patientFirstName: {
                //    minlength: "Patient First Name must be at least 1 character",
                //    maxlength: "Patient First Name must be no more than 20 characters"
                //},
                pcr_patientLastName: {
                    minlength: "Patient Last Name must be at least 2 characters"//,
                    //  maxlength: "Patient Last Name must be no more than 20 characters"
                },
                //pcr_patientMiddleName: {
                //    minlength: "Patient Middle Name must be at least 1 character",
                //    maxlength: "Patient Middle Name must be no more than 20 characters"
                //},
                pcr_patientWeight: {
                    number: "Patient Weight must be a number"
                },
                pcr_patientDOB: {
                    date: "Enter a valid Patient Date of Birth.",
                    daterange: "A Patient must be born between 1890 and 2030"
                },
                pcr_patientPhoneNumber: {

                    nullablePhoneNumber: "Patient Phone must be a 10-digit number"
                },
                pcr_patientSsn: {
                    ssn: "Social security number must be a valid"

                }
            }
        });

        $("#ImmunizationForm").validate({
            rules: {
                ImmunizationForm_immunizationDate: {
                    required: true,
                    digits: true,
                    range: [1890, 2030]
                },
                ImmunizationForm_immunizationType: {
                    required: true
                }
            },
            messages: {
                ImmunizationForm_immunizationDate: {
                    required: "Year is required",
                    digits: "Please enter in a number for the Year",
                    range: "Immunization Year must be between 1890 and 2030"
                },
                ImmunizationForm_immunizationType: {
                    required: "Type is required"
                }
            }
        });
        $("#Personal").validate({
            errorLabelContainer: "#validation-placeholder-div6",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                //pcr_patientDriverLicenseNumber: {
                //    minlength: 1,
                //    maxlength: 30
                //},
                pcr_practitionerLast: {
                    minlength: 2//,
                    // maxlength: 30,
                }
            },
            messages: {
                //pcr_patientDriverLicenseNumber: {
                //    minlength: "Patient Drivers License Number must be at least 1 character",
                //    maxlength: "Patient Drivers License Number must be no more than 30 characters",
                //},
                pcr_practitionerLast: {
                    minlength: "Patient Practitioner Name must be at least 2 characters"//,
                    //  maxlength: "Patient Practitioner Name must be no more than 30 characters"
                }
            }
        });

        $("#PatientComplaints").validate({
            errorLabelContainer: "#validation-placeholder-div7",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_primaryComplaint: {
                    minlength: 2//,
                    //   maxlength: 50
                },
                pcr_primaryComplaintDuration: {
                    digits: true,
                    range: [1, 360]
                },
                pcr_secondaryComplaint: {
                    minlength: 2//,
                    //maxlength: 50
                },
                pcr_secondaryComplaintDuration: {
                    digits: true,
                    range: [1, 360]
                }
            },
            messages: {
                pcr_primaryComplaint: {
                    minlength: "Primary Complaint must be at least 2 characters"//,
                    //maxlength: "Primary Complaint must be no more than 50 characters"
                },
                pcr_primaryComplaintDuration: {
                    digits: "Primary Complaint Duration must be a number",
                    range: "Primary Complaint Duration must be between 1 and 360"
                },
                pcr_secondaryComplaint: {
                    minlength: "Secondary Complaint must be at least 2 characters"///,
                    //maxlength: "Secondary Complaint must be no more than 50 characters"
                },
                pcr_secondaryComplaintDuration: {
                    digits: "Secondary Complaint Duration must be a number",
                    range: "Secondary Complaint Duration must be between 1 and 360"
                },
            }
        });
        $("#VehicleCollision").validate({
            errorLabelContainer: "#validation-placeholder-div8",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_mvcReportNum: {
                    minlength: 2//,
                    // maxlength: 20
                },
                pcr_mvcRow: {
                    minlength: 1,
                    maxlength: 50,
                    digits: true
                }
            },
            messages: {
                pcr_mvcReportNum: {
                    minlength: "MVC Report Number must be at least 2 characters"//,
                    //maxlength: "MVC Report Number must be no more than 20 characters"
                },
                pcr_mvcRow: {
                    minlength: "MVC Row must be at least 1 character",
                    maxlength: "MVC Row must be no more than 50 characters",
                    digits: "Please enter only numbers"
                },
            }
        });




        $("#Trauma").validate({
            errorLabelContainer: "#validation-placeholder-div9",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_injuryHeightOfFall: {
                    range: [0, 50000],
                    digits: true
                }
            },
            messages: {
                pcr_injuryHeightOfFall: {
                    range: "Height of Fall must be between 0 and 50,000",
                    digits: "Please enter only numbers"
                },
            }
        });



        $("#Assessment2").validate({
            errorLabelContainer: "#validation-placeholder-div10",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_priorAid: {
                    minlength: 2//,
                    // maxlength: 30,
                }
            },
            messages: {
                pcr_priorAid: {
                    minlength: "Prior Aid must be at least 2 characters"//,
                    // maxlength: "Prior Aid must be at least 30 characters"
                },
            }
        });

        $("#VitalsForm").validate({
            rules: {
                VitalForm_vitalTemp: {
                    number: true,
                    range: [90, 110]
                },
                VitalForm_vitalSbp: {
                    digits: true,
                    range: [0, 400]
                },
                VitalForm_vitalDbp: {
                    digits: true,
                    range: [0, 200]
                },
                VitalForm_vitalPulse: {
                    digits: true,
                    range: [0, 500]
                },
                VitalForm_vitalOx: {
                    digits: true,
                    range: [0, 100]
                },
                VitalForm_vitalCo2: {
                    digits: true,
                    range: [0, 100]
                },
                VitalForm_vitalResp: {
                    digits: true,
                    range: [0, 100]
                },
                VitalForm_vitalGcs: {
                    range: [1, 4]
                },
                VitalForm_vitalGcsM: {
                    range: [1, 6]
                },
                VitalForm_vitalGcsV: {
                    range: [1, 5]
                },
                VitalForm_vitalGcsTotal: {
                    range: [1, 15]
                },
                VitalForm_vitalPain: {
                    digits: true,
                    range: [0, 10]
                },
                VitalForm_vitalApgar: {
                    digits: true,
                    range: [0, 10]
                },
                VitalForm_vitalRevisedTrauma: {
                    digits: true,
                    range: [0, 12]
                },
                VitalForm_vitalPediatricTrauma: {
                    digits: true,
                    range: [-6, 12]
                },
                VitalForm_vitalBg: {
                    digits: true,
                    range: [0, 2000]
                },

            },
            messages: {
                VitalForm_vitalTemp: {
                    digits: "Temperature must be a number between 90 and 110",
                    range: "Temperature must be a number between 90 and 110"
                },
                VitalForm_vitalSbp: {
                    digits: "SBP must be a number between 0 and 400",
                    range: "SBP must be a number between 0 and 400"
                },
                VitalForm_vitalDbp: {
                    digits: "DBP must be a number between 0 and 200",
                    range: "DBP must be a number between 0 and 200"
                },
                VitalForm_vitalPulse: {
                    digits: "Pulse must be a number between 0 and 500",
                    range: "Pulse must be a number between 0 and 500"
                },
                VitalForm_vitalOx: {
                    digits: "Ox must be a number between 0 and 100",
                    range: "Ox must be a number between 0 and 100"
                },
                VitalForm_vitalCo2: {
                    digits: "Co2 must be a number between 0 and 100",
                    range: "Co2 must be a number between 0 and 100"
                },
                VitalForm_vitalResp: {
                    digits: "Resp must be a number between 0 and 100",
                    range: "Resp must be a number between 0 and 100"
                },
                VitalForm_vitalGcs: {
                    range: "Glascow Coma Scale must be between 1 and 4"
                },
                VitalForm_vitalGcsM: {
                    range: "Glascow Coma Scale - Motor must be between 1 and 6"
                },
                VitalForm_vitalGcsV: {
                    range: "Glascow Coma Scale - Verbal must be between 1 and 5"
                },
                VitalForm_vitalGcsTotal: {
                    range: "Glascow Coma Scale - Total must be between 1 and 15"
                },
                VitalForm_vitalPain: {
                    digits: "Pain must be a number between 0 and 10",
                    range: "Pain must be a number between 0 and 10"
                },
                VitalForm_vitalApgar: {
                    digits: "Apgar must be a number between 0 and 10",
                    range: "Apgar must be a number between 0 and 10"
                },
                VitalForm_vitalRevisedTrauma: {
                    digits: "Revised Trauma must be a number between 0 and 12",
                    range: "Revised Trauma must be a number between 0 and 12"
                },
                VitalForm_vitalPediatricTrauma: {
                    digits: "Pediatric Trauma must be a number between -6 and 12",
                    range: "Pediatric Trauma must be a number between -6 and 12"
                },
                VitalForm_vitalBg: {
                    digits: "BG must be a number between 0 and 2000",
                    range: "BG must be a number between 0 and 2000"
                },
            }
        });

        $("#TreatMedForm").validate({
            rules: {
                TreatmentMedicationForm_medDosage: {
                    number: true,
                    range: [0.0, 1000000.0]
                },
                TreatmentMedicationForm_medAuthPhysician: {
                    minlength: 3//,
                    //maxlength: 20
                },
                TreatmentMedicationForm_medName: {
                    required: true
                },
                TreatmentMedicationForm_medCrewMemberId: {
                    required: true
                }
            },
            messages: {
                TreatmentMedicationForm_medDosage: {
                    number: "Please enter only numbers",
                    range: "Medication Dosage must contain a value between 0.0 and 1000000.0"
                },
                TreatmentMedicationForm_medAuthPhysician: {
                    minlength: "Auth Physician must be at least 3 characters"//,
                    // maxlength: "Auth Physician must be at most 20 characters"
                },
                TreatmentMedicationForm_medName: {
                    required: "Medication Name is required."
                },
                TreatmentMedicationForm_medCrewMemberId: {
                    required: "Crew Member is required"
                }
            }

        });
        $("#ProcedureForm").validate({
            rules: {
                ProcedureForm_procDate: {
                    date: true,
                },
                ProcedureForm_procProcedure: {
                    required: true//,
                    //  minlength: 0,
                    //  maxlength: 1000
                },
                ProcedureForm_procSizeOfEquipment: {
                    minlength: 2//,
                    //   maxlength: 20
                },
                ProcedureForm_procAuthPhysician: {
                    minlength: 2//,
                    //  maxlength: 20
                },
                ProcedureForm_procCrewMemberId: {
                    required: true
                },
                ProcedureForm_procNumberOfAttempts: {
                    number: true,
                    range: [-25, 100]
                }
            },

            messages: {
                ProcedureForm_procDate: {
                    date: "Procedure Date requires a valid date."
                },
                ProcedureForm_procProcedure: {
                    required: "Procedure Name is required."//,
                    //  minlength: "Procedure requires at least 0 characters",
                    // maxlength: "Procedure may contain no more than 1000 characters"
                },
                ProcedureForm_procSizeOfEquipment: {
                    minlength: "Size of Equipment requires at least 2 characters"//,
                    // maxlength: "Size of Equipment may contain no more than 20 characters"
                },
                ProcedureForm_procAuthPhysician: {
                    minlength: "AuthPhysician must contain at least 2 characters"//,
                    //maxlength: "AuthPhysician may contain no more than 20 characters"
                },
                ProcedureForm_procCrewMemberId: {
                    required: "Crew Member is required."
                },
                ProcedureForm_procNumberOfAttempts: {
                    number: "Number of Attempts must be a number between -25 and 100",
                    range: "Number of Attempts must be a number between -25 and 100"
                }

            }
        });
        $("#Location").validate({
            errorLabelContainer: "#validation-placeholder-div11",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                //pcr_incidentLocationZone: {
                //    minlength: 1,
                //    maxlength: 20
                //},
                pcr_incidentLocationFacilityCode: {
                    minlength: 2//,
                    // maxlength: 30
                },
                //pcr_emdCardNumber: {
                //    minlength: 1,
                //    maxlength: 10
                //},
                pcr_incidentLocationLong: {
                    number: true
                },
                pcr_incidentLocationLat: {
                    number: true
                }
            },
            messages: {
                //pcr_incidentLocationZone: {
                //    minlength: "Location Zone must contain at least 1 character",
                //    maxlength: "Location Zone may contain no more than 20 characters"
                //},
                pcr_incidentLocationFacilityCode: {
                    minlength: "Facility Code must contain at least 2 characters"//,
                    // maxlength: "Facility Code may contain no more than 30 characters"
                },
                //pcr_emdCardNumber: {
                //    minlength: "EMD Card Number Location must contain at least 1 character",
                //    maxlength: "EMD Card Number Location may contain no more than 10 characters"
                //},
                pcr_incidentLocationLong: {
                    number: "Scene Longitude must be a decimal number"
                },
                pcr_incidentLocationLat: {
                    number: "Scene Latitude must be a decimal number"
                }
            }
        });

        $("#Treatment").validate({
            errorLabelContainer: "#validation-placeholder-div12",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_protocols: {
                    minlength: 2//,
                    //   maxlength: 30
                },
            },
            messages: {
                pcr_protocols: {
                    minlength: "Protocols must contain at least 2 characters"//,
                    //  maxlength: "Protocols may contain no more than 30 characters"
                },
            }

        });
        $("#DestinationInfo").validate({
            errorLabelContainer: "#validation-placeholder-div13",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_incidentDestinationCode: {
                    minlength: 2//,
                    //maxlength: 50
                }
            },
            messages: {
                pcr_incidentDestinationCode: {
                    minlength: "Destination Code must contain at least 2 characters"//,
                    //maxlength: "Destination Code may contain no more than 50 characters"
                }
            }

        });
        $("#TransportInformation").validate({
            errorLabelContainer: "#validation-placeholder-div14",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div"

        });
        $("#ReportingInfo").validate({
            errorLabelContainer: "#validation-placeholder-div15",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a data-input-ng-model='" + (errorList[i].element.getAttribute('data-ng-model') || errorList[i].element.getAttribute('ng-model')) + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_traumaRegistryId: {
                    minlength: 2,//
                    // maxlength: 20
                },
                pcr_incidentFireReportNum: {
                    minlength: 2//,
                    // maxlength: 20
                },
                pcr_incidentPatientIdTagNum: {
                    minlength: 2//,
                    //  maxlength: 20
                },
                pcr_facilityMedicalRecordNumber: {
                    minlength: 2//,
                    // maxlength: 30
                },
                //pcr_incidentDestinationZone: {
                //    minlength: 1,
                //    maxlength: 30
                //},
                pcr_incidentDestinationMedRecordNum: {
                    minlength: 2//,
                    // maxlength: 30
                },
                pcr_incidentDestinationLat: {
                    number: true
                },
                pcr_incidentDestinationLong: {
                    number: true
                },
                pcr_vehicleDispatchLat: {
                    number: true
                },
                pcr_vehicleDispatchLong: {
                    number: true
                }
            },
            messages: {
                pcr_traumaRegistryId: {
                    minlength: "Registry ID must contain at least 2 characters"//,
                    //maxlength: "Registry ID may contain no more than 20 characters"
                },
                pcr_incidentFireReportNum: {
                    minlength: "Fire Report Number must contain at least 2 characters"//,
                    // maxlength: "Fire Report Number may contain no more than 20 characters"
                },
                pcr_incidentPatientIdTagNum: {
                    minlength: "Patient ID Tag Number must contain at least 2 characters"//,
                    // maxlength: "Patient ID Tag Number may contain no more than 20 characters"
                },
                pcr_facilityMedicalRecordNumber: {
                    minlength: "Medical Record Number must contain at least 2 characters"//,
                    //maxlength: "Medical Record Number may contain no more than 30 characters"
                },
                //pcr_incidentDestinationZone: {
                //    minlength: "Destination Zone must contain at least 2 characters",
                //    maxlength: "Destination Zone may contain no more than 30 characters"
                //},
                pcr_incidentDestinationMedRecordNum: {
                    minlength: "Medical Record Number must contain at least 2 characters"//,
                    //maxlength: "Medical Record Number may contain no more than 30 characters"
                },
                pcr_incidentDestinationLat: {
                    number: "Destination Latitude must be a decimal value"
                },
                pcr_incidentDestinationLong: {
                    number: "Destination Longitude must be a decimal value"
                },
                pcr_vehicleDispatchLat: {
                    number: "Vehicle Longitude must be a decimal value"
                },
                pcr_vehicleDispatchLong: {
                    number: "Vehicle Longitude must be a decimal value"
                }
            }
        });
        $("#Narrative_TEMP_BREAK").validate({
            errorLabelContainer: "#validation-placeholder-div16",
            errorElement: 'div',
            showErrors: function (errorMap, errorList) {

                for (var i = 0; i < errorList.length; i++) {
                    errorList[i].message = "<div class='alert alert-info'><a  data-input-ng-model='" + errorList[i].element.getAttribute('data-ng-model') + "' onclick='GoToField(this);'>" + errorList[i].message + "</a></div>";
                }
                // alert("error list" + JSON.stringify(errorList));
                this.defaultShowErrors();
            },
            wrapper: "div",
            rules: {
                pcr_narrative: {
                    minlength: 2//,
                    //maxlength: 4000
                },
            },
            messages: {
                pcr_narrative: {
                    minlength: "Narrative must contain at least 2 characters"//,
                    // maxlength: "Narrative may contain no more than 4000 characters"

                }
            }

        });

        $scope.generateNarrative = function () {
            var narrString = $scope.demo2.narrGens[$scope.genForm.narrativeTemplate].narrGen; // get the whole narrative string as defined in demographics

            var counterRegex = /\{\{#\}\}/g; // number replacement regex, for adding in "i" to the gen string
            var tokenRegex = /\{\{([^\{\}]*)\}\}/; // single-value token replacement regex
            var counter = 0;
            while (tokenRegex.test(narrString) && counter++ < 1000) {
                var token = tokenRegex.exec(narrString); // get the token replacement values

                if (tempGenStructures[token[1]]) { // if the token has a special case, execute it

                    var genStruct = tempGenStructures[token[1]]; // find the special case structure
                    var replaceString = "";
                    if (genStruct) {
                        var tokenString = genStruct.tokenString; // get the case's token string
                        if (genStruct.condition == undefined || $scope.$eval(genStruct.condition) == true) { // if no condition exists, or it is true, continue
                            var list = $parse(genStruct.list)($scope); // get the associated list
                            if (genStruct.list != undefined && list) { // if the list exists, loop through it
                                for (var i = 0; i < list.length; i++) {
                                    var thisStr = tokenString.replace(counterRegex, i); // get the token string for each iteration
                                    replaceString += thisStr + "\n";
                                }
                                replaceString = replaceString.substr(0, replaceString.length - 1);
                            } else if (genStruct.list == undefined) {
                                replaceString = tokenString; // if the list does not exist, it is not a one-to-many case, so just use the unaltered token string
                            }
                        }
                    }
                    narrString = narrString.replace(token[0], replaceString); // replace the special case token with the string

                } else { // if the token does not have a special case, just try to replace it with a value
                    var value = $parse(token[1])($scope); // find the value of the token
                    narrString = narrString.replace(token[0], value != null && value != undefined ? value : ""); // replace the token with the value
                }
            };

            $scope.$evalAsync(function () {
                $scope.genForm.gennarrative = narrString; // set the narrative string to the compiled string
            });
        }

        $scope.copyNarrative = function () {
            if (!$scope.pcr.narrative) // handles "", null, and undefined
                $scope.pcr.narrative = $scope.genForm.gennarrative;
            else
                $scope.pcr.narrative += "\n\n" + $scope.genForm.gennarrative;
            $scope.clearNarrGen();
            $scope.$apply();
        };

        $scope.genForm = {
            gennarrative: "",
            narrativeTemplate: -1
        };
        $scope.clearNarrGen = function () {
            $scope.genForm.gennarrative = "";
            $scope.genForm.narrativeTemplate = -1;
        }

        $("#NewDevice").validate({
            rules: {
                DeviceForm_shockEnergy: {
                    range: [2, 9000]
                },
                DeviceForm_numberOfShocks: {
                    range: [1, 100]
                },
                DeviceForm_pacingRate: {
                    range: [1, 1000]
                },
                DeviceForm_heartRate: {
                    range: [1, 500]
                },
                DeviceForm_pulseRate: {
                    range: [1, 500]
                },
                DeviceForm_sbp: {
                    range: [0, 400]
                },
                DeviceForm_dbp: {
                    range: [1, 300]
                },
                DeviceForm_resp: {
                    range: [0, 100]
                },
                DeviceForm_ox: {
                    range: [0, 100]
                },
                DeviceForm_co2: {
                    range: [0, 100]
                },
                DeviceForm_invasivePressureMean: {
                    range: [0, 1000]
                },
                DeviceForm_ecgInterp: {
                    range: [0, 2000]
                },
            },
            messages: {
                DeviceForm_shockEnergy: {
                    range: "Shock Energy must be between 2 and 9000"
                },
                DeviceForm_numberOfShocks: {
                    range: "Number of Shocks must be between 1 and 100"
                },
                DeviceForm_pacingRate: {
                    range: "Pacing Rate must be between 1 and 1000"
                },
                DeviceForm_heartRate: {
                    range: "Heart Rate must be between 1 and 500"
                },
                DeviceForm_pulseRate: {
                    range: "Pulse Rate must be between 1 and 500"
                },
                DeviceForm_sbp: {
                    range: "Systolic Blood Pressure must be between 0 and 400"
                },
                DeviceForm_dbp: {
                    range: "Systolic Blood Pressure must be between 1 and 300"
                },
                DeviceForm_resp: {
                    range: "Respiratory must be between 0 and 100"
                },
                DeviceForm_ox: {
                    range: "Oxygen must be between 0 and 100"
                },
                DeviceForm_co2: {
                    range: "CO2 must be between 0 and 100"
                },
                DeviceForm_invasivePressureMean: {
                    range: "Invasive Pressure Mean must be between 0 and 1000"
                },
                DeviceForm_ecgInterp: {
                    range: "ECG Interp must be between 0 and 2000"
                },
            }

        });

        // modals
        $("#PatientMedication").validate({
            rules: {
                PatientMedicationForm_medicationDosage: {
                    number: true,
                    range: [0, 1000000]
                }
            },
            messages: {
                PatientMedicationForm_medicationDosage: {
                    number: "Medication dosage must be a decimal number",
                    range: "Medication dosage must be between 0 and 1,000,000",
                }
            }
        });

        // exams modal tabs
        $("#ExamHeadNeckForm").validate({});
        $("#ExamChestAbd").validate({});
        $("#ExamExtremeties").validate({});
        $("#ExamBack").validate({});
        $("#ExamNotes").validate({});

        $('InsuranceForm').validate({
            rules: {
                InsuranceForm_insuranceCompany: {
                    minlength: 2//,
                    // maxlength: 30
                },
                InsuranceForm_city: {
                    digits: true,
                    exactlength: 5
                },
                InsuranceForm_zip: {
                    regx: /^(\d{5}(-?\d{4})?)?$/
                },
                InsuranceForm_insuranceGroup: {
                    minlength: 2//,
                    //  maxlength: 30
                },
                InsuranceForm_insurancePolicy: {
                    minlength: 2//,
                    // maxlength: 30
                },
                InsuranceForm_insuranceLastName: {
                    minlength: 2//,
                    //maxlength: 20
                }//,
                //InsuranceForm_insuranceFirstName: {
                //    minlength: 1,
                //    maxlength: 20
                //},
                //InsuranceForm_insuranceMiddleName: {
                //    minlength: 1,
                //    maxlength: 20
                //}

            },
            messages: {
                InsuranceForm_insuranceCompany: {
                    minlength: "Insurance Company must be at least 2 characters"//,
                    // maxlength: "Insurance Company can't be more than 30 characters"
                },
                InsuranceForm_city: {
                    digits: "Insurance City Must be a 5-digit city FIPS Code",
                    exactlength: "Insurance City Must be a 5-digit city FIPS Code"
                },
                InsuranceForm_zip: {
                    regx: "Insurance Address Zip Code must contain only digits, and be either 5 or 9 digits long"
                },
                InsuranceForm_insuranceGroup: {
                    minlength: "Insurance Group must be at least 2 characters"//,
                    //  maxlength: "Insurance Group can't be more than 30 characters"
                },
                InsuranceForm_insurancePolicy: {
                    minlength: "Insurance Policy must be at least 2 characters"//,
                    // maxlength: "Insurance Policy can't be more than 30 characters"
                },
                InsuranceForm_insuranceLastName: {
                    minlength: "Insurance Last Name must be at least 2 characters"//,
                    //maxlength: "Insurance Last Name can't be more than 20 characters"
                }//,
                //InsuranceForm_insuranceFirstName: {
                //    minlength: "Insurance First Name must be at least 1 characters",
                //    maxlength: "Insurance First Name can't be more than 20 characters"
                //},
                //InsuranceForm_insuranceMiddleName: {
                //    minlength: "Insurance Middle Name must be at least 1 characters",
                //    maxlength: "Insurance Middle Name can't be more than 20 characters"
                //}
            }
        });


        $("#patientAddressForm").validate({});
        $("#EmployerAddressForm").validate({});
        $("#GuardianAddressForm").validate({});
        $("#DestinationAddressForm").validate({});

        // END modals

        //$timeout(sync, 1);
        //buildTimeline();



    });

    $scope.attemptOnlineSave = attemptOnlineSave;
    function attemptOnlineSave() {
        $scope.SaveOrUpdateHandler();
    }

    function sync() {
        if ($scope.extraPcrData && $scope.extraPcrData.Status == "0") {
            var promise = DataService.pcr.SyncPcr($scope.pcrid);
            if (promise != null) {
                attemptingSync = true;
                promise.done(function (newPcr) {
                    if (angular.isObject(newPcr)) {
                        if ($scope.pcrid != newPcr.ID) {
                            $scope.pcrid = newPcr.ID;
                            $state.transitionTo('PcrForm', { id: newPcr.ID }, { notify: false });
                        }
                    }
                    attemptingSync = false;
                    $rootScope.app.online = true;
                    $("#StatusCircle").addClass("text-primary");
                    $("#StatusCircle").removeClass("text-warning");
                    DataService.pcr.LastSync = new Date().toDateTime();
                    toastr.success("PCR saved successfully");
                    loadPcrDocs();
                }).fail(function (msg) {
                    attemptingSync = false;
                    $rootScope.app.online = false;
                    $("#StatusCircle").removeClass("text-primary");
                    $("#StatusCircle").addClass("text-warning");
                    //toastr.error(msg);
                });
            }
        }
    }


    var $addressLookupFields = $("[modaltarget$='AddressModal']").find("[data-ng-model$='.city'], [data-ng-model$='.state'], [data-ng-model$='.zip']");
    $addressLookupFields.on("keyup", updateFips);
    function updateFips() {
        if (!allowAutofips) // return if autofips is disabled
            return;
        //$timeout is used for event timing.. Must be delayed so that ng-models have time to update
        $timeout(function () {
            var address = $scope.cache.targetAddress;
            if ($scope.pcr[address].city && $scope.pcr[address].state && $scope.pcr[address].zip) {
                var key = ($scope.pcr[address].city.toUpperCase() + " " + $scope.pcr[address].state.toUpperCase() + " " + $scope.pcr[address].zip.toUpperCase()).trim();
                if (autofillFipsDictionary[key] != undefined) {
                    //toastr.success($filter('json')(autofillFipsDictionary[key]), key);
                    $scope.pcr[address].fips = autofillFipsDictionary[key].city;
                    $scope.pcr[address].fipsCounty = autofillFipsDictionary[key].county;
                    $scope.pcr[address].municipality = "";
                    $("[ng-model='pcr." + address + ".municipality']").select2("val", null);
                }
            }
        });
    }

    //loadPcrDocs();
    function loadPcrDocs() {
        return $.ajax({
            type: "GET",
            url: "/api/PcrDocuments?pcrid=" + $scope.pcrid
        })
        .done(function (msg) {
            $scope.$evalAsync(function () {
                loadedPcrDocs = true;
                $scope.PcrDocuments = msg;
            });
        });
    }

    $scope.removeAttachment = function (id) {
        if (confirm("Are you sure you would like to remove this attachment? It cannot be undone.")) {
            $.ajax({
                type: "DELETE",
                url: "/api/PcrDocuments?id=" + id
            })
            .done(function (msg) {
                loadPcrDocs();
            });
        }
    }

    window.ClonePcr = function () {
        $scope.updatePcrToList();
        var newId = DataService.pcr.Clone($scope.pcrid);
        if (newId) {
            window.openInNewTab("PcrForm?id=" + newId);
        }
    }

    var syncService = setInterval(attemptOnlineSave, 60000);

    var stateChange = $rootScope.$on('$stateChangeSuccess',
    function (event, toState, toParams, fromState, fromParams) {
        if (toState.name != "PcrForm" && fromState.name == "PcrForm") {
            window.ClonePcr = null;
            destroyHandlers();
            $scope.$destroy();
            $scope = null;
        }
    });

    function destroyHandlers() {
        DataService.pcr.LastSync = null;
        $addressLookupFields.off("change", updateFips);
        $(".modal").off('show.bs.modal');
        $(".modal").off('hide.bs.modal');
        saveThenOpenDisplayPageHandler();
        submitHandler();
        saveOrUpdateHandler();
        stateChange();
        clearInterval(syncService);
    }

    $scope.nextTab = function () { toAdjacentTab("next"); }
    $scope.prevTab = function () { toAdjacentTab("prev"); }

    /*
        Changes tabs to the next or previous visible tab
            toTab: String, "next" or "prev"
    */
    function toAdjacentTab(toTab) {
        toTab = toTab.toLowerCase();
        if (toTab != "next" && toTab != "prev")
            return;

        var $currentTab = $("#left-panel li.selected");
        var selector = $rootScope.UI.isMobile ?
            "[ng-show='UI.isMobile']" :
            "[ng-hide='UI.isMobile']";
        var newTab = $currentTab[toTab + "All"]().children("li a" + selector + ":visible").first().attr("name");
        ShowTab('tabs-' + newTab);
        window.scrollTo(0, 0);
        $scope.SaveOrUpdateHandler();
    }

    function loadNotes() {
        $.ajax({
            type: "GET",
            url: "/api/Notes/" + $scope.pcrid
        }).done(function (data) {
            $scope.$evalAsync(function () {
                $scope.pcrNotes = data;
                if (data.length > 0)
                    ShowTab("tabs-Notes");
                //notes = pcrNote.LoadNotes(data); //?
                //pcr.notes = data;//?
            });
        });
    }

    $scope.GenerateIncidentNumber = function () {
        $.ajax({
            type: "POST",
            url: "/api/Utility/IncidentNumber",
            data: {
                "": $scope.pcrid
            }
        })
        .done(function (data) {
            $scope.$evalAsync(function () {
                $scope.pcr.incidentNumber = data;
            })
        })
        .fail(function (msg) {
            toastr.error("Failed To Calculate Incident Number");
        });
    };

    $scope.CreateNote = function () {
        $scope.noteForm = { ID: null, PCRID: $scope.pcrid, CreatedBy: currentUser, Created: new Date().toDateTime(), CreatedFormat: new Date().toDateTime(), Flags: "", Body: "" };
        $timeout(function () {
            $("#NotesModal").modal("show");
            InstantiateSelect2s($("#NotesModal"));
        });
    }

    $scope.LoadNote = function (note, index) {
        $scope.noteForm = { ID: note.ID, PCRID: note.PCRID, CreatedBy: note.CreatedBy, Created: note.Created, CreatedFormat: note.CreatedFormat, Flags: note.Flags, Body: note.Body, index: index };
        $timeout(function () {
            $("#NotesModal").modal("show");
            InstantiateSelect2s($("#NotesModal"));
        });
    }

    $scope.SaveNote = function () {
        if ($scope.noteForm.Body) {
            $("#NotesModal").modal("hide");
            var note = angular.copy($scope.noteForm);
            SaveNoteToServer(note).done(function (newId) {
                $scope.$evalAsync(function () {
                    if (note.ID == null) {
                        $scope.pcrNotes.push(note);
                        note.ID = newId;
                    } else {
                        for (var i = 0; i < $scope.pcrNotes.length; i++) {
                            if ($scope.pcrNotes[i].ID == note.ID) {
                                $scope.pcrNotes.splice(i, 1, note);
                                break;
                            }
                        }
                    }
                    toastr.success("Note Saved");
                });
            });
            $scope.noteForm = { ID: null, PCRID: $scope.pcrid, CreatedBy: "", Created: "", Flags: "", Body: "" };
        }
    }

    $scope.DeleteNote = function (index) {
        var note = $scope.pcrNotes[index];
        DeleteNoteFromServer(note).done(function () {
            $scope.$evalAsync(function () {
                $scope.pcrNotes.splice(index, 1);
                toastr.success("Note Deleted");
            });
        });
    }

















});