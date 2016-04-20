using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Organizations;
using KyleTanczos.TestKyle.Authorization.Roles;
using KyleTanczos.TestKyle.Authorization.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StateId { get; set; }
        public string AgencyCertificationStatus { get; set; }
        public bool IsEmt { get; set; }
        public bool Active { get; set; }
        public bool SendActivationEmail { get; set; }
        public bool SetRandomPassword { get; set; }
        public bool ShouldChangePassword { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }


    }

    public class ManageUsersAppService: TestKyleAppServiceBase, IManageUsersAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IUserEmailer _userEmailer;
        private readonly RoleManager _roleManager;

        public ManageUsersAppService(IRepository<User, long> userRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, IUserEmailer userEmailer, RoleManager roleManager)
        {
            _userRepository = userRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userEmailer = userEmailer;
            _roleManager = roleManager;
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
                            IsEmt = user.IsEmt,
                            Active = user.IsActive,
                            EmailAddress = user.EmailAddress,
                            FirstName = user.Name,
                            LastName = user.Surname
                        };

            //var query = from announcements in db.announcements where announcements.id NOT IN 
            //            (select announcement_id in db.annoncemnts_users where annoncemnts_users.userid == current_id)                     


            var users = await query.ToListAsync();
            return users;
        }

        public async Task<UsersDto> Update(UsersDto userDto)
        {
            if (userDto.Id.HasValue)
            {
               return await UpdateUserAsync(userDto);
            }
            else
            {
                return await CreateUserAsync(userDto);
            }
          
        }

        protected virtual async Task<UsersDto> CreateUserAsync(UsersDto userDto)
        {
            OrganizationUnit org = await GetCurrentOrganizationUnitAsync();
            User user = new User()
            {
                Roles = new List<UserRole>()
            };
            user.UserName = userDto.UserName;
            user.Name = userDto.FirstName;
            user.Surname = userDto.LastName;
            user.StateId = userDto.StateId;
            user.AgencyCertificationStatus = userDto.AgencyCertificationStatus;
            user.IsEmt = userDto.IsEmt;
            user.IsActive = userDto.Active;           
            user.TenantId = AbpSession.TenantId;
            user.EmailAddress = userDto.EmailAddress;
           

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                CheckErrors(await UserManager.PasswordValidator.ValidateAsync(userDto.Password));
            }
            else
            {
                userDto.Password = User.CreateRandomPassword();
            }

            user.Password = new PasswordHasher().HashPassword(userDto.Password);
            user.ShouldChangePasswordOnNextLogin = userDto.ShouldChangePassword;

            //user.Roles = new Collection<UserRole>();
            //foreach (var roleName in input.AssignedRoleNames)
            //{
                var role = await _roleManager.GetRoleByNameAsync("Admin");
                user.Roles.Add(new UserRole { RoleId = role.Id });
            //}

            var result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return null;
            }

            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

            await UserManager.AddToOrganizationUnitAsync(user.Id, org.Id);
          

            if (userDto.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, userDto.Password);
            }

            userDto.Id = user.Id;
            return userDto;
        }

        protected virtual async Task<UsersDto> UpdateUserAsync(UsersDto userDto)
        {
            User user = await UserManager.GetUserByIdAsync(userDto.Id.Value);
            if (user == null)
            {
                throw new ApplicationException("There is no user!");
            }

            user.UserName = userDto.UserName;
            user.Name = userDto.FirstName;
            user.Surname = userDto.LastName;
            user.StateId = userDto.StateId;
            user.AgencyCertificationStatus = userDto.AgencyCertificationStatus;
            user.IsEmt = userDto.IsEmt;
            user.IsActive = userDto.Active;
            user.ShouldChangePasswordOnNextLogin = userDto.ShouldChangePassword;
            user.EmailAddress = userDto.EmailAddress;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                var pwResult = await UserManager.ChangePasswordAsync(user, userDto.Password);
                if (!pwResult.Succeeded)
                {
                    return null;
                }
            }

            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return null;
            }

            if (userDto.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, userDto.Password);
            }

            return userDto;

            //if (!input.User.Password.IsNullOrEmpty())
            //{
            //    CheckErrors(await UserManager.ChangePasswordAsync(user, input.User.Password));
            //}

            // CheckErrors(await UserManager.UpdateAsync(user));

            //Update roles
            // CheckErrors(await UserManager.SetRoles(user, input.AssignedRoleNames));

            //if (userDto.SendActivationEmail)
            //{
            //    user.SetNewEmailConfirmationCode();
            //    await _userEmailer.SendEmailActivationLinkAsync(user, userDto.Password);
            //}
        }


    }
}
