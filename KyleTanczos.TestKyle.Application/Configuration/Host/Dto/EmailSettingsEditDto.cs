﻿using Abp.Runtime.Validation;

namespace KyleTanczos.TestKyle.Configuration.Host.Dto
{
    public class EmailSettingsEditDto : IValidate
    {
        //No validation is done, since we may don't want to use email system.

        public string DefaultFromAddress { get; set; }

        public string DefaultFromDisplayName { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUserName { get; set; }

        public string SmtpPassword { get; set; }

        public string SmtpDomain { get; set; }

        public bool SmtpEnableSsl { get; set; }

        public bool SmtpUseDefaultCredentials { get; set; }
    }
}