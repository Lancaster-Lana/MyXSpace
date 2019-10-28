using System;
using System.Linq;
using System.Collections.Generic;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Interfaces;

namespace MyXSpace.Core.Services
{
    /// <summary>
    /// TODO:Tenant management logic
    /// </summary>
    public class TenantManager_DB : DomainService, ITenantManager
    {
        private readonly IRepository<Tenant, int> _tenantRepository;

        public TenantManager_DB(IRepository<Tenant, int> tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public IList<Tenant> GetAll()
        {
            return _tenantRepository.GetAll().ToList();
        }

        public Tenant GetById(int id)
        {
            return _tenantRepository.Get(id);
        }
        public Tenant GetByName(string name)
        {
            return _tenantRepository.GetAll().FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Tenant GetByHost(string host)
        {
            var tenant =  _tenantRepository.GetAll().FirstOrDefault(t => t.Host == host);
            return tenant;
        }

        /// <summary>
        /// Create tenant with own admin
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="admin"></param>
        /// <returns></returns>
        public Tenant Register(Tenant tenant, AppUser admin)
        {
            //assign admin of the tenant
            tenant.Admin = admin;

            _tenantRepository.Add(tenant);
            _tenantRepository.SaveChanges();
            //UnitOfWork.SaveChanges();

            return tenant;
        }

        public Tenant Register(Tenant tenant)
        {
            _tenantRepository.Add(tenant);
            _tenantRepository.SaveChanges();
            //UnitOfWork.SaveChanges();

            return tenant;
        }
    }
}