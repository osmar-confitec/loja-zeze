using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Data.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();

        DbContext GetContext();

        IRepositoryConsult<T> GetRepository<T>() where T : class;
    }
}
