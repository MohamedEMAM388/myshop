using myShop.DAL.Models;
using myShop.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Specifications
{
    public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        public BaseSpecification(Expression<Func<TEntity , bool>> criteria) 
        {

            Criteria = criteria;
        
        }

        public BaseSpecification() 
        {
        
        }

        public  Expression<Func<TEntity, bool>>? Criteria { get; protected set; }

        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderby) {

            OrderBy = orderby;
        }


        protected void AddOrderByDescinding(Expression<Func<TEntity, object>> orderbydescinding) {

            OrderByDescending = orderbydescinding;
        }
        #region Pagination
        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool IsPaginated { get; private set; }

        protected void ApplyPagination(int pagesize, int pageindex)
        {

            IsPaginated = true;
            Skip = (pageindex - 1) * pagesize;
            Take = pagesize;
        } 
        #endregion
    }
}
