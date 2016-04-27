using Abp.Application.Services;
using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyleTanczos.TestKyle.Settings
{
    [DontWrapResult]
    public interface IAgencyInfoAppService : IApplicationService
    {
        AgencyInfoDto Get();

        Task<AgencyInfoDto> Update(AgencyInfoDto agencyInfoDto);
    }
}
