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
    public class StockRepository : GenericRepository<Stock>,IStockDal
    {
        public StockRepository(IDatabaseSettings settings) : base(settings)
        {
        }
    }
}
