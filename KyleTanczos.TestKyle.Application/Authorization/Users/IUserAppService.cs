﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using KyleTanczos.TestKyle.Authorization.Users.Dto;
using KyleTanczos.TestKyle.Dto;

namespace KyleTanczos.TestKyle.Authorization.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<PagedResultOutput<UserListDto>> GetUsers(GetUsersInput input);

        Task<FileDto> GetUsersToExcel();

        Task<GetUserForEditOutput> GetUserForEdit(NullableIdInput<long> input);

        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(IdInput<long> input);

        Task ResetUserSpecificPermissions(IdInput<long> input);

        Task UpdateUserPermissions(UpdateUserPermissionsInput input);

        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        Task DeleteUser(IdInput<long> input);
    }
}