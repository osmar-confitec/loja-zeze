using BuildBlockCore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Data
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        protected DbContext _dbContext;

        protected BaseUnitOfWork(DbContext dbContext)
        {

            _dbContext = dbContext;

        }
        public async Task<bool> CommitAsync() => await _dbContext.SaveChangesAsync() > 0;

        public void Dispose() => GC.SuppressFinalize(this);

        public DbContext GetContext() => _dbContext;

        public IRepositoryConsult<T> GetRepository<T>() where T : class => new RepositoryConsult<T>(GetContext());

    }
}
