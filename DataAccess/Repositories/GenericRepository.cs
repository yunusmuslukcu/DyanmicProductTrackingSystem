using DataAccess.Abstract;
using MongoDB.Bson; // ObjectId dönüşümü için gerekli
using MongoDB.Driver;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public GenericRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            // Koleksiyon adını T tipinin sonuna 's' ekleyerek alıyoruz (Örn: Categorys)
            _collection = database.GetCollection<T>(typeof(T).Name + "s");
        }

        public virtual async Task CreateAsync(T entity) => await _collection.InsertOneAsync(entity);

        public virtual async Task<List<T>> GetAllAsync() => await _collection.Find(_ => true).ToListAsync();

        public virtual async Task<T> GetByIdAsync(string id)
        {
            // MongoDB'de _id alanı ObjectId tipindedir, string'i ObjectId'ye çevirip arıyoruz.
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task UpdateAsync(string id, T entity)
        {
            // Güncelleme yaparken de yine ObjectId dönüşümü şart.
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public virtual async Task DeleteAsync(string id)
        {
            // İnatçı silme hatasının çözümü: 
            // 1. Alan adını "_id" yaptık.
            // 2. Gelen string'i ObjectId.Parse ile MongoDB'nin anlayacağı tipe çevirdik.
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.DeleteOneAsync(filter);
        }

        public virtual async Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> filter) =>
            await _collection.Find(filter).ToListAsync();
    }
}