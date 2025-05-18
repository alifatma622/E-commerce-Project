using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        //get what i need based on the condition and store it in array includes
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        Task<T> GetByIdAsync(int Id);
        Task<T> GetByIdAsync(int Id, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);

        Task UpdateAsync(T entity);
        Task DeleteAsync(int Id);
        Task SaveChangesAsync();
        Task<int> CounAsync();

    }
}
