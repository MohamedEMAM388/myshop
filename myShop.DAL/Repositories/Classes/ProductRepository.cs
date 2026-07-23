using Microsoft.EntityFrameworkCore;
using myshop.DataAccess;
using myshop.Models;
using myShop.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DAL.Repositories.Classes
{
    public class ProductRepository : GenericRepository<Product> , IProductRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public ProductRepository(ApplicationDbContext applicationDb) : base(applicationDb) 
        {
            _applicationDb = applicationDb;
        }
        public IQueryable<Product> GetAllWithLoadedData()
        {
            return  _applicationDb.Products.Include(x => x.Category);
        }
    }
}
