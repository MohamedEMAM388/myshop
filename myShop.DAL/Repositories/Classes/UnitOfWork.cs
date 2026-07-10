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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [];
        private readonly ApplicationDbContext _applicationDb;

        public UnitOfWork(ApplicationDbContext applicationDb , IProductRepository productRepository)
        {
            _applicationDb = applicationDb;
            ProductRepository = productRepository;
        }

        public IProductRepository ProductRepository { get; }

        public IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : BaseEntity, new()
        {

            // get the type of the entity
            var type = typeof(TEntity);
            // check if the dictionary already contains the type
            if(_repositories.TryGetValue(type, out var repo))
                return (IGenericRepository<TEntity>)repo; // unbox the repository from the dictionary and return it

            // if the repository does not exist, create a new one and add it to the dictionary
            var newRepo = new GenericRepository<TEntity>(_applicationDb);
            // add the new repository to the dictionary
            _repositories.Add(type, newRepo);
            return newRepo;
        }


        public Task<int> SaveChangesAsync()
        {
            return  _applicationDb.SaveChangesAsync();
        }
    }
}
