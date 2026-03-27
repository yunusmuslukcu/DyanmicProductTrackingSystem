using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGenericService<T> where T:class
    {
        Task TCreateAsync(T entity);
        Task TUpdateAsync(string id, T entity);
        Task TDeleteAsync(string id);
        Task<List<T>> TGetAllAsync();
        Task<T> TGetByIdAsync(string id);
        Task<List<T>> TGetByFilterAsync(Expression<Func<T, bool>> filter);
    }
}
