(function($) {
    $(function () {

        //Back to my account

        $('#UserProfileBackToMyAccountButton').click(function(e) {
            e.preventDefault();
            abp.ajax({
                url: abp.appPath + 'Account/BackToImpersonator'
            });
        });

        //My settings

        var mySettingsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Profile/MySettingsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Profile/_MySettingsModal.js',
            modalClass: 'MySettingsModal'
        });

        $('#UserProfileMySettingsLink').click(function (e) {
            e.preventDefault();
            mySettingsModal.open();
        });

        //Change password

        var changePasswordModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Profile/ChangePasswordModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Profile/_ChangePasswordModal.js',
            modalClass: 'ChangePasswordModal'
        });

        $('#UserProfileChangePasswordLink').click(function(e) {
            e.preventDefault();
            changePasswordModal.open();
        });

        //Change profile picture

        var changeProfilePictureModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Profile/ChangePictureModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Profile/_ChangePictureModal.js',
            modalClass: 'ChangeProfilePictureModal'
        });

        $('#UserProfileChangePictureLink').click(function (e) {
            e.preventDefault();
            changeProfilePictureModal.open();
        });

    });
})(jQuery);