using System.Collections.Generic;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Interfaces;

namespace MyXSpace.Core.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer, int>
    {
        IEnumerable<Customer> GetTopActiveCustomers(int count);
        IEnumerable<Customer> GetAll();
    }
}
