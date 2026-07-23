using myShop.DAL.Models;
using myShop.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DAL
{
    public class SpecificationEvaluator
    {

        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>
             (IQueryable<TEntity> entryPoint, ISpecification<TEntity, TKey> specification)
             where TEntity : BaseEntity<TKey>
        {

            var Query = entryPoint;

            // check if specification not null
            if (specification is not null) 
            {

                if (specification.IsPaginated) { 
                
                    Query = Query.Skip(specification.Skip).Take(specification.Take);
                }
            
            }

            return Query;
        
        }
    }
}
