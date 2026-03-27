using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    [CollectionName("Users")]
    public class AppUser : MongoIdentityUser<Guid>
    {
        // Standart Identity özelliklerine ek olarak istediğin alanları ekleyebilirsin
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
