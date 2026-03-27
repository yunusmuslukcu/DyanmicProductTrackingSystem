using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entity.Concrete
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } // Satılan ürün

        public string CustomerName { get; set; } // Müşteri adı (Dashboard için lazım olur)

        public int Quantity { get; set; } // Kaç adet satıldı?

        public decimal TotalPrice { get; set; } // Satış anındaki toplam tutar

        public DateTime OrderDate { get; set; } = DateTime.Now; // Sipariş zamanı
    }
}