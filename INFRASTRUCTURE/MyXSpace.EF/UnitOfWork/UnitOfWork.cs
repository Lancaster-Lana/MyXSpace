
namespace MyXSpace.EF
{
    /// <summary>
    /// TODO: can be choosen some framework
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        readonly TenantDbContext _context;

        //ICustomerRepository _customers;

        public UnitOfWork(TenantDbContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
