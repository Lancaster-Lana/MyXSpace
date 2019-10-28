
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Services;

namespace MyXSpace.AppServices
{
    public class UserAppService : AppServiceBase, IUserAppService
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly TenantManager _tenantManager;

        public UserAppService(
            TenantManager tenantManager,
            IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager): base(tenantManager, userManager, httpContextAccessor)
        {
            //SESSION accessor
            //_httpContextAccessor = httpContextAccessor;
            //_tenantManager = tenantManager;
        }

        //public async Task CreateUser(CreateUserInput input)
        //{
        //    var context = _httpContextAccessor.HttpContext;

        //    user.TenantId = context.TenantId;
        //    user.Password = _passwordHasher.HashPassword(user, input.Password);
        //    user.IsEmailConfirmed = true;

        //    CheckErrors(await UserManager.CreateAsync(user));
        //}
    }

    /*[AbpAuthorize(PermissionNames.Users)]
    public class UserAppService : AppServicesAppServiceBase, IUserAppService
    {
        private readonly IRepository<ApplicationUser, long> _userRepository;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UserAppService(
            IRepository<ApplicationUser, long> userRepository, 
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var permission = PermissionManager.GetPermission(input.PermissionName);

            await UserManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            CheckErrors(await UserManager.RemoveFromRoleAsync(user, roleName));
        }

        public async Task<ListResultDto<UserListDto>> GetUsers()
        {
            var users = await _userRepository.GetAllListAsync();

            return new ListResultDto<UserListDto>(
                ObjectMapper.Map<List<UserListDto>>(users) );
        }

        public async Task CreateUser(CreateUserInput input)
        {
            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.Password = _passwordHasher.HashPassword(user, input.Password);
            user.IsEmailConfirmed = true;

            CheckErrors(await UserManager.CreateAsync(user));
        }
    }*/
}