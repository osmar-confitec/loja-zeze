using BuildBlockCore.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data.Repository
{
    public interface ICustomerRepository : IBaseRepository<Models.Customer>
    {
    }
}
