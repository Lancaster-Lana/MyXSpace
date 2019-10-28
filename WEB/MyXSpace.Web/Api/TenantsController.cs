using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyXSpace.AppServices.MultiTenancy;
using MyXSpace.AppServices.Sessions;

namespace MyXSpace.Web.Controllers
{
    //[Authorize(PermissionNames.Pages_Tenants)]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TenantsController : Controller
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly ISessionAppService _sessionAppService;

        public TenantsController(
            ITenantAppService tenantAppService,
            ISessionAppService sessionAppService)
        {
            _tenantAppService = tenantAppService;
            _sessionAppService = sessionAppService;
        }

        [HttpGet]
        //[Route("Current")]
        //ProducesResponseType(typeof(Tenant), StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        //public Tenant GetCurrentTenant()
        //{
        //    var tenant = _sessionAppService.GetCurrentTenant();
        //    return tenant;
        //}

        public IActionResult GetCurrentTenant()
        {
            var tenant = _sessionAppService.GetCurrentTenant();
            return Ok(tenant);
        }

        [HttpGet]
        [Route("list")]
        //[Authorize(PermissionNames.Pages_Tenants)]        
        public IActionResult GetAll()
        {
            var list = _tenantAppService.GetTenants();

            return Ok(list);
        }
    }
}