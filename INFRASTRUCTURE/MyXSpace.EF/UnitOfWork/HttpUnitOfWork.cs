
using Microsoft.AspNetCore.Http;
using AspNet.Security.OpenIdConnect.Primitives;

namespace MyXSpace.EF
{
    public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(TenantDbContext context, IHttpContextAccessor httpAccessor) : base(context)
        {
            //context.CurrentUserId = httpAccessor.HttpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();

            //var tenantId = httpAccessor.HttpContext.User.FindFirst("TenantId")?.Value;
        }
    }
}
