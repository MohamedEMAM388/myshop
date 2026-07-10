using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using myshop.DataAccess;
using myShop.DAL.Models;
using myShop.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
                                   where TEntity : BaseEntity, new()
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GenericRepository(ApplicationDbContext applicationDbContext) 
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _applicationDbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
             _applicationDbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
           return await _applicationDbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
          return await  _applicationDbContext.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity)
        {
          _applicationDbContext.Set<TEntity>().Update(entity);
        }
    }
}
