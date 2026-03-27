using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class StockManager : GenericManager<Stock>, IStockService
    {
        private readonly IGenericRepository<Stock> _stockRepository;

        public StockManager(IGenericRepository<Stock> stockRepository) : base(stockRepository)
        {
            _stockRepository = stockRepository;
        }
    }
}
