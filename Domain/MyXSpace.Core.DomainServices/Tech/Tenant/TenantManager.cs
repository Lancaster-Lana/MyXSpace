using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Services;

namespace MyXSpace.Core.Services
{
    public class ConfigSectionNames
    {
        public const string TENANTS  = "Tenants";
    }
       
    /// <summary>
    /// Tenant manager - gets TENANTS from config
    /// </summary>
    public class TenantManager : ITenantManager
    {       
        /// <summary>
        /// Application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Tenants list.
        /// </summary>
        private IEnumerable<Tenant> _tenants;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public TenantManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets or sets a collection containg all the tenants.
        /// </summary>
        /// <value>The tenants.</value>
        public IEnumerable<Tenant> Tenants
        {
            get
            {
                if (_tenants == null || !_tenants.Any())
                {

                    var result = new List<Tenant>();

                     _configuration.GetSection(ConfigSectionNames.TENANTS).Bind(result); //var sections = GetValue<List<Tenant>>("Multitenancy");//_configuration.GetValue<List<Tenant>>(ConfigSectionNames.TENANTS)

                    _tenants = result;
                }
                return _tenants;
            }
        }

        public IList<Tenant> GetAll()
        {
            return Tenants.ToList();
        }

        public Tenant GetByHost(string host)
        {
            return Tenants.SingleOrDefault(t => t.Host == host);
        }

        public Tenant GetById(int id)
        {
            return Tenants.SingleOrDefault(t => t.Id == id);
        }

        public Tenant GetByName(string tenantName)
        {
            return Tenants.SingleOrDefault(t => t.Name == tenantName);
        }

        public Tenant Register(Tenant tenant, AppUser admin)
        {
            throw new System.NotImplementedException();
        }

        public Tenant Register(Tenant tenant)
        {
            throw new System.NotImplementedException();
        }
    }
}