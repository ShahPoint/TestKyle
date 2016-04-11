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
    public interface IManageUsersAppService : IApplicationService
    {
        Task<List<UsersDto>> Get();

        //Task<List<StationsDto>> CreateOrUpdateStations(List<StationsDto> stationsDto);
        Task<UsersDto> Update(UsersDto usersDto);

    }
}
