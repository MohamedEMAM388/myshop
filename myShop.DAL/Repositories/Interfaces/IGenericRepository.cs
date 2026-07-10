using myShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new() 
    {
        // get all data 
       Task<IEnumerable<TEntity>> GetAllAsync();

        // get by id 
        Task<TEntity?> GetByIdAsync(int id);

        //add
        Task AddAsync(TEntity entity);
        // update
        void Update(TEntity entity);
        // delete
        void Delete(TEntity entity);


    }
}
