using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductID { get; set; }

        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryID { get; set; } 

        
        // Dictionary kullanarak Key (Özellik Adı) ve Value (Değeri) tutacağız.
        // Örn: "RAM" -> "16GB", "Renk" -> "Siyah"
        public Dictionary<string, string> DynamicAttributes { get; set; } = new Dictionary<string, string>();
    }
}
