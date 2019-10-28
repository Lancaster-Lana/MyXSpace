namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// Tenant Entity - to save host information of the customer
    /// Tenant hosted mainly in a separate domain (with own DB, AD, security boundaries)
    /// Customer(company) is an owner of a tenant
    /// </summary>
    public class Tenant : Entity<int>
    {
        public string Name { get; set; }

        /// <summary>
        /// Name of the customer brand : MyAdsearch, MySigmar, ...
        /// </summary>
        //[typeof(BrandName)]
        public string BrandName { get; set; }

        public string Host { get; set; }

        private string Theme { get; set; }

        /// <summary>
        /// DB Connection string for specific tenant
        /// </summary>
        public string ConnectionString { get; set; }

        public bool IsActive { get; set; } = true;

        //[FK]
        public string AdminId { get; set; }

        /// <summary>
        /// Tenant admin
        /// </summary>
        public AppUser Admin { get; set; }

        //FK to Customer (see FluentAPI in )
        //public int CustomerId { get; set; }
        /// <summary>
        /// Customer - owner (admin) of the tenant 
        /// </summary>
        //public virtual Customer Customer { get; set; }

        public string Email { get; set; }

        public Tenant()
        {
        }

        public Tenant(string name)
        {
            Name = name;
            BrandName = name; //the same as Tenant assosiated with single brand
        }

        public Tenant(string name, string host): this(name)
        {
            Host = host;
        }

        public Tenant(string name, string host, string connectionString) : this(name, host)
        {
            ConnectionString = connectionString;
        }

        public Tenant(string name, string host, string connectionString, AppUser admin) : this(name, host, connectionString)
        {
            Admin = admin;
        } 
    }
}
