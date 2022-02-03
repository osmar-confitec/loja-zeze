using BuildBlockCore.Data;
using CustomerApi.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data.Uow
{
    public class UnitOfWorkCustomer : BaseUnitOfWork
    {
        public UnitOfWorkCustomer(CustomerContext dbContext) : base(dbContext)
        {
        }
    }
}
