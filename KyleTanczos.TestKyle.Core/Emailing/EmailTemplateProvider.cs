﻿using System.Reflection;
using System.Text;
using Abp.Dependency;
using Abp.IO.Extensions;

namespace KyleTanczos.TestKyle.Emailing
{
    public class EmailTemplateProvider : IEmailTemplateProvider, ITransientDependency
    {
        public string GetDefaultTemplate()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KyleTanczos.TestKyle.Emailing.EmailTemplates.default.html"))
            {
                var bytes = stream.GetAllBytes();
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            }
        }
    }
}