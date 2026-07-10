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
        IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : BaseEntity, new();

        IProductRepository ProductRepository { get; }

        // save changes to the database
        Task<int> SaveChangesAsync();
    }
}
