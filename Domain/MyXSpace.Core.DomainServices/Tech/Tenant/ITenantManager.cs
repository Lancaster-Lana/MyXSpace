using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

using MyXSpace.Core.Entities;
using System.Threading.Tasks;

namespace MyXSpace.Core.Services
{
    public interface ITenantManager
    {
        /// <summary>
        /// Registers a new <see cref="Tenant"/> in the application.
        /// </summary>
        /// <param name="tenant">Tenant to register.</param>
        /// <param name="admin">Admin user of the <see cref="Tenant"/>.</param>
        /// <returns></returns>
        Tenant Register(Tenant tenant, AppUser admin);

        /// <summary>
        /// Registers a new <see cref="Tenant"/> with default admin
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        Tenant Register(Tenant tenant);

        /// <summary>
        /// Gets a <see cref="Tenant"/> by the given id.
        /// </summary>
        /// <param name="id">The id of the tenant to get.</param>
        /// <returns></returns>
        Tenant GetById(int id);

        Tenant GetByName(string tenantName);

        /// <summary>
        /// Get tenant by host address
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        Tenant GetByHost(string host);

        /// <summary>
        /// Gets a collection of all the tenannts in the system.
        /// </summary>
        /// <returns></returns> 
        IList<Tenant> GetAll();
    }
}