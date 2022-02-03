using BuildBlockCore.Data;
using BuildBlockCore.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data.Repository
{
    public class CustomerRepository : BaseRepository<Models.Customer>, ICustomerRepository
    {
        public CustomerRepository(IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {
        }
    }
}
