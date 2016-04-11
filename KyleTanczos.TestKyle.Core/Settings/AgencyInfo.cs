using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using KyleTanczos.TestKyle.Authorization.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyleTanczos.TestKyle.Settings
{
    public class AgencyInfo : FullAuditedEntity, IMustHaveOrganizationUnit
    {
        public virtual string OptionsAsJson { get; set; }
        public virtual long OrganizationUnitId { get; set; }
        [JsonIgnore]
        public virtual OrganizationUnit OrganizationUnit { get; set; }
    }
}
