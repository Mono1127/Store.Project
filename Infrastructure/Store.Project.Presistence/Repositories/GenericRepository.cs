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
            return await _context.Products.Include(P => P.Brand).Include(P => P.Type).FirstOrDefaultAsync(P => P.Id == key as int?) as TEntity;
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
        //

      
    }
}
