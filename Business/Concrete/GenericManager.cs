using Business.Abstract;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericManager(IGenericRepository<T> repository)
        {
            _repository = repository;
        }
        public virtual async Task TCreateAsync(T entity) => await _repository.CreateAsync(entity);

        public virtual async Task TDeleteAsync(string id) => await _repository.DeleteAsync(id);

        public virtual async Task<List<T>> TGetAllAsync() => await _repository.GetAllAsync();

        public virtual async Task<T> TGetByIdAsync(string id) => await _repository.GetByIdAsync(id);

        public virtual async Task TUpdateAsync(string id, T entity) => await _repository.UpdateAsync(id, entity);

        public virtual async Task<List<T>> TGetByFilterAsync(Expression<Func<T, bool>> filter) => await _repository.GetByFilterAsync(filter);
    }
}
