using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entity.Concrete
{
    public class Category
    {
        [BsonId] // Bu alanın veritabanındaki benzersiz anahtar (ID) olduğunu belirtir
        [BsonRepresentation(BsonType.ObjectId)] // MongoDB'deki ObjectId tipini string olarak kullanmamızı sağlar
        public string CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string CategoryIcon { get; set; }

        // Örn: ["RAM", "İşlemci", "Ekran Boyutu"] gibi listeleri burada tutacağız.
        public List<string> AttributeTemplate { get; set; } = new List<string>();
    }
}
