using Microsoft.EntityFrameworkCore;
using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities;
using Store.Project.Domain.Entities.Products;
using Store.Project.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Persistence.Repositories
{
    public class GenericRepository<TKey, TEntity>(StoreDbContext _context) : IGenericRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool changeTraker = false)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return changeTraker ?
                await _context.Products.Include(P => P.Brand).Include(P => P.Type).ToListAsync() as IEnumerable<TEntity>
                : await _context.Products .Include(P => P.Brand).Include(P => P.Type).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;

            }
            
            return changeTraker ?
                await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().AsNoTracking().ToListAsync();
       }
        public async Task<TEntity?> GetAsync(TKey key)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Type)
                    .FirstOrDefaultAsync(p => p.Id.Equals(key)) as TEntity;
            }

            return await _context.Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id.Equals(key));
        }

        public async Task AddAsync(TEntity entity)
        {
           await _context.AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
           _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TKey, TEntity> spec, bool changeTraker = false)
        {
          return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(ISpecifications<TKey, TEntity> spec)
        {
           return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<TKey, TEntity> spec)
        {
          return await ApplySpecifications(spec).CountAsync();
        }

        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TKey,TEntity> spec)
        {
           return  SpecificationsEvaluator.GetQuery(_context.Set<TEntity>(), spec);
        }

        
    }
}
