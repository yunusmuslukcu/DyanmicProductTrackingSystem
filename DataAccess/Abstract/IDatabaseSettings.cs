using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    //Veri tabanı bağlantımızı interface oalrak yani sözleşme mantığında tanımlıyoruz.
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ProductCollectionName { get; set; }
        public string StockCollectionName { get; set; }
        public string OrderCollectionName { get; set; }
    }
}
