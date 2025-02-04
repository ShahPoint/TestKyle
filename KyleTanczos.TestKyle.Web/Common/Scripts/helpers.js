﻿var app = app || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('TestKyle');
    app.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

    app.downloadTempFile = function (file) {
        location.href = abp.appPath + 'File/DownloadTempFile?fileType=' + file.fileType + '&fileToken=' + file.fileToken + '&fileName=' + file.fileName;
    };

    app.createDateRangePickerOptions = function() {
        var todayAsString = moment().format('YYYY-MM-DD');
        var options = {
            locale: {
                format: 'YYYY-MM-DD',
                applyLabel: app.localize('Apply'),
                cancelLabel: app.localize('Cancel'),
                customRangeLabel: app.localize('CustomRange')
            },
            min: '2015-05-01',
            minDate: '2015-05-01',
            max: todayAsString,
            maxDate: todayAsString,
            ranges: {}
        };

        options.ranges[app.localize('Today')] = [moment().startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('Yesterday')] = [moment().subtract(1, 'days').startOf('day'), moment().subtract(1, 'days').endOf('day')];
        options.ranges[app.localize('Last7Days')] = [moment().subtract(6, 'days').startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('Last30Days')] = [moment().subtract(29, 'days').startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('ThisMonth')] = [moment().startOf('month'), moment().endOf('month')];
        options.ranges[app.localize('LastMonth')] = [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')];

        return options;
    };

})();