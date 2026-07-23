using myShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity , TKey> where TEntity : BaseEntity<TKey> , new() 
    {
        // get all data 
       Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllWithSpecification
               (ISpecification<TEntity, TKey> specification);

        // get by id 
        Task<TEntity?> GetByIdAsync(int id);

        //add
        Task AddAsync(TEntity entity);
        // update
        void Update(TEntity entity);
        // delete
        void Delete(TEntity entity);

        Task<int> CountAsync();


    }
}
