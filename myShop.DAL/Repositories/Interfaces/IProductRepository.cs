using myshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product , int>
    {

        IQueryable<Product> GetAllWithLoadedData();
    }
}
