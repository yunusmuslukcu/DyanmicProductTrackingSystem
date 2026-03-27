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
    public class CategoryRepository : GenericRepository<Category>, ICategoryDal
    {
        public CategoryRepository(IDatabaseSettings settings) : base(settings)
        {
        }
    }
}
