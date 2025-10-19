using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities;
using Store.Project.Persistence.Data.Contexts;
using Store.Project.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Persistence
{
    public class UnitOfWork(StoreDbContext _context) : IUnitOfWork
    {
        private Dictionary<string, object> _repository = new Dictionary<string, object>();
        public IGenericRepository<Tkey, TEntity> GetRepository<Tkey, TEntity>() where TEntity : BaseEntity<Tkey>
        {
            var key = typeof(TEntity).Name;
            if (_repository.ContainsKey(key)) 
            { 
                var repository = new GenericRepository<Tkey,TEntity>(_context);
                _repository.Add(key, repository); 
            }
            return (IGenericRepository<Tkey,TEntity>) _repository[key];

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
