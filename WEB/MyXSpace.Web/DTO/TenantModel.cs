
namespace MyXSpace.WebSPA.DTO
{
    public class TenantModel
    {
        /// <summary>
        ///  Name of the customer brand : MyAdsearch, MySigmar, ...
        /// </summary>
        public string Name { get; set; }

        public string Host { get; set; }

        private string Theme { get; set; }

        /// <summary>
        /// DB Connection string for specific tenant
        /// </summary>
        public string ConnectionString { get; set; }

        public bool IsActive { get; set; } = true;
    }
}