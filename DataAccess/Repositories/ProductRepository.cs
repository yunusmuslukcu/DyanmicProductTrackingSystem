using DataAccess.Abstract;
using DataAccessLayer.Repositories;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductDal
    {
        public ProductRepository(IDatabaseSettings settings) : base(settings)
        {
        }
    }
}
