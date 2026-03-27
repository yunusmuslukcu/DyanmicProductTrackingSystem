using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StockID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } // Hangi ürünün stoğu?

        public int Quantity { get; set; } // Mevcut adet

        public int CriticalLevel { get; set; } = 5; // Dashboard'daki "Kritik Stok" uyarısı için (Varsayılan: 5)
    }
}
