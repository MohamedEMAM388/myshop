using myShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DAL.Repositories.Interfaces
{
    public interface ISpecification<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {

        // pagination 
        public int Skip { get; }
        public int Take { get; }
        public bool IsPaginated { get; }

        // criteria 
        public Expression<Func<TEntity, bool>>? Criteria { get; }

        // sorting 
        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDescending { get; }
    }
}
