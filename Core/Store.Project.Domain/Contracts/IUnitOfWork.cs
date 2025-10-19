using Store.Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<Tkey,TEntity> GetRepository<Tkey, TEntity>() where TEntity :BaseEntity<Tkey>;

        Task<int> SaveChangesAsync();
    }
}
