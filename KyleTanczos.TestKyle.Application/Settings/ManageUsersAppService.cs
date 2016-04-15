using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Organizations;
using KyleTanczos.TestKyle.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyleTanczos.TestKyle.Settings
{

    public class UsersDto
    {
        public long? Id { get; set; }
        public string UserName { get; set; }
        public string StateId { get; set; }
        public string AgencyCertificationStatus { get; set; }
        public bool IsEmt { get; set; }

    }

    public class ManageUsersAppService: TestKyleAppServiceBase, IManageUsersAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;

        public ManageUsersAppService(IRepository<User, long> userRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository)
        {
            _userRepository = userRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
        }

        public async Task<List<UsersDto>> Get()
        {
            OrganizationUnit org = await GetCurrentOrganizationUnitAsync();
            var query = from uou in _userOrganizationUnitRepository.GetAll()
                        join user in UserManager.Users on uou.UserId equals user.Id
                        where uou.OrganizationUnitId == org.Id
                        select new UsersDto() {
                            Id = user.Id,
                            UserName = user.UserName,
                            StateId = user.StateId,
                            AgencyCertificationStatus = user.AgencyCertificationStatus,
                            IsEmt = user.IsEmt
                        };

            var query = from announcements in db.announcements where announcements.id NOT IN 
                        (select announcement_id in db.annoncemnts_users where annoncemnts_users.userid == current_id)                     


            var users = await query.ToListAsync();
            return users;
        }

        public async Task<UsersDto> Update(UsersDto userDto)
        {
           User user = await UserManager.GetUserByIdAsync(userDto.Id.Value);
           if (user == null)
           {
                throw new ApplicationException("There is no user!");
           }

            user.UserName = userDto.UserName;
            user.StateId = userDto.StateId;
            user.AgencyCertificationStatus = userDto.AgencyCertificationStatus;
            user.IsEmt = userDto.IsEmt;

            var result = await UserManager.UpdateAsync(user);
            
            if (result.Succeeded)
            {
                return userDto;
            }
            else
            {
                return null;
            }
        }
    }
}
