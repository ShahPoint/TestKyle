(function () {
    appModule.controller('common.views.layout.header', [
        '$rootScope', '$scope', '$uibModal', 'appSession',
        function ($rootScope, $scope, $uibModal, appSession) {
            var vm = this;

            $scope.$on('$includeContentLoaded', function () {
                Layout.initHeader(); // init header
            });

            vm.languages = abp.localization.languages;
            vm.currentLanguage = abp.localization.currentLanguage;
            vm.isImpersonatedLogin = abp.session.impersonatorUserId;

            vm.getShownUserName = function () {
                if (!abp.multiTenancy.isEnabled) {
                    return appSession.user.userName;
                } else {
                    if (appSession.tenant) {
                        return appSession.tenant.tenancyName + '\\' + appSession.user.userName;
                    } else {
                        return '.\\' + appSession.user.userName;
                    }
                }
            };

            vm.editMySettings = function () {
                $uibModal.open({
                    templateUrl: '~/App/common/views/profile/mySettingsModal.cshtml',
                    controller: 'common.views.profile.mySettingsModal as vm',
                    backdrop: 'static'
                });
            };

            vm.changePassword = function () {
                $uibModal.open({
                    templateUrl: '~/App/common/views/profile/changePassword.cshtml',
                    controller: 'common.views.profile.changePassword as vm',
                    backdrop: 'static'
                });
            };

            vm.changePicture = function() {
                $uibModal.open({
                    templateUrl: '~/App/common/views/profile/changePicture.cshtml',
                    controller: 'common.views.profile.changePicture as vm',
                    backdrop: 'static'
                });
            };

            vm.changeLanguage = function(languageName) {
                location.href = abp.appPath + 'AbpLocalization/ChangeCulture?cultureName=' + languageName + '&returnUrl=' + window.location.href;
            };

            vm.backToMyAccount = function () {
                abp.ajax({
                    url: abp.appPath + 'Account/BackToImpersonator'
                });
            }
        }
    ]);
})();