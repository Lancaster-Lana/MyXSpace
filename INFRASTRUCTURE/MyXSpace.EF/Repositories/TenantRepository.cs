using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyXSpace.Core.Base;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Repositories.Interfaces;
using MyXSpace.EF;

namespace MyXSpace.Core.Repositories
{
    /// <summary>
    /// Repository with tenatns info
    /// </summary>
    public class TenantRepository : CRepository<Tenant, int>, ITenantRepository
    {
        public TenantRepository(CatalogDbContext context) : base(context)
        { }

        public new IEnumerable<Tenant> GetAll()
        {
            return _appContext.Tenants
                .OrderBy(c => c.Name)
                .ToList();
        }

        private CatalogDbContext _appContext => (CatalogDbContext)_context;
    }
}
