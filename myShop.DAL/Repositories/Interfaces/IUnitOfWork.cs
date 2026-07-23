using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using myShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // Define properties for each repository 
        IGenericRepository<TEntity , TKey> GetGenericRepository<TEntity , TKey>() where TEntity : BaseEntity<TKey>, new();

        IProductRepository ProductRepository { get; }

        // save changes to the database
        Task<int> SaveChangesAsync();
    }
}
